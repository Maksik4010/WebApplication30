using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication30.Data;
using WebApplication30.Models;

namespace WebApplication30.Repositories
{
    public class BookReposiotry
    {
        // Properties
        public ApplicationDbContext DbContext { get; set; }

        // Constructor
        public BookReposiotry(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        // Methods
        public List<Book> ReadAll()
        {
            return DbContext.Books.ToList();
        }

        public Book Read(Guid id)
        {
            return DbContext.Books.FirstOrDefault(o => o.Id == id);
        }

        public void Create(Book enemy)
        {
            DbContext.Books.Add(enemy);
            DbContext.SaveChanges();
        }

        public void Update(Book enemy) // Should have id
        {
            DbContext.Books.Update(enemy);
            DbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            DbContext.Remove(new Book() { Id = id });
            DbContext.SaveChanges();
        }
    }
}
