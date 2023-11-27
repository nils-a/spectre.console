namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Lifecycle
    {
        public sealed class VerifyCommand : Command<VerifyCommand.Settings>
        {
            public sealed class Settings : CommandSettings
            {
                public Action Verify { get; set; }
            }

            public override int Execute(CommandContext context, Settings settings)
            {
                settings.Verify();
                return 0;
            }
        }

        public sealed class VerifyStartup : ICommandLifecycle
        {
            private readonly Action<VerifyCommand.Settings> _startup;

            public VerifyStartup(Action<VerifyCommand.Settings> startup)
            {
                _startup = startup;
            }

            public Task StartupAsync(ITypeResolver resolver, CommandContext context, CommandSettings settings)
            {
                _startup(settings as VerifyCommand.Settings);
                return Task.CompletedTask;
            }
        }

        [Fact]
        public void Should_Run_Startup_Before_Running_The_Command()
        {
            // Given
            var count = 0;
            var app = new CommandApp<VerifyCommand>();
            app.Configure(config =>
            {
                config.SetStartup((_, _, settings) =>
                {
                    (settings as VerifyCommand.Settings)!.Verify = () => count.ShouldBe(1);

                    count++;
                });
            });

            // When
            app.Run(Array.Empty<string>());

            // Then
            count.ShouldBe(1); // to be sure
        }

        [Fact]
        public void Should_Run_StartupInterface_Before_Running_The_Command()
        {
            // Given
            var count = 0;
            var app = new CommandApp<VerifyCommand>();
            var startup = new VerifyStartup(settings =>
            {
                settings!.Verify = () => count.ShouldBe(1);

                count++;
            });

            app.Configure(config =>
            {
                config.Settings.Registrar.RegisterInstance<ICommandLifecycle, VerifyStartup>(startup);
            });

            // When
            app.Run(Array.Empty<string>());

            // Then
            count.ShouldBe(1); // to be sure
        }
    }
}
