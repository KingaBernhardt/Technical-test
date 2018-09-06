
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
            OrderService orderService = new OrderService();
            string jsonString = ReadJsonFromFile();
            orderService.CreateUpdatedCsv(orderService.GetOrderDates(JsonToCsv(jsonString)), orderService.GetToppings(JsonToCsv(jsonString)),
                                           orderService.DecideBooleanValue(JsonToCsv(jsonString)), JsonToCsv(jsonString));
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
