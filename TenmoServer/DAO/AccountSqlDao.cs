using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
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
            Account account = new Account();
            string sql = "Select account_id,account.user_id,balance from account" +
                " join tenmo_user on tenmo_user.user_id = account.user_id where account.user_id = @user_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account = MapRowToAccount(reader);
                            }
                        }
                    }
                }


            }
            catch (DaoException ex)
            {
                throw new DaoException("unable to retreive account", ex);
            }
            return account;
        }
        public Account GetAccountByUsername(string username)
        {
            throw new NotImplementedException();
        }
        public Account CreateAccount(User user)
        {
            throw new NotImplementedException();
        }
        public Account UpdateSenderAccount(Transfer transfer,Account account)
        {
            string sql = "update account set balance=@balance where account_id=@account_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        account.Balance = account.Balance - transfer.Amount;
                        cmd.Parameters.AddWithValue("@balance", account.Balance);
                        cmd.Parameters.AddWithValue("@account_id",account.AccountId);
                        int count = cmd.ExecuteNonQuery();
                        if (count == 1)
                        {
                            return account;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }


            }
            catch (DaoException ex)
            {
                throw new DaoException("unable to complete transfer", ex);
            }

        }
        public Account UpdateReceiverAccount(Transfer transfer, Account account)
        {
            string sql = "update account set balance=@balance where account_id=@account_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        account.Balance = account.Balance + transfer.Amount;
                        cmd.Parameters.AddWithValue("@balance", account.Balance);
                        cmd.Parameters.AddWithValue("@account_id", account.AccountId);
                        int count = cmd.ExecuteNonQuery();
                        if (count == 1)
                        {
                            return account;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }


            }
            catch (DaoException ex)
            {
                throw new DaoException("unable to complete transfer", ex);
            }

        }
        private Account MapRowToAccount(SqlDataReader reader)
        {
            Account account = new Account();
            account.AccountId = Convert.ToInt32(reader["account_id"]);
            account.UserId = Convert.ToInt32(reader["user_id"]);
            account.Balance = Convert.ToDecimal(reader["balance"]);
            return account;
        }

    }

}
