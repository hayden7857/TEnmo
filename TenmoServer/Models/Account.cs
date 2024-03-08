namespace TenmoServer.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }

        //made for tests
        public Account (int accountId, int userId, decimal balance)
        {
            AccountId = accountId;
            UserId = userId;
            Balance = balance;
        }
        public Account() { }
    }
}
