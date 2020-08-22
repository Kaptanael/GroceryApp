using AutoMapper;
using GroceryApp.Common;
using GroceryApp.Data;
using GroceryApp.Models;
using GroceryApp.Models.AuxiliaryModels;
using GroceryApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IUnitOfWork uow, IMapper mapper, ILogger<CustomersController> logger)
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
        public async Task<IActionResult> GetAllCustomerByFilter(DtParameters dtParameters)
        {
            var sortColumnName = "CustomerId";
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

            var customersResult = await _uow.Customers.GetAllCustomerAsync(dtParameters.Start, dtParameters.Length, sortColumnName, orderAscendingDirection, firstName, lastName, fullName, mobile);
            var customersModel = _mapper.Map<List<CustomerForListViewModel>>(customersResult.Data);

            return Json(new DtResult<CustomerForListViewModel>
            {
                Data = customersModel,
                Draw = dtParameters.Draw,
                RecordsFiltered = customersResult.TotalFilteredCount,
                RecordsTotal = customersResult.TotalCount
            });
        }

        //[HttpPost]
        //public async Task<IActionResult> GetAllCustomerByFilter()
        //{

        //    var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

        //    var start = Request.Form["start"].FirstOrDefault();

        //    var length = Request.Form["length"].FirstOrDefault();

        //    var sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

        //    var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

        //    var firstName = Request.Form["columns[1][search][value]"].FirstOrDefault();
        //    var lastName = Request.Form["columns[2][search][value]"].FirstOrDefault();
        //    var fullName = Request.Form["columns[3][search][value]"].FirstOrDefault();
        //    var mobile = Request.Form["columns[4][search][value]"].FirstOrDefault();

        //    var customersResult = await _uow.Customers.GetAllCustomerAsync(Convert.ToInt32(start), Convert.ToInt32(length), sortColumnName, sortColumnDirection, firstName, lastName, fullName, mobile);
        //    var customersModel = _mapper.Map<List<CustomerForListViewModel>>(customersResult.Data);

        //    return Json(new DtResult<CustomerForListViewModel>
        //    {
        //        Data = customersModel,
        //        Draw = Convert.ToInt32(draw),
        //        RecordsFiltered = customersResult.TotalFilteredCount,
        //        RecordsTotal = customersResult.TotalCount
        //    });
        //}

        [HttpGet]
        public async Task<IActionResult> IndexCustomePagination(int? pageNumber, int pageSize, string sortOrder, string firstName, string lastName, string fullName, string mobile, string submit)
        {
            if (!string.IsNullOrEmpty(submit) && submit == "clear")
            {
                firstName = string.Empty;
                lastName = string.Empty;
                fullName = string.Empty;
                mobile = string.Empty;
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentPageNumber"] = pageNumber;
            ViewData["FirstNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            ViewData["LastNameSortParm"] = sortOrder == "LastName" ? "lastname_desc" : "LastName";
            ViewData["FullNameSortParm"] = sortOrder == "FullName" ? "fullname_desc" : "FullName";
            ViewData["MobileSortParm"] = sortOrder == "Mobile" ? "mobile_desc" : "Mobile";

            ViewData["FirstNameFilter"] = firstName;
            ViewData["LastNameFilter"] = lastName;
            ViewData["FullNameFilter"] = fullName;
            ViewData["MobileFilter"] = mobile;
            ViewData["PageSize"] = pageSize;

            pageSize = 2;

            var customersResult = await _uow.Customers.GetAllCustomerAsync(pageNumber ?? 1, pageSize, sortOrder, firstName, lastName, fullName, mobile);
            var customersModel = _mapper.Map<List<CustomerForListViewModel>>(customersResult.Data);

            return View(PaginatedList<CustomerForListViewModel>.CreateAsync(customersModel, customersResult.TotalCount, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> AddEditCustomer(int? id)
        {
            CustomerForCreateUpdateViewModel customerForCreateUpdateViewModel = new CustomerForCreateUpdateViewModel();

            if (id.HasValue)
            {
                var customerFromRepo = await _uow.Customers.GetByIdAsync(id.Value);

                if (customerFromRepo == null)
                {
                    return NotFound();
                }

                customerForCreateUpdateViewModel = _mapper.Map<CustomerForCreateUpdateViewModel>(customerFromRepo);
                customerForCreateUpdateViewModel.IsActiveSelectList = new SelectList(GetIsActiveSelectList(), "Value", "Text", customerForCreateUpdateViewModel.IsActive);
            }
            else
            {
                customerForCreateUpdateViewModel.IsActiveSelectList = new SelectList(GetIsActiveSelectList(), "Value", "Text", "True");
            }

            return View(customerForCreateUpdateViewModel);
        }

        [HttpPost, ActionName("AddEditCustomer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditCustomer(int? id, CustomerForCreateUpdateViewModel customerForCreateUpdateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isNew = !id.HasValue;

                    if (isNew)
                    {
                        customerForCreateUpdateViewModel.FullName = customerForCreateUpdateViewModel.FirstName + " " + customerForCreateUpdateViewModel.LastName;
                        var customerToCreate = _mapper.Map<Customer>(customerForCreateUpdateViewModel);
                        await _uow.Customers.AddAsync(customerToCreate);
                        await _uow.SaveAsync();
                        TempData["Message"] = "Saved Successfully";
                        TempData["Status"] = "success";
                    }
                    else
                    {
                        customerForCreateUpdateViewModel.FullName = customerForCreateUpdateViewModel.FirstName + " " + customerForCreateUpdateViewModel.LastName;
                        var customerToUpdate = _mapper.Map<Customer>(customerForCreateUpdateViewModel);
                        _uow.Customers.Update(customerToUpdate);
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

            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("Post", "Delete")]
        [ActionName("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customerFromRepo = await _uow.Customers.GetByIdAsync(id);

                if (customerFromRepo == null)
                {
                    return NotFound();
                }

                _uow.Customers.Delete(customerFromRepo.CustomerId);
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

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsMobileInUse(string mobile, int CustomerId)
        {
            var customerFromRepo = await _uow.Customers.GetCustomerByMobileAsync(mobile, CustomerId);

            if (customerFromRepo == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Mobile {mobile} is already in use.");
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email, int CustomerId)
        {
            var customerFromRepo = await _uow.Customers.GetCustomerByEmailAsync(email, CustomerId);

            if (customerFromRepo == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }

        public async Task<JsonResult> GetAllCustomerByFirstName(string term)
        {
            var customerFirstNamesFromRepo = await _uow.Customers.GetAllCustomerByFirstNameAsync(term);
            return Json(customerFirstNamesFromRepo);
        }

        public async Task<JsonResult> GetAllCustomerByLastName(string term)
        {
            var customerLastNamesFromRepo = await _uow.Customers.GetAllCustomerByLastNameAsync(term);
            return Json(customerLastNamesFromRepo);
        }

        public async Task<JsonResult> GetAllCustomerByFullName(string term)
        {
            var customerFullNamesFromRepo = await _uow.Customers.GetAllCustomerByFullNameAsync(term);
            return Json(customerFullNamesFromRepo);
        }

        public async Task<JsonResult> GetAllCustomerByMobile(string term)
        {
            var customerMobilesFromRepo = await _uow.Customers.GetAllCustomerByMobileAsync(term);
            return Json(customerMobilesFromRepo);
        }

        private List<SelectListItem> GetIsActiveSelectList()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            var items = new[]{
                 new SelectListItem{ Value=true.ToString(),Text="Active"},
                 new SelectListItem{ Value=false.ToString(),Text="Inactive"}
             };

            selectListItems = items.ToList();
            return selectListItems;
        }

        private List<SelectListItem> GetPageSizeSelectList()
        {
            var items = new List<SelectListItem>();

            for (int i = 1; i <= 50; i = i + 5)
            {
                items.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }

            return items.ToList();
        }
    }
}