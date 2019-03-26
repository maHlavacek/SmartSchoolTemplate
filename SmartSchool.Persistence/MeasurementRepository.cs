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
                .OrderByDescending(m => m.Value)
                .ThenByDescending(m => m.Time)
                .Where(s => s.Sensor.Location == location && s.Sensor.Name == name)                
                .Take(3)
                .ToArray();                
        }

        public double GetValidCo2ValuesInOffice(string location, string name, int minValue, int maxValue)
        {
            return _dbContext
                .Measurements
                .Include(sensor => sensor.Sensor)
                .Where(s => s.Sensor.Location == location
                && s.Sensor.Name == name
                && s.Value > minValue
                && s.Value < maxValue)
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
                        .Select(m => m.Value)
                        .Average(), 2)
                )).OrderBy(s => s.Item1.Location)
                .ThenBy(s => s.Item1.Name)
                .ToArray();
        }
    }
}