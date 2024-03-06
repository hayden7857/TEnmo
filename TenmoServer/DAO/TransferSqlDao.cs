using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Exceptions;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;
        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public Transfer CreateTransfer(Transfer transfer)
        {
            Transfer newTransfer = new Transfer();
            string sql = "INSERT INTO transfer (transfer_type_id, transfer_status_id,account_from,account_to,amount) " +
                "OUTPUT INSERTED.transfer_id " +
                "VALUES (2, 2,@account_from,@account_to,@amount)";
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // create user
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to",transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount",transfer.Amount);

                    newTransfer.TransferId= Convert.ToInt32(cmd.ExecuteScalar());
                    newTransfer.TransferStatusId = 2;
                    newTransfer.TransferTypeId = 2;
                    newTransfer.AccountFrom = transfer.AccountFrom;
                    newTransfer.AccountTo = transfer.AccountTo;
                    newTransfer.Amount = transfer.Amount;

                   
                }
                
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return newTransfer;

        }

        public List<Transfer> ListTransfer(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
