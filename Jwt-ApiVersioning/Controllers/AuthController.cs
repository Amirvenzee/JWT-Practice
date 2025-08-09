using Asp.Versioning;
using Jwt_ApiVersioning.Context;
using Jwt_ApiVersioning.Dto;
using Jwt_ApiVersioning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Jwt_ApiVersioning.Controllers
{
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApiContext _Context;

        public AuthController(ApiContext context)
        {
            _Context = context;
        }

        [HttpPost("[Action]")]
        public IActionResult Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var customer = _Context.Customers.Any(x=>x.Mobile == model.Mobile);
            if (customer)
            {
                return BadRequest(new { error = "شماره موبایل تکراری است ." });
            }
            
            Customer newcustomer = new()
            {
                Mobile = model.Mobile,
                Pass = model.Pass,
                Role = Roles.User
            };

            _Context.Customers.Add(newcustomer);
            _Context.SaveChanges();
            return StatusCode(201,newcustomer);

           
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromBody] RegisterDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = _Context.Customers.FirstOrDefault(x=>x.Mobile == login.Mobile&& x.Pass==login.Pass);
            if (customer == null)
                return NotFound();
            
                var key = Encoding.ASCII.GetBytes("flkbgmboksbfbmbdf-fdbbfbSBDF-dfbbdaddafb-nflbnboibjr");

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDiscription = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    { 
                        new Claim(ClaimTypes.Name, customer.CustomerId.ToString()),
                        new Claim(ClaimTypes.Role, customer.Role.ToString()) 
                    }),

                    Expires = DateTime.Now.AddMinutes(2),

                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature),

                    Issuer = "Venzee",

                    Audience = "MyApi"

                };

            var token = tokenHandler.CreateToken(tokenDiscription);
            customer.JwtSecret = tokenHandler.WriteToken(token);

            

                
            
           
            return Ok(customer);
        }
    }
}
