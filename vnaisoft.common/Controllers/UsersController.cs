using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using quan_ly_kho.common.Helpers;
using quan_ly_kho.common.Models.Users;
using quan_ly_kho.common.Services;
using quan_ly_kho.DataBase.System;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace quan_ly_kho.common.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IMongoClient _mongoClient;

        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IMongoClient mongoClient,
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");
            if (!string.IsNullOrEmpty(CaptchaCode))
            {
                if (CaptchaCode.ToLower() != model.capcha.ToLower()) return BadRequest(new { message = "Captcha không chính xác" });
            }
            if (user == null)
            {
                return BadRequest(new { message = "Tài khoản hoặc mật khẩu không chính xác" });
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            HttpContext.Session.Remove("CaptchaCode");
            // return basic user info and authentication token
            return Ok(new
            {
                user.id,
                username = user.Username,
                full_name = user.ho_va_ten,
                token = tokenString,
                type = user.loai,
            });
        }

        [HttpGet("getUser")]
        public IActionResult getUser()
        {
            string id_user = User.Identity.Name;
            var user = _userService.GetById(id_user);
            return Ok(new
            {
                user.id,
                user_name = user.Username,
                full_name = user.ho_va_ten,
            });
        }

        [AllowAnonymous]
        [HttpGet("checklogincapcha")]
        public IActionResult checklogincapcha()
        {
            var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");
            if (string.IsNullOrEmpty(CaptchaCode))
            {
                return Json("1");
            }
            else
            {
                return Json("2");
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {

                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.id = id;

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _userService.Delete(id);
            return Ok();
        }
        [HttpPost("changePassword")]
        public IActionResult ChangePassword([FromBody] JObject json)
        {
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());
            var UserId = User.Identity.Name;

            var oldPassword = dictionary["oldPassword"];
            var password = dictionary["password"];

            var user = _userService.Authenticate(null, oldPassword, UserId);

            if (user == null)
                return BadRequest(new { message = "Old Password is incorrect" });
            _userService.Update(user, password);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                user.id,
                username = user.Username,

                full_name = user.ho_va_ten,
                token = tokenString
            });
        }
    }
}
