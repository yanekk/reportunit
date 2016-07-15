using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ReportUnit.Logging;
using ReportUnit.Parsers;
using ReportUnit.Reporting;
using ReportUnit.Utils.CommandLineOptions;
using ReportUnit.Workers.CreateReport;

namespace ReportUnit.DependencyInjection
{
    public class ParserResolverInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Register(Component.For<IParserResolvingService>().ImplementedBy<ParserResolvingService>());
            container.Register(Classes.FromThisAssembly().BasedOn<ITestFileParserResolver>().WithServiceFromInterface());
            container.Register(Component.For<IReportingService>().ImplementedBy<ReportingService>());
            container.Register(Component.For<ICreateReportWorker>().ImplementedBy<CreateReportWorker>());
            container.Register(Component.For<ICommandLineOptionsParserService>().ImplementedBy<CommandLineOptionsParserService>());
            container.Register(Component.For<ILogger>().ImplementedBy<Logger>());
        }
    }
}
