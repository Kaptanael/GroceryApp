using GroceryApp.Common;
using GroceryApp.Models;
using GroceryApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.Data
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<List<CustomerNameViewModel>> GetAllCustomerFullNameAsync();

        Task<List<CustomerNameViewModel>> GetAllActiveCustomerFullNameAsync();

        Task<DataResult<List<Customer>>> GetAllCustomerAsync(int pageNumber, int pageSize, string sortOrder, string firstName, string lastName, string fullName, string mobile);

        Task<DataResult<List<Customer>>> GetAllCustomerAsync(int start, int length, string sortColumnName, string sortColumnDirection, string firstName, string lastName, string fullName, string mobile);

        Task<DataResult<List<Customer>>> GetAllCustomerAsync(int start, int length, string sortColumnName, bool orderAscendingDirection, string firstName, string lastName, string fullName, string mobile);

        Task<List<Customer>> GetAllCustomerByFullNameAsync(string name);

        Task<List<Customer>> GetAllCustomerByFirstNameAsync(string name);

        Task<List<Customer>> GetAllCustomerByLastNameAsync(string name);

        Task<List<Customer>> GetAllCustomerByMobileAsync(string mobile);

        Task<List<Customer>> GetAllCustomerByEmailAsync(string email);

        Task<Customer> GetCustomerByMobileAsync(string mobile, int customerId);

        Task<Customer> GetCustomerByEmailAsync(string email, int customerId);        
    }
}
