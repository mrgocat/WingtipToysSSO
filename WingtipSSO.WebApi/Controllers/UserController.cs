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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserPoco poco = null;
            try
            {
                poco = _service.Authenticate(request.UserId, request.Password, request.LoginIP);
            }
            catch (LoginException ex)
            {
                return Unauthorized(ex.Message);
            }

            var token = _jwtProvider.GenerateJwtToken(poco);
            return Ok(new { token = token, username = poco.FirstName + " " + poco.LastName });
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
        
        [HttpGet("{Id}/CheckIdExists")]
        public ActionResult Get(string userId)
        {
            bool result = _service.CheckIdExists(userId);
            return Ok(new { Exists = result });
        }

        [HttpPost]
        public ActionResult Create([FromBody]UserCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserPoco poco = _mapper.Map<UserPoco>(dto);
            string userId = null;
            try
            {
                userId = _service.Create(poco);
            }
            catch(UpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            return Created($"api/v1/users/{userId}", new { UserId = userId });
        }

        [HttpPut]
        [Authorize]
        public ActionResult Update([FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var claimId = this.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            if (claimId == null)
            {
                return Unauthorized("Authentication Information required.");
            }
            string userId = claimId.Value;
            UserPoco poco = _mapper.Map<UserPoco>(dto);
            poco.Id = userId;
            _service.Update(poco);
            return Ok(new { result = "ok" });
        }

        [HttpPatch]
        [Authorize]
        public ActionResult Patch([FromBody] UserPasswordChangeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var claimId = this.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            if (claimId == null)
            {
                return Unauthorized("Authentication Information required.");
            }
            string userId = claimId.Value;
            try
            {
                _service.UpdatePasswrod(userId, dto.OldPassword, dto.NewPassword);
            }catch(UpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { result = "ok" });
        }
        [HttpPatch("{userId}")]
        [Authorize]
        public ActionResult Patch(string userId, [FromBody] UserKeyValueDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!UserKeyValueDto.AccecptedKeys.Any(e => e == dto.key))
            {
                return BadRequest($"You cannot change {dto.key} column");
            }

            var claimId = this.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            if (claimId == null)
            {
                return Unauthorized("Authentication Information required.");
            }
            userId = claimId.Value;
            try
            {
                _service.Patch(userId, dto.key, dto.value);
            }
            catch (UpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { result = "ok" });
        }
    }
}
