using Logging.Commands;
using Serilog.Core;
using Spectre.Console.Cli;

namespace Logging.Infrastructure;

public class LogInterceptor : ICommandSettingsInterceptor
{
    public static readonly LoggingLevelSwitch LogLevel = new();

    public void Intercept(CommandContext context, CommandSettings settings, ITypeResolver resolver)
    {
        if (settings is LogCommandSettings logSettings)
        {
            LoggingEnricher.Path = logSettings.LogFile ?? "application.log";
            LogLevel.MinimumLevel = logSettings.LogLevel;
        }
    }
}
