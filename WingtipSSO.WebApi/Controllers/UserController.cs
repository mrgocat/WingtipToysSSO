using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WingtipSSO.BusinessLogicLayer;
using WingtipSSO.BusinessLogicLayer.Identity;
using WingtipSSO.DataAccessLayer;
using WingtipSSO.POCOS;
using WingtipSSO.WebApi.Models;

namespace WingtipSSO.WebApi.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;

        public UserController(IUserService service, IMapper mapper, IJwtProvider jwtProvider)
        {
            _service = service;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
        }

        [HttpPost("login")]
        public ActionResult PostSecurityLogin([FromBody]UserLoginDto request)
        {
            UserPoco poco = null;
            try
            {
                poco = _service.Authenticate(request.UserId, request.Password);
            }
            catch (LoginException ex)
            {
                return Unauthorized(ex.Message);
            }

            var token = _jwtProvider.GenerateJwtToken(poco);
            return Ok(token);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            var claimId = this.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            if(claimId == null)
            {
                return Unauthorized("Authentication Information required.");
            }
            string userId = claimId.Value;
            var poco = _service.Get(userId);
            return Ok(_mapper.Map<UserDto>(poco));
        }
        /*[HttpGet("{Id}")]
        public ActionResult Get(string userId)
        {
            var poco = _service.Get(userId);
            if (poco == null) return NotFound();
            return Ok(_mapper.Map<UserDto>(poco));
        }*/
        [HttpPost]
        public ActionResult Create([FromBody]UserCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserPoco poco = _mapper.Map<UserPoco>(dto);
            string userId = _service.Create(poco);
            return Created($"api/v1/users/{userId}", new { UserId = userId });
        }
    }
}
