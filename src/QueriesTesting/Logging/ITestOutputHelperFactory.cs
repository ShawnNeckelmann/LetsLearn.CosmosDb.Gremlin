using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace QueriesTesting.Logging;

public class ITestOutputHelperFactory : ILoggerFactory
{
    private readonly ILogger _logger;

    public ITestOutputHelperFactory(ITestOutputHelper helper)
    {
        _logger = XUnitLogger.CreateLogger(helper);
    }

    public void Dispose()
    {
        //throw new NotImplementedException();
    }

    public void AddProvider(ILoggerProvider provider)
    {
        //throw new NotImplementedException();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _logger;
    }
}