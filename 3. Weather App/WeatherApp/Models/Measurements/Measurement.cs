using System;

namespace WeatherApp.Models
{
    public class Measurement
    {
        public (double, string) InitalMeasurment;
        public IMeasurementUnits MeasurementUnit { get; protected set; }

        public string Display => GetDisplay();
        public double Value { get; private set; }
        public string Unit { get; private set; }

        public Measurement(double value, string unit)
        {
            this.InitalMeasurment = (value, unit);
            this.Value = value;
            this.Unit = unit;
        }

        public void ConvertTo(string desiredUnit)
        {
            if (desiredUnit == InitalMeasurment.Item2)
            {
                ResetToInitial();
                return;
            }

            Value = MeasurementUnit.ConvertFromTo(Value, Unit, desiredUnit);
            Unit = desiredUnit;
        }

        public void ResetToInitial()
        {
            this.Value = InitalMeasurment.Item1;
            this.Unit = InitalMeasurment.Item2;
        }

        public string GetDisplay()
        {
            return string.Format("{0} {1}", Math.Round(Value, 2), Unit);
        }
    }
}