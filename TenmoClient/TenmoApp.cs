﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using TenmoClient.Models;
using TenmoClient.Services;

namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;

        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {
                // View your current balance
                GetAccountBalance();
            }

            if (menuSelection == 2)
            {
                // View your past transfers
                ViewPreviousTransfers();
                GetPreviousTransferById();
            }

            if (menuSelection == 3)
            {
                // View your pending requests
                ViewPendingTransfers();
                DecisionPendingTransfers();
            }

            if (menuSelection == 4)
            {
                // Send TE bucks
                TransferOut();
            }

            if (menuSelection == 5)
            {
                // Request TE bucks
                CreateRequest();

            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }

        public void GetAccountBalance()
        {
            Account account = tenmoApiService.GetAccount();
            console.ShowAccountBalance(account.Balance);
            console.Pause();
        }
        public List<ApiUser> GetApiUsers()
        {
            ApiUser currentUser = null;
            List<ApiUser> userList = tenmoApiService.GetUsers();
            foreach (ApiUser user in userList)
            {
                if (user.Username == tenmoApiService.Username)
                {
                    userList.Remove(user);
                    currentUser = user;
                    break;
                }
            }
            return userList;
        }
        public void TransferOut()
        {
            List<ApiUser> userList = GetApiUsers();
            console.SendTEbucks(userList);
            int enteredUserId = console.PromptForInteger("Enter User Id for receicpient. Cannot be your account 0 to cancle");
            if (enteredUserId == 0)
            {
                return;
            }
            decimal enteredAmount = console.PromptForDecimal("Enter amount to send. Must be greater than 0 and less than current account balance");
            Transfer newTransfer = new Transfer();
            newTransfer.AccountFrom = tenmoApiService.UserId;
            newTransfer.AccountTo = enteredUserId;
            newTransfer.Amount = enteredAmount;
            newTransfer.TransferStatusId = 2;
            newTransfer.TransferTypeId = 2;
            try
            {
                tenmoApiService.PostTransferOut(newTransfer);
                console.Pause("Transfer Complete\nPress any key to continue");


            }
            catch (HttpRequestException ex)
            {
                console.Pause("Unable to complete transfer.");
            }

        }
        public void ViewPreviousTransfers()
        {
            List<Transfer> transferList = tenmoApiService.GetPreviousTransfers();
            Dictionary<int, string[]> idToUsername = new Dictionary<int, string[]>();
            foreach (Transfer transfer in transferList)
            {
                if (transfer.AccountFrom == tenmoApiService.UserId)
                {
                    idToUsername[transfer.TransferId] = new string[] { tenmoApiService.GetUserById(transfer.AccountTo).Username, transfer.Amount.ToString() };

                }
                else
                {
                    idToUsername[transfer.TransferId] = new string[] { tenmoApiService.GetUserById(transfer.AccountFrom).Username, transfer.Amount.ToString() };

                }
            }
            console.ViewPreviousTransfers(idToUsername, transferList, tenmoApiService.UserId);
        }
        public void GetPreviousTransferById()
        {
            int transferId = console.PromptForInteger("Please enter transfer ID to view details (0 to cancel)");
            if (transferId == 0)
            {
                return;
            }
            Transfer retreivedTransfer = tenmoApiService.GetTransferById(transferId);
            List<string> transferDetails = new List<string>();
            transferDetails.Add(retreivedTransfer.TransferId.ToString());

            transferDetails.Add(tenmoApiService.GetUserById(retreivedTransfer.AccountFrom).Username);
            transferDetails.Add(tenmoApiService.GetUserById(retreivedTransfer.AccountTo).Username);
            switch (retreivedTransfer.TransferTypeId)
            {
                case 1:
                    transferDetails.Add("Request");
                    break;
                case 2:
                    transferDetails.Add("Send");
                    break;

            }
            switch (retreivedTransfer.TransferStatusId)
            {
                case 1:
                    transferDetails.Add("Pending");
                    break;
                case 2:
                    transferDetails.Add("Approved");
                    break;
                case 3:
                    transferDetails.Add("Rejected");
                    break;
            }
            transferDetails.Add(retreivedTransfer.Amount.ToString());
            console.ViewTransferById(transferDetails);
            console.Pause();
        }
        public void CreateRequest()
        {
            List<ApiUser> userList = GetApiUsers();
            console.SendTEbucks(userList);
            int userIdForRequest = console.PromptForInteger("Id of the user you are requesting from[0 to cancel]");
            if (userIdForRequest == 0)
            {
                return;
            }
            decimal requestAmount = console.PromptForDecimal("Enter amount to request");
            Transfer newTransfer = new Transfer();
            newTransfer.AccountFrom = userIdForRequest;
            newTransfer.AccountTo = tenmoApiService.UserId;
            newTransfer.Amount = requestAmount;
            newTransfer.TransferStatusId = 1;
            newTransfer.TransferTypeId = 1;
            tenmoApiService.PostTransferOut(newTransfer);
            console.Pause("Request sent.");
        }
        public void ViewPendingTransfers()
        {
            List<Transfer> pendingTransferList = tenmoApiService.GetPendingTransfers();

            Dictionary<int, string[]> idToUsername = new Dictionary<int, string[]>();
            foreach (Transfer transfer in pendingTransferList)
            {
                idToUsername[transfer.TransferId] = new string[] { tenmoApiService.GetUserById(transfer.AccountTo).Username, transfer.Amount.ToString() };

            }
            console.ViewPendingTransfersOut(pendingTransferList, idToUsername);
        }
        public void DecisionPendingTransfers()
        {
            int transferId = console.PromptForInteger("Please enter transfer ID to approve/reject (0 to cancel)");
            if (transferId == 0)
            {
                return;
            }
            console.ViewPendingOptions();
            int pendingOption = console.PromptForInteger("Please choose an option");
            switch (pendingOption)
            {
                case 1:
                    Transfer approvedTransfer = tenmoApiService.GetTransferById(transferId);
                    if (tenmoApiService.GetAccount().Balance >= approvedTransfer.Amount)
                    {
                        approvedTransfer.TransferStatusId = 2;
                    }
                    else
                    {
                        Console.WriteLine("Unable to approve transfer");
                        console.Pause();
                        return;
                    }

                    try
                    {
                        tenmoApiService.UpdateTranferApproved(approvedTransfer);
                        console.Pause("Transfer approved.");
                    }catch(HttpRequestException ex)
                    {
                        Console.WriteLine("Unable to approve transfer");
                        console.Pause();
                    }

                    break;
                case 2:
                    Transfer rejectedTransfer = tenmoApiService.GetTransferById(transferId);
                    rejectedTransfer.TransferStatusId = 3;
                    tenmoApiService.UpdateTranferRejected(rejectedTransfer);
                    console.Pause("Transfer rejected");
                    break;
                case 0:
                    console.Pause("Returning to main menu.");
                    break;
            }
        }
    }

}
