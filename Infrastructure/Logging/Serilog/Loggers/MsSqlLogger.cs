using Infrastructure.Logging.Serilog.ConfigurationModels;
using Infrastructure.Logging.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Infrastructure.Logging.Serilog.Loggers;

public class MsSqlLogger : LoggerServiceBase
{
    public MsSqlLogger(IConfiguration configuration)
    {
        MsSqlConfiguration logConfiguration =
            configuration.GetSection("SeriLogConfigurations:MsSqlConfiguration").Get<MsSqlConfiguration>()
            ?? throw new Exception(SerilogMessages.NullOptionsMessage);

        MSSqlServerSinkOptions sinkOptions = new()
        {
            TableName = logConfiguration.TableName,
            AutoCreateSqlDatabase = logConfiguration.AutoCreateSqlTable
        };

        ColumnOptions columnOptions = new();

        global::Serilog.Core.Logger seriLogConfig = new LoggerConfiguration().WriteTo
            .MSSqlServer(logConfiguration.ConnectionString, sinkOptions, columnOptions: columnOptions)
            .CreateLogger();

        Logger = seriLogConfig;
    }
}
