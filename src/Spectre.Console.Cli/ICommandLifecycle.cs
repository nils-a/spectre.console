namespace Spectre.Console.Cli;

public interface ICommandLifecycle
{
    /// <summary>
    /// This method will be called before running the command.
    /// </summary>
    Task StartupAsync(
        ITypeResolver resolver,
        CommandContext context,
        CommandSettings? settings);
}