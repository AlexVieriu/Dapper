using Dapper;
using DapperRipTutorial.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DapperRipTutorial._02_BasicQuerying
{
    public class BasicQuerying
    {
        private readonly string _connectionString;

        public BasicQuerying(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void QueryWithDynamicParams()
        {
            var color = "Black";
            var age = 4;

            var query = "Select * from Cats where Color = @Color and Age > @Age";
            var dynamicParams= new DynamicParameters();
            dynamicParams.Add("Color", color);
            dynamicParams.Add("Age", age);

            using (var connection = new SqlConnection(_connectionString))
            {
                var cats = connection.Query<Cat>(query, dynamicParams).ToList();

                foreach (var cat in cats)
                {
                    Console.WriteLine($"{cat.Age} - {cat.Color}");
                }
            }
        }

        public void QueryForStaticType()
        {
            var parameters = new { age = 3 };
            var sql = "Select * from Dogs where Age > @age";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var dogs = connection.Query<Dog>(sql, parameters).ToList();

                foreach (var dog in dogs)
                {
                    Console.WriteLine($"{dog.Age} - {dog.Name}");
                }
            }
        }

        public void QueryForDynamicTypes()
        {
            var sql = "Select 1 as A, 2 as B";
            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                var result = connection.QueryFirst(sql);
                int a = (int)result.A;
                int b = (int)result.B;

                Console.WriteLine(a);
                Console.WriteLine(b);
            }
        }
    }
}
