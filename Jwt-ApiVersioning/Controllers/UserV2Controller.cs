using Asp.Versioning;
using Jwt_ApiVersioning.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jwt_ApiVersioning.Controllers
{
    [Route("api/v{version:ApiVersion}/User")]
    [Authorize(Roles ="Admin")]
    [ApiVersion("2.0")]
    [ApiController]
    public class UserV2Controller : ControllerBase
    {

        private readonly ApiContext _context;

        public UserV2Controller(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AllUsers()
        {
            var user = _context.Users.Find(1);
            return Ok(user);
        }
    }
}
