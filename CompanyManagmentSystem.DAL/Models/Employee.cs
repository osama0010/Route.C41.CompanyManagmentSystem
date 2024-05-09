using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagmentSystem.DAL.Models
{
    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male = 1,
        [EnumMember(Value = "Female")]
        Female = 2
    }
    public enum EmpType
    {
        FullTime = 1, PartTime = 2
    }
    public class Employee : ModelBase
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

        [Display(Name="Is Active")]
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
        public EmpType EmployeType { get; set; }
        public bool isDelete { get; set; } = false;
    }
}
