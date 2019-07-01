using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmailApi.Domain.Models;
using EmailApi.Domain.Services;
using EmailApi.Domain.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmailApi.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        // GET: /<controller>/
        private readonly IUserRepository _userService;

        public UsersController(IUserRepository userService)
        {
            _userService = userService;
        }

        //GET: user
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userService.GetAll();
        }

        //GET user/id
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //POST user
        [HttpPost]
        public IActionResult Post([FromBody]User value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var createdUser = _userService.Add(value);


            return CreatedAtRoute("GetUser", new { id = createdUser.UserId }, createdUser);
        }

        // PUT user/id
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]User value)
        {
            if (value == null)
            {
                return BadRequest();
            }


            var note = _userService.GetById(id);


            if (note == null)
            {
                return NotFound();
            }


            value.UserId = id;
            _userService.Update(value);


            return NoContent();


        }
        // DELETE user/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            _userService.Delete(user);


            return NoContent();
        }


    }

}

