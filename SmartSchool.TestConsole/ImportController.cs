using System;
using System.Collections.Generic;
using System.Linq;
using SmartSchool.Core.Entities;
using Utils;

namespace SmartSchool.TestConsole
{
    public class ImportController
    {
        const string Filename = "measurements.csv";

        /// <summary>
        /// Liefert die Messwerte mit den dazugehörigen Sensoren
        /// </summary>
        public static Measurement[] ReadFromCsv()
        {
            string[][] matrix = MyFile.ReadStringMatrixFromCsv(Filename, true);
            var sensors = matrix.GroupBy(line => line[2])
                .Select(sensorGroup =>
                {
                    var parts = sensorGroup.Key.Split('_');
                    return new Sensor
                    {
                        Name = parts[1],
                        Location = parts[0]
                    };
                }
                ).ToArray();

            var measurements = matrix
                .Select(line => new Measurement
                {
                    Time = DateTime.Parse($"{line[0]} {line[1]}"),
                    Sensor = sensors.Single(sensor => line[2] == $"{sensor.Location}_{sensor.Name}"),
                    Value = double.Parse(line[3])
                }
                ).ToArray();

            return measurements;
        }
    }
}
