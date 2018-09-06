
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
        static void Main(string[] args)
        {  
            string jsonString = ReadJsonFromFile();
            OrderService order = new OrderService();
            foreach (var item in order.DecideBooleanValue(JsonToCsv(jsonString)))
            {
                Console.WriteLine(item);
            }
            order.CreateUpdatedCsv(order.GenerateGuid(), order.GetOrderDates(JsonToCsv(jsonString)),order.ConcatToppingsToString(JsonToCsv(jsonString)), order.DecideBooleanValue(JsonToCsv(jsonString)), JsonToCsv(jsonString));
            order.CollectNonVegetarianToppings(order.GetToppings(JsonToCsv(jsonString)));
            //order.CreateEntity(JsonToCsv(jsonString));
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

        public static void WriteCsvIntoFile(string csvString)
        {
            try
            {
                string ordersCsvFilepath = @"../../../orders.csv";
                File.WriteAllText(ordersCsvFilepath, JsonToCsv(csvString));
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}

