using System;
using System.Data.Entity;

namespace TestWebAPI.Models
{
    public interface IMovieAppContext : IDisposable
    {
        DbSet<Movie> Movies { get; }
        int SaveChanges();
        void MarkAsModified(Movie item);
    }
}
