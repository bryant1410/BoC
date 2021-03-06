using BoC.InversionOfControl;

namespace BoC.Logging.Sitecore.DefaultSetupTasks
{
    public class InitDefaultLoggerTask : IContainerInitializer
    {
        private readonly IDependencyResolver dependencyResolver;

        public InitDefaultLoggerTask(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        public void Execute()
        {
            if (!dependencyResolver.IsRegistered<ILogger>())
            {
                log4net.Config.XmlConfigurator.Configure();
                dependencyResolver.RegisterSingleton<ILogger, SitecoreLogger>();
            }
        }
    }
}