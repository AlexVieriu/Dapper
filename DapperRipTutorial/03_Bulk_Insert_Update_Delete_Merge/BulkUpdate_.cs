using Dapper;
using DapperRipTutorial.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Z.Dapper.Plus;

namespace DapperRipTutorial.Bulk_Insert_Update_Delete_Merge
{
    public class BulkUpdate_
    {
        // https://dapper-tutorial.net/bulk-update

        private readonly string _connectionString;

        public BulkUpdate_(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void UpdateSingle()
        {
            var customer = new Customer() { CustomerName = "Vieriu Alexandru", City = "Bucharest" };

            DapperPlusManager.Entity<Customer>().Table("Customers");

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.BulkUpdate(customer);
            }
        }

        public void updateMany()
        {
            var customers = new List<Customer>()
            {
                new Customer(){ CustomerName = "Vieriu1", City = "Bucharest" },
                new Customer(){ CustomerName = "Vieriu2 ", City = "Bucharest" },
                new Customer(){ CustomerName = "Vieriu3 ", City = "Bucharest" }
           };

            DapperPlusManager.Entity<Customer>().Table("Customers");
        }

        public void UpdateOneToOne()
        {
            var suppliers = new List<Supplier>();

            // get List from DataBase
            var sql = @"Select TOP 1 A.SupplierID, A.SupplierName, B.ProductID, B.ProductName, B.SupplierID 
                        FROM Suppliers as A 
                        inner join Products as B 
                        on B.SupplierID =A.SupplierID 
                        where productID = 4";

            using (var connection = new SqlConnection(_connectionString))
            {
                var results = connection.Query<Supplier, Product, Supplier>(sql,
                            (supplier, product) =>
                            {
                                supplier.Product = product;
                                return supplier;
                            },
                            splitOn: "ProductID"
                        ).ToList();

                suppliers.AddRange(results);
            }

            // update values on classes 
            suppliers.ForEach(x => x.SupplierName = "BulkUpdate_Supplier");
            suppliers.ForEach(x => x.Product.ProductName = "BulkUpdate_Product");

            DapperPlusManager.Entity<Supplier>().Table("Suppliers").Identity(x => x.SupplierID);
            DapperPlusManager.Entity<Product>().Table("Customers").Identity(x => x.ProductID);

            using(var connection = new SqlConnection())
            {
                connection.BulkUpdate(suppliers, x => x.Product);
            }
        }

        public void UpdateOneToMany()
        {
            var suppliers = new List<Supplier>();
            var sql = @"Select A.SupplierID, A.SupplierName, B.ProductID, B.ProductName, B.Unit, B.SupplierID 
                        FROM Suppliers as A 
                        inner join Products as B 
                        on B.SupplierID = A.SupplierID 
                        where A.supplierID = 1";

            using (var connection = new SqlConnection(_connectionString))
            {
                var supplierDictionary = new Dictionary<int, Supplier>();

                suppliers = connection.Query<Supplier, Product, Supplier>(sql,
                (supplier, product) =>
                {
                    Supplier supplierEntry;

                    if (!supplierDictionary.TryGetValue(supplier.SupplierID, out supplierEntry))
                    {
                        supplierEntry = supplier;
                        supplierEntry.Products = new List<Product>();
                        supplierDictionary.Add(supplier.SupplierID, supplierEntry);
                    }

                    supplierEntry.Products.Add(product);
                    return supplierEntry;
                }, 
                splitOn: "ProductID")
                    .Distinct()
                    .ToList();
            }

            suppliers.ForEach(x =>
            {
                x.SupplierName = "Supplier_BulkUpdate";
                x.Products.ForEach(y => y.ProductName = "Product_BulkUpdate");
            });


            DapperPlusManager.Entity<Supplier>().Table("Suppliers").Identity(x => x.SupplierID);
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.ProductID);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.BulkUpdate(suppliers, x => x.Products);
            }            
        }
    }
}
