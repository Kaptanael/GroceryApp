using Microsoft.EntityFrameworkCore;
using GroceryApp.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryApp.ViewModels;
using System;
using GroceryApp.Common;
using GroceryApp.Extensions;
using GroceryApp.Models.AuxiliaryModels;
using System.Globalization;

namespace GroceryApp.Data
{
    public class CustomerTransactionRepository : Repository<CustomerTransaction>, ICustomerTransactionRepository
    {
        public CustomerTransactionRepository(DbContext context) : base(context)
        {
        }
        public ApplicationDbContext ApplicationDbContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public async Task<DataResult<List<CustomerTransactionForListViewModel>>> GetAllCustomerTransactionAsync(int pageNumber, int pageSize, string sortOrder, string firstName, string lastName, string fullName, string mobile, DateTime? fromDate, DateTime? toDate)
        {
            var customerTransactions = ApplicationDbContext.CustomerTransactions
                .Include(t => t.Customer)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(firstName))
            {
                customerTransactions = customerTransactions.Where(ct => ct.Customer.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                customerTransactions = customerTransactions.Where(ct => ct.Customer.LastName.Contains(lastName));
            }
            if (!string.IsNullOrEmpty(fullName))
            {
                customerTransactions = customerTransactions.Where(ct => ct.Customer.FullName.Contains(fullName));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                customerTransactions = customerTransactions.Where(ct => ct.Customer.Mobile.Contains(mobile));
            }
            if (fromDate != null && toDate != null)
            {
                customerTransactions = customerTransactions.Where(ct => ct.TransactionDate >= fromDate && ct.TransactionDate <= toDate);
            }

            int count = await customerTransactions.CountAsync();
            customerTransactions = customerTransactions.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            switch (sortOrder)
            {
                case "firstname_desc":
                    customerTransactions = customerTransactions.OrderByDescending(ct => ct.Customer.FirstName);
                    break;
                case "LastName":
                    customerTransactions = customerTransactions.OrderBy(ct => ct.Customer.LastName);
                    break;
                case "lastname_desc":
                    customerTransactions = customerTransactions.OrderByDescending(ct => ct.Customer.LastName);
                    break;
                case "fullname_desc":
                    customerTransactions = customerTransactions.OrderByDescending(ct => ct.Customer.FullName);
                    break;
                case "Mobile":
                    customerTransactions = customerTransactions.OrderBy(ct => ct.Customer.Mobile);
                    break;
                case "mobile_desc":
                    customerTransactions = customerTransactions.OrderByDescending(ct => ct.Customer.Mobile);
                    break;
                case "Date":
                    customerTransactions = customerTransactions.OrderBy(ct => ct.TransactionDate);
                    break;
                case "date_desc":
                    customerTransactions = customerTransactions.OrderByDescending(ct => ct.TransactionDate);
                    break;
                default:
                    customerTransactions = customerTransactions.OrderBy(ct => ct.Customer.FirstName);
                    break;
            }

            var filteredCustomerTransactions = await customerTransactions.Select(ct => new CustomerTransactionForListViewModel
            {
                CustomerId = ct.CustomerId,
                FirstName = ct.Customer.FirstName,
                LastName = ct.Customer.LastName,
                FullName = ct.Customer.FullName,
                Email = ct.Customer.Email,
                Mobile = ct.Customer.Mobile,
                TransactionId = ct.CustomerTransactionId,
                SoldAmount = ct.SoldAmount,
                ReceivedAmount = ct.ReceivedAmount,
                TransactionDate = ct.TransactionDate
            }).ToListAsync();

            return new DataResult<List<CustomerTransactionForListViewModel>>(filteredCustomerTransactions, count);
        }

