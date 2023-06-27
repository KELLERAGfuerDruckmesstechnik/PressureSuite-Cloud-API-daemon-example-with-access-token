using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // Works better than using System.Text.Json because of reasons
using PermanentAccessTokenExample.ViewModels;

namespace PermanentAccessTokenExample
{
    class Program2
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string BaseUrl = "https://api.kolibricloud.ch";

        private const string PermanentAccessToken = "bqCrDAQABACUKUpXST6JCEgPeStCyeHhfmNz/+yE29F+VbVeKl7eU....  ASK kolibri@keller-druck.com for your permanent acces token";

        static async Task Main2(string[] args)
        {
            var deviceId = 2913;

            // Add the permanent access token to the header with the userOid key
            HttpClient.DefaultRequestHeaders.Add("userOid", PermanentAccessToken);
            
            HttpResponseMessage response = await HttpClient.GetAsync($"https://api.kolibricloud.ch/v1/Devices/{deviceId}");
            var responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseText + Environment.NewLine);

            //convert JSON to DeviceDetailsViewModel object
            var deviceDetails = JsonConvert.DeserializeObject<DeviceDetailsViewModel>(responseText);

            DateTime now = DateTime.UtcNow;
            
            // Get all data from the last 12h as CSV text
            // eg.  https://api.kolibricloud.ch/v1/Export?deviceIds=2913&from=2023-06-26T15%3A40%3A02.000Z&to=2023-06-27T15%3A40%3A02.000Z&measurementDefinitionIds=2&measurementDefinitionIds=5&measurementDefinitionIds=7&measurementDefinitionIds=8&measurementDefinitionIds=11&measurementDefinitionIds=36&exportFileType=0
            string requestUrl =  BaseUrl + $"/v1/Export?deviceIds={deviceId}&from={(now - TimeSpan.FromHours(12)):yyyy-MM-ddTHH:mm:sssZ}&to={now:yyyy-MM-ddTHH:mm:sssZ}";
            foreach (var measurementDefinition in deviceDetails.MeasurementDefinitions)
            {
                requestUrl += $"&measurementDefinitionIds={measurementDefinition.Id}";
            }
            requestUrl += $"&measurementDefinitionIds=36"; //With the Export endpoint it is possible to export calculated water level, too!!
            requestUrl += "&exportFileType=0";

            Task<string> request = GetResponseWithRequestUrlAsync(requestUrl);
            Console.WriteLine((DateTime.UtcNow - now).TotalMilliseconds + " ms passed");

            var csvText = request.Result;

            Console.WriteLine($"Device {deviceId} hast this measurement data: {Environment.NewLine}{csvText}");
        }

        static async Task<string> GetResponseWithRequestUrlAsync(string requestUrl)
        {
            HttpResponseMessage response = await HttpClient.GetAsync(requestUrl);
            string responseText;
            if (response.IsSuccessStatusCode)
            {
                responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            else
            {
                responseText = "Failed to retrieve the values from the API\nError:  {response.ReasonPhrase}\n";
            }
            return responseText;
        }
    }
}
