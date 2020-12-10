using GroceryApp.Common;
using GroceryApp.Models;
using GroceryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryApp.Data
{
    public interface ICustomerTransactionRepository: IRepository<CustomerTransaction>
    {
        Task<DataResult<List<CustomerTransactionForListViewModel>>> GetAllCustomerTransactionAsync(int pageNumber, int pageSize, string sortOrder, string firstName, string lastName, string fullName, string mobile, DateTime? fromDate, DateTime? toDate);

        Task<DataResult<List<CustomerTransactionForListViewModel>>> GetAllCustomerTransactionAsync(int start, int length, string sortColumnName, bool orderAscendingDirection, string firstName, string lastName, string fullName, string mobile, DateTime? fromDate, DateTime? toDate);

        Task<DataResult<List<CustomerTransactionSummaryForListViewModel>>> GetAllCustomerTransactionSummaryAsync(int pageNumber, int pageSize, string sortOrder, string fullName, string mobile, DateTime? fromDate, DateTime? toDate);

        Task<DataResult<List<CustomerTransactionSummaryForListViewModel>>> GetAllCustomerTransactionSummaryAsync(int start, int length, string sortColumnName, bool orderAscendingDirection, string fullName, string mobile,string email, DateTime? fromDate, DateTime? toDate);

        Task<List<CustomerTransactionSummaryForListViewModel>> GetCurrentYearDueSummaryByMonthAsync();

        Task<List<CustomerTransactionSummaryForListViewModel>> GetCurrentYearSaleSummaryByMonthAsync();
    }
}
