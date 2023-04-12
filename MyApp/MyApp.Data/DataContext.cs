using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyApp.Models;

namespace MyApp.Data
{
    public class DataContext
    {
        // Databas tables
        private List<User> Users { get; set; }
        private List<LogEntry> LogEntries { get; set; }


        public DataContext()
        {
            // On startup we want to seed the data in
            Seed();
        }

        /// <summary>
        /// Seed the default data for the app
        /// </summary>
        private void Seed()
        {
            Users = new List<User>
            {
                new User { Id = 1, Forename = "Grant", Surname = "Cooper", DateOfBirth = new DateTime(1980,1,31), Email = "grant.cooper@example.com", IsActive = true },
                new User { Id = 2, Forename = "Tom", Surname = "Gathercole", DateOfBirth = new DateTime(1982,3,31), Email = "tom.gathercole@example.com", IsActive = false },
                // User with Id 3 was added then deleted
                new User { Id = 4, Forename = "Mark", Surname = "Edmondson", DateOfBirth = new DateTime(1984,5,31), Email = "mark.edmondson@example.com", IsActive = true },
                new User { Id = 5, Forename = "Graham", Surname = "Clark", DateOfBirth = new DateTime(1986, 7, 31), Email = "graham.clark@example.com", IsActive = false }
            };

            LogEntries = new List<LogEntry>
            {
                new LogEntry { Id = 1, UserId = 1, LogEntryType = LogEntryType.Add, TimeStamp = new DateTime(2023, 04,  05, 10, 37, 13, 876), FullName = "Grant Cooper"},
                new LogEntry { Id = 2, UserId = 2, LogEntryType = LogEntryType.Add, TimeStamp = new DateTime(2023, 04,  05, 13, 25, 08, 198), FullName = "Tom Gathercole"},
                new LogEntry { Id = 3, UserId = 3, LogEntryType = LogEntryType.Add, TimeStamp = new DateTime(2023, 04,  05, 14, 11, 34, 571), FullName = "John Ferguson"},
                new LogEntry { Id = 4, UserId = 4, LogEntryType = LogEntryType.Add, TimeStamp = new DateTime(2023, 04,  05, 17, 05, 43, 319), FullName = "Mark Edmondson"},
                new LogEntry { Id = 5, UserId = 5, LogEntryType = LogEntryType.Add, TimeStamp = new DateTime(2023, 04,  05, 17, 25, 37, 817), FullName = "Graham Clark"},

                new LogEntry { Id = 6, UserId = 2, LogEntryType = LogEntryType.View, TimeStamp = new DateTime(2023, 04,  06, 08, 55, 31, 478), FullName = "Tom Gathercole"},
                new LogEntry { Id = 7, UserId = 2, LogEntryType = LogEntryType.Edit, TimeStamp = new DateTime(2023, 04,  07, 11, 12, 29, 656), FullName = "Tom Gathercole"},
                new LogEntry { Id = 8, UserId = 3, LogEntryType = LogEntryType.Delete, TimeStamp = new DateTime(2023, 04,  08, 15, 21, 52, 523), FullName = "John Ferguson"},
            };
        }

        public List<TEntity> Set<TEntity>() where TEntity : class
        {
            var propertyInfo = PropertyInfos.FirstOrDefault(p => p.PropertyType == typeof(List<TEntity>));


            if (propertyInfo != null)
            {
                // Get the List<T> from 'this' Context instance
                var x = propertyInfo.GetValue(this, null) as List<TEntity>;

                return x;
            }

            throw new Exception("Type collection not found");
        }
        private IEnumerable<PropertyInfo> _propertyInfos;
        private IEnumerable<PropertyInfo> PropertyInfos
        {
            get
            {
                return _propertyInfos ??
                        (_propertyInfos = GetType()
                                            .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                                            .Where(p => p.PropertyType.IsGenericType &&
                                                        p.PropertyType.GetGenericTypeDefinition() == typeof(List<>)));
            }
        }
    }
}