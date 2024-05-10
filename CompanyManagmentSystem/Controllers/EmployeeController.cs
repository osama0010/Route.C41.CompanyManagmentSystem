using CompanyManagmentSystem.BLL.Interfaces;
using CompanyManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CompanyManagmentSystem.BLL.Repositories;
using Microsoft.Extensions.Hosting;

namespace CompanyManagmentSystem.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostEnvironment _env;

        //Ask CLR for Creating an Object from Class Implmenting IEmployee
        public EmployeeController(IEmployeeRepository employeeRepository, IHostEnvironment hostEnvironment)
        {
            _employeeRepository = employeeRepository;
            _env = hostEnvironment;
        }
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();
            return View(employees);
        } 
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid) //Server side validation
            {
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
        public IActionResult Details(int? id, string ViewName)
        {
            if (!id.HasValue)
                return BadRequest();

            var employee = _employeeRepository.Get(id.Value);

            if (employee == null)
                return NotFound();

            return View(ViewName, employee);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int? id, Employee employee)
        {
            if(id != employee.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(employee);

            try
            {
                var count = _employeeRepository.Update(employee);
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

                return View(employee);
            }

            return Details(id, "Edit");
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            try
            {
                _employeeRepository.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An error has occured during Deleting the Record");

                return View(employee);
            }
        }
    }
}
