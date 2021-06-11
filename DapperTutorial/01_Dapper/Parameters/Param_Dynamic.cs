using Dapper;
using DapperTutorial.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DapperTutorial.Dapper.Parameters
{
    public class Param_Dynamic
    {
        private readonly string _connectionString;

        public Param_Dynamic(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Single(string sql)
        {
            using(var conection = new SqlConnection(_connectionString))
            {
                var p = new DynamicParameters();

                p.Add("FirstName","Vieriu");
                p.Add("LastName","Alexandru");
                p.Add("Email","vieriu.alexandru@gmail.com");

                var affectedRows = conection.Execute(sql, p, commandType: CommandType.StoredProcedure) * -1;

                Console.WriteLine($"Single: {affectedRows}");
            }
        }

        public void Many(string sql)
        {
            var parameters = new List<DynamicParameters>();

            for (int i = 0; i < 3; i++)
            {
                var p = new DynamicParameters();
                p.Add("FirstName", "Vieriu" + i);
                p.Add("LastName", "Alexandru" + i);
                p.Add("Email", "vieriu.alexandru" + i +"@gmail.com");

                parameters.Add(p);
            }

            using(var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = connection.Execute(sql, parameters, commandType: CommandType.StoredProcedure) * -1;

                Console.WriteLine($"Many: {affectedRows}");
            }
        }

        public void List(string sql)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var customers = connection.Query<Customer>(sql).ToList();

                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} - {customer.LastName}");                }
            }
        }

        public void String(string sql)
        {
            
        }

        // TVP lets you pass a table to allow you to perform "IN" clause, massive insert, and a lot of more.
        public void TableValueParam_AtFirstExecution()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                // Let first, create the table.
                connection.Execute(@"
                   CREATE TABLE [Customer]
                    (
                        [CustomerID] [INT] IDENTITY(1,1) NOT NULL,
                        [FirstName] [VARCHAR](20) NULL,
                        [LastName] [VARCHAR](20) NULL,

                        CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
                        (
                            [CustomerID] ASC
                        )
                    )
                ");

                // Then create the TVP type.
                connection.Execute(@"
                    CREATE TYPE TVP_Customer AS TABLE
                    (
                        [FirstName] [VARCHAR](20) NULL,
                        [LastName] [VARCHAR](20) NULL
                    )
                ");

                // Create the stored procedure that will take the TVP type as a parameter.
                connection.Execute(@"
                    CREATE PROCEDURE Customer_Seed
                        @Customers TVP_Customer READONLY
                    AS
                    BEGIN
                        INSERT INTO Customer (FirstName, LastName)
                        SELECT FirstName, LastName
                        FROM @Customers
                    END
                ");

                // Use a TVP parameter
                var dt = new DataTable();
                dt.Columns.Add("FirstName");
                dt.Columns.Add("LastName");

                for (int i = 0; i < 5; i++)
                {
                    dt.Rows.Add("FirstName_" + i, "LastName_" + i);
                }

                var executed = connection.Execute("Customer_Seed",
                                                   new { Customers = dt.AsTableValuedParameter("TVP_Customer") },
                                                   commandType: CommandType.StoredProcedure);
            }
        }

        public void TableValueParam()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Let first, create the table.
                // Then create the TVP type.
                // Create the stored procedure that will take the TVP type as a parameter.
                // Use a TVP parameter
                var dt = new DataTable();
                dt.Columns.Add("FirstName");
                dt.Columns.Add("LastName");

                for (int i = 0; i < 5; i++)
                {
                    dt.Rows.Add("FirstName_" + i, "LastName_" + i);
                }

                var executed = connection.Execute("Customer_Seed",
                                                   new { Customers = dt.AsTableValuedParameter("TVP_Customer") },
                                                   commandType: CommandType.StoredProcedure);

                Console.WriteLine($"RowsAffected - {executed}");
            }
        }
    }
}
