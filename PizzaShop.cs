public static class PizzaShop
{
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
                case 2: MenuItem.ShowMenu(); break;
                case 3: goto EndProgram;
                default: AnsiConsole.WriteLine("Not a valid number"); break;
            }
            AnsiConsole.Markup("To [yellow]order[/] please enter  1\nTo see the [blue]menu[/] please enter 2 \nTo [red]cancel[/] please enter 3");
            AnsiConsole.WriteLine();
            Choice = AnsiConsole.Ask<int>("");
        }
        EndProgram: AnsiConsole.WriteLine("Good bye");
        Order: Order.PlaceAnOrder();
    }
}
