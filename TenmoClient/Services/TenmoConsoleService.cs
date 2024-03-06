﻿using System;
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
            Console.WriteLine("| -------------- Users ------------- |"+
                              "| Id     | Username                  |"+
                              "| -------+---------------------------|");
            foreach (ApiUser user in userList)
            {

            Console.WriteLine($"| {user.UserId.ToString().PadRight(7)}| {user.Username.PadRight(27)}|");
            }
            Console.WriteLine("|-----------------------------------|");
        }

    }
}
