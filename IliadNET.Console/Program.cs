using IliadNET;
using Newtonsoft.Json;
using System;

namespace NIliad
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Username:");
            var username = Console.ReadLine();
            Console.WriteLine("Password:");
            var password = Console.ReadLine();

            var connector = new Iliad(username, password);
            var customerData = connector.GetInfo();

            Console.Write(JsonConvert.SerializeObject(customerData, Formatting.Indented));

            Console.ReadLine();
        }
    }
}
