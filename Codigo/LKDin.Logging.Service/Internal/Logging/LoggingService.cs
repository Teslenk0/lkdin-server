using LKDin.Logging.Service.Internal.DTOs;

namespace LKDin.Logging.Service.Internal.Logging
{
    public class LoggingService
    {
        private List<Log> _logs;

        private static readonly object _singletonSyncRoot = new();

        private static volatile LoggingService _instanceSingleton;

        private LoggingService()
        {
            _logs = new List<Log>();
        }

        public static LoggingService Instance
        {
            get
            {
                // Double-checked locking pattern.
                if (_instanceSingleton != null)
                {
                    return _instanceSingleton;
                }
                lock (_singletonSyncRoot)
                {
                    return _instanceSingleton ??= new LoggingService();
                }
            }
        }

        public void SaveLog(Log log)
        {
            _logs.Add(log);
        }

        public List<Log> GetLogs(FilterParams parameters)
        {
            if(parameters == null)
            {
                return _logs.ToList();
            }

            IEnumerable<Log> filteredList = _logs;

            if(parameters.NameSpace != null)
            {
                filteredList = filteredList.Where(log => log.NameSpace == parameters.NameSpace);
            }

            if(parameters.Level != null)
            {
                filteredList = filteredList.Where(log => log.Level.ToString().ToUpper() == parameters.Level.ToUpper());
            }

            if(parameters.To != null)
            {
                filteredList = filteredList.Where(log => log.TimeStamp < parameters.To);
            }

            if (parameters.From != null)
            {
                filteredList = filteredList.Where(log => log.TimeStamp > parameters.From);
            }

            return filteredList.ToList();
        }
    }
}
