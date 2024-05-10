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

namespace CompanyManagmentSystem.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IHostEnvironment _env;
        private readonly IMapper _mapper;

        //Ask CLR for Creating an Object from Class Implmenting IEmployee
        public EmployeeController(IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository ,
            IHostEnvironment hostEnvironment, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _env = hostEnvironment;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();
            return View(_mapper.Map<IEnumerable<EmployeeViewModel>>(employees));
            //_mapper.Map<IEnumerable<EmployeeViewModel>, IEnumerable<Employee>>(employeeVm);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVm)
        {
            if (ModelState.IsValid) //Server side validation
            {
                var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = _departmentRepository.GetAll();
            return View(employeeVm);
        }
        public IActionResult Details(int? id, string ViewName)
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = _employeeRepository.Get(id.Value);

            if (employee == null)
                return NotFound();

            return View(ViewName, _mapper.Map<EmployeeViewModel>(employee));
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ViewBag.Departments = _departmentRepository.GetAll();
            return Details(id, "Edit");
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int? id, EmployeeViewModel employeeVm)
        {
            if(id != employeeVm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(employeeVm);

            try
            {
                var count = _employeeRepository.Update(_mapper.Map<Employee>(employeeVm));
                if (count > 0)
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

            return Details(id, "Edit");
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                _employeeRepository.Delete(_mapper.Map<Employee>(employeeVM));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An error has occured during Deleting the Record");

                return View(employeeVM);
            }
        }
    }
}
