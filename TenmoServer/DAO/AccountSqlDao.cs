using System.Data;
using System.Data.SqlClient;
using TenmoServer.Exceptions;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountDao
    {
        private readonly string connectionString;


        public AccountSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public Account GetAccountById(int id)
        {
            string sql = "Select account_id,account.user_id,balance from account"+
                "join tenmo_user on tenmo_user.user_id = account.user_id where account.user_id = @user_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlGetPet, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", petId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pet = MapRowToPet(reader);
                            }
                        }
                    }
                }
                

            }catch(DaoException ex)
            {
                throw new DaoException("unable to retreive account", ex);
            }
            return pet;
        }
        public Account GetAccountByUsername(string username)
        {

        }
        public Account CreateAccount(User user)
        {

        }
        private Account MapRowToUser(S)
    }

}