        public async Task<DataResult<List<CustomerTransactionForListViewModel>>> GetAllCustomerTransactionAsync(int start, int length, string sortColumnName, bool orderAscendingDirection, string firstName, string lastName, string fullName, string mobile, DateTime? fromDate, DateTime? toDate)
        {
            int coustomerTransactionsFilteredCount = 0;
            int coustomerTransactionsTotalCount = 0;

            var customerTransactionsFilterData = ApplicationDbContext.CustomerTransactions
                .Include(t => t.Customer)
                .AsNoTracking().Select(ct => new CustomerTransactionForListViewModel
                {
                    CustomerId = ct.CustomerId,
                    FirstName = ct.Customer.FirstName,
                    LastName = ct.Customer.LastName,
                    FullName = ct.Customer.FullName,
                    Email = ct.Customer.Email,
                    Mobile = ct.Customer.Mobile,
                    Status= ct.Customer.IsActive,
                    TransactionId = ct.CustomerTransactionId,
                    SoldAmount = ct.SoldAmount,
                    ReceivedAmount = ct.ReceivedAmount,
                    TransactionDate = ct.TransactionDate
                });

            if (!string.IsNullOrEmpty(firstName))
            {
                customerTransactionsFilterData = customerTransactionsFilterData.Where(ct => ct.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                customerTransactionsFilterData = customerTransactionsFilterData.Where(ct => ct.LastName.Contains(lastName));
            }
            if (!string.IsNullOrEmpty(fullName))
            {
                customerTransactionsFilterData = customerTransactionsFilterData.Where(ct => ct.FullName.Contains(fullName));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                customerTransactionsFilterData = customerTransactionsFilterData.Where(ct => ct.Mobile.Contains(mobile));
            }
            if (fromDate != null && toDate != null)
            {
                customerTransactionsFilterData = customerTransactionsFilterData.Where(ct => ct.TransactionDate >= fromDate && ct.TransactionDate <= toDate);
            }

            coustomerTransactionsFilteredCount = await customerTransactionsFilterData.CountAsync();
            customerTransactionsFilterData = customerTransactionsFilterData.Skip(start).Take(length);

            if (coustomerTransactionsFilteredCount > 0)
            {
                coustomerTransactionsTotalCount = coustomerTransactionsFilteredCount;
            }
            else
            {
                coustomerTransactionsTotalCount = await ApplicationDbContext.CustomerTransactions.Include(t => t.Customer).AsNoTracking().CountAsync();
            }

            if (!string.IsNullOrEmpty(sortColumnName))
            {
                customerTransactionsFilterData = orderAscendingDirection ? customerTransactionsFilterData.OrderByDynamic(sortColumnName, DtOrderDir.Asc) : customerTransactionsFilterData.OrderByDynamic(sortColumnName, DtOrderDir.Desc).AsQueryable();
            }

            return new DataResult<List<CustomerTransactionForListViewModel>>(await customerTransactionsFilterData.ToListAsync(), coustomerTransactionsTotalCount, coustomerTransactionsFilteredCount);
        }

        public async Task<DataResult<List<CustomerTransactionSummaryForListViewModel>>> GetAllCustomerTransactionSummaryAsync(int pageNumber, int pageSize, string sortOrder, string fullName, string mobile, DateTime? fromDate, DateTime? toDate)
        {

            var customerTransactions = ApplicationDbContext.CustomerTransactions.Include(c => c.Customer).AsNoTracking();

            if (!string.IsNullOrEmpty(fullName))
            {
                customerTransactions = customerTransactions.Where(ct => ct.Customer.FullName.Contains(fullName));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                customerTransactions = customerTransactions.Where(ct => ct.Customer.Mobile.Contains(mobile));
            }            
            if (fromDate != null && toDate != null)
            {
                customerTransactions = customerTransactions.Where(ct => ct.TransactionDate >= fromDate && ct.TransactionDate <= toDate);
            }

            int count = await ApplicationDbContext.Customers.CountAsync();

            var filteredCustomerTransactions = customerTransactions
                .GroupBy(g => new { g.CustomerId, g.Customer.FullName, g.Customer.Mobile, g.Customer.Email })
                .Select(group => new CustomerTransactionSummaryForListViewModel
                {
                    CustomerId = group.Key.CustomerId,
                    FullName = group.Key.FullName,
                    Mobile = group.Key.Mobile,
                    Email = group.Key.Email,
                    TotalSellAmount = group.Sum(s => s.SoldAmount),
                    TotalReceiveAmount = group.Sum(s => s.ReceivedAmount),
                    TotalAmount = group.Sum(s => s.SoldAmount) - group.Sum(s => s.ReceivedAmount)
                });

            filteredCustomerTransactions = filteredCustomerTransactions.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            switch (sortOrder)
            {
                case "fullname_desc":
                    filteredCustomerTransactions = filteredCustomerTransactions.OrderByDescending(ct => ct.FullName);
                    break;
                case "Mobile":
                    filteredCustomerTransactions = filteredCustomerTransactions.OrderBy(ct => ct.Mobile);
                    break;
                case "mobile_desc":
                    filteredCustomerTransactions = filteredCustomerTransactions.OrderByDescending(ct => ct.Mobile);
                    break;
                case "Sold":
                    filteredCustomerTransactions = filteredCustomerTransactions.OrderBy(ct => ct.TotalSellAmount);
                    break;
                case "sold_desc":
                    filteredCustomerTransactions = filteredCustomerTransactions.OrderByDescending(ct => ct.TotalSellAmount);
                    break;
                case "Received":
                    filteredCustomerTransactions = filteredCustomerTransactions.OrderBy(ct => ct.TotalReceiveAmount);
                    break;
                case "received_desc":
                    filteredCustomerTransactions = filteredCustomerTransactions.OrderByDescending(ct => ct.TotalReceiveAmount);
                    break;
                case "Total":
                    filteredCustomerTransactions = filteredCustomerTransactions.OrderBy(ct => ct.TotalAmount);
                    break;
                case "total_desc":
                    filteredCustomerTransactions = filteredCustomerTransactions.OrderByDescending(ct => ct.TotalAmount);
                    break;
                default:
                    filteredCustomerTransactions = filteredCustomerTransactions.OrderBy(ct => ct.FullName);
                    break;
            }

            return new DataResult<List<CustomerTransactionSummaryForListViewModel>>(filteredCustomerTransactions.ToList(), count);
        }

        public async Task<DataResult<List<CustomerTransactionSummaryForListViewModel>>> GetAllCustomerTransactionSummaryAsync(int start, int length, string sortColumnName, bool orderAscendingDirection, string fullName, string mobile, string email,DateTime? fromDate, DateTime? toDate)
        {
            int coustomerTransactionsFilteredCount = 0;
            int coustomerTransactionsTotalCount = 0;

            var customerTransactions = ApplicationDbContext.CustomerTransactions
                .Include(t => t.Customer)
                .AsNoTracking().Select(ct => new
                {
                    CustomerId = ct.CustomerId,
                    FullName = ct.Customer.FullName,
                    Email = ct.Customer.Email,
                    Mobile = ct.Customer.Mobile,
                    SoldAmount = ct.SoldAmount,
                    ReceivedAmount = ct.ReceivedAmount,
                    TransactionDate = ct.TransactionDate
                });

            if (!string.IsNullOrEmpty(fullName))
            {
                customerTransactions = customerTransactions.Where(ct => ct.FullName.Contains(fullName));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                customerTransactions = customerTransactions.Where(ct => ct.Mobile.Contains(mobile));
            }
            if (!string.IsNullOrEmpty(email))
            {
                customerTransactions = customerTransactions.Where(ct => ct.Email.Contains(email));
            }
            if (fromDate != null && toDate != null)
            {
                customerTransactions = customerTransactions.Where(ct => ct.TransactionDate >= fromDate && ct.TransactionDate <= toDate);
            }

            var customerTransactionsFilterData = customerTransactions.GroupBy(g => new { g.CustomerId, g.FullName, g.Mobile, g.Email })
                .Select(group => new CustomerTransactionSummaryForListViewModel
                {
                    CustomerId = group.Key.CustomerId,
                    FullName = group.Key.FullName,
                    Mobile = group.Key.Mobile,
                    Email = group.Key.Email,
                    TotalSellAmount = group.Sum(s => s.SoldAmount),
                    TotalReceiveAmount = group.Sum(s => s.ReceivedAmount),
                    TotalDueAmount = group.Sum(s => s.SoldAmount) - group.Sum(s => s.ReceivedAmount),
                    TotalAmount = group.Sum(s => s.SoldAmount) - group.Sum(s => s.ReceivedAmount)
                });

            coustomerTransactionsFilteredCount = await customerTransactionsFilterData.CountAsync();
            customerTransactionsFilterData = customerTransactionsFilterData.Skip(start).Take(length);

            if (coustomerTransactionsFilteredCount > 0)
            {
                coustomerTransactionsTotalCount = coustomerTransactionsFilteredCount;
            }
            else
            {
                coustomerTransactionsTotalCount = await ApplicationDbContext.CustomerTransactions.Include(t => t.Customer).AsNoTracking().CountAsync();
            }

            if (!string.IsNullOrEmpty(sortColumnName))
            {
                customerTransactionsFilterData = orderAscendingDirection ? customerTransactionsFilterData.OrderByDynamic(sortColumnName, DtOrderDir.Asc) : customerTransactionsFilterData.OrderByDynamic(sortColumnName, DtOrderDir.Desc).AsQueryable();
            }

            return new DataResult<List<CustomerTransactionSummaryForListViewModel>>(await customerTransactionsFilterData.ToListAsync(), coustomerTransactionsTotalCount, coustomerTransactionsFilteredCount);
        }

        public async Task<List<CustomerTransactionSummaryForListViewModel>> GetCurrentYearDueSummaryByMonthAsync()
        {
            var transactionsGroupByMonth = ApplicationDbContext.CustomerTransactions.GroupBy(g => new { g.TransactionDate.Month })
                .Select(group => new CustomerTransactionSummaryForListViewModel
                {
                    TotalDueAmount = group.Sum(s => s.SoldAmount) - group.Sum(s => s.ReceivedAmount)
                });

            return await transactionsGroupByMonth.ToListAsync();
        }

        public async Task<List<CustomerTransactionSummaryForListViewModel>> GetCurrentYearSaleSummaryByMonthAsync() 
        {
            var transactionsGroupByMonth = ApplicationDbContext.CustomerTransactions.GroupBy(g => new { g.TransactionDate.Month })
                .Select(group => new CustomerTransactionSummaryForListViewModel
                {                    
                    TotalSellAmount = group.Sum(s => s.SoldAmount),
                    TotalReceiveAmount = group.Sum(s => s.ReceivedAmount),
                    TotalAmount = group.Sum(s => s.SoldAmount) - group.Sum(s => s.ReceivedAmount)                    
                });

            return await transactionsGroupByMonth.ToListAsync();
        }
    }
}
