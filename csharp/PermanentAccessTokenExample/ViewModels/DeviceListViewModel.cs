using System.Collections.Generic;

namespace PermanentAccessTokenExample.ViewModels
{
    /// <summary>
    /// Use this with this:
    /// HttpResponseMessage response = await HttpClient.GetAsync("https://api.kolibricloud.ch/v1/Devices");
    /// jsonText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    /// var devices = JsonSerializer.Deserialize<DeviceListViewModel>(jsonText);
    /// </summary>
    public class DeviceListViewModel
    {
        public int TotalRecords { get; set; }
        public IList<DeviceViewModel> Devices { get; set; }
    }
}