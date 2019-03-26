using System;
using SmartSchool.Core.Entities;

namespace SmartSchool.Core.Contracts
{
    public interface IMeasurementRepository
    {
        void AddRange(Measurement[] measurements);
        int GetCountMeasurementsForSensor(string location, string name);
        Measurement[] GetLastThreeHighestMeasurements(string location, string name);
        double GetValidCo2ValuesInOffice(string location, string name, int minValue, int maxValue);
        (Sensor, double)[] GetSensorsWithAvgMeasurements();
    }
}
