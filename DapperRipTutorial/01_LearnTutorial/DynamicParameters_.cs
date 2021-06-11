using Dapper;
using DapperRipTutorial.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace DapperRipTutorial.LearnTutorial
{
    public class DynamicParameters_
    {        
        private readonly string _connectionString;

        public DynamicParameters_(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void GetAuthorBooks()
        {
            string sql = "GetAuthor";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                using (var result = connection.QueryMultiple(sql,
                                                             new { Id = 3 },
                                                             commandType: CommandType.StoredProcedure))
                {
                    var author = result.Read<Author>().SingleOrDefault();
                    var books = result.Read<Book>().ToList();

                    if (author != null && books != null)
                    {
                        author.Books = books;

                        Console.WriteLine(author.FirstName + "" + author.LastName);

                        foreach (var book in books)
                        {
                            Console.WriteLine("\t Title: {0} \t  Category: {1}", book.Title, book.Category);
                        }
                    }
                }
            }
        }

        public void GetAuthorBooks_DynamicParams()
        {
            string sql = "GetAuthor";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", 3);

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                using (var result = connection.QueryMultiple(sql,
                                                             parameters,
                                                             commandType: CommandType.StoredProcedure))
                {
                    var author = result.Read<Author>().SingleOrDefault();
                    var books = result.Read<Book>().ToList();

                    if(author != null && books.Count >=0)
                    {
                        author.Books = books;
                        Console.WriteLine($"{author.FirstName} -  {author.LastName}");

                        foreach (var book in books)
                        {
                            Console.WriteLine("\t Title: {0} \t  Category: {1}", book.Title, book.Category);
                        }
                    }
                }                               
            }
        }

        public void InsertSingleAuthor_DynamicParam()
        {
            var sql = "Insert Into Authors(FirstName, LastName) Values(@FirstName, @LastName)";
            using (IDbConnection conection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("FirstName", "William");
                parameters.Add("LastName", "Shakespeare");

                var result = conection.Execute(sql, parameters) * -1;

                Console.WriteLine($"Results: {result}");
            }
        }
    }
}
