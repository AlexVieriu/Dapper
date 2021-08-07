using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DapperRipTutorial._DynamicParameters
{
    public class DynamicParameters_
    {
        private IDbConnection connetion;

        public DynamicParameters_(string connectionString)
        {
            connetion = new SqlConnection(connectionString);
        }

        public void DynamicParams()
        {
            var p = new DynamicParameters(new { a = 1, b = 2 });

            p.Add("c", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (connetion)
            {
                
            }
        }
    }
}
