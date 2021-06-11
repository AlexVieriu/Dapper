using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DapperTutorial.Dapper.Methods
{
    public class ExecuteReader_
    {
        private readonly string _connectionString;

        public ExecuteReader_(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Command(string sql)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var reader = connection.ExecuteReader(sql);

                DataTable table = new DataTable();
                table.Load(reader);

                for(int i=0; i<table.Rows.Count; i++)
                {
                    Console.WriteLine(table.Rows[i]["FirstName"]);
                }
            }
        }

        public void Procedure(string procedure)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var reader = connection.ExecuteReader($"{procedure} @CustomerId= @Id",
                                                      new { Id = 2 });

                DataTable table = new DataTable();
                table.Load(reader);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    Console.WriteLine(table.Rows[i]["FirstName"]);
                }
            }
        }
    }
}
