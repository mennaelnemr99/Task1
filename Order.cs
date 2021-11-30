using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.IO;
using System.Text.Json.Nodes;
using Spectre.Console;
using System.Collections.Generic;
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

    public static Order PlaceOrder(List<MenuItem> Pizzas)
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
}