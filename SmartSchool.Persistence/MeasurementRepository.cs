using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;

namespace SmartSchool.Persistence
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private ApplicationDbContext _dbContext;

        public MeasurementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddRange(Measurement[] measurements)
        {
            _dbContext.Measurements.AddRange(measurements);
        }

        public int GetCountMeasurementsForSensor(string location, string name)
        {
            return _dbContext
                .Sensors
                .Include(sensor => sensor.Measurements)
                .First(sensor =>
                    sensor.Location == location
                    && sensor.Name == name)
                .Measurements.Count();
        }

        public Measurement[] GetLastThreeHighestMeasurements(string location, string name)
        {
            return _dbContext
                .Measurements
                .Include(sensor => sensor.Sensor)
                .OrderByDescending(meas => meas.Value)
                .ThenByDescending(meas => meas.Time)
                .Where(sens => sens.Sensor.Location == location && sens.Sensor.Name == name)                
                .Take(3)
                .ToArray();                
        }

        public double GetValidCo2ValuesInOffice(string location, string name, int minValue, int maxValue)
        {
            return _dbContext
                .Measurements
                .Include(sensor => sensor.Sensor)
                .Where(sens => sens.Sensor.Location == location
                && sens.Sensor.Name == name
                && sens.Value > minValue
                && sens.Value < maxValue)
                .Average(avg => avg.Value);                
        }

        public (Sensor, double)[] GetSensorsWithAvgMeasurements()
        {            
            return _dbContext
                .Sensors
                .Select(sensor => ValueTuple.Create
                (
                    (Sensor)sensor,
                    Math.Round(sensor
                        .Measurements
                        .Select(measurement => measurement.Value)
                        .Average(), 2)
                )).OrderBy(sensor => sensor.Item1.Location)
                .ThenBy(sensor => sensor.Item1.Name)
                .ToArray();
        }
    }
}