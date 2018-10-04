using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string csv_file_path = @"C:\Users\osidev6\Desktop\WebApplication2\test.csv";
            DataTable csvData = GetDataTableFromCSVFile(csv_file_path);
            Console.WriteLine("Rows count: " + csvData.Rows.Count);
        }

        public static void InsertDataIntoSQLServerUsingSQLBulkCopy(DataTable csvFileData)
        {
            using(SqlConnection dbConnection = new SqlConnection("Data Source="))
            {
                dbConnection.Open();
                using(SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = "TblPa401kEmployer";
                    s.WriteToServer(csvFileData);
                }
            }
        }

        public static DataTable GetDataTableFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using(TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { ";" });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach(string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while(!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();

                        for (int i=0; i<fieldData.Length; i++)
                        {
                            if(fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }

                        csvData.Rows.Add(fieldData);
                    }
                }
            }catch(Exception ex)
            {

            }
            InsertDataIntoSQLServerUsingSQLBulkCopy(csvData);
            return csvData;
        }
    }
}
