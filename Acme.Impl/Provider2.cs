using System;
using System.Collections.Generic;
using MefContrib.Hosting.Isolation;
using MefContrib.Isolation.Samples.Contracts;

namespace Acme.Impl
{
    [IsolatedExport(typeof(IMovieProvider), HostPerInstance = true, Isolation = IsolationLevel.Process, IsolationGroup = "SomeName")]
    public class Provider2 : IMovieProvider
    {
        private List<Movie> _movies = new List<Movie>();
        private int _count = 0;

        public Provider2()
        {
            _movies.Add(new Movie { Title = "Movie 2 1" });
            _movies.Add(new Movie { Title = "Movie 2 2" });
            _movies.Add(new Movie { Title = "Movie 2 3" });
            _movies.Add(new Movie { Title = "Movie 2 4" });
        }

        public string Name
        {
            get { return "Provider 2 - throws exception."; }
        }

        public string Description
        {
            get { return "Returns a collection of movies. AppDomain name is " + AppDomain.CurrentDomain.FriendlyName; }
        }

        public IEnumerable<Movie> GetMovies(string filter)
        {
            if (_count++ == 1)
            {
                throw new InvalidOperationException("Non critical unhandled exception.");
            }

            return _movies;
        }
    }
}