using Dapper;
using DapperRipTutorial.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DapperRipTutorial.LearnTutorial
{
    public class MultiMapping
    {
        private readonly string _connectionString;

        /*
        * In Dapper, multi mapping is a useful feature that you can use when you have 
        * a one-to-one or one-to-many relationship between objects, and you want to 
        * load all objects with a single query eagerly.
        */

        public MultiMapping(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void GetAuthorBooks()
        {
            var sql = @"SELECT * 
                        FROM Authors A
                        INNER JOIN Books B
                        ON A.Id = B.AuthorId";

            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                var authorDictionary = new Dictionary<int, Author>();

                var authors = connection.Query<Author, Book, Author>(
                    sql,
                    (author, book) =>
                    {
                        Author authorEntity;
                        if (!authorDictionary.TryGetValue(author.Id, out authorEntity))
                        {
                            authorEntity = author;
                            authorEntity.Books = new List<Book>();
                            authorDictionary.Add(authorEntity.Id, authorEntity);
                        }
                        authorEntity.Books.Add(book);
                        return authorEntity;
                    },
                    splitOn: "Id")
                    .Distinct()
                    .ToList();

                foreach (var author in authors)
                {
                    Console.WriteLine(author.FirstName + "" + author.LastName);

                    foreach(var book in author.Books)
                    {
                        Console.WriteLine("\t Title: {0} \t  Category: {1}", book.Title, book.Category);
                    }
                }
            }
        }
    }
}
