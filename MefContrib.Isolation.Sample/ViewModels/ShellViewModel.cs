using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Caliburn.Micro;

namespace MefContrib.Isolation.Samples.ViewModels
{
    [Export]
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly CompositionContainer _container;
        private int count = 1;

        [ImportingConstructor]
        public ShellViewModel(CompositionContainer container)
        {
            _container = container;

            DisplayName = "MEF Isolation Demo";
        }

        public void OpenTab()
        {
            var model = _container.GetExportedValue<TabViewModel>();
            model.DisplayName = "Tab " + count++;
            ActivateItem(model);
        }
    }
}