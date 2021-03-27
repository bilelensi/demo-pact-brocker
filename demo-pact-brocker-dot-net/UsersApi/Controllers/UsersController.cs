using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using UsersApi.Models;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private static readonly User[] _users = new[]
        {
            new User {
                 ID = 1,
                Email= "toto.titi@mail.com",
                FirstName = "Toto",
                LastName = "Titi",
                Gender = GenderEnum.Male,
                Position = PositionEnum.Engineer,
                LastUpdate = DateTime.Now.AddDays(-30)
            },
            new User {
                ID = 2,
                Email= "bilel.rezgui@mail.com",
                FirstName = "Bilel",
                LastName = "Rezgui",
                Gender = GenderEnum.Male,
                Position = PositionEnum.Engineer,
                LastUpdate = DateTime.Now.AddDays(-4)
                
            },
            new User {
                ID = 3,
                Email = "lala.lili@mail.com",
                FirstName = "Lala",
                LastName = "Lili",
                Gender = GenderEnum.Female,
                Position = PositionEnum.Coordinator,
                LastUpdate = DateTime.Now.AddDays(-90)
            }
        };

        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
           return _users;
        }


        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var user = _users.FirstOrDefault(u => u.ID == id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
