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

        [HttpGet]
        public async Task<IActionResult> GetCurrentYearSaleSummaryByMonth()
        {
            var customerTransactionsFromRepo = await _uow.CustomerTransactions.GetCurrentYearSaleSummaryByMonthAsync();
            var customerTransactionsToReturn = _mapper.Map<List<CustomerTransactionSummaryForListViewModel>>(customerTransactionsFromRepo);

            return Json(new
            {
                SellAmount = customerTransactionsToReturn.Select(c => c.TotalSellAmount)                
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentYearDueSummaryByMonth()
        {
            var customerTransactionsFromRepo = await _uow.CustomerTransactions.GetCurrentYearDueSummaryByMonthAsync();
            var customerTransactionsToReturn = _mapper.Map<List<CustomerTransactionSummaryForListViewModel>>(customerTransactionsFromRepo);

            return Json(new
            {
                DueAmount = customerTransactionsToReturn.Select(c => c.TotalDueAmount)
            });
        }
    }
}
