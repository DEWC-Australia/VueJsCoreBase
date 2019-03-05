using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Data.DatabaseLogger;

namespace Middleware.DatabaseLogger
{
    public class DatabaseLoggerConfiguration
    {
        public LogLevel LogLevel { get; set; }
        public int EventId { get; set; }

        public bool EnableConsole { get; set; }
    }
    public class DatabaseLogger : ILogger
    {
        private readonly string _name;
        private readonly DatabaseLoggerConfiguration _config;
        private readonly DatabaseLoggerContext _mDb;
        private readonly string _applicationName;
        private readonly string _environmentName;
        public DatabaseLogger(string name, DatabaseLoggerConfiguration config, DatabaseLoggerContext db, string applicationName, string environmentName)
        {
            _name = name;
            _config = config;
            _mDb = db;
            _applicationName = applicationName;
            _environmentName = environmentName;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _config.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, System.Exception exception, Func<TState, System.Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (_config.EventId == 0 || _config.EventId == eventId.Id)
            {
                if (_config.EnableConsole)
                {
                    Console.WriteLine($"{logLevel.ToString()} - {eventId.Id} - {_name} - {formatter(state, exception)}");
                }
               
                // want to add users - can get once it gets to controller
                // annonymous page = annonymous user
                // setup or somewhere else == loading
                // everywhere else = userName

                // might add different types of logs

                // exceptions
                // controllers - user/actions
                // caldav - look at the controller being called


                _mDb.DatabaseLog.Add(new DatabaseLog
                {
                    Id = eventId.Id,
                    Callsite = eventId.Name,
                    Level = logLevel.ToString(),
                    Logged = DateTime.UtcNow,
                    Logger = "Database Logger",
                    Exception = exception == null ? null : exception.StackTrace,
                    Message = formatter(state, exception),
                    MachineName = $"{_applicationName} - {_environmentName}"
                }
                );
                _mDb.SaveChanges();


            }
        }
    }



    public class DatabaseLoggerProvider : ILoggerProvider
    {
        private readonly DatabaseLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string, DatabaseLogger> _loggers = new ConcurrentDictionary<string, DatabaseLogger>();
        private readonly DatabaseLoggerContext _mDb;
        private readonly string _applicationName;
        private readonly string _environmentName;

        public DatabaseLoggerProvider(DatabaseLoggerConfiguration config, DatabaseLoggerContext db, string applicationName, string environmentName)
        {
            _config = config;
            _mDb = db;
            _applicationName = applicationName;
            _environmentName = environmentName;

        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new DatabaseLogger(name, _config, _mDb, _applicationName, _environmentName));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }


}
