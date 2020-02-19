using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;
using ServerApp.Models;

namespace ServerApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            _context = context;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
           var values = await _context.Values.OrderByDescending(x => x.DateCreated).ToListAsync();
           return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            return Ok( await _context.Values.FirstOrDefaultAsync(x=>x.Id.Equals(id)));
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Value value)
        {
            await _context.Values.AddAsync(value);
            await _context.SaveChangesAsync();
            return Ok("Value created");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
