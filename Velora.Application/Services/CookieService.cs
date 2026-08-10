using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Services
{
    using Microsoft.AspNetCore.Http;
    using Velora.Application.Shared.Services;

    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CookieService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }



        private HttpResponse Response =>
            _httpContextAccessor.HttpContext!.Response;


        private IRequestCookieCollection RequestCookies =>
            _httpContextAccessor.HttpContext!.Request.Cookies;




        public string? Get(string key)
        {
            if (RequestCookies.TryGetValue(key, out var value))
            {
                return value;
            }


            return null;
        }




        public void Set(
            string key,
            string value,
            int expireDays = 30)
        {

            var options = new CookieOptions
            {
                Expires =
                    DateTimeOffset.UtcNow.AddDays(expireDays),


                HttpOnly = true,


                Secure = true,


                SameSite =
                    SameSiteMode.Lax
            };


            Response.Cookies.Append(
                key,
                value,
                options);
        }




        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }





        public bool Exists(string key)
        {
            return RequestCookies.ContainsKey(key);
        }





        public string GetOrCreate(
            string key,
            Func<string> generator,
            int expireDays = 30)
        {

            var value = Get(key);


            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }



            value = generator();



            Set(
                key,
                value,
                expireDays);



            return value;
        }

        public void Restore(
    string key,
    string value,
    int expireDays = 30)
        {
            Set(
                key,
                value,
                expireDays);
        }
    }
}
