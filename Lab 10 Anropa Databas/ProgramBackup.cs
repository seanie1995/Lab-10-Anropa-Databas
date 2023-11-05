using Lab_10_Anropa_Databas.Data;
using Lab_10_Anropa_Databas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Lab_10_Anropa_Databas
{
    internal class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
               using (NorthContext context = new NorthContext())
                {
                    Console.WriteLine("Welcome to the program");
                    Console.WriteLine("Press [1] to show all customers");
                    Console.WriteLine("Press [2] to search for a specific customer");
                    Console.WriteLine("Press [3] to add new customer data");
                    Console.WriteLine("press [4] to quit program");

                    int userInput = Int32.Parse(Console.ReadLine());

                    switch (userInput)
                    {
                        case 1:
                            PrintAllCustomers(context);
                            break;

                        case 2:
                            SearchCustomerInfo(context);
                            break;
                        
                        case 3:
                            AddNewCustomer(context);
                            break;

                        case 4:
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
                
                List<Customer> allCustomers = context.Customers
                    .OrderBy(c => c.CompanyName)
                    .ToList();
                 
                foreach (var c in allCustomers)
                {
                    Console.WriteLine($"{c.CompanyName} {c.Country} {c.Region} {c.Phone} {c.Orders.Count}");
                }
                
            }
            else if (userInput == "d") // OrderBy Descending
            {               
                List<Customer> allCustomers = context.Customers
                    .OrderByDescending(c => c.CompanyName)
                    .ToList();

                foreach (var c in allCustomers)
                {
                    Console.WriteLine($"{c.CompanyName} {c.Country} {c.Region} {c.Phone} {c.Orders.Count}");
                }            
            }
        }
        static void SearchCustomerInfo(NorthContext context)
        {
            Console.WriteLine("Please enter company name you wish to search for:");

            string userInput = Console.ReadLine();

            var searchedCustomer = context.Customers
                    .Where(c => c.CompanyName == userInput)
                    .Include(c => c.Orders)
                    .Select(c => new { c.CompanyName, c.ContactName, c.ContactTitle, c.Address, c.City, c.Region, c.PostalCode, c.Country, c.Fax })
                    .ToList();

                foreach (var c in searchedCustomer)
                {
                    Console.WriteLine(c);
                }

                List<Order> listOfOrders = context.Customers
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
        static void AddNewCustomer(NorthContext context)
        {
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
            Console.WriteLine("Enter contact name:");
            string contactName = Console.ReadLine();
            Console.WriteLine("Please enter contact title:");
            string contactTitle = Console.ReadLine();

            Console.WriteLine("Please enter address:");
            string address = Console.ReadLine();
            Console.WriteLine("Please enter city:");
            string city = Console.ReadLine();

            Console.WriteLine("Please enter region:");
            string region = Console.ReadLine();
            Console.WriteLine("Please enter postal code:");
            string postalCode = Console.ReadLine();

            Console.WriteLine("Please enter country:");
            string country = Console.ReadLine();

            Console.WriteLine("Please enter phone and fax numbers");
            string phoneNumber = Console.ReadLine();
            Console.WriteLine("Please enter phone and fax numbers");
            string faxNumber = Console.ReadLine();

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