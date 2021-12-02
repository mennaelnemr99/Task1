using System.Collections.Generic;
using System;
using System.Text.Json;
using System.IO;
public class MenuItem
{
    public int Number { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] ExtraToppings { get; set; }
    public string[] Size { get; set; }
    public int Price { get; set; }

    public static MenuItem[] GetMenuItems()
    {   
        var Options= new JsonSerializerOptions(){PropertyNameCaseInsensitive=true};
        String JsonStringMenu = File.ReadAllText("Menu.json");
        MenuItem[] Menu = JsonSerializer.Deserialize<MenuItem[]>(JsonStringMenu,Options);
        return Menu;
    }

    public static List<string> GetPizzasNames()
    {
        List<string> Pizzas = new List<string>();
        MenuItem[] Menu = GetMenuItems();
        foreach (MenuItem Item in Menu)
        {
            Pizzas.Add($"{Item.Number}.{Item.Name}");
        }
        return Pizzas;
    }

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
}