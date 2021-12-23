using System;

namespace WeatherApp.Models
{
    public interface IMeasurementUnits
    {
        string[] Units { get; }
        Func<double, bool, double>[] UnitConversions => null;

        double ConvertFromTo(double value, string from, string to)
        {
            if (UnitConversions == null) throw new NotImplementedException();
            int? toInd = null, fromInd = null;

            for (int i = 0; i < Units.Length; i++)
            {
                string item = Units[i];
                if (item == to) toInd = i;
                else if (item == from) fromInd = i;
                else continue;

                if (toInd != null && fromInd != null) break;
            }
            int delta = (int)(toInd - fromInd);
            if (delta == 0) return value;

            if (delta > 0)
            {
                for (int i = (int)fromInd; i < (int)toInd; i++)
                    value = UnitConversions[i](value, true);
            }
            else
            {
                for (int i = (int)toInd; i < (int)fromInd; i++)
                    value = UnitConversions[i](value, false);
            }
            return value;
        }
    }
}