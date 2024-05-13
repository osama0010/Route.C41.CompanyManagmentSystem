using CompanyManagmentSystem.BLL.Interfaces;
using CompanyManagmentSystem.BLL.Repositories;
using CompanyManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace CompanyManagmentSystem.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostEnvironment _env;

        public DepartmentController(IUnitOfWork unitOfWork, IHostEnvironment env) // Ask CLR for Creating an Object from Class Implmenting IDepartmentRepos[
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.Repository<Department>().GetAllAsync();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) //Server side validation
            {
                _unitOfWork.Repository<Department>().Add(department);
                var count = await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName)
        {
            if(!id.HasValue) 
                return BadRequest(); //400

            var department = await _unitOfWork.Repository<Department>().GetAsync(id.Value);

            if (department == null)
                return NotFound(); //404

            return View(ViewName, department);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
            #region MyRegion
            //if(!id.HasValue) 
            //    return BadRequest();

            //var department = _departmentRepository.Get(id.Value);

            //if (department == null)
            //    return NotFound();

            //return View(department); 
            #endregion
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, Department department)
        {
                if (id != department.Id)
                    return BadRequest(new ViewResult());

                if (!ModelState.IsValid)
                    return View(department);

            try
            {
                _unitOfWork.Repository<Department>().Update(department);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //1.To Log Exceptions
                //2.Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An error has occured during updating the department");

                return View(department);
            }
        }

        [HttpGet]
        public Task<IActionResult> Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Department department)
        {
            try
            {
                _unitOfWork.Repository<Department>().Delete(department);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An error has occured during updating the department");

                return View(department);
            }
        }

    }
}
