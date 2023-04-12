using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using MyApp.Models;
using MyApp.Services.Factories.Implementations;
using MyApp.Services.Factories.Interfaces;
using MyApp.WebMS.Controllers.Base;
using MyApp.WebMS.Models;

namespace MyApp.WebMS.Controllers
{
    [RoutePrefix("users")]
    public class UsersController : BaseController
    {
        public UsersController(IServiceFactory serviceFactory) : base(serviceFactory) { }

        [Route("1", Name = "UserList")]
        public ActionResult List([Bind(Prefix = "WhetherActive")] bool? p_blnWhetherActive = null)
        {
            var items = ServiceFactory.UserService.GetAll().Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                DateOfBirth = p.DateOfBirth,
                Email = p.Email,
                IsActive = p.IsActive
            });

            var model = new UserListViewModel
            {
                Items = items.Where(x => p_blnWhetherActive == null || x.IsActive.Equals(p_blnWhetherActive)).ToList()
            };

            return View(model);
        }

        [Route("2", Name = "AddUser")]
        public ActionResult Add()
        {
            try
            {
                UserListItemViewModel objUserListItem = new UserListItemViewModel();
                return View(objUserListItem);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [Route("3", Name = "ViewUser")]
        public ActionResult Details([Bind(Prefix = "id")] int? p_intId)
        {
            if (p_intId == null)
            {
                return View("Error");
            }
            try
            {
                var items = ServiceFactory.LogEntryService.GetAll().Select(p => new LogEntryListItemViewModel
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    LogEntryType = p.LogEntryType,
                    TimeStamp = p.TimeStamp,
                    Fullname = p.FullName,
                });

                MyApp.Models.User objUser = ServiceFactory.UserService.GetById(p_intId.Value);

                UserListItemViewModel objUserListItemViewModel = new UserListItemViewModel()
                {
                    Forename = objUser.Forename,
                    Surname = objUser.Surname,
                    DateOfBirth = objUser.DateOfBirth,
                    Email = objUser.Email,
                    IsActive = objUser.IsActive,
                    LogEntryItems = items.Where(x => x.UserId == p_intId).ToList(),
                };

                /***This can cause double view events. like List, the framework can currently go in here twice***/
                // this won't show up this view, but next view
                LogEntry objLogEntry = new LogEntry()
                {
                    UserId = objUser.Id,
                    LogEntryType = LogEntryType.View,
                    TimeStamp = DateTime.Now,
                    FullName = objUser.Forename + " " + objUser.Surname,
                };

                ServiceFactory.LogEntryService.Create(objLogEntry);

                return View(objUserListItemViewModel);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [Route("4", Name = "EditUser")]
        public ActionResult Edit([Bind(Prefix = "id")] int? p_intId)
        {
            if (p_intId == null)
            {
                return View("Error");
            }
            try
            {
                MyApp.Models.User objUser = ServiceFactory.UserService.GetById(p_intId.Value);
                UserListItemViewModel objUserListItemViewModel = new UserListItemViewModel()
                {
                    Forename = objUser.Forename,
                    Surname = objUser.Surname,
                    DateOfBirth = objUser.DateOfBirth,
                    Email = objUser.Email,
                    IsActive = objUser.IsActive,
                };
                return View(objUserListItemViewModel);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [Route("5", Name = "DeleteUser")]
        public ActionResult Delete([Bind(Prefix = "id")] int? p_intId)
        {
            if (p_intId == null)
            {
                return View("Error");
            }
            try
            {
                MyApp.Models.User objUser = ServiceFactory.UserService.GetById(p_intId.Value);
                ServiceFactory.UserService.Delete(objUser);

                LogEntry objLogEntry = new LogEntry()
                {
                    UserId = objUser.Id,
                    LogEntryType = LogEntryType.Delete,
                    TimeStamp = DateTime.Now,
                    FullName = objUser.Forename + " " + objUser.Surname,
                };

                ServiceFactory.LogEntryService.Create(objLogEntry);

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPopulated([Bind(Include = "Forename,Surname,DateOfBirth,Email,IsActive")] UserListItemViewModel p_objUserListItemViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MyApp.Models.User objUser = new MyApp.Models.User()
                    {
                        Forename = p_objUserListItemViewModel.Forename,
                        Surname = p_objUserListItemViewModel.Surname,
                        DateOfBirth = p_objUserListItemViewModel.DateOfBirth,
                        Email = p_objUserListItemViewModel.Email,
                        IsActive = p_objUserListItemViewModel.IsActive,
                    };

                    ServiceFactory.UserService.Create(objUser);

                    LogEntry objLogEntry = new LogEntry()
                    {
                        UserId = objUser.Id,
                        LogEntryType = LogEntryType.Add,
                        TimeStamp = DateTime.Now,
                        FullName = objUser.Forename + " " + objUser.Surname,
                    };

                    ServiceFactory.LogEntryService.Create(objLogEntry);

                    return RedirectToAction("List");
                }

                return View("Add", p_objUserListItemViewModel);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPopulated([Bind(Include = "Id,Forename,Surname,DateOfBirth,Email,IsActive")] UserListItemViewModel p_objUserListItemViewModel)
        {
            try
            {
                if (p_objUserListItemViewModel.Id == 0)
                    return View("Error");

                if (ModelState.IsValid)
                {
                    MyApp.Models.User objUser = ServiceFactory.UserService.GetById((int)p_objUserListItemViewModel.Id); // set id this way, as id setter inaccessible

                    if (objUser == null)
                        return View("Error");

                    objUser.Forename = p_objUserListItemViewModel.Forename;
                    objUser.Surname = p_objUserListItemViewModel.Surname;
                    objUser.DateOfBirth = p_objUserListItemViewModel.DateOfBirth;
                    objUser.Email = p_objUserListItemViewModel.Email;
                    objUser.IsActive = p_objUserListItemViewModel.IsActive;

                    ServiceFactory.UserService.Update(objUser);

                    LogEntry objLogEntry = new LogEntry()
                    {
                        UserId = objUser.Id,
                        LogEntryType = LogEntryType.Edit,
                        TimeStamp = DateTime.Now,
                        FullName = objUser.Forename + " " + objUser.Surname,
                    };

                    ServiceFactory.LogEntryService.Create(objLogEntry);

                    return RedirectToAction("List");
                }

                return View("Edit", p_objUserListItemViewModel);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

    }
}