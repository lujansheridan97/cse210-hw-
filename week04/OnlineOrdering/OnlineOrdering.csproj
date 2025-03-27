using System;
using System.Collections.Generic;

class Product
{
    public string Name { get; set; }
    public int ProductId { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }

    // Constructor to initialize product
    public Product(string name, int productId, double price, int quantity)
    {
        Name = name;
        ProductId = productId;
        Price = price;
        Quantity = quantity;
    }

    // Method to calculate the total cost of the product
    public double CalculateTotalCost()
    {
        return Price * Quantity;
    }
}

class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    // Constructor to initialize address
    public Address(string street, string city, string state, string country)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
    }

    // Method to check if the address is in the USA
    public bool IsInUSA()
    {
        return Country == "USA";
    }

    // Method to return the full address as a string
    public string GetFullAddress()
    {
        return $"{Street}\n{City}, {State}\n{Country}";
    }
}

class Customer
{
    public string Name { get; set; }
    public Address CustomerAddress { get; set; }

    // Constructor to initialize customer with name and address
    public Customer(string name, Address customerAddress)
    {
        Name = name;
        CustomerAddress = customerAddress;
    }

    // Method to check if the customer lives in the USA
    public bool LivesInUSA()
    {
        return CustomerAddress.IsInUSA();
    }
}

class Order
{
    public List<Product> Products { get; set; }
    public Customer Customer { get; set; }

    // Constructor to initialize the order with products and customer
    public Order(List<Product> products, Customer customer)
    {
        Products = products;
        Customer = customer;
    }

    // Method to calculate the total price of the order
    public double CalculateTotalPrice()
    {
        double totalCost = 0;

        // Add the total cost of all products
        foreach (var product in Products)
        {
            totalCost += product.CalculateTotalCost();
        }

        // Add shipping cost
        if (Customer.LivesInUSA())
        {
            totalCost += 5;  // Shipping cost for USA
        }
        else
        {
            totalCost += 35; // Shipping cost for outside USA
        }

        return totalCost;
    }

    // Method to return the packing label
    public string GetPackingLabel()
    {
        string label = "Packing Label:\n";
        foreach (var product in Products)
        {
            label += $"{product.Name} (Product ID: {product.ProductId})\n";
        }
        return label;
    }

    // Method to return the shipping label
    public string GetShippingLabel()
    {
        return $"Shipping Label:\n{Customer.Name}\n{Customer.CustomerAddress.GetFullAddress()}";
    }
}

class Program
{
    static void Main()
    {
        // Create products
        Product product1 = new Product("Laptop", 101, 999.99, 1);
        Product product2 = new Product("Smartphone", 102, 499.99, 2);

        // Create address for customer
        Address address1 = new Address("123 Main St", "New York", "NY", "USA");
        Address address2 = new Address("456 Elm St", "Toronto", "ON", "Canada");

        // Create customers
        Customer customer1 = new Customer("John Doe", address1);
        Customer customer2 = new Customer("Jane Smith", address2);

        // Create orders
        Order order1 = new Order(new List<Product> { product1, product2 }, customer1);
        Order order2 = new Order(new List<Product> { product2 }, customer2);

        // Display order details
        DisplayOrderDetails(order1);
        DisplayOrderDetails(order2);
    }

    static void DisplayOrderDetails(Order order)
    {
        Console.WriteLine(order.GetPackingLabel());
        Console.WriteLine(order.GetShippingLabel());
        Console.WriteLine($"Total Price: ${order.CalculateTotalPrice():F2}\n");
    }
}
