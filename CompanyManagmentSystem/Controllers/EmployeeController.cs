using CompanyManagmentSystem.BLL.Interfaces;
using CompanyManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CompanyManagmentSystem.BLL.Repositories;
using Microsoft.Extensions.Hosting;
using CompanyManagmentSystem.PL.ViewModels;
using AutoMapper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CompanyManagmentSystem.BLL;
using CompanyManagmentSystem.PL.Helpers;

namespace CompanyManagmentSystem.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostEnvironment _env;
        private readonly IMapper _mapper;

        //Ask CLR for Creating an Object from Class Implmenting IEmployee
        public EmployeeController(IUnitOfWork unitOfWork,
            #region GenericRepositoryDesignPattern
                //IEmployeeRepository employeeRepository,
                //IDepartmentRepository departmentRepository, 
            #endregion
            IHostEnvironment hostEnvironment, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _env = hostEnvironment;
            _mapper = mapper;
        }
        public IActionResult Index(string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();
            var emploeesRepo = _unitOfWork.Repository<Employee>() as IEmployeeRepository;
            if (string.IsNullOrEmpty(searchInput))
            {
                employees = emploeesRepo.GetAll();
            }
            else
            {
                employees = emploeesRepo.SearchByName(searchInput.ToLower());
            }

            return View(_mapper.Map<IEnumerable<EmployeeViewModel>>(employees));

            #region MyRegion
            //var employees = _employeeRepository.GetAll();
            //return View(_mapper.Map<IEnumerable<EmployeeViewModel>>(employees));
            //_mapper.Map<IEnumerable<EmployeeViewModel>, IEnumerable<Employee>>(employeeVm); 
            #endregion
        }

        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVm)
        {
            if (ModelState.IsValid) //Server side validation
            {
                employeeVm.imageName = DocumentSettings.UploadFile(employeeVm.Image, "Images");

                var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                _unitOfWork.Repository<Employee>().Add(employee);

                #region Benefits of UnitOfWork
                //with UnitOfWork DesignPattern now we able to update Department 
                // _unitOfWorkk.DepartmentRepository.Update(department);
                //or delete a Department
                // _unitOfWorkk.DepartmentRepository.Update(department); 
                #endregion

                var count = _unitOfWork.Complete();
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            //ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(employeeVm);
        }


        public IActionResult Details(int? id, string ViewName)
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = _unitOfWork.Repository<Employee>().Get(id.Value);

            if (employee == null)
                return NotFound();

            if(ViewName.Equals("Delete",StringComparison.CurrentCultureIgnoreCase))
                TempData["ImageName"] = employee.imageName;

            return View(ViewName, _mapper.Map<EmployeeViewModel>(employee));
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, EmployeeViewModel employeeVm)
        {
            if(id != employeeVm.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVm.Image is not null)
                    {
                        employeeVm.imageName = DocumentSettings.UploadFile(employeeVm.Image, "Images");
                    }
                    _unitOfWork.Repository<Employee>().Update(_mapper.Map<Employee>(employeeVm));
                    _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1.To Log Exceptions
                    //2.Friendly Message

                    if (_env.IsDevelopment())
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                        ModelState.AddModelError(string.Empty, "An error has occured during updating the Employee record");

                    return View(employeeVm);
                }
            }
            return View(employeeVm);

        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeVm)
        {
            try
            {
                employeeVm.imageName = TempData["ImageName"] as string;

                _unitOfWork.Repository<Employee>().Delete(_mapper.Map<Employee>(employeeVm));
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    if (employeeVm.imageName is not null)
                    {
                        DocumentSettings.DeleteFile(employeeVm.imageName, "Images");
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(employeeVm);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An error has occured during Deleting the Record");

                return View(employeeVm);
            }
        }
    }
}
