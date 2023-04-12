using MyApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyApp.WebMS.Models
{
    public class LogEntryListViewModel
    {
        public IList<LogEntryListItemViewModel> Items { get; set; }

        [Display(Name = "Search Full Name")]
        public string SearchFullname { get; set; }

        [Display(Name = "Search Email")]
        public string SearchEmail { get; set; }

        [Display(Name = "Search Log Type")]
        public string SearchLogType { get; set; }
    }

    public class LogEntryListItemViewModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public LogEntryType LogEntryType { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}