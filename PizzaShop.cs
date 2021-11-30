using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.IO;
using System.Text.Json.Nodes;
using Spectre.Console;
using System.Collections.Generic;

public static class PizzaShop
{
    public static void ShowMenu()
    {
        var Table = new Table();
        Table.AddColumn("Number").AddColumn("Pizza").AddColumn("Description").AddColumn("Extra toppings").AddColumn("Available sizes").AddColumn("Price");
        MenuItem[] PizzaMenu = MenuItem.GetMenuItems();
        foreach (MenuItem CurrentPizza in PizzaMenu)
        {
            string PizzaNumber = CurrentPizza.Number + "";
            string PizzaName = CurrentPizza.Name;
            string PizzaDesc = CurrentPizza.Description;
            string PizzaToppings = "";
            string PizzaSizes = "";
            string PizzaPrice = CurrentPizza.Price + "LE";
            foreach (string Topping in CurrentPizza.ExtraToppings)
            {
                PizzaToppings += Topping + ",";
            }
            foreach (string Size in CurrentPizza.Size)
            {
                switch (Size)
                {
                    case "small": PizzaSizes += "S,"; break;
                    case "medium": PizzaSizes += "M,"; break;
                    case "large": PizzaSizes += "L,"; break;
                    default: break;
                }
            }
            //Removing the extra commas 
            PizzaToppings = PizzaToppings.Substring(0, PizzaToppings.Length - 1);
            PizzaSizes = PizzaSizes.Substring(0, PizzaSizes.Length - 1);
            //Adding the pizza to the tabel row
            Table.AddRow(PizzaNumber + "", PizzaName, PizzaDesc, PizzaToppings, PizzaSizes, PizzaPrice);
            Table.AddRow("");
        }
        AnsiConsole.Render(Table);
    }
    public static void Order()
    {
        AnsiConsole.Markup("[bold white] how many pizzas do you want to order? [/]");
        AnsiConsole.WriteLine();
        int NumberOfPizzas = AnsiConsole.Ask<int>("");
        MenuItem[] Menu = MenuItem.GetMenuItems();
        List<string> PizzasToChooseFrom = MenuItem.GetPizzasNames();
        List<MenuItem> OrderedPizzas = new List<MenuItem>();
        while (NumberOfPizzas-- > 0)
        {   // User choosing pizza
            var OrderedPizzaName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[bold white]Please select the desired pizza [/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Press [blue]<space>[/] to toggle a size, " +
            "[green]<enter>[/] to accept)[/]")
            .AddChoices(PizzasToChooseFrom));
            int OrderedPizzaNumber = int.Parse(OrderedPizzaName[0] + "");
            MenuItem ChosenPizza = Menu[OrderedPizzaNumber - 1];
            string[] OrderedPizzaExtraToppings = ChosenPizza.ExtraToppings;
            string[] OrderedPizzaSizes = ChosenPizza.Size;
            // User choosing toppings
            var OrderedToppings = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
            .Title("[bold white]Please select the extra toppings you want from this list [/]")
            .NotRequired()
            .PageSize(10)
            .InstructionsText(
            "[grey](Press [blue]<space>[/] to toggle a topping, " +
            "[green]<enter>[/] to accept)[/]")
            .AddChoices(OrderedPizzaExtraToppings));
            AnsiConsole.Markup("[bold white]Please choose the size you want from this list  [/]");
            // User choosing size
            var OrderedSize = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[bold white]Please select the extra toppings you want from this list [/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Press [blue]<space>[/] to toggle a size, " +
            "[green]<enter>[/] to accept)[/]")
            .AddChoices(OrderedPizzaSizes));
            // Adding ordered pizza to the list of ordered pizzas 
            MenuItem OrderedPizza = new MenuItem();
            OrderedPizza.Name = ChosenPizza.Name;
            OrderedPizza.Number = OrderedPizzaNumber;
            OrderedPizza.Description = ChosenPizza.Description;
            string[] SizeInArray = { OrderedSize };
            OrderedPizza.Size = SizeInArray;
            OrderedPizza.ExtraToppings = OrderedToppings.ToArray();
            OrderedPizza.Price = ChosenPizza.Price;
            OrderedPizzas.Add(OrderedPizza);
            // Printing the ordered pizza to the user
            AnsiConsole.WriteLine("Chosen pizza:" + OrderedPizza.Name);
            AnsiConsole.WriteLine("Chosen toppings:" + string.Join(",", OrderedToppings));
            AnsiConsole.WriteLine("Chosen size:" + string.Join("", OrderedSize));
            AnsiConsole.WriteLine("Pizza price:" + OrderedPizza.Price);
            AnsiConsole.WriteLine();
        }
        Order Order = Order.PlaceOrder(OrderedPizzas);
        AnsiConsole.WriteLine("Total price:" + Order.TotalPrice);
        AnsiConsole.Markup("[bold]Thank you for ordering from our pizza shop[/]");
    }
    public static void Main(string[] args)
    {
        AnsiConsole.Markup("[bold]Welcome to our pizza shop[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.Markup("To [yellow]order[/] please enter  1\nTo see the [blue]menu[/] please enter 2 \nTo [red]cancel[/] please enter 3");
        AnsiConsole.WriteLine();
        int Choice = AnsiConsole.Ask<int>("");
        while (true)
        {
            switch (Choice)
            {
                case 1: goto Order;
                case 2: ShowMenu(); break;
                case 3: goto EndProgram;
                default: AnsiConsole.WriteLine("Not a valid number"); break;
            }
            AnsiConsole.Markup("To [yellow]order[/] please enter  1\nTo see the [blue]menu[/] please enter 2 \nTo [red]cancel[/] please enter 3");
            AnsiConsole.WriteLine();
            Choice = AnsiConsole.Ask<int>("");
        }
        EndProgram: AnsiConsole.WriteLine("Good bye");
        Order: Order();
    }
}
