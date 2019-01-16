using System.Data.Entity;

namespace TestWebAPI.Models
{
    public class TestWebAPIContext : DbContext , IMovieAppContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public TestWebAPIContext() : base("name=TestWebAPIContext")
        {
        }

        public DbSet<Movie> Movies { get; set; }

        public void MarkAsModified(Movie item)
        {
            Entry(item).State = EntityState.Modified;
        }
    }
}
