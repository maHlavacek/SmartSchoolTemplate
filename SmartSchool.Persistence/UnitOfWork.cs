using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SmartSchool.Core.Contracts;
using Utils;

namespace SmartSchool.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private bool _disposed;

        public UnitOfWork()
        {
            _dbContext = new ApplicationDbContext();
            MeasurementRepository = new MeasurementRepository(_dbContext);
            SensorRepository = new SensorRepository(_dbContext);

            MyLogger.InitializeLogger();
            Log.Information("Start Logging ...");

            var serviceProvider = _dbContext.GetInfrastructure();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddSerilog();
        }

        public IMeasurementRepository MeasurementRepository { get; set; }

        public ISensorRepository SensorRepository { get; set; }

        /// <summary>
        /// Repository-übergreifendes Speichern der Änderungen
        /// </summary>
        public int SaveChanges() => _dbContext.SaveChanges();
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }
        public void DeleteDatabase() => _dbContext.Database.EnsureDeleted();
        public void MigrateDatabase() => _dbContext.Database.Migrate();
    }
}
