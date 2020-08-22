using GroceryApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.ViewModels
{
    public class CustomerTransactionForSearchViewModel
    {
        public string FirstNameFilter { get; set; }
        public string LastNameFilter { get; set; }
        public string FullNameFilter { get; set; }
        public string MobileFilter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }

        public string FirstNameSortParm { get; set; }
        public string LastNameSortParm { get; set; }
        public string FullNameSortParm { get; set; }
        public string MobileSortParm { get; set; }
        public string DateSortParm { get; set; }

        public int? PageNumber { get; set; }
        public int? CurrentPageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortOrder { get; set; }
        public string CurrentSortOrder { get; set; }
        public string Submit { get; set; }
        public PaginatedList<CustomerTransactionForListViewModel> CustomerTransaction { get; set; }       
    }
}
