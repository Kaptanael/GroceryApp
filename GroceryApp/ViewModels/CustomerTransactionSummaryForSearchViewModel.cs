using GroceryApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.ViewModels
{
    public class CustomerTransactionSummaryForSearchViewModel
    {        
        public string FullNameFilter { get; set; }
        public string MobileFilter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
       
        public string FullNameSortParm { get; set; }
        public string MobileSortParm { get; set; }
        public string SoldSortParm { get; set; }
        public string ReceivedSortParm { get; set; }
        public string TotalSortParm { get; set; }

        public int? PageNumber { get; set; }
        public int? CurrentPageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortOrder { get; set; }
        public string CurrentSortOrder { get; set; }
        public string Submit { get; set; }
        public PaginatedList<CustomerTransactionSummaryForListViewModel> CustomerTransaction { get; set; }       
    }
}
