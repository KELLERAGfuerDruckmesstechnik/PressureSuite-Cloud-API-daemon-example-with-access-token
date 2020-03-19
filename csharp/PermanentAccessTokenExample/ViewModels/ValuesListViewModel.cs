using System.Collections.Generic;

namespace PermanentAccessTokenExample.ViewModels
{
    public class ValuesListViewModel
    {
        public int DeviceId { get; set; }
        public IList<SimpleMeasurementPair> Values { get; set; }
        public int UnitId { get; set; }
    }
}