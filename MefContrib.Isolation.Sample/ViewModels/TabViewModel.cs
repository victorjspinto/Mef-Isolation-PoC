using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using MefContrib.Hosting.Isolation.Runtime;
using MefContrib.Isolation.Samples.Contracts;
using MefContrib.Isolation.Samples.Models;

namespace MefContrib.Isolation.Samples.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TabViewModel : Screen
    {
        private ProviderViewModel _selectedProvider;
        private bool _isFaulted;

        public TabViewModel()
        {
            Movies = new ObservableCollection<Movie>();
            ProviderViewModels = new ObservableCollection<ProviderViewModel>();
        }

        protected override void OnInitialize()
        {
            ActivationHost.Faulted += PartHost_Faulted;

            if (Providers != null)
            {
                foreach (var movieProvider in Providers)
                {
                    var model = new ProviderViewModel(movieProvider);
                    ProviderViewModels.Add(model);
                }   
            }
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                ActivationHost.Faulted -= PartHost_Faulted;

                foreach (var movieProvider in Providers)
                {
                    var objectReferenceAware = (IObjectReferenceAware)movieProvider;
                    var host = ActivationHost.GetActivationHost(objectReferenceAware.Reference);
                    if (!host.Faulted)
                    {
                        host.Stop();
                    }
                }
            }
        }

        private void PartHost_Faulted(object sender, Hosting.Isolation.Runtime.Activation.ActivationHostEventArgs e)
        {
            foreach (var movieProvider in Providers)
            {
                var objectReferenceAware = (IObjectReferenceAware) movieProvider;
                if (e.Description.Id == objectReferenceAware.Reference.Description.Id)
                {
                    // we have a problem - one of the host hosting our plugin has died, disable this tab
                    Execute.OnUIThread(() => IsFaulted = true);
                    break;
                }
            }
        }
        
        public bool IsFaulted
        {
            get { return _isFaulted; }
            private set
            {
                _isFaulted = value;
                NotifyOfPropertyChange(() => IsFaulted);
            }
        }

        [ImportMany]
        public IMovieProvider[] Providers { get; set; }

        public ObservableCollection<Movie> Movies { get; private set; }

        public ObservableCollection<ProviderViewModel> ProviderViewModels { get; private set; }
        
        public ProviderViewModel SelectedProvider
        {
            get { return _selectedProvider; }
            set
            {
                _selectedProvider = value;
                NotifyOfPropertyChange(() => SelectedProvider);
            }
        }

        public void LoadMovies()
        {
            Movies.Clear();

            try
            {
                var list = SelectedProvider.MovieProvider.GetMovies("");
                foreach (var movie in list)
                {
                    Movies.Add(movie);
                }
            }
            catch (Exception exception)
            {
                var message = exception.Message;
                if (exception.InnerException != null)
                {
                    message += "\n" + exception.InnerException.Message;
                }
                MessageBox.Show(message, "Error");
            }
        }
    }
}