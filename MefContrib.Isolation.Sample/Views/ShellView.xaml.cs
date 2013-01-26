using System.ComponentModel.Composition;

namespace MefContrib.Isolation.Samples.Views
{
    [Export]
    public partial class ShellView
    {
        public ShellView()
        {
            InitializeComponent();
        }
    }
}
