using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using vnaisoft.common.BaseClass;
using vnaisoft.common.common;
using vnaisoft.common.Common;
using vnaisoft.common.Helpers;
using vnaisoft.common.Models;
using vnaisoft.common.Services;
using vnaisoft.DataBase.commonFunc;
using vnaisoft.DataBase.Mongodb;
using vnaisoft.DataBase.Mongodb.Collection.HocAI;
using vnaisoft.DataBase.System;
using vnaisoft.fireBase.API;
using vnaisoft.system.data.DataAccess;
using vnaisoft.system.data.Models;


namespace vnaisoft.system.web.Controller
{
    public partial class sys_userController : BaseAuthenticationController
    {
        private readonly HttpClient _httpClient;
        private sys_user_repo repo;
        public MongoClientFactory config;
        private IConfiguration _configuration;
        private IMailService _mailService;
        public AppSettings _appsetting;
        //public SendNotificationApi firebaseAPI;
        private IUserService _userService;

        public common_mongo_repo _common_repo;
        public sys_userController(HttpClient httpClient, IConfiguration configuration, IUserService userService, IMailService mailService, MongoDBContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            _httpClient = httpClient;
            _appsetting = appsetting.Value;
            repo = new sys_user_repo(context);
            _userService = userService;
            _mailService = mailService;
            //firebaseAPI = new SendNotificationApi(context);
            _configuration = configuration;
            config = new MongoClientFactory(configuration);
            _common_repo = new common_mongo_repo(context);
        }

        public class ZipItem
        {
            public string doc_no { get; set; }
            public string file_name { get; set; }
            public string file_path { get; set; }

        }
        public class FacebookUser
        {
            public string id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string photoUrl { get; set; }


        }
        public string GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                // Lấy Secret key từ file cấu hình (phải giống hệt key ở Source 1)
                var key = Encoding.ASCII.GetBytes(_appsetting.Secret);

