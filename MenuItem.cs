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
        String JsonStringMenu = File.ReadAllText("Menu.json");
        MenuItem[] Menu = JsonSerializer.Deserialize<MenuItem[]>(JsonStringMenu);
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
}