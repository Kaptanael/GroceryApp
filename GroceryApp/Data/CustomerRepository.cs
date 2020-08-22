using GroceryApp.Common;
using GroceryApp.Extensions;
using GroceryApp.Models;
using GroceryApp.Models.AuxiliaryModels;
using GroceryApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.Data
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext context) : base(context)
        {
        }
        public ApplicationDbContext ApplicationDbContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public async Task<List<CustomerNameViewModel>> GetAllCustomerFullNameAsync()
        {
            var customers = ApplicationDbContext.Customers.AsNoTracking().Select(c => new CustomerNameViewModel
            {
                FullName = c.FirstName + " " + c.LastName,
                CustomerId = c.CustomerId
            }).ToListAsync();

            return await customers;
        }

        public async Task<List<CustomerNameViewModel>> GetAllActiveCustomerFullNameAsync()
        {
            var customers = ApplicationDbContext.Customers.AsNoTracking().Where(c => c.IsActive == true).Select(c => new CustomerNameViewModel
            {
                FullName = c.FirstName + " " + c.LastName,
                CustomerId = c.CustomerId
            }).ToListAsync();

            return await customers;
        }

        public async Task<DataResult<List<Customer>>> GetAllCustomerAsync(int pageNumber, int pageSize, string sortOrder, string firstName, string lastName, string fullName, string mobile)
        {
            IQueryable<Customer> customers = ApplicationDbContext.Customers.AsNoTracking();

            if (!string.IsNullOrEmpty(firstName))
            {
                customers = customers.Where(c => c.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                customers = customers.Where(c => c.LastName.Contains(lastName));
            }
            if (!string.IsNullOrEmpty(fullName))
            {
                customers = customers.Where(c => c.FullName.Contains(fullName));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                customers = customers.Where(c => c.Mobile.Contains(mobile));
            }

            int count = await customers.CountAsync();
            customers = customers.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            switch (sortOrder)
            {
                case "firstname_desc":
                    customers = customers = customers.OrderByDescending(c => c.FirstName);
                    break;
                case "LastName":
                    customers = customers = customers.OrderBy(c => c.LastName);
                    break;
                case "lastname_desc":
                    customers = customers = customers.OrderByDescending(c => c.LastName);
                    break;
                case "fullname_desc":
                    customers = customers = customers.OrderByDescending(c => c.LastName);
                    break;
                case "Mobile":
                    customers = customers = customers.OrderBy(c => c.Mobile);
                    break;
                case "mobile_desc":
                    customers = customers = customers.OrderByDescending(c => c.Mobile);
                    break;
                default:
                    customers = customers = customers.OrderBy(s => s.FirstName);
                    break;
            }

            var filteredCustomers = await customers.ToListAsync();

            return new DataResult<List<Customer>>(filteredCustomers, count);
        }

        public async Task<DataResult<List<Customer>>> GetAllCustomerAsync(int start, int length, string sortColumnName, string sortColumnDirection, string firstName, string lastName, string fullName, string mobile)
        {
            int coustomersFilteredCount = 0;
            int coustomersTotalCount = 0;

            IQueryable<Customer> customersFilteredData = ApplicationDbContext.Customers.AsNoTracking();

            if (!string.IsNullOrEmpty(firstName))
            {
                customersFilteredData = customersFilteredData.Where(c => c.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                customersFilteredData = customersFilteredData.Where(c => c.LastName.Contains(lastName));
            }
            if (!string.IsNullOrEmpty(fullName))
            {
                customersFilteredData = customersFilteredData.Where(c => c.FullName.Contains(fullName));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                customersFilteredData = customersFilteredData.Where(c => c.Mobile.Contains(mobile));
            }

            coustomersFilteredCount = await customersFilteredData.AsNoTracking().CountAsync();
            customersFilteredData = customersFilteredData.Skip(start).Take(length);

            if (coustomersFilteredCount > 0)
            {
                coustomersTotalCount = coustomersFilteredCount;
            }
            else
            {
                coustomersTotalCount = await ApplicationDbContext.Customers.AsNoTracking().CountAsync();
            }

            if (!string.IsNullOrEmpty(sortColumnDirection) && !string.IsNullOrEmpty(sortColumnName))
            {
                customersFilteredData = (!string.IsNullOrEmpty(sortColumnDirection) && sortColumnDirection == "asc") ? customersFilteredData.OrderByDynamic(sortColumnName, DtOrderDir.Asc) : customersFilteredData.OrderByDynamic(sortColumnName, DtOrderDir.Desc).AsQueryable<Customer>();
            }

            return new DataResult<List<Customer>>(await customersFilteredData.ToListAsync(), coustomersTotalCount, coustomersFilteredCount);
        }

        public async Task<DataResult<List<Customer>>> GetAllCustomerAsync(int start, int length, string sortColumnName, bool orderAscendingDirection, string firstName, string lastName, string fullName, string mobile)
        {
            int coustomersFilteredCount = 0;
            int coustomersTotalCount = 0;

            IQueryable<Customer> customersFilteredData = ApplicationDbContext.Customers.AsNoTracking();

            if (!string.IsNullOrEmpty(firstName))
            {
                customersFilteredData = customersFilteredData.Where(c => c.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                customersFilteredData = customersFilteredData.Where(c => c.LastName.Contains(lastName));
            }
            if (!string.IsNullOrEmpty(fullName))
            {
                customersFilteredData = customersFilteredData.Where(c => c.FullName.Contains(fullName));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                customersFilteredData = customersFilteredData.Where(c => c.Mobile.Contains(mobile));
            }

            coustomersFilteredCount = await customersFilteredData.AsNoTracking().CountAsync();
            customersFilteredData = customersFilteredData.Skip(start).Take(length);

            if (coustomersFilteredCount > 0)
            {
                coustomersTotalCount = coustomersFilteredCount;
            }
            else
            {
                coustomersTotalCount = await ApplicationDbContext.Customers.AsNoTracking().CountAsync();
            }

            if (!string.IsNullOrEmpty(sortColumnName))
            {
                customersFilteredData = orderAscendingDirection ? customersFilteredData.OrderByDynamic(sortColumnName, DtOrderDir.Asc) : customersFilteredData.OrderByDynamic(sortColumnName, DtOrderDir.Desc).AsQueryable<Customer>();
            }

            return new DataResult<List<Customer>>(await customersFilteredData.ToListAsync(), coustomersTotalCount, coustomersFilteredCount);
        }

        public async Task<List<Customer>> GetAllCustomerByFullNameAsync(string name)
        {
            var customers = ApplicationDbContext.Customers.AsNoTracking()
             .Where(c => c.FullName.ToLower().StartsWith(name.ToLower()));

            return await customers.ToListAsync();
        }

        public async Task<List<Customer>> GetAllCustomerByFirstNameAsync(string name)
        {
            var customers = ApplicationDbContext.Customers.AsNoTracking()
             .Where(c => c.FirstName.ToLower().StartsWith(name.ToLower()));

            return await customers.ToListAsync();
        }

        public async Task<List<Customer>> GetAllCustomerByLastNameAsync(string name)
        {
            var customers = ApplicationDbContext.Customers.AsNoTracking()
             .Where(c => c.LastName.ToLower().StartsWith(name.ToLower()));

            return await customers.ToListAsync();
        }

        public async Task<List<Customer>> GetAllCustomerByMobileAsync(string mobile)
        {
            var customers = ApplicationDbContext.Customers.AsNoTracking()
             .Where(c => c.Mobile.ToLower().StartsWith(mobile.ToLower()));

            return await customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByMobileAsync(string mobile, int customerId)
        {
            Customer customer;

            if (customerId == 0)
            {
                customer = await ApplicationDbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Mobile.ToLower() == mobile.ToLower());
            }
            else
            {
                customer = await ApplicationDbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Mobile.ToLower() == mobile.ToLower() && c.CustomerId != customerId);
            }

            return customer;
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email, int customerId)
        {
            Customer customer;
            if (customerId == 0)
            {
                customer = await ApplicationDbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
            }
            else
            {
                customer = await ApplicationDbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower() && c.CustomerId != customerId);
            }

            return customer;
        }
    }
}
