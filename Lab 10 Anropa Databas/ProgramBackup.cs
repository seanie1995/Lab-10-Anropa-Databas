using Lab_10_Anropa_Databas.Data;
using Lab_10_Anropa_Databas.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_10_Anropa_Databas
{
    internal class Program
    {
        static void Main(string[] args)
        {            
            using (var context = new NorthContext()) 
            {
                string userInput;

                PrintAllCustomers(context);
                Console.WriteLine();
                
                Console.WriteLine("Please enter company name you wish to search for:");
                
                userInput = Console.ReadLine();
                
                SearchCustomerInfo(userInput, context);
                
                Console.ReadKey();

                Console.WriteLine("You will now add a new customer into the database.");
                AddNewCustomer(context);            
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
        static void SearchCustomerInfo(string input, NorthContext context)
        {           
                var searchedCustomer = context.Customers
                    .Where(c => c.CompanyName == input)
                    .Include(c => c.Orders)
                    .Select(c => new { c.CompanyName, c.ContactName, c.ContactTitle, c.Address, c.City, c.Region, c.PostalCode, c.Country, c.Fax })
                    .ToList();

                foreach (var c in searchedCustomer)
                {
                    Console.WriteLine(c);
                }

                List<Order> listOfOrders = context.Customers
                    .Where(c => c.CompanyName == input)
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
                CustomerId = rnd.Next(10000, 99999).ToString(),
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