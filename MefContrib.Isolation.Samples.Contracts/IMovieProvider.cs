using System.Collections.Generic;

namespace MefContrib.Isolation.Samples.Contracts
{
    public interface IMovieProvider
    {
        string Name { get; }

        string Description { get; }

        IEnumerable<Movie> GetMovies(string filter);
    }
}