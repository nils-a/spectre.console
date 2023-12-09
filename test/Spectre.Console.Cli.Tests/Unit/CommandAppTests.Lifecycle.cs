namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Lifecycle
    {
       public sealed class NoCommand : Command<NoCommand.Settings>
        {
            public sealed class Settings : CommandSettings
            {
            }

            public override int Execute(CommandContext context, Settings settings)
            {
                return 0;
            }
        }

       public sealed class MySettingsInterceptor : ICommandSettingsInterceptor
       {
           private readonly Action<CommandContext, CommandSettings, ITypeResolver> _action;

           public MySettingsInterceptor(Action<CommandContext, CommandSettings, ITypeResolver> action)
           {
               _action = action;
           }

           public void Intercept(CommandContext context, CommandSettings settings, ITypeResolver resolver)
           {
               _action(context, settings, resolver);
           }
       }

       public sealed class MyResultInterceptor : ICommandResultInterceptor
       {
           private readonly Func<CommandContext, CommandSettings, ITypeResolver, int, int> _function;

           public MyResultInterceptor(Func<CommandContext, CommandSettings, ITypeResolver, int, int> function)
           {
               _function = function;
           }

           public void Intercept(CommandContext context, CommandSettings settings, ITypeResolver resolver, ref int result)
           {
               result = _function(context, settings, resolver, result);
           }
       }

       [Fact]
       public void Should_Run_The_SettingsInterceptor()
        {
            // Given
            var count = 0;
            var app = new CommandApp<NoCommand>();
            var interceptor = new MySettingsInterceptor((_, _, _) =>
            {
                count += 1;
            });
            app.Configure(config => config.SetInterceptor(interceptor));

            // When
            app.Run(Array.Empty<string>());

            // Then
            count.ShouldBe(1); // to be sure
        }

       [Fact]
       public void Should_Run_The_ResultInterceptor()
        {
            // Given
            var count = 0;
            const int Expected = 123;
            var app = new CommandApp<NoCommand>();
            var interceptor = new MyResultInterceptor((_, _, _, _) =>
            {
                count += 1;
                return Expected;
            });
            app.Configure(config => config.SetInterceptor(interceptor));

            // When
            var actual = app.Run(Array.Empty<string>());

            // Then
            count.ShouldBe(1);
            actual.ShouldBe(Expected);
        }
    }
}
