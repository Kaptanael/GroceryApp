using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GroceryApp.Models;
using Microsoft.AspNetCore.Authorization;
using GroceryApp.Data;
using AutoMapper;
using GroceryApp.ViewModels;
using cloudscribe.Pagination.Models;
using GroceryApp.Common;
using GroceryApp.Models.AuxiliaryModels;

namespace GroceryApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IUnitOfWork uow, IMapper mapper, ILogger<HomeController> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllTransactionSummaryByFilter(DtParameters dtParameters)
        {
            var sortColumnName = "CustomerId";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                sortColumnName = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            
            var fullName = Request.Form["columns[1][search][value]"].FirstOrDefault();
            var mobile = Request.Form["columns[2][search][value]"].FirstOrDefault();
            var fromDate = Request.Form["columns[3][search][value]"].FirstOrDefault();
            var toDate = Request.Form["columns[4][search][value]"].FirstOrDefault();

            DateTime? vFromDate = null;
            DateTime? vToDate = null;

            if (!string.IsNullOrEmpty(fromDate))
            {
                vFromDate = Convert.ToDateTime(fromDate).AddHours(00).AddMinutes(00).AddSeconds(00);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                vToDate = Convert.ToDateTime(toDate).AddHours(23).AddMinutes(59).AddSeconds(59);
            }

            var customerTransactionsResult = await _uow.CustomerTransactions.GetAllCustomerTransactionSummaryAsync(dtParameters.Start, dtParameters.Length, sortColumnName, orderAscendingDirection, fullName, mobile, vFromDate, vToDate);
            var customerTransactionsModel = _mapper.Map<List<CustomerTransactionSummaryForListViewModel>>(customerTransactionsResult.Data);

            return Json(new DtResult<CustomerTransactionSummaryForListViewModel>
            {
                Data = customerTransactionsModel,
                Draw = dtParameters.Draw,
                RecordsFiltered = customerTransactionsResult.TotalFilteredCount,
                RecordsTotal = customerTransactionsResult.TotalCount
            });
        }

        //public async Task<IActionResult> Index(int? pageNumber, int pageSize, string sortOrder, string fullName, string mobile, DateTime? fromDate, DateTime? toDate, string submit)
        //{
        //    var transactionSummaryForSearchVM = new CustomerTransactionSummaryForSearchViewModel();

        //    if (!string.IsNullOrEmpty(submit) && submit == "clear")
        //    {
        //        transactionSummaryForSearchVM.FullNameFilter = string.Empty;
        //        transactionSummaryForSearchVM.MobileFilter = string.Empty;
        //        transactionSummaryForSearchVM.FromDateFilter = null;
        //        transactionSummaryForSearchVM.ToDateFilter = null;
        //        //transactionForSearchVM.FromDateFilter = DateTime.Now.AddHours(00).AddMinutes(00).AddSeconds(00);
        //        //transactionForSearchVM.ToDateFilter = DateTime.Now.AddHours(23).AddMinutes(59).AddSeconds(59);
        //    }
        //    else
        //    {
        //        transactionSummaryForSearchVM.FullNameFilter = fullName;
        //        transactionSummaryForSearchVM.MobileFilter = mobile;

        //        DateTime? vFromDate = null;
        //        DateTime? vToDate = null;

        //        if (fromDate != null && fromDate.HasValue)
        //        {
        //            vFromDate = fromDate.Value.AddHours(00).AddMinutes(00).AddSeconds(00);
        //            transactionSummaryForSearchVM.FromDateFilter = vFromDate.Value.Date;
        //        }
        //        if (toDate != null && toDate.HasValue)
        //        {
        //            vToDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
        //            transactionSummaryForSearchVM.ToDateFilter = vToDate.Value.Date;
        //        }
        //    }

        //    transactionSummaryForSearchVM.SortOrder = sortOrder;
        //    transactionSummaryForSearchVM.CurrentSortOrder = sortOrder;
        //    transactionSummaryForSearchVM.PageNumber = pageNumber;
        //    transactionSummaryForSearchVM.CurrentPageNumber = pageNumber;

        //    transactionSummaryForSearchVM.FullNameSortParm = sortOrder == "FullName" ? "fullname_desc" : "FullName";
        //    transactionSummaryForSearchVM.MobileSortParm = sortOrder == "Mobile" ? "mobile_desc" : "Mobile";
        //    transactionSummaryForSearchVM.SoldSortParm = sortOrder == "Sold" ? "sold_desc" : "Sold";
        //    transactionSummaryForSearchVM.ReceivedSortParm = sortOrder == "Received" ? "received_desc" : "Received";
        //    transactionSummaryForSearchVM.TotalSortParm = sortOrder == "Total" ? "total_desc" : "Total";

        //    transactionSummaryForSearchVM.PageSize = 2;

        //    var customerTransactionsResult = await _uow.CustomerTransactions.GetAllCustomerTransactionAsync(transactionSummaryForSearchVM.PageNumber ?? 1, transactionSummaryForSearchVM.PageSize, transactionSummaryForSearchVM.SortOrder, transactionSummaryForSearchVM.FullNameFilter, transactionSummaryForSearchVM.MobileFilter, transactionSummaryForSearchVM.FromDateFilter, transactionSummaryForSearchVM.ToDateFilter);

        //    transactionSummaryForSearchVM.CustomerTransaction = PaginatedList<CustomerTransactionSummaryForListViewModel>.CreateAsync(customerTransactionsResult.Data, customerTransactionsResult.TotalCount, transactionSummaryForSearchVM.PageNumber ?? 1, transactionSummaryForSearchVM.PageSize);

        //    return View(transactionSummaryForSearchVM);
        //}

        public async Task<JsonResult> GetAllCustomerByFirstName(string term)
        {
            var customerNamesFromRepo = await _uow.Customers.GetAllCustomerByFirstNameAsync(term);
            return Json(customerNamesFromRepo);
        }

        public async Task<JsonResult> GetAllCustomerByLastName(string term)
        {
            var customerNamesFromRepo = await _uow.Customers.GetAllCustomerByLastNameAsync(term);
            return Json(customerNamesFromRepo);
        }

        public async Task<JsonResult> GetAllCustomerByFullName(string term)
        {
            var customerNamesFromRepo = await _uow.Customers.GetAllCustomerByFullNameAsync(term);
            return Json(customerNamesFromRepo);
        }

        public async Task<JsonResult> GetAllCustomerByMobile(string term)
        {
            var customerNamesFromRepo = await _uow.Customers.GetAllCustomerByMobileAsync(term);
            return Json(customerNamesFromRepo);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
