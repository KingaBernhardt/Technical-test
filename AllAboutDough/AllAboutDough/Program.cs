
using AllAboutDough.Repositories;
using AllAboutDough.Services;
using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace AllAboutDough
{
    public class Program
    {
        private OrderService orderService;
        public Program(OrderService orderService)
        {
            this.orderService = orderService;
        }

        static void Main(string[] args)
        {
            /*var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<OrderService>()
            .AddSingleton<OrderRepository>()
            .BuildServiceProvider();

            //configure console logging
            serviceProvider
             .GetService<ILoggerFactory>()
             .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");


            //do the actual work here
            var bar = serviceProvider.GetService<OrderService>();*/
           

            string jsonString = ReadJsonFromFile();

            orderServices.CollectNonVegetarianToppings(order.GetToppings(JsonToCsv(jsonString)));
            order.CreateEntity(JsonToCsv(jsonString));
            foreach (var item in order.ConcatToppingsToString(JsonToCsv(jsonString)))
            {
                Console.WriteLine(item);
            }
            var lista = order.GetToppings(JsonToCsv(jsonString));
            var top = order.SplitCsvRowsIntoParts(JsonToCsv(jsonString));
            var kist = "";

            // get the orderDate
            for (int i = 0; i < top.GetLength(0) - 1; i++)
            {
                Console.WriteLine(top[i][0]);
            }


            // get the topping in one line, yet not one string

            /*string[] lol = new string[top.GetLength(0) - 1];
            string temp = "";
            for (int i = 0; i < top.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < top[i].Length; j++)
                {
                    temp += top[i][j] + " ";
                    lol[i] = temp;
                    Console.Write(top[i][j] + " ");

                }
                temp = "";
                Console.WriteLine();
            }

            Console.WriteLine("the real");
            for (int i = 0; i < lol.Length; i++)
            {
                Console.WriteLine(lol[i]);
            }*/

            foreach (var item in order.GetToppings(JsonToCsv(jsonString)))
            {
               // Console.WriteLine(item);
            }
            foreach (var item in order.SplitCsvIntoRows(JsonToCsv(jsonString)))
            {
                // Console.WriteLine(item);
            }
            //order.SplitCsvIntoRows(JsonToCsv(jsonString));

            order.SplitCsvRowsIntoParts(JsonToCsv(jsonString));
            foreach (var item in order.GetOrderDates(JsonToCsv(jsonString)))
            {
                // Console.WriteLine(item);
            }
            WriteCsvIntoFile(jsonString);
            // Console.WriteLine(JsonToCsv(jsonString));
            Console.ReadLine();
        }

        public static string ReadJsonFromFile()
        {
            string jsonString = String.Empty;
            try
            {
                string ordersJsonFilepath = @"../../../orders.json";
                using (StreamReader r = new StreamReader(ordersJsonFilepath))
                {
                    jsonString = r.ReadToEnd();
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return jsonString;
        }

        public static DataTable JsonStringToTable(string jsonString)
        {
            DataSet orderDataTable = JsonConvert.DeserializeObject<DataSet>(jsonString);
            var table = orderDataTable.Tables[0];
            return table;
        }

        public static string JsonToCsv(string jsonString)
        {
            StringWriter csvString = new StringWriter();
            using (var csv = new CsvWriter(csvString))
            {
                using (var data = JsonStringToTable(jsonString))
                {
                    foreach (DataColumn column in data.Columns)
                    {
                        csv.WriteField(column.ColumnName);
                    }
                    csv.NextRecord();

                    foreach (DataRow row in data.Rows)
                    {
                        for (var i = 0; i < data.Columns.Count; i++)
                        {
                            csv.WriteField(row[i]);
                        }
                        csv.NextRecord();
                    }
                }
            }
            return csvString.ToString();
        }

        public static void WriteCsvIntoFile(string jsonString)
        {
            try
            {
                string ordersCsvFilepath = @"../../../orders.csv";
                File.WriteAllText(ordersCsvFilepath, JsonToCsv(jsonString));
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
