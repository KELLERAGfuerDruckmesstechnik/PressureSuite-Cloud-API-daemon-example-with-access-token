using System.Collections.Generic;

namespace PermanentAccessTokenExample.ViewModels
{
    public class SimpleMeasurementPairEqualityComparer : IEqualityComparer<SimpleMeasurementPair>
    {
        public bool Equals(SimpleMeasurementPair x, SimpleMeasurementPair y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            bool isTheSame = x.Time.Equals(y.Time) && x.Value.Equals(y.Value);
            return isTheSame;
        }

        public int GetHashCode(SimpleMeasurementPair obj)
        {
            unchecked
            {
                int hashCode = obj.Time.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Value.GetHashCode();
                return hashCode;
            }
        }
    }
}