using DapperTutorial.Dapper.Utilities;

namespace DapperTutorial;

class Program
{
    static void Main(string[] args)
    {
        // Rewrite the ballshit implementation
        // https://dapper-tutorial.net/execute

        var connectionString = "Server= apptestsrv; DataBase = AlexDapper; Trusted_Connection = True";

        #region Execute
        //var execute = new Execute_(stringConnection);

        //execute.SingleProcedure("sp_Customer_Insert");
        //execute.ManyProcedure("sp_Customer_Insert");

        //var sqlInsert = "Insert Into Customer(FirstName) Values(@FirstName)";
        //execute.SingleInsert(sqlInsert);            
        //execute.ManyInsert(sqlInsert);

        //var sqlUpdate = "Update Customers Set FirstName = @FirstName where CustomerId = @CustomerId";
        //execute.SingleUpdate(sqlUpdate);
        //execute.ManyUpdate(sqlUpdate);

        //var sqlDelete = "Delete Customers where CustomerId = @CustomerId";
        //execute.SingleDelete(sqlDelete);
        //execute.ManyDelete(sqlDelete);
        #endregion

        #region ExecuteReader (not working yet)

        //var executeReader = new ExecuteReader_(stringConnection);

        //var sqlReaderSelect = "Select * from Customers";
        //executeReader.Command(sqlReaderSelect);
        //executeReader.Procedure("sp_SelectCustomerById");

        #endregion

        #region Query, QueryFirst, QueryFirstOrDefault, QuerySingle, QuerySingleOrDefault, QueryMultiple

        //var sql = "Select * from Customers";
        //var sqlMulti = "select * from Customers as C inner join CustomerAddress as CA on c.CustomerID = ca.CustomerId";

        //var query = new Query_(stringConnection);

        //query.Anonymous(sql);
        //query.MultiMapping_OneToOne(sqlMulti);
        //query.MultiMapping_OneToMany(sqlMulti);
        // query.MultiType(sql);

        #endregion

        #region Parameters: Anonymous
        //var sql = "Insert Into Customers(FirstName) Values(@FirstName)";

        //var parameters = new Param_Anonymous(connectionString);

        //parameters.Single(sql);
        //parameters.Many(sql);

        #endregion

        #region Parameters: Dynamic, List, String, Table-Value Parameter
        //var sql = "sp_Customer_Insert";
        //var sqlSelect = "sp_SelectCustomers";
        //var param = new Param_Dynamic(connectionString);
        //param.Single(sql);
        //param.Many(sql);
        //param.List(sqlSelect);
        //param.TableValueParam_AtFirstExecution();
        //param.TableValueParam();

        #endregion

        #region Result: Anonymous, Strongly Typed, Multi-Mapping, Multi-Result, Multi-Type
        //var result = new Result_Anonymous(connectionString);

        //result.Query();
        //result.QueryFirst();
        //result.QueryFirstOrDefault();
        //result.QuerySingle();
        //result.QuerySingleOrDefault();

        #endregion

        #region Utilities: Async, Transation, Store Procedure
        //var async = new Async_(connectionString);
        //async.ExecuteAsync();
        //async.QueryAsync();
        //async.QuerySingleOrDefaultAsync();

        //var transation = new Transaction_(connectionString);
        //transation.Transation();

        var procedure = new StoreProcedure_(connectionString);
        //procedure.ExecuteSingle();
        procedure.ExecuteMany();
        //procedure.QueryMany();  
        
        #endregion

        Console.ReadLine();
    }
}
