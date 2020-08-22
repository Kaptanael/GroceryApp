using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GroceryApp.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private ICustomerRepository _customers;
        public ICustomerRepository Customers
        {
            get
            {
                if (_customers == null)
                {
                    _customers = new CustomerRepository(_context);
                }
                return _customers;
            }
        }

        private IUserRepository _users;
        public IUserRepository Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new UserRepository(_context);
                }
                return _users;
            }
        }

        private ICustomerTransactionRepository _customerTransaction;
        public ICustomerTransactionRepository CustomerTransactions
        {
            get
            {
                if (_customerTransaction == null)
                {
                    _customerTransaction = new CustomerTransactionRepository(_context);
                }
                return _customerTransaction;
            }
        }

        public int Save()
        {
            int result;
            try
            {
                result = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

            return result;
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            int result;
            try
            {
                result = await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
