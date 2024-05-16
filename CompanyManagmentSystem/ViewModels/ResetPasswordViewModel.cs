using System.ComponentModel.DataAnnotations;

namespace CompanyManagmentSystem.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password is Required")]
		[MinLength(5, ErrorMessage = "Minimum Password length is 5")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirm Password doesn't match Password")]
		public string ConfirmPassword { get; set; }
	}
}
