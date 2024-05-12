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
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public bool isActive { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HiringDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public Gender Gender { get; set; }
        public EmpType EmployeType { get; set; }
        public bool isDelete { get; set; } = false;

        public Department Department { get; set; }
        public int? DepartmentId { get; set; }

        public string? imageName { get; set; }

        #region FrontEnd Validation
        //[MinLength(5, ErrorMessage = "Max length is 5 character")]
        //[Range(21, 30)]
        //[DataType(DataType.Currency)]
        //[Display(Name="Is Active")]
        //[EmailAddress]
        //[Display(Name = "Phone Number")]
        //[Phone]
        //[Display(Name = "Hiring Date")]
        #endregion
    }
}
