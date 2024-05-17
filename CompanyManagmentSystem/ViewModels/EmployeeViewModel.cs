using CompanyManagmentSystem.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace CompanyManagmentSystem.PL.ViewModels
{
    public class EmployeeViewModel : ModelBase
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Max length is 50 character")]
        [MinLength(5, ErrorMessage = "Max length is 5 character")]
        public string Name { get; set; }

        [Range(21, 30)]
        public int? Age { get; set; }
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "Is Active")]
        public bool isActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public Gender Gender { get; set; }
        [Display(Name = "Employee Type")]
        public EmpType? EmployeeType { get; set; }
        public int? EmpTypeINP { get; set; }
        public bool isDelete { get; set; } = false;

        public Department Department { get; set; }
        public int? DepartmentId { get; set; }

        public string? imageName { get; set; }
        public IFormFile? Image { get; set; }
    }
}
