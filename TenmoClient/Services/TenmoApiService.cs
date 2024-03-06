using RestSharp;
using System.Collections.Generic;
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
            IRestResponse<Account> responce = client.Get<Account>(request);
            CheckForError(responce, "get account");
            return responce.Data;
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
