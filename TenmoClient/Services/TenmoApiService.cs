using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        public readonly string ApiUrl;

        public TenmoApiService(string apiUrl) : base(apiUrl) { }

        // Add methods to call api here...
        public Account GetAccount()
        {
            RestRequest request = new RestRequest("/account");
            IRestResponse<Account> response = client.Get<Account>(request);
            CheckForError(response, "get account");
            return response.Data;
        }
        public List<ApiUser> GetUsers()
        {
            RestRequest request = new RestRequest("/user");
            IRestResponse<List<ApiUser>> response = client.Get<List<ApiUser>>(request);
            CheckForError(response, "Get users");
            return response.Data;
        }
        public ApiUser GetUserById(int id)
        {
            RestRequest request = new RestRequest($"/user/{id}");
            IRestResponse<ApiUser> response = client.Get<ApiUser>(request);
            CheckForError(response, "Get user by Id");
            return response.Data;
        }
        public Transfer PostTransferOut(Transfer transfer)
        {
            RestRequest request = new RestRequest("/transfer");
            request.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            CheckForError(response, "Post transfer out incomplete");
            return (response.Data);
        }
        
        public List<Transfer> GetPreviousTransfers()
        {
            RestRequest request = new RestRequest("/transfer");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            CheckForError(response, "Get users");
            return response.Data;
        }
        public Transfer GetTransferById(int id)
        {
            RestRequest request = new RestRequest($"/transfer/{id}");
            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            CheckForError(response, "Get Transfer by Id");
            return response.Data;
        }
        private void CheckForError(IRestResponse response, string action)
        {

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                // TODO: Write a log message for future reference

                throw new HttpRequestException($"There was an error communicating with the server. Action: " + action);
            }
            else if (!response.IsSuccessful)
            {
                // TODO: Write a log message for future reference

                throw new HttpRequestException($"There was an error in the call to the server Action: " + action);
            }

            return;

        }
    }
}