                // Cấu hình các tham số để xác thực token
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // Đặt là false nếu bạn không set Issuer khi tạo token
                    ValidateAudience = false, // Đặt là false nếu bạn không set Audience khi tạo token
                    ClockSkew = TimeSpan.Zero // Không cho phép chênh lệch thời gian
                };
                try
                {

                    // Phương thức ValidateToken sẽ tự động throw exception nếu token không hợp lệ
                    // bao gồm cả trường hợp token hết hạn
                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                    var userIdClaim = principal.FindFirst(ClaimTypes.Name);

                    if (userIdClaim == null)
                    {
                        // Trường hợp token hợp lệ nhưng không tìm thấy claim cần thiết
                        throw new SecurityTokenValidationException("Token không chứa thông tin User ID.");
                    }

                    return userIdClaim.Value;
                }
                catch (Exception)
                {
                    // Ném lại exception để khối try-catch bên ngoài (trong Action) có thể xử lý
                    throw;
                }
                // Xác thực token

            }
            catch
            {
                // Nếu có lỗi trong quá trình xác thực (token sai, hết hạn,...) thì trả về null
                return null;
            }
        }


        public async Task<IActionResult> signInWithFacebook([FromBody] JObject json)
        {
            var accessToken = json.GetValue("data").ToString();
            // Construct the Graph API URL
            var graphApiUrl = $"https://graph.facebook.com/me?fields=id,name,email,photos&access_token={accessToken}";

            try
            {
                // Send the request to the Graph API
                var response = await _httpClient.GetAsync(graphApiUrl);
                response.EnsureSuccessStatusCode();

                // Parse the response
                var userData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<FacebookUser>(userData);
                var user = repo._context.sys_user_col.AsQueryable().Where(q => q.email == data.email).SingleOrDefault();



                if (user == null)
                {
                    var newUser = new User
                    {
                        id = Guid.NewGuid().ToString(),
                        ho_va_ten = data.name ?? "", // Tên đầy đủ từ Google
                        hinh_anh_dai_dien = "", // Ảnh đại diện từ Google
                        loai = 1, // Loại mặc định
                        Username = data.email ?? "", // Sử dụng tên đầu tiên làm username (nếu có)
                        email = data.email ?? "", // Email từ Google
                        dia_chi = "", // Địa chỉ để trống nếu chưa có thông tin
                        status_del = 1, // Trạng thái mặc định
                        ngay_cap_nhat = DateTime.Now, // Có thể đặt giá trị này là ID admin hoặc chính ID người dùng
                        so_dien_thoai = "" // Điền sau nếu cần
                    };
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("123456@#", out passwordHash, out passwordSalt);

                    newUser.PasswordHash = passwordHash;
                    newUser.PasswordSalt = passwordSalt;

                    // Lưu người dùng vào cơ sở dữ liệu
                    repo._context.sys_user_col.InsertOne(newUser);
                    user = newUser;
                }


                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appsetting.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,"school")
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
                    id = user.id,
                    username = user.Username,
                    avatar_path = user.hinh_anh_dai_dien,
                    email = user.email,
                    full_name = user.ho_va_ten,
                    status_del = user.status_del,
                    token = tokenString,
                    type = user.loai,
                    host = "https://" + Request.Host.Value
                });

            }
            catch (HttpRequestException ex)
            {
                // Handle errors, such as invalid tokens or API rate limits
                return BadRequest(new { message = "Invalid token", error = ex.Message });
            }
        }


        public async Task<IActionResult> GoogleLogin([FromBody] JObject json)
        {
            try
            {
                var idToken = json.GetValue("idToken").ToString();
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

                var user = repo._context.sys_user_col.AsQueryable().Where(q => q.email == payload.Email).SingleOrDefault();


                if (user == null)
                {
                    var newUser = new User
                    {
                        id = Guid.NewGuid().ToString(),
                        ho_va_ten = payload.Name ?? "", // Tên đầy đủ từ Google
                        hinh_anh_dai_dien = payload.Picture ?? "", // Ảnh đại diện từ Google
                        loai = 1, // Loại mặc định
                        Username = payload.Email ?? "", // Sử dụng tên đầu tiên làm username (nếu có)
                        email = payload.Email ?? "", // Email từ Google
                        dia_chi = "", // Địa chỉ để trống nếu chưa có thông tin
                        status_del = 1, // Trạng thái mặc định
                        ngay_cap_nhat = DateTime.Now, // Thời gian hiện tại
                        nguoi_cap_nhat = payload.Email, // Có thể đặt giá trị này là ID admin hoặc chính ID người dùng
                        so_dien_thoai = "" // Điền sau nếu cần
                    };
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("123456@#", out passwordHash, out passwordSalt);

                    newUser.PasswordHash = passwordHash;
                    newUser.PasswordSalt = passwordSalt;

                    // Lưu người dùng vào cơ sở dữ liệu
                    repo._context.sys_user_col.InsertOne(newUser);
                    user = newUser;
                }


                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appsetting.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,"school")
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
                    id = user.id,
                    username = user.Username,
                    avatar_path = user.hinh_anh_dai_dien,
                    email = user.email,
                    full_name = user.ho_va_ten,
                    status_del = user.status_del,
                    token = tokenString,
                    type = user.loai,
                    host = "https://" + Request.Host.Value
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid token", error = ex.Message });
            }
        }
        public async Task<IActionResult> send_mail_otp_new([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            var email = model.db.Username;

            var code = "mau_mail_otp";

            //string mail = json?["data"]?["db"]?["username"]?.ToString().Trim();
            var user = new User();
            if (string.IsNullOrEmpty(model.password))
            {
                ModelState.AddModelError("password", "required");
            }
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            else
            {

                user = repo._context.sys_user_col.AsQueryable().Where(q => q.email == email.Trim() || q.Username == email.Trim()).FirstOrDefault();
                if (user == null)
                {
                    var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                  + "@"
                                  + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                    var checkEmail = rgEmail.IsMatch(email);
                    if (checkEmail == false)
                    {
                        ModelState.AddModelError("db.email", "Email không hợp lệ");
                    }
                    else
                    {
                        ModelState.AddModelError("db.email", "Tên đăng nhập hoặc mật khẩu không chính xác. Vui lòng thử lại.\"");
                    }
                }
                else
                {
                    if (!VerifyPasswordHash(model.password, user.PasswordHash, user.PasswordSalt))
                        ModelState.AddModelError("password", "Mật khẩu không chính xác");
                }
            }
            if (model.showCaptcha == 1)
            {
                if (string.IsNullOrEmpty(model.capcha))
                {
                    ModelState.AddModelError("capcha", "required");
                }
                else
                {
                    var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");
                    if (CaptchaCode.ToLower() != model.capcha.ToLower())
                    {
                        ModelState.AddModelError("capcha", "captcha_invalid");
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                return generateError();
            }





            var rnd = new Random();
            var otp = rnd.Next(111111, 999999).ToString();
            var pass = GenerateRandomPassword(12);
            try
            {

                var msg = "";
                var body = "";

                var maumail = repo._context.sys_template_mail_col.AsQueryable().Where(t => t.type == code).FirstOrDefault();
                //maumail.noidung ?? "";
                body = maumail.template;
                body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
                body = body.Replace("@@link_dang_nhap@@", "https://" + Request.Host.Value + "/sign-in");
                body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
                body = body.Replace("@@mat_khau@@", pass);
                body = body.Replace("@@otp@@", otp);
                body = body.Replace("@@tai_khoan@@", email);
                body = body.Replace("@@full_name@@", model.db.email);// user.ho_va_ten);
                body = body.Replace("@@current_year@@", DateTime.Now.Year.ToString());

                var dblogmail = new sys_log_email_col();
                dblogmail.tieu_de = maumail.name;
                dblogmail.noi_dung = body;
                dblogmail.id_template = maumail.id;
                dblogmail.email = email;// user.email;
                dblogmail.id = Guid.NewGuid().ToString();
                dblogmail.send_date = DateTime.Now;
                dblogmail.user_id = null;// id;
                dblogmail.ket_qua = 0;
                dblogmail.status_del = 2;
                dblogmail.otp = otp;
                await repo._context.sys_log_email_col.InsertOneAsync(dblogmail);

                try
                {
                    //if(email == "administrator") { email = "thevinhbk0412@gmail.com"; }
                    // mo lai khi demo
                    _mailService.SendEmailAsync(new MailRequest
                    {
                        Body = body,
                        Subject = dblogmail.tieu_de,
                        ToEmail = email, //CMAESCrypto.DecryptText(),
                        CCEmail = "",
                    });

                }
                catch (Exception e)
                {

                    return Json("error:" + e.ToString());

                }

            }
            catch
            {
                return Json("error");

            }
            return Json("");


        }
        public async Task<IActionResult> send_mail_otp([FromBody] JObject json)
        {

            var code = "mau_mail_otp";
            string mail = json?["data"]?["db"]?["username"]?.ToString().Trim();

            if (mail == "administrator")
                return Json("");
            if (mail == "admintruonghoc")
                return Json("");

            var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                            + "@"
                            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
            var checkEmail = rgEmail.IsMatch(mail);
            if (checkEmail == false)
            {
                ModelState.AddModelError("db.username", "Vui lòng nhập đúng định dạng");
                return generateError();
            }

            var user = repo._context.sys_user_col.AsQueryable().Where(q => q.email == mail.Trim() || q.Username == mail.Trim()).FirstOrDefault();
            var model = new sys_user_model();
            if (user == null)
            {
                Response.StatusCode = 400;
                return Json(new { result = "not_exist", msg = "Tên đăng nhập hoặc mật khẩu không chính xác. Vui lòng thử lại." });
            }

            var rnd = new Random();
            var otp = rnd.Next(111111, 999999).ToString();
            var pass = GenerateRandomPassword(12);
            try
            {
                var email = mail;
                var msg = "";
                var body = "";

                var maumail = repo._context.sys_template_mail_col.AsQueryable().Where(t => t.type == code).FirstOrDefault();
                //maumail.noidung ?? "";
                body = maumail.template;
                body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
                body = body.Replace("@@link_dang_nhap@@", "https://" + Request.Host.Value + "/sign-in");
                body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
                body = body.Replace("@@mat_khau@@", pass);
                body = body.Replace("@@otp@@", otp);
                body = body.Replace("@@tai_khoan@@", email);
                body = body.Replace("@@full_name@@", mail);// user.ho_va_ten);
                body = body.Replace("@@current_year@@", DateTime.Now.Year.ToString());

                var dblogmail = new sys_log_email_col();
                dblogmail.tieu_de = maumail.name;
                dblogmail.noi_dung = body;
                dblogmail.id_template = maumail.id;
                dblogmail.email = mail;// user.email;
                dblogmail.id = Guid.NewGuid().ToString();
                dblogmail.send_date = DateTime.Now;
                dblogmail.user_id = null;// id;
                dblogmail.ket_qua = 0;
                dblogmail.status_del = 2;
                dblogmail.otp = otp;
                await repo._context.sys_log_email_col.InsertOneAsync(dblogmail);

                try
                {
                    // mo lai khi demo
                    //_mailService.SendEmailAsync(new MailRequest
                    //{
                    //    Body = body,
                    //    Subject = dblogmail.tieu_de,
                    //    ToEmail = email, //CMAESCrypto.DecryptText(),
                    //    CCEmail = "",
                    //});

                }
                catch (Exception e)
                {

                    return Json("error:" + e.ToString());

                }

            }
            catch
            {
                return Json("error");

            }
            return Json("");


        }

        public static string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(chars[rnd.Next(chars.Length)]);
            }
            return res.ToString();
        }
        [HttpPost]
        public async Task<IActionResult> confirm_otp([FromBody] JObject json)
        {
            //1 hô nông dân , 2 hợp tác xã
            var otp = json.GetValue("otp").ToString();
            var username = json.GetValue("username").ToString();
            var timeCheck = DateTime.Now.AddMinutes(-30);
            var isEmail = username.Contains("@");

            var user = repo._context.sys_user_col.AsQueryable().Where(q => q.email == username || username == "administrator").FirstOrDefault();
            //update  User status_del = 1 khi xac thuc thanh cong
            var model = new sys_user_model();
            if (user != null && user.status_del != 1)
            {
                var filterUser = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(d => d.id, user.id));
                var update1 = Builders<User>.Update
                       .Set(x => x.status_del, 1)
                   .Set(x => x.ngay_login, DateTime.Now);
                repo._context.sys_user_col.UpdateOne(filterUser, update1);
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsetting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user == null ?  model.db.id :user.id.ToString() ),
                    new Claim(ClaimTypes.NameIdentifier,"school")

                }),
                Expires = DateTime.UtcNow.AddYears(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new
            {
                id = user.id.ToString(),
                username = user.Username,
                email = user.email,
                full_name = user.ho_va_ten,
                status_del = user.status_del,
                token = tokenString,
                type = user.loai,
                host = "https://" + Request.Host.Value
            });

        }
        [HttpPost]
        public async Task<IActionResult> confirm_otp_new([FromBody] JObject json)
        {
            //1 hô nông dân , 2 hợp tác xã
            var otp = json.GetValue("otp").ToString();
            var username = (json.GetValue("username").ToString() ?? "").ToLower();
            var timeCheck = DateTime.Now.AddMinutes(-30);
            var isEmail = username.Contains("@");

            // Comment de demo
            //// tru so nay de apple submit
            if ((otp == "793210"))
            {
                var user = repo._context.sys_user_col.AsQueryable().Where(q => q.Username.ToLower().Equals(username)).FirstOrDefault();


                //var user = repo._context.sys_user_col.AsQueryable().Where(q => q.Username.ToLower().Equals(username) || username == "administrator").FirstOrDefault();


                //update  User status_del = 1 khi xac thuc thanh cong
                var model = new sys_user_model();
                if (user != null && user.status_del != 1)
                {
                    var filterUser = Builders<User>.Filter.And(
                    Builders<User>.Filter.Eq(d => d.id, user.id));
                    var update1 = Builders<User>.Update
                           .Set(x => x.status_del, 1)
                       .Set(x => x.ngay_login, DateTime.Now);
                    repo._context.sys_user_col.UpdateOne(filterUser, update1);
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appsetting.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                   new Claim(ClaimTypes.Name, user == null ?  model.db.id :user.id.ToString() ),
                    new Claim(ClaimTypes.NameIdentifier,"school")
                    }),
                    Expires = DateTime.UtcNow.AddYears(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new
                {
                    id = user.id.ToString(),
                    username = user.Username,
                    email = user.email,
                    full_name = user.ho_va_ten,
                    status_del = user.status_del,
                    token = tokenString,
                    //school_id = school_id.id,
                    type = user.loai,
                    host = "https://" + Request.Host.Value
                });
            }
            else
            {
                //if (username == "administrator" && otp != "793210")
                //{
                //    ModelState.AddModelError("db.otp", "ma_xac_thuc_khong_hop_le");
                //    return generateError();
                //}
                if (isEmail)
                {

                    var check_otp = repo._context.sys_log_email_col.AsQueryable().Where(q => q.email == username && q.otp == otp && q.send_date > timeCheck && q.status_del == 2).OrderByDescending(q => q.send_date).FirstOrDefault();
                    if (check_otp == null)
                    {
                        var check_otp_user = repo._context.sys_user_col.AsQueryable().Where(q => q.email == username && q.otp == otp).FirstOrDefault();
                        if (check_otp_user == null)
                        {
                            ModelState.AddModelError("db.otp", "ma_xac_thuc_khong_hop_le");
                            return generateError();
                        }
                        else
                        {

                            var user = repo._context.sys_user_col.AsQueryable().Where(q => q.email == username || username == "administrator").FirstOrDefault();
                            //update  User status_del = 1 khi xac thuc thanh cong
                            var model = new sys_user_model();
                            if (user != null && user.status_del != 1)
                            {
                                var filterUser = Builders<User>.Filter.And(
                                Builders<User>.Filter.Eq(d => d.id, user.id));
                                var update1 = Builders<User>.Update
                                       .Set(x => x.status_del, 1)
                                   .Set(x => x.ngay_login, DateTime.Now);
                                repo._context.sys_user_col.UpdateOne(filterUser, update1);
                            }
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes(_appsetting.Secret);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                    new Claim(ClaimTypes.Name, user == null ?  model.db.id :user.id.ToString() ),
                    new Claim(ClaimTypes.NameIdentifier,"school")
                                }),
                                Expires = DateTime.UtcNow.AddYears(2),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                            };
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            var tokenString = tokenHandler.WriteToken(token);
                            return Ok(new
                            {
                                id = user.id.ToString(),
                                username = user.Username,
                                email = user.email,
                                full_name = user.ho_va_ten,
                                status_del = user.status_del,
                                token = tokenString,
                                type = user.loai,
                                host = "https://" + Request.Host.Value
                            });
                        }
                    }
                    else
                    {
                        var filterotp = Builders<sys_log_email_col>.Filter.And(
                        Builders<sys_log_email_col>.Filter.Eq(d => d.id, check_otp.id));
                        check_otp.status_del = 1;
                        check_otp.active_date = DateTime.Now;
                        repo._context.sys_log_email_col.ReplaceOne(filterotp, check_otp);

                        var user = repo._context.sys_user_col.AsQueryable().Where(q => q.email == username || username == "administrator").FirstOrDefault();
                        //update  User status_del = 1 khi xac thuc thanh cong
                        var model = new sys_user_model();
                        if (user != null && user.status_del != 1)
                        {
                            var filterUser = Builders<User>.Filter.And(
                            Builders<User>.Filter.Eq(d => d.id, user.id));
                            var update1 = Builders<User>.Update
                                   .Set(x => x.status_del, 1)
                               .Set(x => x.ngay_login, DateTime.Now);
                            repo._context.sys_user_col.UpdateOne(filterUser, update1);
                        }
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_appsetting.Secret);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                    new Claim(ClaimTypes.Name, user == null ?  model.db.id :user.id.ToString() ),
                    new Claim(ClaimTypes.NameIdentifier,"school")
                            }),
                            Expires = DateTime.UtcNow.AddYears(2),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var tokenString = tokenHandler.WriteToken(token);
                        return Ok(new
                        {
                            id = user.id.ToString(),
                            username = user.Username,
                            email = user.email,
                            full_name = user.ho_va_ten,
                            status_del = user.status_del,
                            token = tokenString,
                            type = user.loai,
                            host = "https://" + Request.Host.Value
                        });
                    }

                }
                else
                {
                    //var check_otp = repo._context.sys_send_otp_col.AsQueryable().Where(q => q.phone == username && q.otp == otp && q.send_date > timeCheck && q.status_del == 2).OrderByDescending(q => q.send_date).FirstOrDefault();


                    var check_otp = repo._context.sys_user_col.AsQueryable().Where(q => q.Username == username && q.otp == otp).FirstOrDefault();



                    if (check_otp == null)
                    {
                        ModelState.AddModelError("db.otp", "ma_xac_thuc_khong_hop_le");
                        return generateError();
                    }
                    else
                    {
                        //var filterotp = Builders<sys_send_otp_col>.Filter.And(
                        //Builders<sys_send_otp_col>.Filter.Eq(d => d.id, check_otp.id));
                        //check_otp.status_del = 1;
                        //check_otp.active_date = DateTime.Now;
                        //repo._context.sys_send_otp_col.ReplaceOne(filterotp, check_otp);

                        var user = repo._context.sys_user_col.AsQueryable().Where(q => q.Username == username || username == "administrator").FirstOrDefault();
                        //update  User status_del = 1 khi xac thuc thanh cong
                        var model = new sys_user_model();
                        if (user != null && user.status_del != 1)
                        {
                            var filterUser = Builders<User>.Filter.And(
                            Builders<User>.Filter.Eq(d => d.id, user.id));
                            var update1 = Builders<User>.Update
                                   .Set(x => x.status_del, 1)
                               .Set(x => x.ngay_login, DateTime.Now);
                            repo._context.sys_user_col.UpdateOne(filterUser, update1);
                        }
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_appsetting.Secret);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                    new Claim(ClaimTypes.Name, user == null ?  model.db.id :user.id.ToString() ),
                    new Claim(ClaimTypes.NameIdentifier,"school")
                            }),
                            Expires = DateTime.UtcNow.AddYears(2),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var tokenString = tokenHandler.WriteToken(token);
                        return Ok(new
                        {
                            id = user.id.ToString(),
                            username = user.Username,
                            email = user.email,
                            full_name = user.ho_va_ten,
                            status_del = user.status_del,
                            token = tokenString,
                            type = user.loai,
                            host = "https://" + Request.Host.Value
                        });
                    }
                }
            }


        }
        public async Task<IActionResult> authenticate([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            //byte[] passwordHash, passwordSalt;
            // model = new sys_user_model();
            //CreatePasswordHash("123456@#", out passwordHash, out passwordSalt);
            //model.db.PasswordHash = passwordHash;
            //model.db.PasswordSalt = passwordSalt;
            //model.db.id = "administrator";
            //model.db.email = "administrator";
            //model.db.phone = "0357818605";

            //model.db.ho_va_ten = "administrator";
            //model.db.Username = "administrator";
            //model.db.nguoi_cap_nhat = "administrator";

            //model.db.hinh_anh_dai_dien =  "" ;

            //model.db.ngay_cap_nhat = DateTime.Now;
            //model.db.status_del = 1;
            //model.db.loai = 1;
            //await repo.insert(model);

            //var doc = await repo._context._database.GetCollection<BsonDocument>("User").Find(_ => true).ToListAsync();
            //  var doc1 = await repo._context._database.GetCollection<User>("User").Find(_ => true).ToListAsync();
            // var nt = repo._context.User.AsQueryable().FirstOrDefault();


            //var check = checkModelStateCreate(model);



            var user = new User();
            if (string.IsNullOrEmpty(model.password))
            {
                ModelState.AddModelError("password", "required");
            }
            if (string.IsNullOrEmpty(model.db.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            else
            {
                var email = model.db.email;


                user = repo._context.sys_user_col.AsQueryable().Where(d => d.Username == email && d.status_del == 1).SingleOrDefault();
                if (user == null)
                {
                    var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                  + "@"
                                  + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                    var checkEmail = rgEmail.IsMatch(email);
                    if (checkEmail == false)
                    {
                        ModelState.AddModelError("db.email", "Email không hợp lệ");
                    }
                    else
                    {
                        ModelState.AddModelError("db.email", "Email này chưa đăng ký");
                    }
                }
                else
                {
                    if (!VerifyPasswordHash(model.password, user.PasswordHash, user.PasswordSalt))
                        ModelState.AddModelError("password", "Mật khẩu không chính xác");
                }
            }
            if (model.showCaptcha == 1)
            {
                if (string.IsNullOrEmpty(model.capcha))
                {
                    ModelState.AddModelError("capcha", "required");
                }
                else
                {
                    var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");
                    if (CaptchaCode.ToLower() != model.capcha.ToLower())
                    {
                        ModelState.AddModelError("capcha", "captcha_invalid");
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                return generateError();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsetting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,"school")
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
                id = user.id,
                username = user.Username,
                avatar_path = user.hinh_anh_dai_dien,
                email = user.email,
                full_name = user.ho_va_ten,
                status_del = user.status_del,
                token = tokenString,
                type = user.loai,
                host = "https://" + Request.Host.Value
            });
        }
        public async Task<IActionResult> moi_lai([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.update_status_del(id, getUserId(), 3);
            //   send_mail(id, "10");
            return Json("");
        }
        public async Task<IActionResult> get_host()

        {
            var host = Request.Host.Value;
            return Json(host);
        }



        public async Task<IActionResult> go_bo([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.update_status_del(id, getUserId(), 2);
            //send_mail(id, "12");
            return Json("");
        }
        //private async Task<IActionResult> send_mail(string id, string type)
        //{
        //    var rnd = new Random();
        //    var code = rnd.Next(11111, 99999).ToString();
        //    try
        //    {
        //        var user = repo._context.sys_user_col.AsQueryable().Where(q => q.id == id).FirstOrDefault();


        //        var email = user.email;
        //        var msg = "";
        //        var body = "";
        //       var maumail = repo._context.erp_mau_mails.AsQueryable().Where(t => t.ma == type).FirstOrDefault();


        //        body = maumail.noi_dung;
        //        body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
        //        body = body.Replace("@@link_dang_nhap@@", "https://" + Request.Host.Value + "/sign-in");
        //        body = body.Replace("@@user_name@@", user.email);
        //        body = body.Replace("@@current_year@@", DateTime.Now.Year.ToString());
        //        body = body.Replace("@@link_loi_moi@@", "https://" + Request.Host.Value + "/invitationNonLogin/" + id);
        //        body = body.Replace("@@ten_cong_ty@@", cong_ty.name);
        //        body = body.Replace("@@dia_chi_cong_ty@@", cong_ty.dia_chi);
        //        body = body.Replace("@@dien_thoai_cong_ty@@", cong_ty.dien_thoai);
        //        body = body.Replace("@@email_cong_ty@@", cong_ty.email);




        //        var dblogmail = new sys_log_mail_db();
        //        dblogmail.tieu_de = maumail.ten;
        //        dblogmail.noi_dung = body;
        //        dblogmail.id_template = maumail.id;
        //        dblogmail.email = user.email;
        //        dblogmail.id = Guid.NewGuid().ToString();
        //        dblogmail.send_date = DateTime.Now;
        //        dblogmail.user_id = id;
        //        dblogmail.ket_qua = 0;

        //        repo._context.sys_log_mails.InsertOne(dblogmail);

        //        try
        //        {
        //            _mailService.SendEmailAsync(new MailRequest
        //            {
        //                Body = body,
        //                Subject = dblogmail.tieu_de,
        //                ToEmail = email, 
        //                CCEmail = "",
        //            });
        //        }
        //        catch (Exception e)
        //        {
        //            return Json("error:" + e.ToString());
        //        }
        //    }
        //    catch
        //    {
        //        return Json("error");
        //    }
        //    return Json("");
        //}


        //[HttpPost]
        //public async Task<IActionResult> xac_thuc([FromBody] JObject json)
        //{
        //    var user_id = "";
        //    var code = "";
        //    try { user_id = json.GetValue("user_id").ToString(); } catch { }
        //    try { code = json.GetValue("code").ToString(); } catch { }
        //    var user = repo._context.sys_user_col.AsQueryable().Where(t => t.id == user_id).FirstOrDefault();
        //    if (user != null)
        //    {
        //        if (user.otp == code)
        //        {

        //            send_mail(user_id, "12");

        //            var epochTime = ConverDateToEpochTime(DateTime.Now);

        //            //Thông tin cá nhân_Chào mừng
        //            var user_recive = new List<String>();
        //            user_recive.Add(user.id);
        //            var type = "1";

        //            //var modelN = new sys_notification_ref_model();

        //            //await firebaseAPI.send_notificatin_web("", type, modelN, user_recive);

        //            //await firebaseAPI.sendNotification(user.id, user.full_name, epochTime, user, user_recive, _appsetting.domain, user, "");
        //            return Json("");
        //        }
        //        else
        //        {
        //            return Json("Mã xác thực không đúng");
        //        }
        //    }
        //    else
        //    {
        //        return Json("Mã xác thực không đúng");
        //    }
        //}

        public async Task<IActionResult> getUserLogin()
        {
            var user_id = getUserId();
            var result = repo.FindAll().Where(d => d.id == user_id).FirstOrDefault();
            return Json(result);
        }

        public IActionResult getUserLoginSS0()
        {
            var user_id = getUserId();
            var result = repo.FindAll().Where(d => d.id == user_id).FirstOrDefault();
            //if (result != null)
            //{
            //    if (string.IsNullOrEmpty(result.hinh_anh_dai_dien))
            //    {
            //        var cau_hinh_anh = repo._context.sys_cau_hinh_anh_mac_dinh_col.AsQueryable().Where(q => q.type == 7).FirstOrDefault();

            //        var file_path = repo._context.sys_file_upload_col.AsQueryable().Where(q => q.id == cau_hinh_anh.image).Select(q => q.file_path).SingleOrDefault();

            //        result.hinh_anh_dai_dien = file_path;
            //    }
            //}
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> getUserOtp([FromBody] JObject json)
        {

            var user_id = json.GetValue("id").ToString();

            var result = repo.FindAll().Where(d => d.id == user_id).Select(d => d.email).FirstOrDefault();

            return Json(result);
        }
        public async Task<IActionResult> getListUse([FromBody] JObject json)
        {
            var search = "";
            try
            {
                search = (json.GetValue("search").ToString() ?? "").ToLower();
            }
            catch
            {

            }
            var result = repo._context.sys_user_col.AsQueryable()
                .Where(d => d.status_del == 1)
                 .Where(t => t.ho_va_ten.ToLower().StartsWith(search.ToLower()) ||
                 t.so_dien_thoai.StartsWith(search) || t.email.ToLower().Equals(search) || search == "")
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.ho_va_ten,
                     avatar = d.hinh_anh_dai_dien,
                     email = d.email,
                 }).Take(10).ToList();
            return Json(result);
        }
        public async Task<IActionResult> getListUseNew()
        {

            var result = repo._context.sys_user_col.AsQueryable()
                .Where(d => d.status_del == 1 && (d.loai == 2 || d.loai == 1))

                 .Select(d => new
                 {
                     id = d.id,
                     name = d.ho_va_ten,
                     avatar = d.hinh_anh_dai_dien,
                     email = d.email,
                 }).Take(10).ToList();
            return Json(result);
        }




        [HttpPost]
        public async Task<IActionResult> getListUserQuanTri([FromBody] JObject json)
        {
            var search = "";
            try
            {
                search = (json.GetValue("search").ToString() ?? "").ToLower();
            }
            catch
            {

            }
            var result = repo._context.sys_user_col.AsQueryable()
                .Where(d => d.status_del == 1 && d.loai == 1)
                 .Where(t => t.so_dien_thoai.ToLower().Equals(search) || t.email.ToLower().Equals(search))
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.ho_va_ten
                 }).Take(5).ToList();
            return Json(result);
        }



        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            //var company = repo._context.erp_cong_tys.AsQueryable().FirstOrDefault();
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
            model.db.PasswordHash = passwordHash;
            model.db.PasswordSalt = passwordSalt;
            model.db.id = ObjectId.GenerateNewId().ToString();
            model.db.email = model.email.ToLower();
            model.db.so_dien_thoai = model.phone;
            model.db.Username = model.email.ToLower();
            model.db.ho_va_ten = model.ho_va_ten;
            // model.db.Username = model.email.ToLower();
            model.db.nguoi_cap_nhat = getUserId();
            //model.db.hinh_anh_dai_dien = _common_repo.get_anh_dai_dien(model.hinh_anh_dai_dien, 1);
            model.db.ngay_cap_nhat = DateTime.Now;
            model.db.status_del = 1;
            model.db.loai = 1;
            await repo.insert(model);

            model.file.id_phieu = model.db.id;
            // await _common_repo.upset_file(model.file);
            return Json(model);
        }


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




        [HttpPost]
        public async Task<IActionResult> register([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            if (string.IsNullOrEmpty(model.db.ho_va_ten))
            {
                ModelState.AddModelError("db.full_name", "required");
            }
            //if (string.IsNullOrEmpty(model.capcha))
            //{
            //    ModelState.AddModelError("capcha", "required");
            //}
            //else
            //{
            //    var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");
            //    if (CaptchaCode.ToLower() != model.capcha.ToLower())
            //    {
            //        ModelState.AddModelError("capcha", "captcha_invalid");
            //        return generateError();
            //    }

            //}



            if (string.IsNullOrEmpty(model.password))
            {
                ModelState.AddModelError("db.password", "required");
            }
            else
            {
                if (model.password.Length < 8)
                {
                    ModelState.AddModelError("db.password", "msgmatkhau");
                }
            }

            if (string.IsNullOrEmpty(model.repassword))
            {
                ModelState.AddModelError("db.repass", "required");

            }
            else
            {
                if (model.password != model.repassword && model.password != "")
                {
                    ModelState.AddModelError("db.repass", "matkhaukhongkhop");
                }
            }
            if (string.IsNullOrEmpty(model.db.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            else
            {
                var email = model.db.email.Trim();
                var user = repo._context.sys_user_col.AsQueryable().Where(d => d.Username == email).SingleOrDefault();
                if (user == null)
                {
                    var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                    + "@"
                                    + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                    var checkEmail = rgEmail.IsMatch(email);
                    if (checkEmail == false)
                    {
                        ModelState.AddModelError("db.email", "Email chưa đúng định dạng");
                    }
                    else
                    {
                    }
                }
                else
                {
                    ModelState.AddModelError("db.email", "Email này đã tồn tại trong hệ thống");
                }
            }
            if (!ModelState.IsValid)
            {
                return generateError();
            }



            model.db.id = Guid.NewGuid().ToString();
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(model.password, out passwordHash, out passwordSalt);

            model.db.PasswordHash = passwordHash;
            model.db.PasswordSalt = passwordSalt;

            model.db.status_del = 1;
            model.db.Username = model.db.email;
            //1 admin 0 user thường
            model.db.loai = 1;


            var i = await repo.insert(model);
            return Json(model.db.id);
        }

        //public async Task send_otp(string id, string type)
        //{
        //    var rnd = new Random();
        //    var code = rnd.Next(11111, 99999).ToString();
        //    var user = repo._context.sys_user_col.AsQueryable().Where(q => q.id == id).FirstOrDefault();
        //    user.otp = code;
        //    var update = Builders<User>.Update
        //            .Set(x => x.otp, code);

        //    var filter = Builders<User>.Filter.Eq(x => x.id, user.id);
        //    repo._context.sys_user_col.UpdateOne(filter, update);

        //    var email = user.email;
        //    var msg = "";
        //    var body = "";

        //    var maumail = repo._context.erp_mau_mails.AsQueryable().Where(t => t.ma == type).FirstOrDefault();
        //    body = maumail.noi_dung;
        //    body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
        //    body = body.Replace("@@link_dang_nhap@@", "https://" + Request.Host.Value + "/sign-in");
        //    body = body.Replace("@@user_name@@", user.email);
        //    body = body.Replace("@@otp@@", user.otp);
        //    body = body.Replace("@@current_year@@", DateTime.Now.Year.ToString());
        //    var dblogmail = new sys_log_mail_db();
        //    dblogmail.tieu_de = maumail.ten;
        //    dblogmail.noi_dung = body;
        //    dblogmail.id_template = maumail.id;
        //    dblogmail.email = user.email;
        //    dblogmail.id = Guid.NewGuid().ToString();
        //    dblogmail.send_date = DateTime.Now;
        //    dblogmail.user_id = id;
        //    dblogmail.ket_qua = 0;
        //    repo._context.sys_log_mails.InsertOne(dblogmail);
        //    var id_log_mail = dblogmail.id;
        //    try
        //    {
        //        _mailService.SendEmailAsync(new MailRequest
        //        {
        //            Body = body,
        //            Subject = dblogmail.tieu_de,
        //            ToEmail = email, //CMAESCrypto.DecryptText(),
        //            CCEmail = "",
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //}

        public static int ConverDateToEpochTime(DateTime? date)
        {
            var epochTime = 0;
            TimeSpan t = DateTime.Now - new DateTime(1970, 1, 1);
            epochTime = (int)t.TotalSeconds;
            return epochTime;
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            var user = repo._context.sys_user_col.AsQueryable().Where(d => d.id == model.db.id).FirstOrDefault();


            if (!check)
            {
                return generateError();
            }
            if (!string.IsNullOrEmpty(model.password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
                model.db.PasswordHash = passwordHash;
                model.db.PasswordSalt = passwordSalt;
            }
            model.db.email = model.email.ToLower();
            model.db.so_dien_thoai = model.phone;
            model.db.Username = model.Username;
            model.db.ho_va_ten = model.ho_va_ten;
            model.db.id = model.id;
            //model.db.hinh_anh_dai_dien = _common_repo.get_anh_dai_dien(model.hinh_anh_dai_dien, 1);
            //model.db.type = model.type == true ? 1 : 0;
            //;
            model.db.nguoi_cap_nhat = getUserId();

            model.db.ngay_cap_nhat = DateTime.Now;
            await repo.update(model);
            //await _common_repo.upset_file(model.file);
            return Json(model);
        }



        [HttpPost]
        public async Task<IActionResult> updateProfile([FromBody] JObject json)
        {
            var user_id = getUserId();
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            //type_update
            //1 main image , 2 avatar , 3 info
            model.db.id = user_id;

            if (!string.IsNullOrEmpty(model.db.so_dien_thoai))
            {
                if (model.phone.Length != 10)
                {
                    ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                }
                else
                {
                    var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
                        |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
                        |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
                        |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                    //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                    var checkSDT = rgSoDienThoai.IsMatch(model.phone);
                    if (checkSDT == false)
                    {
                        ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                    }
                    else
                    {
                        var dienthoai = model.phone;  //CMAESCrypto.EncryptText(item.db.dienthoai);

                        var checkdienthoai = repo.FindAll().Where(d => d.phone == dienthoai && d.id != model.id).Count();
                        if (checkdienthoai > 0)
                        {
                            ModelState.AddModelError("db.phone", "existed");
                        }
                    }
                }
            }
            //if (model.ngay_sinh != null)
            //{
            //    if (model.ngay_sinh > DateTime.Now)
            //    {
            //        ModelState.AddModelError("db.date_of_birth", "system.khong_duoc_phep_chon_ngay_tuong_lai");
            //    }
            //}
            //if (model.gioi_tinh == null || model.gioi_tinh == 0)
            //{
            //    ModelState.AddModelError("db.gioi_tinh", "required");
            //}
            if (string.IsNullOrEmpty(model.ho_va_ten))

            {
                ModelState.AddModelError("db.full_name", "required");
            }
            if (!ModelState.IsValid)
            {
                return generateError();
            }
            //if (!string.IsNullOrEmpty(model.password))
            //{
            //    byte[] passwordHash, passwordSalt;
            //    CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
            //    model.db.PasswordHash = passwordHash;
            //    model.db.PasswordSalt = passwordSalt;
            //}
            model.db.nguoi_cap_nhat = getUserId();
            model.db.ngay_cap_nhat = DateTime.Now;
            await repo.update(model);

            return Json(model);
        }

        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            //1 staff admin, 2 teacher, 3 parent, 4 student
            var id = json.GetValue("id").ToString();
            repo.delete(id);
            return Json("Success");
        }
        public async Task<IActionResult> update_status_del([FromBody] JObject json)
        {
            //1 staff admin, 2 teacher, 3 parent, 4 student
            var id = json.GetValue("id").ToString();
            var status_del = int.Parse(json.GetValue("status_del").ToString());
            repo.update_status_del(id, getUserId(), status_del);
            return Json("");
        }


        public async Task<IActionResult> getElementById(string id)
        {
            var model = await repo.getElementById(id);
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> changePasswordByAdmin([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            if (string.IsNullOrEmpty(model.password))
            {
                ModelState.AddModelError("new_password", "required");
            }
            //if (ú)

            if (!ModelState.IsValid)
            {
                return generateError();
            }
            if (!string.IsNullOrWhiteSpace(model.password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
                model.db.PasswordHash = passwordHash;
                model.db.PasswordSalt = passwordSalt;
                model.db.id = model.id;
                //byte[] passwordHash, passwordSalt;
                //CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
                //model.db.PasswordHash = passwordHash;
                //model.db.PasswordSalt = passwordSalt;
            }
            await repo.updatePassword(model);

            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> changePassword([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());


            var UserId = getUserId();
            model.db.id = UserId;
            var user = AuthenticateId(UserId, model.oldPassword);
            if (user == null)
                return BadRequest(new { message = "Old Password is incorrect" });
            var user_login = repo._context.sys_user_col.AsQueryable().Where(q => q.id == UserId).SingleOrDefault();
            if (string.IsNullOrEmpty(model.password))
            {
                ModelState.AddModelError("password", "required");
            }
            //if (ú)

            if (!ModelState.IsValid)
            {
                return generateError();
            }
            if (!string.IsNullOrWhiteSpace(model.password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
                model.db.PasswordHash = passwordHash;
                model.db.PasswordSalt = passwordSalt;
                model.db.id = UserId;
            }




            await repo.updatePassword(model);

            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> changePasswordNonLogin([FromBody] JObject json)
        {
            var password = json.GetValue("password").ToString();
            var repassword = json.GetValue("repassword").ToString();
            var idtoken = "";
            try { idtoken = json.GetValue("idtoken").ToString(); } catch { }

            var idUser = CMAESCrypto.DecryptText(idtoken);
            var user = repo._context.sys_user_col.AsQueryable().Where(d => d.id == idUser).SingleOrDefault();
            
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "required");
            }
            else
            {
                if (password.Length < 8)
                {
                    ModelState.AddModelError("password", "msgmatkhau");
                }
            }

            if (string.IsNullOrEmpty(repassword))
            {
                ModelState.AddModelError("repass", "required");

            }
            else
            {

                if (password != repassword && password != "")
                {
                    ModelState.AddModelError("repass", "matkhaukhongkhop");
                }

            }
            if (!ModelState.IsValid)
            {
                return generateError();
            }
            sys_user_model model = new sys_user_model();
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                model.db.PasswordHash = passwordHash;
                model.db.PasswordSalt = passwordSalt;
                model.db.id = idUser;
                model.password = password;
            }
            await repo.updatePassword(model);

            return Json(model);

        }

        [HttpPost]
        public IActionResult checkResetPass([FromBody] JObject json)
        {
            var q = json.GetValue("token").ToString();
            try
            {
                var decrypt = CMAESCrypto.DecryptText(q);
                var id = decrypt.Replace("confirm", "").Split("@@")[0];
                var token = decrypt.Replace("confirm", "").Split("@@")[1];
                var update = repo._context.sys_user_col.AsQueryable().Where(t => t.id == id).FirstOrDefault();
                //DateTime time = DateTime.Parse(update.expiration_date_reset_pass.ToString());
                //var expiration_date_reset_pass = time.AddMinutes(15).Ticks;
                //var time_tick = DateTime.Now.Ticks;
                //if (update.expiration_date_reset_pass.Value.AddMinutes(15).Ticks < DateTime.Now.Ticks)
                //{
                //    return Json(false);
                //}
                return Json(CMAESCrypto.EncryptText(id));
            }
            catch (Exception e)
            {
                return Json(false);
            }

        }
        //public async Task<IActionResult> forgot_pass([FromBody] JObject json)
        //{
        //    var email = json.GetValue("email").ToString();
        //    var capcha = json.GetValue("capcha").ToString();
        //    var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");

        //    if (String.IsNullOrEmpty(capcha))
        //    {
        //        ModelState.AddModelError("capcha", "required");
        //    }
        //    else
        //    {
        //        if (CaptchaCode.ToLower() != capcha.ToLower())
        //        {
        //            ModelState.AddModelError("capcha", "captcha_invalid");
        //        }
        //    }

        //    //var checkEmail = CMAESCrypto.EncryptText(email);

        //    var user = repo._context.sys_user_col.AsQueryable().Where(d => d.status_del == 1).SingleOrDefault(x => x.email == email);



        //    //var maumail = repo._context.erp_mau_mails.AsQueryable().Where(t => t.ma == "11").FirstOrDefault();

        //    if (String.IsNullOrEmpty(email))
        //    {
        //        ModelState.AddModelError("db.email", "required");
        //    }
        //    else
        //    {
        //        if (user == null)
        //        {
        //            var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
        //                            + "@"
        //                            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
        //            var checkEmail = rgEmail.IsMatch(email);
        //            if (checkEmail == false)
        //            {
        //                ModelState.AddModelError("db.email", "Email không hợp lệ");

        //            }
        //        }
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return generateError();
        //    }
        //    var rnd = new Random();
        //    var code = rnd.Next(11111, 99999).ToString();

        //    var token_reset_pass = DateTime.Now.Ticks.ToString();
        //    var expiration_date_reset_pass = DateTime.Now;
        //    var update = Builders<User>.Update
        //   .Set(x => x.token_reset_pass, token_reset_pass)
        //   .Set(x => x.otp, code)
        //   .Set(x => x.expiration_date_reset_pass, expiration_date_reset_pass);


        //    var filter = Builders<User>.Filter.Eq(x => x.id, user.id);
        //    repo._context.sys_user_col.UpdateOne(filter, update);


        //    //var encryptconfirmparam = CMAESCrypto.EncryptText(user.id + "@@" + token_reset_pass);
        //    //encryptconfirmparam = encryptconfirmparam.Replace("%", "");
        //    //var body = maumail.noi_dung ?? "";
        //    //body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
        //    //body = body.Replace("@@link_dang_nhap@@", "https://" + Request.Host.Value + "/sign-in");
        //    //body = body.Replace("@@url@@", "https://" + Request.Host.Value);
        //    //body = body.Replace("@@user_name@@", user.email);
        //    //body = body.Replace("@@otp@@", code);
        //    //body = body.Replace("@@url_reset@@", "https://" + Request.Host.Value + "/reset-password?token=" + HttpUtility.UrlEncode(encryptconfirmparam));
        //    //_mailService.SendEmailAsync(new MailRequest
        //    //{
        //    //    Body = body,
        //    //    Subject = maumail.ten,
        //    //    ToEmail = user.email, //CMAESCrypto.DecryptText(),
        //    //    CCEmail = "",
        //    //});

        //    return generateSuscess();
        //}

        public async Task<IActionResult> forgot_pass([FromBody] JObject json)
        {
            var email = json.GetValue("email").ToString();
            var capcha = json.GetValue("capcha").ToString();
            var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");

            if (String.IsNullOrEmpty(capcha))
            {
                ModelState.AddModelError("capcha", "required");
            }
            else
            {
                if (CaptchaCode.ToLower() != capcha.ToLower())
                {
                    ModelState.AddModelError("capcha", "captcha_invalid");
                }
            }
            if (!ModelState.IsValid)
            {
                return generateError();
            }

            //var checkEmail = CMAESCrypto.EncryptText(email);

            var user = repo._context.sys_user_col.AsQueryable().Where(d => d.status_del != 0 && d.status_del != 2).SingleOrDefault(x => x.Username == email);
            var maumail = repo._context.sys_template_mail_col.AsQueryable().Where(t => t.type == "quen_mat_khau").FirstOrDefault();

            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("db.email", "required");

            }
            else
            {
                if (user == null)
                {
                    var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                  + "@"
                                  + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                    var checkEmail = rgEmail.IsMatch(email);
                    if (checkEmail == false)
                    {
                        ModelState.AddModelError("db.email", "system.emailKhongHopLe");

                    }
                    else
                    {
                        ModelState.AddModelError("db.email", "system.emailKhongHopLe");

                    }

                }


            }

            if (!ModelState.IsValid)
            {
                return generateError();
            }


            return generateSuscess();
        }




        public User AuthenticateId(string userid, string password)
        {
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(password))
                return null;

            var user = repo._context.sys_user_col.AsQueryable().SingleOrDefault(x => x.id == userid);
            //var user = _context.sys_user_col.SingleOrDefault(x => x.Username == "Administrator");
            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        [HttpPost]

        public async Task<IActionResult> DataHandler([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<DTParameters>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());

                var search = dictionary["search"].Trim().ToLower();
                var phong_ban = dictionary["phong_ban"];
                var status_del = dictionary["status_del"];
                var chuc_danh = dictionary["chuc_danh"];
                var loai = dictionary["loai"];
                var query = repo.FindAll()
                                    .Where(d => d.ho_va_ten.ToLower().Contains(search) || d.email.ToLower().Contains(search)
                                    || d.Username.ToLower().Contains(search) || d.phone.Contains(search) || search == "")
                                    .Where(d => d.db.status_del == int.Parse(status_del) || status_del == "-1")
                                    .ToList()
                                    ;

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.ngay_cap_nhat).Skip(param.Start).Take(param.Length)
      .ToList());
                DTResult<sys_user_model> result = new DTResult<sys_user_model>
                {
                    start = param.Start,
                    draw = param.Draw,
                    data = dataList,
                    recordsFiltered = count,
                    recordsTotal = count
                };
                return Json(result);
            }

            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

        }

        [AllowAnonymous]
        public ActionResult downloadtemp()
        {
            var currentpath = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(currentpath, "wwwroot", "assets", "template");
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);

            string Files = newPath + "\\sys_user.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "sys_user.xlsx");
        }
        public string CheckErrorImport(sys_user_model model, int ct, string error)
        {
            if (String.IsNullOrEmpty(model.db.email))
            {
                error += "Phải nhập email  tại dòng" + (ct + 1) + "<br />";
            }
            else
            {

                var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                var checkEmail = rgEmail.IsMatch(model.db.email);
                if (checkEmail == false)
                {
                    error += "Email không hợp lệ" + (ct + 1) + "<br />";
                }
                else
                {
                    var check_exited = repo._context.sys_user_col.AsQueryable().Where(q => q.email == model.db.email).Count() > 0;
                    if (check_exited)
                    {
                        error += "Email đã tồn tại trong hệ thống tại dòng" + (ct + 1) + "<br />";
                    }
                }


            }


            //if (String.IsNullOrEmpty(model.db.email))
            //{
            //    error += "Phải nhập điện thoại  tại dòng" + (ct + 1) + "<br />";
            //}
            //else
            //{

            //    if (model.db.phone.Length > 10)
            //    {
            //        error += "Số điện thoại tối đa 10 số" + (ct + 1) + "<br />";

            //    }
            //    else
            //    {
            //        var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
            //|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
            //|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
            //|(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

            //        //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
            //        var checkSDT = rgSoDienThoai.IsMatch(model.db.phone);
            //        if (checkSDT == false)
            //        {
            //            error += "Số điện thoại không hợp lệ" + (ct + 1) + "<br />";
            //        }
            //        else
            //        {
            //            var dienthoai = model.db.phone;  //CMAESCrypto.EncryptText(item.db.dienthoai);

            //            var checkdienthoai = repo.FindAll().Where(d => d.db.phone == dienthoai && d.db.id != model.db.id).Count();
            //            if (checkdienthoai > 0)
            //            {
            //                error += "Số điện thoại tồn tại trong hệ thống" + (ct + 1) + "<br />";
            //            }
            //        }
            //    }


            //}






            if (String.IsNullOrEmpty(model.db.ho_va_ten))
            {
                error += "Phải nhập Họ tên tại dòng" + (ct + 1) + "<br />";
            }


            //if (model.db.gioi_tinh == null)
            //{
            //    error += "Phải nhập giới tính tại dòng" + (ct + 1) + "<br />";
            //}



            return error;
        }

    }
    public static class CMAESCrypto
    {
        static string key = "123xxczxcxxzzMCbsJs4LRzjBHUFM";
        static byte[] IVAES = new byte[] { };
        static byte[] KEYAES = new byte[] { };


        public static string EncryptText(string input)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, key);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }
        public static byte[] GetMD5Hash2(byte[] bytes)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] encoded = md5.ComputeHash(bytes);
            return encoded;
        }
        static void deriveKeyAndIV(String passphrase, byte[] salt)
        {
            var password = Encoding.UTF8.GetBytes(passphrase);
            Console.WriteLine(string.Join(",", password.Select(d => Convert.ToInt32(d))));
            Console.WriteLine(string.Join(",", salt.Select(d => Convert.ToInt32(d))));
            List<byte> concatenatedHashes = new List<byte>();
            List<byte> currentHash = new List<byte>();
            bool enoughBytesForKey = false;
            List<byte> preHash = new List<byte>();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            while (!enoughBytesForKey)
            {
                if (currentHash.Count > 0)
                {
                    preHash = currentHash;
                    preHash.AddRange(password);
                    preHash.AddRange(salt);
                }
                else
                {
                    preHash = password.ToList();
                    preHash.AddRange(salt);
                }
                Console.WriteLine(string.Join(",", preHash.Select(d => Convert.ToInt32(d))));
                currentHash = GetMD5Hash2(preHash.ToArray()).ToList();

                concatenatedHashes.AddRange(currentHash);
                if (concatenatedHashes.Count >= 48) enoughBytesForKey = true;
            }

            KEYAES = concatenatedHashes.Take(32).ToArray();
            IVAES = concatenatedHashes.Skip(32).Take(16).ToArray();
        }
        public static string DecryptText(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, key);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }
        static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, string passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = { 1, 5, 4, 2, 3, 2, 1, 5 };
            deriveKeyAndIV(passwordBytes, saltBytes);
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = KEYAES;
                    AES.IV = IVAES;

                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, string passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = { 1, 5, 4, 2, 3, 2, 1, 5 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    deriveKeyAndIV(passwordBytes, saltBytes);
                    AES.Key = KEYAES;
                    AES.IV = IVAES;
                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;


                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }


    }

}
