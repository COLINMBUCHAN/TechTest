using MyApp.Models.Base;
using System;

namespace MyApp.Models
{
    public enum LogEntryType
    {
        Add,
        Edit,
        View,
        Delete
    }

    public class LogEntry : ModelBase
    {
        public long UserId { get; set; }

        public LogEntryType LogEntryType { get; set; }

        public DateTime TimeStamp { get; set; }
 
        public string FullName { get; set; } // good fall back name when displaying log entries of deleted users

        public string Email { get; set; }
    }
}