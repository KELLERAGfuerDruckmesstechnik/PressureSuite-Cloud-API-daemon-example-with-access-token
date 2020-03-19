using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // Works better than using System.Text.Json because of reasons
using PermanentAccessTokenExample.ViewModels;

namespace PermanentAccessTokenExample
{
    class Program
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string BaseUrl = "https://api.kolibricloud.ch";

        private const string PermanentAccessToken = "bqCrDAQABACUKUpXST6JCEgPeStCyeHhfmNz/+yE29F+VbVeKl7eU....  ASK kolibri@keller-druck.com for your permanent acces token";

        static async Task Main(string[] args)
        {
            // Add the permanent access token to the header with the userOid key
            HttpClient.DefaultRequestHeaders.Add("userOid", PermanentAccessToken);
            // With WebClient it would be   WebClient.Headers.Add("userOid", PermanentAccessToken);

            // Simple Example: Get a list of all devices the token gives access to
            HttpResponseMessage response = await HttpClient.GetAsync("https://api.kolibricloud.ch/v1/Devices");
            Console.WriteLine(await response.Content.ReadAsStringAsync().ConfigureAwait(false) + Environment.NewLine);

            // Now, lets get the measurement data of the last 48 hours from a certain device for a certain "measurementDefinitionId"
            DateTime now = DateTime.UtcNow;
            // Get the measurement data of measurementDefId 2 from the last 48 hours from device 1831
            // eg.  https://api.kolibricloud.ch/v1/Measurements?measurementDefinitionId=2&deviceId=1831&start=2020-03-04T13%3A34%3A40.499Z&end=2020-03-04T13%3A34%3A40.499Z
            string requestUrl =  BaseUrl + $"/v1/Measurements?measurementDefinitionId={2}&deviceId={1831}&start={(now - TimeSpan.FromHours(48)):yyyy-MM-ddTHH:mm:sssZ}&end={now:yyyy-MM-ddTHH:mm:sssZ}";
            Task<string> request = GetResponseWithRequestUrlAsync(requestUrl);
            // It is also possible to not use UTC but get the timestamps in a certain timezone: GetResponseWithRequestUrlAsync(requestUrl+ "&ianaTimeZone=Europe/Stockholm");

            // Run the task and request the data
            // Hint: Run multiple Tasks in parallel like this: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/start-multiple-async-tasks-and-process-them-as-they-complete
            var jsonText = request.Result;
            var measurementsOfDevice1831 = JsonConvert.DeserializeObject<MeasurementValuesListViewModel>(jsonText);
            Console.WriteLine($"Device {measurementsOfDevice1831.DeviceId} has {measurementsOfDevice1831.Values.Count} measurements");

            // Get device information
            var requestUrlForDeviceDetails = BaseUrl + $"/v1/Devices/{1831}";
            var deviceDetailsJsonText = GetResponseWithRequestUrlAsync(requestUrlForDeviceDetails).Result;
            var deviceDetailsOfDevice1831 = JsonConvert.DeserializeObject<DeviceDetailsViewModel>(deviceDetailsJsonText);
            Console.WriteLine($"Device {deviceDetailsOfDevice1831.DeviceId} has a guessed {deviceDetailsOfDevice1831.BatteryInfoCapacityInPercent} % battery capacity and the following measurementsDefinitionIds: {string.Join(",", deviceDetailsOfDevice1831.MeasurementDefinitions.Select(_=>_.Name).ToArray())}");

            Console.WriteLine((DateTime.UtcNow-now).TotalMilliseconds+" ms passed");
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
