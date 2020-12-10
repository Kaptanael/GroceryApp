using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GroceryApp.Data;
using GroceryApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using GroceryApp.Models;
using GroceryApp.Common;
using GroceryApp.Models.AuxiliaryModels;
using Microsoft.Data.SqlClient;

namespace GroceryApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(IUnitOfWork uow, IMapper mapper, ILogger<TransactionsController> logger)
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
        public IActionResult TransactionSummary()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllTransactionByFilter(DtParameters dtParameters)
        {
            var sortColumnName = "TransactionId";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                sortColumnName = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            var firstName = Request.Form["columns[1][search][value]"].FirstOrDefault();
            var lastName = Request.Form["columns[2][search][value]"].FirstOrDefault();
            var fullName = Request.Form["columns[3][search][value]"].FirstOrDefault();
            var mobile = Request.Form["columns[4][search][value]"].FirstOrDefault();
            var fromDate = Request.Form["columns[5][search][value]"].FirstOrDefault();
            var toDate = Request.Form["columns[6][search][value]"].FirstOrDefault();

            DateTime? vFromDate = null;
            DateTime? vToDate = null;

            if (!string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                vFromDate = Convert.ToDateTime(fromDate).AddHours(00).AddMinutes(00).AddSeconds(00);
                vToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            }
            else if (string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                vFromDate = new DateTime(DateTime.Now.Year, 01, 01, 00, 00, 00);
                vToDate = Convert.ToDateTime(toDate).AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                vFromDate = Convert.ToDateTime(fromDate).AddHours(00).AddMinutes(00).AddSeconds(00);
                vToDate = Convert.ToDateTime(toDate).AddHours(23).AddMinutes(59).AddSeconds(59);
            }

            var customerTransactionsResult = await _uow.CustomerTransactions.GetAllCustomerTransactionAsync(dtParameters.Start, dtParameters.Length, sortColumnName, orderAscendingDirection, firstName, lastName, fullName, mobile, vFromDate, vToDate);
            var customerTransactionsModel = _mapper.Map<List<CustomerTransactionForListViewModel>>(customerTransactionsResult.Data);

            return Json(new DtResult<CustomerTransactionForListViewModel>
            {
                Data = customerTransactionsModel,
                Draw = dtParameters.Draw,
                RecordsFiltered = customerTransactionsResult.TotalFilteredCount,
                RecordsTotal = customerTransactionsResult.TotalCount
            });
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
            var email = Request.Form["columns[3][search][value]"].FirstOrDefault();
            var fromDate = Request.Form["columns[4][search][value]"].FirstOrDefault();
            var toDate = Request.Form["columns[5][search][value]"].FirstOrDefault();

            DateTime? vFromDate = null;
            DateTime? vToDate = null;

            if (!string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                vFromDate = Convert.ToDateTime(fromDate).AddHours(00).AddMinutes(00).AddSeconds(00);
                vToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            }
            else if (string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                vFromDate = new DateTime(DateTime.Now.Year, 01, 01, 00, 00, 00);
                vToDate = Convert.ToDateTime(toDate).AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                vFromDate = Convert.ToDateTime(fromDate).AddHours(00).AddMinutes(00).AddSeconds(00);
                vToDate = Convert.ToDateTime(toDate).AddHours(23).AddMinutes(59).AddSeconds(59);
            }

            var customerTransactionsResult = await _uow.CustomerTransactions.GetAllCustomerTransactionSummaryAsync(dtParameters.Start, dtParameters.Length, sortColumnName, orderAscendingDirection, fullName, mobile, email, vFromDate, vToDate);
            var customerTransactionsModel = _mapper.Map<List<CustomerTransactionSummaryForListViewModel>>(customerTransactionsResult.Data);

            return Json(new DtResult<CustomerTransactionSummaryForListViewModel>
            {
                Data = customerTransactionsModel,
                Draw = dtParameters.Draw,
                RecordsFiltered = customerTransactionsResult.TotalFilteredCount,
                RecordsTotal = customerTransactionsResult.TotalCount
            });
        }

        //[HttpGet]
        //public async Task<IActionResult> Index(int? pageNumber, int pageSize, string sortOrder, string firstName, string lastName, string fullName, string mobile, DateTime? fromDate, DateTime? toDate, string submit)
        //{
        //    var transactionForSearchVM = new CustomerTransactionForSearchViewModel();

        //    if (!string.IsNullOrEmpty(submit) && submit == "clear")
        //    {
        //        transactionForSearchVM.FirstNameFilter = string.Empty;
        //        transactionForSearchVM.LastNameFilter = string.Empty;
        //        transactionForSearchVM.FullNameFilter = string.Empty;
        //        transactionForSearchVM.MobileFilter = string.Empty;
        //        transactionForSearchVM.FromDateFilter = null;
        //        transactionForSearchVM.ToDateFilter = null;
        //        //transactionForSearchVM.FromDateFilter = DateTime.Now.AddHours(00).AddMinutes(00).AddSeconds(00);
        //        //transactionForSearchVM.ToDateFilter = DateTime.Now.AddHours(23).AddMinutes(59).AddSeconds(59);
        //    }
        //    else
        //    {
        //        transactionForSearchVM.FirstNameFilter = firstName;
        //        transactionForSearchVM.LastNameFilter = lastName;
        //        transactionForSearchVM.FullNameFilter = fullName;
        //        transactionForSearchVM.MobileFilter = mobile;

        //        DateTime? vFromDate = null;
        //        DateTime? vToDate = null;

        //        if (fromDate != null && fromDate.HasValue)
        //        {
        //            vFromDate = fromDate.Value.AddHours(00).AddMinutes(00).AddSeconds(00);
        //            transactionForSearchVM.FromDateFilter = vFromDate.Value.Date;
        //        }
        //        if (toDate != null && toDate.HasValue)
        //        {
        //            vToDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
        //            transactionForSearchVM.ToDateFilter = vToDate.Value.Date;
        //        }
        //    }

        //    transactionForSearchVM.SortOrder = sortOrder;
        //    transactionForSearchVM.CurrentSortOrder = sortOrder;
        //    transactionForSearchVM.PageNumber = pageNumber;
        //    transactionForSearchVM.CurrentPageNumber = pageNumber;

        //    transactionForSearchVM.FirstNameSortParm = string.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
        //    transactionForSearchVM.LastNameSortParm = sortOrder == "LastName" ? "lastname_desc" : "LastName";
        //    transactionForSearchVM.FullNameSortParm = sortOrder == "FullName" ? "fullname_desc" : "FullName";
        //    transactionForSearchVM.MobileSortParm = sortOrder == "Mobile" ? "mobile_desc" : "Mobile";
        //    transactionForSearchVM.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

        //    transactionForSearchVM.PageSize = 2;

        //    var customerTransactionsResult = await _uow.CustomerTransactions.GetAllTransactionWithCustomer(transactionForSearchVM.PageNumber ?? 1, transactionForSearchVM.PageSize, transactionForSearchVM.SortOrder, transactionForSearchVM.FirstNameFilter, transactionForSearchVM.LastNameFilter, transactionForSearchVM.FullNameFilter, transactionForSearchVM.MobileFilter, transactionForSearchVM.FromDateFilter, transactionForSearchVM.ToDateFilter);

        //    transactionForSearchVM.CustomerTransaction = PaginatedList<CustomerTransactionForListViewModel>.CreateAsync(customerTransactionsResult.Data, customerTransactionsResult.TotalCount, transactionForSearchVM.PageNumber ?? 1, transactionForSearchVM.PageSize);

        //    return View(transactionForSearchVM);
        //}

        [HttpGet]
        public async Task<IActionResult> AddEditSell(int? id)
        {
            TransactionForSellViewModel transactionForSellViewModel = new TransactionForSellViewModel();

            if (id.HasValue)
            {
                var sellTransactionFromRepo = await _uow.CustomerTransactions.GetByIdAsync(id.Value);

                if (sellTransactionFromRepo == null)
                {
                    return NotFound();
                }

                transactionForSellViewModel = _mapper.Map<TransactionForSellViewModel>(sellTransactionFromRepo);
                transactionForSellViewModel.CustomerSelectList = new SelectList(await _uow.Customers.GetAllActiveCustomerFullNameAsync(), "CustomerId", "FullName", transactionForSellViewModel.CustomerId);

            }
            else
            {
                transactionForSellViewModel.CustomerSelectList = new SelectList(await _uow.Customers.GetAllActiveCustomerFullNameAsync(), "CustomerId", "FullName");
            }

            return View(transactionForSellViewModel);
        }

        [HttpPost, ActionName("AddEditSell")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditSell(int? id, TransactionForSellViewModel transactionForSellViewModel, string submit)
        {
            string message = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    bool isNew = !id.HasValue;

                    if (isNew)
                    {
                        var transactionSellToCreate = _mapper.Map<CustomerTransaction>(transactionForSellViewModel);
                        await _uow.CustomerTransactions.AddAsync(transactionSellToCreate);
                        await _uow.SaveAsync();
                        TempData["Message"] = "Saved Successfully";
                        TempData["Status"] = "success";
                    }
                    else
                    {
                        var transactionSellToUpdate = _mapper.Map<CustomerTransaction>(transactionForSellViewModel);
                        _uow.CustomerTransactions.Update(transactionSellToUpdate);
                        _uow.Save();
                        TempData["Message"] = "Updated Successfully";
                        TempData["Status"] = "success";
                    }
                }
            }
            catch (DbUpdateConcurrencyException ce)
            {
                _logger.LogError(ce.Message, id);
                TempData["Message"] = "Failed to update for concurrency";
                TempData["Status"] = "danger";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["Message"] = "Failed to save or update";
                TempData["Status"] = "danger";
            }

            return RedirectToAction(nameof(AddEditSell));
        }

        [HttpGet]
        public async Task<IActionResult> AddEditReceive(int? id)
        {
            TransactionForReceiveViewModel transactionForReceiveViewModel = new TransactionForReceiveViewModel();

            if (id.HasValue)
            {
                var sellTransactionFromRepo = await _uow.CustomerTransactions.GetByIdAsync(id.Value);

                if (sellTransactionFromRepo == null)
                {
                    return NotFound();
                }

                transactionForReceiveViewModel = _mapper.Map<TransactionForReceiveViewModel>(sellTransactionFromRepo);
                transactionForReceiveViewModel.CustomerSelectList = new SelectList(await _uow.Customers.GetAllActiveCustomerFullNameAsync(), "CustomerId", "FullName", transactionForReceiveViewModel.CustomerId);

            }
            else
            {
                transactionForReceiveViewModel.CustomerSelectList = new SelectList(await _uow.Customers.GetAllActiveCustomerFullNameAsync(), "CustomerId", "FullName");
            }

            return View(transactionForReceiveViewModel);
        }

        [HttpPost, ActionName("AddEditReceive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditReceive(int? id, TransactionForReceiveViewModel transactionForReceiveViewModel)
        {
            string message = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    bool isNew = !id.HasValue;

                    if (isNew)
                    {
                        var transactionReceiveToCreate = _mapper.Map<CustomerTransaction>(transactionForReceiveViewModel);
                        await _uow.CustomerTransactions.AddAsync(transactionReceiveToCreate);
                        await _uow.SaveAsync();
                        TempData["Message"] = "Saved Successfully";
                        TempData["Status"] = "success";
                    }
                    else
                    {
                        var transactionReceiveToUpdate = _mapper.Map<CustomerTransaction>(transactionForReceiveViewModel);
                        _uow.CustomerTransactions.Update(transactionReceiveToUpdate);
                        _uow.Save();
                        TempData["Message"] = "Updated Successfully";
                        TempData["Status"] = "success";
                    }
                }
            }
            catch (DbUpdateConcurrencyException ce)
            {
                _logger.LogError(ce.Message, id);
                TempData["Message"] = "Failed to update for concurrency";
                TempData["Status"] = "danger";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
            }

            return RedirectToAction(nameof(AddEditReceive));
        }

        [HttpGet]
        public async Task<IActionResult> AddEditTransaction(int? id)
        {
            TransactionForCreateUpdateViewModel transactionForCreateUpdateViewModel = new TransactionForCreateUpdateViewModel();

            if (id.HasValue)
            {
                var transactionFromRepo = await _uow.CustomerTransactions.GetByIdAsync(id.Value);

                if (transactionFromRepo == null)
                {
                    return NotFound();
                }

                transactionForCreateUpdateViewModel = _mapper.Map<TransactionForCreateUpdateViewModel>(transactionFromRepo);

                if (transactionFromRepo.SoldAmount > 0)
                {
                    transactionForCreateUpdateViewModel.Amount = transactionFromRepo.SoldAmount;
                    transactionForCreateUpdateViewModel.TransactionType = 1;
                }
                else if (transactionFromRepo.ReceivedAmount > 0)
                {
                    transactionForCreateUpdateViewModel.Amount = transactionFromRepo.ReceivedAmount;
                    transactionForCreateUpdateViewModel.TransactionType = 2;
                }

                transactionForCreateUpdateViewModel.CustomerSelectList = new SelectList(await _uow.Customers.GetAllCustomerFullNameAsync(), "CustomerId", "FullName", transactionForCreateUpdateViewModel.CustomerId);

            }
            else
            {
                transactionForCreateUpdateViewModel.CustomerSelectList = new SelectList(await _uow.Customers.GetAllCustomerFullNameAsync(), "CustomerId", "FullName");
            }

            return View(transactionForCreateUpdateViewModel);
        }

        [HttpPost, ActionName("AddEditTransaction")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditTransaction(int? id, TransactionForCreateUpdateViewModel transactionForCreateUpdateViewModel)
        {
            string message = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    bool isNew = !id.HasValue;

                    if (isNew)
                    {
                        var transactionToCreate = _mapper.Map<CustomerTransaction>(transactionForCreateUpdateViewModel);
                        await _uow.CustomerTransactions.AddAsync(transactionToCreate);
                        await _uow.SaveAsync();
                        TempData["Message"] = "Saved Successfully";
                        TempData["Status"] = "success";
                    }
                    else
                    {
                        var transactionToUpdate = _mapper.Map<CustomerTransaction>(transactionForCreateUpdateViewModel);

                        if (transactionForCreateUpdateViewModel.TransactionType == 1)
                        {
                            transactionToUpdate.SoldAmount = transactionForCreateUpdateViewModel.Amount;
                        }
                        else if (transactionForCreateUpdateViewModel.TransactionType == 2)
                        {
                            transactionToUpdate.ReceivedAmount = transactionForCreateUpdateViewModel.Amount;
                        }

                        _uow.CustomerTransactions.Update(transactionToUpdate);
                        _uow.Save();
                        TempData["Message"] = "Updated Successfully";
                        TempData["Status"] = "success";
                    }
                }
            }
            catch (DbUpdateConcurrencyException ce)
            {
                _logger.LogError(ce.Message, id);
                TempData["Message"] = "Failed to update for concurrency";
                TempData["Status"] = "danger";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
            }

            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("Post", "Delete")]
        [ActionName("DeleteTransaction")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                var transactionFromRepo = await _uow.CustomerTransactions.GetByIdAsync(id);

                if (transactionFromRepo == null)
                {
                    return NotFound();
                }

                _uow.CustomerTransactions.Delete(transactionFromRepo.CustomerTransactionId);
                _uow.Save();
            }
            catch (DbUpdateException ex)
            {
                var sqlex = ex.InnerException.InnerException as SqlException;

                if (sqlex != null && sqlex.Number == 547)
                {
                    return Json(new ResponseMessage(ResponseMessage.MessageType.Warn, "In use, can not be delete"));
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var sqlex = ex.InnerException as SqlException;

                    if (sqlex != null && sqlex.Number == 547)
                    {
                        return Json(new ResponseMessage(ResponseMessage.MessageType.Warn, "In use, can not be delete"));
                    }
                }
                else
                {
                    _logger.LogError(ex.Message, ex.StackTrace);
                    return Json(new ResponseMessage(ResponseMessage.MessageType.Error, "Failed to Delete"));
                }
            }

            return Json(new ResponseMessage(ResponseMessage.MessageType.Success, "Delete Successfully"));
        }
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

        public async Task<JsonResult> GetAllCustomerByEmail(string term)
        {
            var customerNamesFromRepo = await _uow.Customers.GetAllCustomerByEmailAsync(term);
            return Json(customerNamesFromRepo);
        }
    }
}