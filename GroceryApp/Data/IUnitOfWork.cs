using System;
using System.Threading;
using System.Threading.Tasks;

namespace GroceryApp.Data
{
    public interface IUnitOfWork: IDisposable
    {
        ICustomerRepository Customers { get; }
        IUserRepository Users { get; }
        ICustomerTransactionRepository CustomerTransactions { get; }
        int Save();
        Task<int> SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
