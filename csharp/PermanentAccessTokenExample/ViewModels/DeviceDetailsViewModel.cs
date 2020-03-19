using System;
using System.Collections.Generic;

namespace PermanentAccessTokenExample.ViewModels
{
    public class DeviceDetailsViewModel
    {
        public int DeviceId { get; set; }
        public string StationId { get; set; }
        public IList<MeasurementDefinitionViewModel> MeasurementDefinitions { get; set; }
        public string Note { get; set; }
        public int NumberOfUnconfirmedAlarms { get; set; }
        public DateTime? LastMeasurementTransmissionDateTime { get; set; }
        public int? SignalQuality { get; set; }
        public int? Humidity { get; set; }
        public float? BatteryInfoVoltageInVolt { get; set; }
        public int? BatteryInfoCapacityInPercent { get; set; }

        //Not supported via API. This is calculated by the front end client
        public TimeSpan? TransmissionInterval { get; set; }
        //Not supported via API. This is calculated by the front end client
        public TimeSpan? SaveInterval { get; set; }
    }
}