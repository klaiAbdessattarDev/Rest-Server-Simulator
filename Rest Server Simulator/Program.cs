using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace Rest_Server_Simulator
{
    // Simulator class to simulate sending requests to a REST server
    public class Simulator
    {
        private static readonly HttpClient _httpClient = new();

        private static readonly string _BaseUrl = "https://localhost:44316/Customers";

        // Main method to run the simulator
        public static async Task Main(string[] args)
        {
            // List of tasks to run concurrently
            var tasks = new List<Task>
        {
            SendPostCustomersRequestAsync(),
            SendGetCustomersRequestAsync()
        };

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);
        }

        // Method to send a POST request to add customers
        private static async Task SendPostCustomersRequestAsync()
        {
            var customers = GenerateCustomers();

            // Send a POST request for each customer
            foreach (var customer in customers)
            {
                List<Customer> customerRequest = new() { customer };
                var response = await _httpClient.PostAsJsonAsync(_BaseUrl, customerRequest);
                response.EnsureSuccessStatusCode();
            }
        }

        // Method to send a GET request to retrieve customers
        private static async Task SendGetCustomersRequestAsync()
        {
            // Send a GET request to retrieve customers
            var response = await _httpClient.GetAsync(_BaseUrl);
            response.EnsureSuccessStatusCode();
            var customers = await response.Content.ReadFromJsonAsync<List<Customer>>();
            Console.WriteLine("Customers:");

            // Print the details of each customer
            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.Id}: {customer.LastName}, {customer.FirstName}, {customer.Age}");
            }
        }

        // Method to generate a list of customers
        private static List<Customer> GenerateCustomers()
        {
            var customers = new List<Customer>();

            var random = new Random();

            // Array of customer data objects
            var data = new[]
            {
            new { FirstName = "Leia", LastName = "Liberty" },
            new { FirstName = "Sadie", LastName = "Ray" },
            new { FirstName = "Jose", LastName = "Harrison" },
            new { FirstName = "Sara", LastName = "Ronan" },
            new { FirstName = "Frank", LastName = "Drew" },
            new { FirstName = "Dewey", LastName = "Powell" },
            new { FirstName = "Tomas", LastName = "Larsen" },
            new { FirstName = "Joel", LastName = "Chan" },
            new { FirstName = "Lukas", LastName = "Anderson" },
            new { FirstName = "Carlos", LastName = "Lane" }
        };

            // Generate 10 customers
            for (int i = 0; i < 10; i++)
            {
                // Select a random customer data object
                var customerData = data[random.Next(0, data.Length)];

                // Generate a random age
                var age = random.Next(10, 90);

                // Generate a unique ID using the hash code of a new Guid
                var id = Guid.NewGuid().GetHashCode();

                // Create a new customer object with the generated data
                customers.Add(new Customer { FirstName = customerData.FirstName, LastName = customerData.LastName, Age = age, Id = id });
            }

            return customers;
        }
    }

    public class Customer
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }
    }
}
