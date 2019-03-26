using System;
using System.Linq;
using SmartSchool.Core.Entities;
using SmartSchool.Core.Contracts;

namespace SmartSchool.Persistence
{
    public class SensorRepository : ISensorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SensorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
    }
}