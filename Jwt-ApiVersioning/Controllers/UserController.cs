using Asp.Versioning;
using Jwt_ApiVersioning.Context;
using Jwt_ApiVersioning.Dto;
using Jwt_ApiVersioning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jwt_ApiVersioning.Controllers
{
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiContext _context;

        public UserController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AllUsers()
        {
            var list = _context.Users.ToList();
            return Ok(list);
        }

        [HttpGet("[action]/{id:int}",Name = "Getdetail")]
        [Authorize]
        public IActionResult Getdetail([FromRoute] int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound();
            
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody]UserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User newUser = new()
            {
                Mobile = model.Mobile,
                Name = model.Name,
                Role = Roles.User
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return CreatedAtRoute("Getdetail", new {id=newUser.Id},newUser);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, [FromBody] EditUserDto model )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.FirstOrDefault(x=>x.Id == id);
            if(user == null)return NotFound();

            if (model.Name != null) user.Name =model.Name ;
            if (model.Mobile != null) user.Mobile = model.Mobile;

            _context.Users.Update(user);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult delete(int id)
        {
           var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();


        }
    }
}
