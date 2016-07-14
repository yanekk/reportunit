using System.Collections.Generic;
using Castle.Windsor;

namespace ReportUnit.DependencyInjection
{
    public static class DI
    {
        private static WindsorContainer _container;
        public static void Initialize()
        {
            _container = new WindsorContainer();
            _container.Install(new ParserResolverInstaller());
        }

        public static IEnumerable<TResolvable> ResolveAll<TResolvable>()
        {
            return _container.ResolveAll<TResolvable>();
        }
    }
}
