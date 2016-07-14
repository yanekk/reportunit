using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ReportUnit.Parsers;

namespace ReportUnit.DependencyInjection
{
    public class ParserResolverInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<ITestFileResolver>().WithServiceFromInterface());
        }
    }
}
