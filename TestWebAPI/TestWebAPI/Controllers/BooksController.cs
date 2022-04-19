﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWebAPI.Models;
using TestWebAPI.Repositories;

namespace TestWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _bookRepository.Get();
        }

        [HttpGet("{title}")]
        public async Task<IEnumerable<Book>> GetBooks(string title)
        {
            return await _bookRepository.Get(title);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody] Book book)
        {
            var newBook = await _bookRepository.Create(book);
            return CreatedAtAction(nameof(GetBooks), new {id = newBook.Id}, newBook);
        }

        [HttpPut]
        public async Task<ActionResult> PutBooks(int id, [FromBody] Book book)
        {
            if(id != book.Id)
            {
                return BadRequest();
            }

            await _bookRepository.Update(book);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var bookToDelete = await _bookRepository.Get(id);
            if (bookToDelete == null)
                return NotFound();
            await _bookRepository.Delete(bookToDelete.Id);
            return NoContent();
        }
    }
}