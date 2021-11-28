using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.IO;
using System.Text.Json.Nodes;
using Spectre.Console;
using System.Collections.Generic;

public static class Program
{

    public static void menu()
    {

        var table = new Table();
        table.AddColumn("number");
        table.AddColumn("Pizza");
        table.AddColumn("description");
        table.AddColumn("extra toppings");
        table.AddColumn("available sizes");
        table.AddColumn("price");
        //deserializing into a class
        string jsonString = File.ReadAllText("menu.json");
        int count = 1;
        MenuItem[] menu = JsonSerializer.Deserialize<MenuItem[]>(jsonString);
        foreach (MenuItem currentPizza in menu)
        {
            string name = currentPizza.name;
            string desc = currentPizza.description;
            string toppings = "";
            string sizes = "";
            string price = currentPizza.price + "LE";
            foreach (string topping in currentPizza.extraToppings)
            {
                toppings += topping + ",";
            }
            foreach (string size in currentPizza.size)
            {
                switch (size)
                {
                    case "small": sizes += "S,"; break;
                    case "medium": sizes += "M,"; break;
                    case "large": sizes += "L,"; break;
                    default: break;
                }
            }
            toppings = toppings.Substring(0, toppings.Length - 1);
            sizes = sizes.Substring(0, sizes.Length - 1);
            table.AddRow(count + "", name, desc, toppings, sizes, price);
            table.AddRow("");
            count++;

        }

        //using json DOM 
        // string jsonString = File.ReadAllText("menu.json");
        // JsonNode menu = JsonNode.Parse(jsonString);
        // Console.WriteLine(menu[0]["extra toppings"].ToString());
        // for (int i = 0; i < 5; i++)
        // {
        //     var currentPizza=menu[i];
        //     var name=currentPizza["name"].ToString();
        //     var description=currentPizza["description"].ToString();
        //     var extraToppings=currentPizza["extra toppings"].ToString();
        //     var sizes=currentPizza["size"].ToString();
        //     var price=currentPizza["price"].ToString();
        //     table.AddRow(new Markup(i+1+""),new Markup(name),new Markup(description),new Markup(extraToppings.EscapeMarkup()),new Markup(sizes.EscapeMarkup()),new Markup(price));  
        // }
        AnsiConsole.Render(table);



    }

    public static void order()
    {
        string jsonStringMenu = File.ReadAllText("menu.json");
        string jsonStringOrders = File.ReadAllText("Orders.json");
        MenuItem[] menu = JsonSerializer.Deserialize<MenuItem[]>(jsonStringMenu);
        AnsiConsole.Markup("[bold white] how many pizzas do you want to order? [/]");
        AnsiConsole.WriteLine();
        float totalPrice = 0;
        List<Order> orders = JsonSerializer.Deserialize<List<Order>>(jsonStringOrders);
        Order order = new Order();
        List<String> pizzas = new List<string>();
        int numberOfPizzas = AnsiConsole.Ask<int>("");
        foreach (MenuItem item in menu)
        {
            pizzas.Add(item.number + "." + item.name);
        }
        while (numberOfPizzas-- > 0)
        {
            var pizza = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[bold white]Please select the desired pizza [/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Press [blue]<space>[/] to toggle a size, " +
            "[green]<enter>[/] to accept)[/]")
            .AddChoices(pizzas));
            int pizzaNumber = int.Parse(pizza[0] + "");
            MenuItem currentPizza = menu[pizzaNumber - 1];


            //int currentPizza=int.Parse(pizza.cha)
            string[] extraToppings = currentPizza.extraToppings;
            string[] sizes = currentPizza.size;
            var toppings = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
            .Title("[bold white]Please select the extra toppings you want from this list [/]")
            .NotRequired()
            .PageSize(10)
            .InstructionsText(
            "[grey](Press [blue]<space>[/] to toggle a topping, " +
            "[green]<enter>[/] to accept)[/]")
            .AddChoices(extraToppings));
            AnsiConsole.Markup("[bold white]please choose the size you want from this list  [/]");

            var size = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[bold white]Please select the extra toppings you want from this list [/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Press [blue]<space>[/] to toggle a size, " +
            "[green]<enter>[/] to accept)[/]")
            .AddChoices(sizes));

            if (orders.Count == 0)
                order.id = 1;
            else
                order.id = orders.Count;
            //saving the ordered pizza along with it's toppings and size in the current order
            MenuItem orderedPizza = new MenuItem();
            orderedPizza.name = currentPizza.name;
            orderedPizza.number = pizzaNumber;
            orderedPizza.description = currentPizza.description;
            string[] sizeInArray = { size };
            orderedPizza.size = sizeInArray;
            orderedPizza.extraToppings = toppings.ToArray();
            orderedPizza.price = currentPizza.price;
            totalPrice += currentPizza.price;
            if (order.pizzas == null)
                order.pizzas = new List<MenuItem>();
            order.pizzas.Add(orderedPizza);
            AnsiConsole.WriteLine("Chosen pizza:" + currentPizza.name);
            AnsiConsole.WriteLine("Chosen toppings:" + string.Join(",", toppings));
            AnsiConsole.WriteLine("Chosen size:" + string.Join("", size));
            AnsiConsole.WriteLine("Pizza price:" + orderedPizza.price);
            AnsiConsole.WriteLine();

        }
        AnsiConsole.WriteLine("Total price:" + totalPrice);
        AnsiConsole.Markup("[bold]thank you for ordering from our pizza shop[/]");
        order.totalPrice = totalPrice;
        orders.Add(order);
        var options = new JsonSerializerOptions { WriteIndented = true };
        jsonStringOrders = JsonSerializer.Serialize<List<Order>>(orders,options);
        File.WriteAllText("Orders.json", jsonStringOrders);


    }


    public static void Main(string[] args)
    {
        AnsiConsole.Markup("[bold]Welcome to our pizza shop[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.Markup("To [yellow]order[/] please enter  1\nTo see the [blue]menu[/] please enter 2 \nTo [red]cancel[/] please enter 3");
        AnsiConsole.WriteLine();
        int choice = AnsiConsole.Ask<int>("");
        while (true)
        {
            switch (choice)
            {
                case 1: goto order;
                case 2: menu(); break;
                case 3: goto endProgram;
                default: AnsiConsole.WriteLine("Not a valid number"); break;
            }
            AnsiConsole.Markup("To [yellow]order[/] please enter  1\nTo see the [blue]menu[/] please enter 2 \nTo [red]cancel[/] please enter 3");
            AnsiConsole.WriteLine();
            choice = AnsiConsole.Ask<int>("");
        }

    endProgram: AnsiConsole.WriteLine("Good bye");
    order: order();







    }
}
