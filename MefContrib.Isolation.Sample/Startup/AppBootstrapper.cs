using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Caliburn.Micro;
using MefContrib.Hosting.Isolation;
using MefContrib.Isolation.Samples.ViewModels;

namespace MefContrib.Isolation.Samples.Startup
{
    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        private readonly CompositionContainer _container;
        public AppBootstrapper()
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (AppBootstrapper).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(path));
            var isolatingCatalog = new IsolatingCatalog(catalog);
            
            _container = new CompositionContainer(isolatingCatalog);
            _container.ComposeExportedValue(_container);
            
        }
        
        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }

        protected override System.Collections.Generic.IEnumerable<object> GetAllInstances(Type service)
        {
            var query = _container.GetExports(service, null, null);
            var list = query.Select(t => t.Value).ToList();

            return list;
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null)
            {
                service = typeof (object);
            }

            var list = _container.GetExports(service, null, key);
            return list.Select(t => t.Value).Single();
        }
    }

    [Export(typeof(IWindowManager))]
    public class MefWindowManager : WindowManager { }
}