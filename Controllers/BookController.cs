using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication30.Models;
using WebApplication30.Repositories;

namespace WebApplication30.Controllers
{
    public class BookController : ControllerBase
    {
        // Propertis 
        public BookReposiotry Books { get; set; }

        // Constructor
        public BookController(BookReposiotry book)
        {
            Books = book;
        }
        // Methods
        [HttpGet]
        [Route("Book")]
        public ActionResult Get()
        {
            var result = Books.ReadAll();
            return Ok(result);
        }

        [HttpGet()]
        [Route("Book/{id}")]
        public ActionResult Get(Guid id)
        {
            var result = Books.Read(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("Book")]
        public void Post([FromBody] Book book)
        {
            Books.Update(book);
        }

        [HttpPut]
        [Route("Enemy")]
        public void Put([FromBody] Book book)
        {
            Books.Create(book);
        }

        [HttpDelete()]
        [Route("Book/{id}")]
        public void Delete(Guid id)
        {
            Books.Delete(id);
        }
    }
}
