﻿
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Petriss.Models.ViewModel
{
    public class UserSignUpView
    {

        //        [UserProfileID]
        //        [int] IDENTITY(1,1) NOT NULL,

        //[UserID] [int] NOT NULL,

        //[PrefferedName] [varchar](50) NOT NULL,

        //[FirstName] [varchar](50) NOT NULL,

        //[LastName] [varchar](50) NOT NULL,

        //[EmailAddress] [varchar](50) NOT NULL,

        //[Gender] [char](1) NOT NULL,

        //[Status]  bit NOT NULL,
        //	[RowCreatedUserID]
        //        [int] NOT NULL,

        //[RowCreatedDateTime] [datetime]
        //        NULL,
        //	[RowModifiedUserID]
        //        [int] NOT NULL,

        //[RowModifiedDateTime] [datetime]
        //        NULL,
        [Key]
        public int UserProfileID { get; set; }
        [Display(Name = "Preferred Name")]
        public string PrefferedName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "EmailAddress")]
        public string EmailAddress { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
              
    }

    public class UserLoginView
    {
        [Key]
        public int UserProfileID { get; set; }
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
        public int SYSUserID { get; set; }
        public int LOOKUPRoleID { get; set; }
        public string RoleName { get; set; }
        public bool? IsRoleActive { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Login ID")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Gender { get; set; }
    }

    public class LOOKUPAvailableRole
    {
        [Key]
        public int LOOKUPRoleID { get; set; }
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
        public int? SelectedRoleID { get; set; }
        public IEnumerable<LOOKUPAvailableRole> UserRoleList { get; set; }
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

