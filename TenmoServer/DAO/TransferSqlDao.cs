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

        public Transfer CreateTransferRequest(Transfer transfer)
        {
            Transfer newTransfer = new Transfer();
            string sql = "INSERT INTO transfer (transfer_type_id, transfer_status_id,account_from,account_to,amount) " +
                "OUTPUT INSERTED.transfer_id " +
                "VALUES (1, 1,@account_from,@account_to,@amount)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // create user
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    newTransfer.TransferId = Convert.ToInt32(cmd.ExecuteScalar());
                    newTransfer.TransferStatusId = 1;
                    newTransfer.TransferTypeId = 1;
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

        public IList<Transfer> ListCurrentUserTransfer(Account user)
        {
            IList<Transfer> transfers = new List<Transfer>();

            string SentTransferSql = "SELECT transfer_id, transfer.account_from, account_to, " +
                "transfer_status_id, transfer_type_id, amount FROM transfer " +
                "join account on account.account_id=transfer.account_from " +
                "where account_from = @account_from";


            string ReceivedTransferSql= "SELECT transfer_id, transfer.account_from, account_to, " +
                "transfer_status_id, transfer_type_id, amount FROM transfer " +
                "join account on account.account_id=transfer.account_to " +
                "where account_to = @account_to";
            //, password_hash, salt 
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    SqlCommand cmd = new SqlCommand(SentTransferSql, conn);
                    cmd.Parameters.AddWithValue("@account_from", user.AccountId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        Transfer transfer = MapRowToTransferGetTransfers(reader);
                        transfers.Add(transfer);
                    }
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(ReceivedTransferSql, conn);
                    cmd.Parameters.AddWithValue("@account_to", user.AccountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer transfer = MapRowToTransferGetTransfers(reader);
                        transfers.Add(transfer);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return transfers;

        }
        public Transfer GetTransferById(int id)
        {
            string sql = "  select * from transfer where transfer_id = @transfer_id";
            Transfer newTransfer = new Transfer();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // create user
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@transfer_id", id);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            newTransfer= MapRowToTransferGetTransfers(reader);
                        }
                    }

                }

            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return newTransfer;

        }

        public IList<Transfer> ListCurrentUserPendingTransfers(Account user)
        {
            IList<Transfer> transfers = new List<Transfer>();

            string SentTransferSql = "SELECT transfer_id, transfer.account_from, account_to, transfer_status_id, " +
                "transfer_type_id, amount FROM transfer join account on " +
                "account.account_id = transfer.account_from where account_from = @account_from " +
                "and transfer_status_id = 1";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SentTransferSql, conn);
                    cmd.Parameters.AddWithValue("@account_from", user.AccountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer transfer = MapRowToTransferGetTransfers(reader);
                        transfers.Add(transfer);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return transfers;

        }
        public Transfer UpdateTransfer(Transfer transfer)
        {
            string sql = "update transfer set transfer_status_id = @transfer_status_id where transfer_id = @transfer_id";
            Transfer updatedTransfer = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@transfer_id", transfer.TransferId);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new DaoException("No changes made in database");
                    }
                    updatedTransfer = GetTransferById(transfer.TransferId);
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return updatedTransfer;
        }
        public Transfer MapRowToTransferGetTransfers(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
            transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);
            return transfer;
        }
    }
}
