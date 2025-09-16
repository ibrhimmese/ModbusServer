using Microsoft.Extensions.Configuration;
using Serilog;

namespace Infrastructure.Logging.Serilog.Loggers;

public class SeqLogger : LoggerServiceBase
{
    public SeqLogger(IConfiguration configuration)
    {
        var seqUrl = configuration.GetValue<string>("SeriLogConfigurations:SeqConfiguration:Url");
       

        if (string.IsNullOrEmpty(seqUrl))
        {
            throw new Exception("Seq URL must be provided in the configuration.");
        }

        Logger = new LoggerConfiguration()
            .WriteTo.Seq(serverUrl: seqUrl)
            .CreateLogger();
    }
}
