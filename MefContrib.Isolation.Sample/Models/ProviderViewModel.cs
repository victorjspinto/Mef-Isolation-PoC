using MefContrib.Isolation.Samples.Contracts;

namespace MefContrib.Isolation.Samples.Models
{
    public class ProviderViewModel
    {
        private readonly IMovieProvider _movieProvider;

        public ProviderViewModel(IMovieProvider movieProvider)
        {
            _movieProvider = movieProvider;

            Name = _movieProvider.Name;
            Description = _movieProvider.Description;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public IMovieProvider MovieProvider
        {
            get { return _movieProvider; }
        }
    }
}