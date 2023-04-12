using MyApp.Data;
using MyApp.Services.Domain.Implementations;
using MyApp.Services.Domain.Interfaces;
using MyApp.Services.Factories.Interfaces;

namespace MyApp.Services.Factories.Implementations
{
    public class ServiceFactory : IServiceFactory
    {
        private IDataAccess _dataAccess;
        protected IDataAccess DataAccess => _dataAccess ?? (_dataAccess = new DataAccess(new DataContext()));


        private IUserService _userService;
        public IUserService UserService => _userService ?? (_userService = new UserService(DataAccess));

        private ILogEntryService _logEntryService;
        public ILogEntryService LogEntryService => _logEntryService ?? (_logEntryService = new LogEntryService(DataAccess));
    }
}
