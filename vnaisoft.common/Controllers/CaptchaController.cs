using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using quan_ly_kho.common.Helpers;
using quan_ly_kho.common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quan_ly_kho.common.Controllers
{
    public class UserEnteredCaptchaCodeModel
    {
        public string UserEnteredCaptchaCode { get; set; }
        public string CaptchaId { get; set; }
    }
    public class CaptchaResult
    {
        public string CaptchaCode { get; set; }
        public byte[] CaptchaByteData { get; set; }
        public string CaptchBase64Data => Convert.ToBase64String(CaptchaByteData);
        public DateTime Timestamp { get; set; }
    }
   
    [ApiController]
    [Route("[controller]")]
    public class CaptchaController : Controller
    {
     
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public CaptchaController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
       
            _mapper = mapper;
            _appSettings = appSettings.Value;
           
        }

        [AllowAnonymous]
        [HttpGet("GetCaptchaImage")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.Remove("CaptchaCode");
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }

        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {

            string sOTP = string.Empty;

            string sTempChars = string.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)

            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }

            return sOTP;

        }
    }
  
}
