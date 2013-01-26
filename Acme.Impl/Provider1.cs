using System;
using System.Collections.Generic;
using MefContrib.Hosting.Isolation;
using MefContrib.Isolation.Samples.Contracts;

namespace Acme.Impl
{
    [IsolatedExport(typeof(IMovieProvider), HostPerInstance = true, Isolation = IsolationLevel.Process, IsolationGroup = "SomeName")]
    public class Provider1 : IMovieProvider
    {
        private List<Movie> _movies = new List<Movie>();

        public Provider1()
        {
            _movies.Add(new Movie { Title = "Movie 1" });
            _movies.Add(new Movie { Title = "Movie 2" });
            _movies.Add(new Movie { Title = "Movie 3" });
            _movies.Add(new Movie { Title = "Movie 4" });
        }

        public string Name
        {
            get { return "Provider 1 - working OKAY"; }
        }

        public string Description
        {
            get { return "Returns a collection of movies. AppDomain name is " + AppDomain.CurrentDomain.FriendlyName; }
        }

        public IEnumerable<Movie> GetMovies(string filter)
        {
            return _movies;
        }
    }
}