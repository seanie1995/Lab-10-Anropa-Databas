using Lab_10_Anropa_Databas.Data;
using Lab_10_Anropa_Databas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Reflection.Metadata;

namespace Lab_10_Anropa_Databas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the program");
                                       
            while (true)
            {               
                using (NorthContext context = new NorthContext())
                {
                    Console.WriteLine();
                    Console.WriteLine("Press [1] to show all customers");
                    Console.WriteLine("Press [2] to search for a specific customer");
                    Console.WriteLine("Press [3] to add new customer data");
                    Console.WriteLine("press [4] to quit program");

                    string userInput = Console.ReadLine();

                    if (userInput != "1" && userInput != "2" && userInput != "3" && userInput != "4") 
                    { 
                        Console.WriteLine("Invalid input!");
                    }

                    switch (userInput)
                    {
                        case "1":
                            PrintAllCustomers(context);
                            break;

                        case "2":
                            SearchCustomerInfo(context);
                            break;
                        
                        case "3":
                            AddNewCustomer(context);
                            break;

                        case "4":
                            Environment.Exit(1);
                            break;                                                        
                    }
                }                          
            }                                                 
        }
        static void PrintAllCustomers(NorthContext context)
        {
            Console.WriteLine("The program will now print out all customer details in alphabetical order according to company name");
            Console.WriteLine("Press 'A' to get list in ascending order or 'D' to get list in descending order");
            string userInput = Console.ReadLine().ToLower();

            while (userInput != "a" && userInput != "d")
            {
                Console.WriteLine("Invalid input. Please press 'A' for list in ascending order or 'D' for listi n descending order");
                userInput = Console.ReadLine().ToLower();
            }
        
            if (userInput == "a") // OrderBy Ascending
            {
                var allCustomers = context.Customers
                   .OrderBy(c => c.CompanyName)
                   .Include(c => c.Orders)
                   .ToList();

                var shippedOrders = context.Orders // Code that counts amount of not-null ShippedDates. Extrauppgift
                    .Join(
                        context.Customers,
                        o => o.CustomerId,
                        c => c.CustomerId,
                        (o, c) => new
                        {
                            c.CompanyName,                           
                            ShippedOrders = o.ShippedDate
                        }
                    )
                    .Where(o => o.ShippedOrders != null)
                    .GroupBy(i => i.CompanyName)
                    .Select(i => new
                    {
                        CompanyName = i.Key,
                        ShippedOrdersCount = i.Count()
                    })
                    .ToList();
               
                foreach (var c in allCustomers)
                {
                    var shippedOrderInfo = shippedOrders.FirstOrDefault(p => p.CompanyName == c.CompanyName);
                    int shippedOrderCount = shippedOrderInfo != null ? shippedOrderInfo.ShippedOrdersCount : 0;
                    int notShippedOrderCount = c.Orders.Count - shippedOrderCount;

                    Console.WriteLine($"{c.CompanyName}, {c.Country} {c.Region} {c.Phone}, Order Count: {c.Orders.Count}, Shipped Order Count: {shippedOrderCount}, Unshipped Order Count: {notShippedOrderCount}");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------");
                }

            }
            else if (userInput == "d") // OrderBy Descending
            {
                var allCustomers = context.Customers
                    .OrderByDescending(c => c.CompanyName)
                    .Include(c => c.Orders)
                    .ToList();

                var shippedOrders = context.Orders
                   .Join(
                       context.Customers,
                       o => o.CustomerId,
                       c => c.CustomerId,
                       (o, c) => new
                       {
                           c.CompanyName,
                           ShippedOrders = o.ShippedDate
                       }
                   )
                   .Where(o => o.ShippedOrders != null)
                   .GroupBy(i => i.CompanyName)
                   .Select(i => new
                   {
                       CompanyName = i.Key,
                       ShippedOrdersCount = i.Count()
                   })
                   .ToList();
               
                foreach (var c in allCustomers)
                {
                    var shippedOrderInfo = shippedOrders.FirstOrDefault(o => o.CompanyName == c.CompanyName);
                    int shippedOrderCount = shippedOrderInfo != null ? shippedOrderInfo.ShippedOrdersCount : 0;
                    int notShippedOrderCount = c.Orders.Count - shippedOrderCount;

                    Console.WriteLine($"{c.CompanyName}, {c.Country} {c.Region} {c.Phone}, Order Count: {c.Orders.Count}, Shipped Order Count: {shippedOrderCount}, Unshipped Order Count: {notShippedOrderCount}");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------");
                }
            }
    }
        static void SearchCustomerInfo(NorthContext context)
        {
            Console.WriteLine("Please enter company name to search:");

            string userInput = Console.ReadLine();

            Console.Clear();

            var searchedCustomer = context.Customers
                    .Where(c => c.CompanyName == userInput)                  
                    .Select(c => new { c.CompanyName, c.ContactName, c.ContactTitle, c.Address, c.City, c.Region, c.PostalCode, c.Country, c.Fax, c.Phone })
                    .ToList();

                foreach (var c in searchedCustomer)
                {
                    Console.WriteLine(c);
                }

            if (searchedCustomer.Count == 0)
            {
                Console.WriteLine($"No company with the name '{userInput}' was found.");
            }

            else 
            {
                var listOfOrders = context.Customers
                    .Where(c => c.CompanyName == userInput)
                    .Include(c => c.Orders)
                    .Single()
                    .Orders
                    .ToList();
                
                foreach (var c in listOfOrders)
                {
                    Console.WriteLine($"OrderID: {c.OrderId} Order Date: {c.OrderDate}");
                }

            }
          
        }
        static void AddNewCustomer(NorthContext context)
        {
            Console.Clear();
            Random rnd = new Random();

            string randomID = "";
            char letter;
            int rndValue;          
                      
            for (int i = 0; i <= 4; i++)
            {
                rndValue = rnd.Next(0, 26);
                letter = Convert.ToChar(rndValue + 65);
                randomID += letter;
            }
            
            Console.WriteLine("Please enter company name:");
            string companyName = Console.ReadLine();          

            while (string.IsNullOrEmpty(companyName) )
            {
                Console.WriteLine("Company Name cannot be empty! Please enter company name:");
                companyName = Console.ReadLine();
            }
           
            Console.WriteLine("Enter contact name:");
            string input = Console.ReadLine();
            string contactName = string.IsNullOrWhiteSpace(input) ? null : input;
            
            Console.WriteLine("Please enter contact title:");
            input = Console.ReadLine();
            string contactTitle = string.IsNullOrWhiteSpace(input) ? null : input;

            Console.WriteLine("Please enter address:");
            input = Console.ReadLine();
            string address = string.IsNullOrWhiteSpace(input) ? null : input;  
            
            Console.WriteLine("Please enter city:");          
            input = Console.ReadLine();
            string city = string.IsNullOrWhiteSpace(input) ? null : input;
           
            Console.WriteLine("Please enter region:");
            input = Console.ReadLine();
            string region = string.IsNullOrWhiteSpace(input) ? null : input;

            Console.WriteLine("Please enter postal code:");
            input = Console.ReadLine();
            string postalCode = string.IsNullOrWhiteSpace(input) ? null : input;

            Console.WriteLine("Please enter country:");
            input = Console.ReadLine();
            string country = string.IsNullOrWhiteSpace(input) ? null : input;

            Console.WriteLine("Please enter phone number");
            input = Console.ReadLine();
            string phoneNumber = string.IsNullOrWhiteSpace(input) ? null : input;

            Console.WriteLine("Please enter fax number");
            input = Console.ReadLine();
            string faxNumber = string.IsNullOrWhiteSpace(input) ? null : input;

            Customer customer = new Customer()
            {
                              
                CustomerId = randomID,
                CompanyName = companyName,
                ContactName = contactName,
                ContactTitle = contactTitle,
                Address = address,
                City = city,
                Region = region,
                PostalCode = postalCode,
                Phone = phoneNumber,
                Fax = faxNumber,
                Country = country,
            };
            context.Customers.Add(customer);
            context.SaveChanges();

        }
    }
}