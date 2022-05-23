using DapperRipTutorial.Bulk_Insert_Update_Delete_Merge;

namespace DapperRipTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            var sql = "Server=(local); Database=Dapper; Trusted_Connection = True;";
            //var sql = "Server=apptestsrv; Database=AlexDapper; Trusted_Connection = True;";

            #region Learn Tutorial          

            //var asyncOperations = new AsyncOperations(sql);
            //asyncOperations.GetAllAuthorAsync();            // QueryAsync
            //asyncOperations.InsertSingleAuthorAsync();      // ExecuteAsync

            //var deleteData = new DeleteData(sql);
            //deleteData.DeleteSingleAuthor();                // Execute - single Obj
            //deleteData.DeleteMultipleAuthors();             // Execute - array of Obj

            //var dynamicObjects = new DynamicObjects(sql);
            //dynamicObjects.GetAuthors();                    // Query
            //dynamicObjects.GetDynamicAuthors();             // Query

            //var dynamicParameters = new DynamicParameters_(sql);
            //dynamicParameters.GetAuthorBooks();                     // QueryMultiple
            //dynamicParameters.GetAuthorBooks_DynamicParams();       // QueryMultiple
            //dynamicParameters.InsertSingleAuthor_DynamicParam();    // Execute

            //var inOperator = new IN_Operator(sql);
            //inOperator.GetAuthors();                                // Query

            //var mapColNameswithClassProp = new MapColNames_with_ClassProp(sql);
            //mapColNameswithClassProp.GetAuthors();                  // Query

            //var multiMapping = new MultiMapping(sql);               
            //multiMapping.GetAuthorBooks();                          // Query  

            //var updateData = new UpdateData(sql);
            //updateData.UpdateAuthor();
            //updateData.UpdateAuthors();

            #endregion

            #region Basic Querying

            //var basicQuerying = new BasicQuerying(sql);
            //basicQuerying.QueryForDynamicTypes();
            //basicQuerying.QueryForStaticType();
            //basicQuerying.QueryWithDynamicParams();

            #endregion

            #region Bulk Insert
            var bulkInsert = new BulkInsert_(sql);
            //bulkInsert.InsertSingle();
            //bulkInsert.InsertMany();
            bulkInsert.InsertOneToOne();


            #endregion

            #region Bulk Update

            #endregion

            #region Bulk Delete

            #endregion

            #region Dynamic Parameters

            #endregion

            Console.ReadLine();
        }
    }
}
