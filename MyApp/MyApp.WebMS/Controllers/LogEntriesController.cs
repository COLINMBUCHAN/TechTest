using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MyApp.Models;
using MyApp.Services.Factories.Implementations;
using MyApp.Services.Factories.Interfaces;
using MyApp.WebMS.Controllers.Base;
using MyApp.WebMS.Models;

namespace MyApp.WebMS.Controllers
{
    [RoutePrefix("logentries")]
    public class LogEntriesController : BaseController
    {
        public LogEntriesController(IServiceFactory serviceFactory) : base(serviceFactory) { }

        [Route("1", Name = "LogEntriesList")]
        public ActionResult ListLogEntries()
        {
            var items = ServiceFactory.LogEntryService.GetAll().Select(p => new LogEntryListItemViewModel
            {
                Id = p.Id,
                UserId = p.UserId,
                LogEntryType = p.LogEntryType,
                TimeStamp = p.TimeStamp,
                Fullname = p.FullName,  // can be overridden by real name from user record
                Email = "",
            }).ToList();

            foreach (var item in items)
            {
                MyApp.Models.User objUser = ServiceFactory.UserService.GetById((int)item.UserId);
                if (objUser != null)
                {
                    item.Fullname = objUser.Forename + " " + objUser.Surname; // possible override of name at time of logging
                    item.Email = objUser.Email;
                }
            }

            var model = new LogEntryListViewModel
            {
                Items = items.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListFilteredLogEntries([Bind(Include = "SearchFullname,SearchEmail,SearchLogType")] LogEntryListViewModel p_objLogEntryListItemViewModel)
        {
            var items = ServiceFactory.LogEntryService.GetAll().Select(p => new LogEntryListItemViewModel
            {
                Id = p.Id,
                UserId = p.UserId,
                LogEntryType = p.LogEntryType,
                TimeStamp = p.TimeStamp,
                Fullname = p.FullName,
            }).ToList();

            foreach (var item in items)
            {
                MyApp.Models.User objUser = ServiceFactory.UserService.GetById((int)item.UserId);
                if (objUser != null)
                {
                    item.Fullname = objUser.Forename + " " + objUser.Surname; // possible override of name at time of logging
                    item.Email = objUser.Email;
                }
            }

            var model = new LogEntryListViewModel
            {
                Items = items.ToList()
            };

            if (!p_objLogEntryListItemViewModel.SearchFullname.IsNullOrWhiteSpace())
                model.Items = model.Items.Where(x => x.Fullname.ToUpper().Contains(p_objLogEntryListItemViewModel.SearchFullname.ToUpper())).ToList();

            if (!p_objLogEntryListItemViewModel.SearchEmail.IsNullOrWhiteSpace())
                model.Items = model.Items.Where(x => x.Email.ToUpper().Contains(p_objLogEntryListItemViewModel.SearchEmail.ToUpper())).ToList();

            if (!p_objLogEntryListItemViewModel.SearchLogType.IsNullOrWhiteSpace())
                model.Items = model.Items.Where(x => x.LogEntryType.ToString().ToUpper().Contains(p_objLogEntryListItemViewModel.SearchLogType.ToUpper())).ToList();

            return View("ListLogEntries", model);
        }
    }
}