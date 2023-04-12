using MyApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace MyApp.WebMS.Models
{
    public class UserListViewModel
    {
        public IList<UserListItemViewModel> Items { get; set; } 
    }

    public class UserListItemViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter Forename")] 
        public string Forename { get; set; }

        [Required(ErrorMessage = "Please enter Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Please enter Date Of Birth")]
        [Display(Name="Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        //[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Please enter a Valid Date")] // fails with 00:00:00 time on end of date
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,20}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a Valid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public IList<LogEntryListItemViewModel> LogEntryItems { get; set; }
    }
}