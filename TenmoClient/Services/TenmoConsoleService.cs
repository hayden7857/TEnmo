using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoConsoleService : ConsoleService
    {
        /************************************************************
            Print methods
        ************************************************************/
        public void PrintLoginMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }

        public void PrintMainMenu(string username)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine($"Hello, {username}!");
            Console.WriteLine("1: View your current balance");
            Console.WriteLine("2: View your past transfers");
            Console.WriteLine("3: View your pending requests");
            Console.WriteLine("4: Send TE bucks");
            Console.WriteLine("5: Request TE bucks");
            Console.WriteLine("6: Log out");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }
        public LoginUser PromptForLogin()
        {
            string username = PromptForString("User name");
            if (String.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            string password = PromptForHiddenString("Password");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        // Add application-specific UI methods here...
        public void ShowAccountBalance(decimal balance)
        {
            Console.WriteLine($"Your current account balance is: ${balance}");
        }

        public void SendTEbucks(List<ApiUser> userList)
        {
            Console.WriteLine("| -------------- Users ------------- |\n"+
                              "| Id     | Username                  |\n"+
                              "| -------+---------------------------|");
            foreach (ApiUser user in userList)
            {
            Console.WriteLine($"| {user.UserId.ToString().PadRight(7)}| {user.Username.PadRight(26)}|");
            }
            Console.WriteLine("|------------------------------------|\n");
            
        }
        
        public void ViewPreviousTransfers(Dictionary<int, string[]> idToUsername,List<Transfer>transferList,int userId)
        {
            Console.WriteLine("-------------------------------------------" +
                "\nTransfers\n" +
                "ID          From/To                 Amount" +
                "\n-------------------------------------------\n");
            foreach (Transfer transfer in transferList)
            {
                Console.Write($"{transfer.TransferId.ToString().PadRight(12)}");
                if (transfer.AccountFrom == userId)
                {
                    Console.Write($"To: {idToUsername[transfer.TransferId][0].PadRight(18)}");
                }
                else
                {
                    Console.Write($"From: {idToUsername[transfer.TransferId][0].PadRight(16)}");
                }
                Console.Write($"${addTrailingZero(transfer.Amount.ToString()).PadLeft(8)}\n");
            }

            //foreach (KeyValuePair<int, string[]>idAndUsername in idToUsername)
            //{

            //    Console.Write($"{idAndUsername.Key.ToString().PadRight(12)}");
            //    if (idAndUsername.Key ==userId)
            //    {
            //        Console.Write($"From: {idAndUsername.Value[0].PadRight(23)}");
            //    }
            //    else
            //    {
            //        Console.Write($"To: {idAndUsername.Value[0].PadRight(23)}");
            //    }
            //    Console.Write($"${idAndUsername.Value[1].PadLeft(8)}\n");
            //}
        }
        public string addTrailingZero(string amount)
        {
            try
            {
                string[] amountArray = amount.Split('.');
                if (amountArray[1].Length == 1)
                {
                    amountArray[1] += "0";
                }
                return String.Join('.', amountArray);
            }
            catch (IndexOutOfRangeException)
            {
                return amount + ".00";
            }

        }
    }
}
