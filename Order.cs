public class Order
{
    public int ID { get; set; }
    public List<MenuItem> Pizzas { get; set; }
    public float TotalPrice { get; set; }

    public static List<Order> GetOrders()
    {
        string JsonStringOrders = File.ReadAllText("Orders.json");
        List<Order> Orders = JsonSerializer.Deserialize<List<Order>>(JsonStringOrders);
        return Orders;
    }

    public static float GetOrderTotalPrice(int ID)
    {
        List<Order> Orders = GetOrders();
        Order NeededOrder = null;
        float TotalPrice = 0;
        foreach (Order Order in Orders)
        {
            if (Order.ID == ID)
            {
                NeededOrder = Order;
                break;
            }
        }
        // If teh order is not found
        if (NeededOrder == null)
        {
            Console.WriteLine("No such order");
        }
        // If the order is found
        else
        {
            foreach (MenuItem Pizza in NeededOrder.Pizzas)
            {
                TotalPrice += Pizza.Price;
            }
        }
        return TotalPrice;
    }

    public static Order SaveOrder(List<MenuItem> Pizzas)
    {
        List<Order> Orders = GetOrders();
        Order Order = new();
        if (Orders.Count == 0)
        {
            Order.ID = 1;
        }
        else
        {
            Order.ID = Orders.Count + 1;
        }
        Order.Pizzas = Pizzas;
        Orders.Add(Order);
        var Options = new JsonSerializerOptions { WriteIndented = true };
        string JsonStringOrders = JsonSerializer.Serialize<List<Order>>(Orders, Options);
        File.WriteAllText("Orders.json", JsonStringOrders);
        Order.TotalPrice = GetOrderTotalPrice(Order.ID);
        return Order;
    }
    public static void PlaceAnOrder()
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
        Order Order = Order.SaveOrder(OrderedPizzas);
        AnsiConsole.WriteLine("Total price:" + Order.TotalPrice);
        AnsiConsole.Markup("[bold]Thank you for ordering from our pizza shop[/]");
    }
}