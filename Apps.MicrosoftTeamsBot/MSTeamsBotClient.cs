using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace Apps.MicrosoftTeamsBot
{
    public class MSTeamsBotClient : BlackBirdRestClient
    {
        public MSTeamsBotClient(string botServiceEndpoint) : base(new RestClientOptions() { BaseUrl = CreateUri(botServiceEndpoint) })
        {
        }

        private static Uri CreateUri(string uriString)
        {
            try
            {
                return new Uri(uriString);
            }
            catch (UriFormatException ex)
            {
                throw new PluginMisconfigurationException($"Invalid Bot Service URL format: {ex.Message}. Please check your input properties and try again");
            }
        }

        protected override Exception ConfigureErrorException(RestResponse response)
        {
            if (response.Content == null)
                return new PluginApplicationException("An error occurred, but no details were provided by the server.");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new PluginApplicationException("Authorization failed. Please check your credentials or ensure you have the necessary permissions.");
            }

            var errorMessage = $"Error: {response.Content} - Message: {response.ErrorMessage} - {response.ErrorException}";

            return new PluginApplicationException(errorMessage);
        }

        public override async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
        {
            var response = await ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
        {
            RestResponse restResponse = await ExecuteAsync(request);
            if (!restResponse.IsSuccessStatusCode)
            {
                throw ConfigureErrorException(restResponse);
            }

            return restResponse;
        }
    }
}
