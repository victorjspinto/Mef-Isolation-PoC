using System;
using System.Collections.Generic;
using System.Threading;
using MefContrib.Hosting.Isolation;
using MefContrib.Isolation.Samples.Contracts;

namespace Acme.Impl
{
    [IsolatedExport(typeof(IMovieProvider), HostPerInstance = true, Isolation = IsolationLevel.Process, IsolationGroup = "SomeName")]
    public class Provider3 : IMovieProvider
    {
        private List<Movie> _movies = new List<Movie>();
        private int _count = 0;

        public Provider3()
        {
            _movies.Add(new Movie { Title = "Movie 3 1" });
            _movies.Add(new Movie { Title = "Movie 3 2" });
            _movies.Add(new Movie { Title = "Movie 3 3" });
            _movies.Add(new Movie { Title = "Movie 3 4" });
        }

        public string Name
        {
            get { return "Provider 3 - throws exception in a thread."; }
        }

        public string Description
        {
            get { return "Returns a collection of movies. AppDomain name is " + AppDomain.CurrentDomain.FriendlyName; }
        }

        public IEnumerable<Movie> GetMovies(string filter)
        {
            if (_count++ == 1)
            {
                Thread t = new Thread(() =>
                {
                    throw new InvalidOperationException("Non critical unhandled exception inside a thread.");
                });
                t.Start();
            }

            return _movies;
        }
    }
}