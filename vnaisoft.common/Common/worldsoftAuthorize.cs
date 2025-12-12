using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using quan_ly_kho.common.BaseClass;
using quan_ly_kho.common.Helpers;
using quan_ly_kho.DataBase.Mongodb;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace quan_ly_kho.common.Common
{
    public class quan_ly_khoAuthorize : ActionFilterAttribute
    {
        public string ResourceKey { get; set; }
        public string OperationKey { get; set; }

        private readonly MongoDBContext dbContext;
        private IMemoryCache _cache;
        public AppSettings _appsetting;
        public quan_ly_khoAuthorize(MongoDBContext _dbContext, IMemoryCache memoryCache, IOptions<AppSettings> appsetting)
        {
            this.dbContext = _dbContext;
            _cache = memoryCache;
            _appsetting = appsetting.Value;

        }

        //Called when access is denied
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // AllowAnonymous
            var endpoint = context.HttpContext.GetEndpoint();
            var hasAllowAnonymous =
                   endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null
                || context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()
                || context.Controller.GetType().GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();

            if (hasAllowAnonymous)
            {
                base.OnActionExecuting(context);
                return;
            }

            // AllowAnonymous
            string actionName = context.RouteData.Values["action"].ToString().ToLower();
            string controllerName = context.RouteData.Values["controller"].ToString().ToLower();
            // AllowAnonymous
            if (controllerName == "khl_de_thi")
            {
                base.OnActionExecuting(context);
                return;
            }
            var pub = new HashSet<string>
            {
                "khl_de_thi;getlistuse",
                "khl_de_thi;create",
                "khl_de_thi;edit",
                "khl_de_thi;getelementbyid",
                "khl_de_thi;update_status_del",
                "khl_de_thi;datahandler",
                "khl_de_thi;importfromexcel"
            };
            if (pub.Contains(controllerName + ";" + actionName))
            {
                base.OnActionExecuting(context);
                return;
            }
            // AllowAnonymous
            if (controllerName == "sys_banner" || actionName == "sendVoipIOS")
            {
                base.OnActionExecuting(context);
                return;
            }
            if (controllerName == "sys_home" || actionName == "downloadtempFileError")
            {
                base.OnActionExecuting(context);
                return;
            }


            if (ListControlller.listnonloginpublicactioncontroller.Contains(controllerName + ";" + actionName))
            {
                base.OnActionExecuting(context);
                return;
            }
            var reponse = context.HttpContext.Response.StatusCode;
            var isValid = false;
            string token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token) && !context.HttpContext.User.Identity.IsAuthenticated)
            {
                SetUserFromToken(context.HttpContext, token);
            }
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new ContentResult { Content = "401" };
                return;
            }
            if (controllerName == "sys_home" || controllerName == "sys_approval" || actionName == "sync_approval")
            {
                base.OnActionExecuting(context);
                return;
            }
            if (ListControlller.listpublicactioncontroller.Contains(controllerName + ";" + actionName))
            {
                base.OnActionExecuting(context);
                return;
            }

            var _checkrole = checkrole(context.HttpContext, controllerName, actionName);
            if (_checkrole == false)
            {
                context.HttpContext.Response.StatusCode = 403;
                context.Result = new ContentResult { Content = "403" };
                return;
            }
            base.OnActionExecuting(context);
        }

        public void SetUserFromToken(HttpContext context, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var nameClaim = jwt.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Name ||
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type == "sub" ||
                c.Type == "unique_name");

            var identity = new ClaimsIdentity(jwt.Claims, "jwt");

            if (identity.FindFirst(ClaimTypes.Name) == null && nameClaim != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, nameClaim.Value));
            }
            context.User = new ClaimsPrincipal(identity);
        }

        private bool checkrole(HttpContext httpContext, string controllerName, string actionName)
        {
            List<string> cacheActionControllerUser;
            var username = httpContext.User.Identity.Name;
            var cachekey = "quan_ly_khoAuthorizeListControllerAction" + username;

            var userid = username;
            var type = dbContext.sys_user_col.AsQueryable().Where(t => t.id == userid).Select(d => d.loai).SingleOrDefault();

            var listresult = new List<string>();




            if (!_cache.TryGetValue(cachekey, out cacheActionControllerUser))
            {
                var list_nhom = dbContext.sys_group_user_col.AsQueryable().Where(t => t.status_del == 1).Select(q => q.id).ToList();



                var listgroupId = dbContext.sys_group_user_detail_col.AsQueryable()
                    .Where(d => list_nhom.Contains(d.id_group_user))
                    .Where(d => d.user_id == userid).Select(d => d.id_group_user).ToList();




                var listrole = dbContext.sys_group_user_role_col.AsQueryable().Where(d => listgroupId.Contains(d.id_group_user)).ToList();

                var listcontrollerName = listrole.Select(d => d.controller_name).Distinct().ToList();
                var listroleId = listrole.Select(d => d.id_controller_role).Distinct().ToList();
                for (int i = 0; i < ListControlller.list.Count; i++)
                {
                    if (listcontrollerName.ToList().Contains(ListControlller.list[i].translate))
                    {
                        for (int j = 0; j < ListControlller.list[i].list_role.Count; j++)
                        {
                            if (listroleId.Contains(ListControlller.list[i].list_role[j].id))
                            {
                                listresult.AddRange(ListControlller.list[i].list_role[j].list_controller_action.Select(d => d.ToLower()));
                            }
                        }
                    }
                }


                //if (type == 1)
                //{


                //}
                //else 
                //{
                //    var role = ListControlller.list.Where(d => d.type_user == type).ToList();
                //    for (int i = 0; i < role.Count; i++)
                //    {

                //        for (int j = 0; j < role[i].list_role.Count; j++)
                //        {

                //            listresult.AddRange(role[i].list_role[j].list_controller_action.Select(d => d.ToLower()));

                //        }
                //    }


                //}


                // Key not in cache, so get data.
                cacheActionControllerUser = listresult;
                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));

                // Save data in cache.
                _cache.Set(cachekey, cacheActionControllerUser, cacheEntryOptions);
            }
            if (!cacheActionControllerUser.Contains(controllerName + ";" + actionName)) return false;

            //var linq = new LinqCommonDataContext();

            // var rd = httpContext.Request.RequestContext.RouteData;
            // string currentAction = rd.GetRequiredString("action");
            // string currentController = rd.GetRequiredString("controller");
            // var linq = new CommonLinqDataContext();
            // if (linq.HT_CongViecs.Where(d => d.notRole == 1).Select(d => d.tenController).ToList().Contains(currentController) || currentController=="HomeApi") return true;
            // var userName = httpContext.User.Identity.Name;
            // var userId = linq.HT_Users.Where(d => d.userName.ToLower() == userName.ToLower())
            //     .Select(d => d.userId).FirstOrDefault();
            // var nhom = linq.HT_NhomUserChiTiets.Where(d => d.userId == userId+"").Select(d => d.maNhom).Distinct().ToList();
            // var checkIsAllow = linq.HT_CongViecMVCActions.Where(d =>
            //linq.HT_CongViecNhomUsers.Where(t => nhom.Contains(t.maNhom))
            //.Where(t => t.maVuViec == d.maVuViec).Count() > 0)
            // .Where(d => d.controller.ToLower() == currentController.ToLower())
            // .Where(d => d.action.ToLower() == currentAction.ToLower()).Count() > 0;
            // if (!checkIsAllow)
            // {
            //     return false;
            // }
            return true;
        }
    }
}
