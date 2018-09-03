
using AllAboutDough.Services;
using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;

namespace AllAboutDough
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonString = ReadJsonFromFile();
            OrderService order = new OrderService();
            foreach (var item in order.GetOrderDates(JsonToCsv(jsonString)))
            {
                Console.WriteLine(item);
            }
            WriteCsvIntoFile(jsonString);

            Console.WriteLine(JsonToCsv(jsonString));
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
