using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Petriss.Models.ViewModel
{
    public class UserSignUpView
    {

        [Key]
        public int UserId { get; set; }
        public int UserLookupRoleId { get; set; }
        public string RoleName { get; set; }
        [Display(Name = "Preferred Name")]
        public string PrefferedName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "EmailAddress")]
        public string EmailAddress { get; set; }
        public string Password { get; set; }       
      

    }

    public class UserLoginView
    {
        [Key]
        public int UserProfileId { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please Enter your Email Address")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
       
    }
     
    public class UserProfileView
    {
        [Key]
        public int UserId { get; set; }
        public int UserLookupRoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsRoleActive { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Login Name")]
        public string PrefferedName { get; set; }
        [Required(ErrorMessage = "Required.")]     
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserLookupAvailableRole
    {
        [Key]
        public int UserLookupRoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }

    public class Gender
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class UserRoles
    {
        public int? SelectedRoleId { get; set; }
        public IEnumerable<UserLookupAvailableRole> UserRoleList { get; set; }
    }

    public class UserGender
    {
        public string SelectedGender { get; set; }
        public IEnumerable<Gender> Gender { get; set; }
    }
    public class UserDataView
    {
        public IEnumerable<UserProfileView> UserProfile { get; set; }
        public UserRoles UserRoles { get; set; }
        public UserGender UserGender { get; set; }
    }


    public class EmailFormModel
    {
        [Required, Display(Name = "Your name")]
        public string FromName { get; set; }
        [Required, Display(Name = "Your email"), EmailAddress]
        public string FromEmail { get; set; }
        [Required]
        public string Message { get; set; }
    }

}


