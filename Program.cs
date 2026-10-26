using System;
using System.Collections.Generic;
using System.Linq;

// Record 1
public record Product(int Id, string Name, decimal Price);

// Record 2
public record OrderLine(Product Product, int Quantity);

// Interface
public interface IDiscountPolicy
{
    decimal ApplyDiscount(OrderLine order);
}

// Discount implementation
public class SimpleDiscountPolicy : IDiscountPolicy
{
    public decimal ApplyDiscount(OrderLine order)
    {
        decimal total = order.Product.Price * order.Quantity;

        // Business rule: 10% discount for 5 or more items
        if (order.Quantity >= 5)
        {
            total = total * 0.90m;
        }

        return total;
    }
}

public class Program
{
    // Generic method
    public static T FindItem<T>(List<T> items, Func<T, bool> condition)
    {
        return items.First(condition);
    }

    public static void Main()
    {
        // Products
        var products = new List<Product>
        {
            new Product(1, "Mouse", 400m),
            new Product(2, "Keyboard", 120m),
            new Product(3, "Stylus Pen", 600m)
        };

        // Orders
        var orders = new List<OrderLine>
        {
            new OrderLine(products[0], 10),
            new OrderLine(products[1], 5),
            new OrderLine(products[2], 6),
            
        };

        IDiscountPolicy discountPolicy = new SimpleDiscountPolicy();

        decimal grandTotal = 0;

Console.WriteLine();
Console.WriteLine("===============================");
        Console.WriteLine("Order Report");
Console.WriteLine("===============================");

        foreach (var order in orders)
        {
            // Invalid input handling
            if (order.Quantity <= 0)
            {
                Console.WriteLine("Invalid quantity found.");
                continue;
            }

            decimal total = discountPolicy.ApplyDiscount(order);

            Console.WriteLine(
                order.Product.Name + " x " +
                order.Quantity + " = " +
                total);

            grandTotal += total;
        }

        // Pattern Matching
        string orderType = grandTotal switch
        {
            >= 100m => "Large Order",
            >= 50m => "Medium Order",
            _ => "Small Order"
        };

        Console.WriteLine();
Console.WriteLine("===============================");
        Console.WriteLine("Grand Total: " + grandTotal);
Console.WriteLine("===============================");
        Console.WriteLine("Order Type: " + orderType);

        // LINQ Filtering
        var cheapProducts = products
            .Where(p => p.Price <= 500)
            .OrderBy(p => p.Price)
            .Select(p => p.Name)
            .ToList();

        Console.WriteLine();
        Console.WriteLine("Cheap Products:");

        foreach (var product in cheapProducts)
        {
            Console.WriteLine(product);
        }

        // LINQ Grouping
        var groupedOrders = orders
            .GroupBy(o => o.Quantity >= 5 ? "Bulk Orders" : "Regular Orders")
            .ToList();

        Console.WriteLine();

        foreach (var group in groupedOrders)
        {
            Console.WriteLine(group.Key + ": " + group.Count());
        }

        // Generic Method Example
        var foundProduct = FindItem(products, p => p.Id == 2);

        Console.WriteLine();
        Console.WriteLine("Found Product: " + foundProduct.Name);
    }
}
