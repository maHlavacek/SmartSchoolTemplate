using System;
using System.Threading.Tasks;
using SmartSchool.Core.Contracts;

namespace SmartSchool.Core.Contracts
{
    public interface IUnitOfWork: IDisposable
    {
 
        int SaveChanges();

        void DeleteDatabase();

        void MigrateDatabase();
    }
}
