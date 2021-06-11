using Dapper;
using DapperTutorial.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DapperTutorial.Dapper.Methods
{
    public class Query_
    {
        /*
         * QueryFirst
         * QueryFirstOrDefault
         * QuerySingle
         * QuerySingleOrDefault
         * QueryMultiple
                    
        First           : Returns the first element of a sequence
        Single          : Returns the only element of a sequence, and throws an exception if there is 
                          not exactly one element in the sequence 
        FirstOrDefault  : Returns the first element of a sequence, or a default value if the sequence 
                          contains no elements
        SingleOrDefault : Returns the only element of a sequence, or a default value if the sequence is
                          empty; this method throws an exception if there is more than one element in the 
                          sequence.         
        */

        private readonly string _connectionString;

        public Query_(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Anonymous(string sql)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var customer = connection.Query(sql).FirstOrDefault();
                Console.WriteLine(customer);
            }
        }

        public void StronglyTyped(string sql)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var customers = connection.Query<Customer>(sql).ToList();

                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} - {customer.LastName}");
                }
            }
        }

        public void MultiMapping_OneToOne(string sql)
        {
            // Return only Customer columns
            using(var connection = new SqlConnection(_connectionString))
            {
                var customers = connection.Query<Customer, CustomerAddress, Customer>(
                    sql,
                    (customer, customerAddress) =>
                    {
                        customer.CustomerAddress = customerAddress;
                        return customer;
                    },
                    splitOn: "CustomerId")
                    .Distinct()
                    .ToList();

                foreach (var customer in customers)
                {
                    Console.WriteLine(  $"{customer.CustomerAddress} - " +
                                        $"{customer.CustomerID} - " +
                                        $"{customer.Email} - " +
                                        $"{customer.LastName}");
                }
            }
        }

        // say what....?
        public void MultiMapping_OneToMany(string sql)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var customerDictionary = new Dictionary<int, Customer>();

                var list = connection.Query<Customer, CustomerAddress, Customer>(
                    sql,
                    (customer, customerAddress) =>
                    {
                        Customer customerEntry;

                        if (!customerDictionary.TryGetValue(customer.CustomerID, out customerEntry))
                        {
                            customerEntry = customer;
                            customerEntry.CustomerAddressList = new List<CustomerAddress>();
                            customerDictionary.Add(customerEntry.CustomerID, customerEntry);
                        }
                        customerEntry.CustomerAddressList.Add(customerAddress);
                        return customerEntry;
                    },
                    splitOn: "CustomerId")
                    .Distinct()
                    .ToList();

                foreach (var item in list)
                {
                    Console.WriteLine($"{item.CustomerID} - {item.Email}");                                      
                }
            }                           
        }

        // say what.... ?
        public void MultiType(string sqlMultyType)
        {
            // https://dev.to/shps951023/trace-dapper-net-source-code-query-multiple-mapping-1imf
        }
    }


}
