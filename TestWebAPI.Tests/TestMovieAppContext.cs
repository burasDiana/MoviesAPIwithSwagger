using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWebAPI.Models;

namespace TestWebAPI.Tests
{
    class TestMovieAppContext : IMovieAppContext
    {
        public TestMovieAppContext()
        {
            Movies = new TestMovieDbSet();
        }

        public DbSet<Movie> Movies { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Movie item)
        {
            
        }

        public void Dispose()
        {
           
        }
    }
}
