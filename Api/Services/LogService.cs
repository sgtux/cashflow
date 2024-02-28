using System.IO;
using System.Reflection;
using Cashflow.Api.Shared;
using log4net;
using log4net.Config;

namespace Cashflow.Api.Services
{
    public class LogService : BaseService
    {
        private readonly ILog _log;

        private readonly AppConfig _appConfig;

        public LogService()
        {
            _appConfig = new AppConfig();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), new FileInfo("log4net.config"));
        }

        public void Debug(string message)
        {
            if (_appConfig.IsDevelopment)
                _log.Debug($"{message}\n");
        }

        public void Info(string message)
        {
            if (_appConfig.IsDevelopment)
                _log.Info($"{message}\n");
        }

        public void Error(string message) => _log.Error($"{message}\n");
    }
}