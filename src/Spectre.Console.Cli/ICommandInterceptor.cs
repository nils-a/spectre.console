namespace Spectre.Console.Cli;

/// <summary>
/// Represents a command settings interceptor that
/// will intercept command settings before it's
/// passed to a command.
/// </summary>
[Obsolete("Use ICommandSettingsInterceptor instead.")]
public interface ICommandInterceptor
{
    /// <summary>
    /// Intercepts command information before it's passed to a command.
    /// </summary>
    /// <param name="context">The intercepted <see cref="CommandContext"/>.</param>
    /// <param name="settings">The intercepted <see cref="CommandSettings"/>.</param>
    void Intercept(CommandContext context, CommandSettings settings);
}

/// <summary>
/// Represents a command settings interceptor that
/// will intercept command settings before it's
/// passed to a command.
/// </summary>
public interface ICommandSettingsInterceptor
{
    /// <summary>
    /// Intercepts command information before it's passed to a command.
    /// </summary>
    /// <param name="context">The intercepted <see cref="CommandContext"/>.</param>
    /// <param name="settings">The intercepted <see cref="CommandSettings"/>.</param>
    /// <param name="resolver">The <see cref="ITypeResolver"/>.</param>
    void Intercept(CommandContext context, CommandSettings settings, ITypeResolver resolver);
}

/// <summary>
/// Represents a command result interceptor that
/// will intercept command results before they
/// are returned.
/// </summary>
public interface ICommandResultInterceptor
{
    /// <summary>
    /// Intercepts command information before it's passed to a command.
    /// </summary>
    /// <param name="context">The intercepted <see cref="CommandContext"/>.</param>
    /// <param name="settings">The intercepted <see cref="CommandSettings"/>.</param>
    /// <param name="resolver">The <see cref="ITypeResolver"/>.</param>
    /// <param name="result">The result from the command execution.</param>
    void Intercept(CommandContext context, CommandSettings settings, ITypeResolver resolver, ref int result);
}