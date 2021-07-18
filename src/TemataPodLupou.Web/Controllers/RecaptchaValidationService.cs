using System.Configuration;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace TemataPodLupou.Web.Controllers
{
    public class ValidateRecaptchaAttribute : ActionFilterAttribute
    {
        private const string RecaptchaResponseKey = "g-recaptcha-response";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod != "POST") return;
            var validationService = Current.Factory.GetInstance<RecaptchaValidationService>();
            var captchaResponse = filterContext.HttpContext.Request[RecaptchaResponseKey];
            var isValid = validationService.Validate(captchaResponse);
            if (!isValid)
                filterContext.Controller.ViewData.ModelState
                    .AddModelError("Recaptcha", "Captcha validation failed.");
        }
    }
    
    public class RecaptchaValidationService
    {
        public class Composer :  IUserComposer
        {
            public void Compose(Composition composition)
            {
                composition.Register<RecaptchaValidationService>();
            }
        }
        
        private const string ApiUrl = "https://www.google.com/recaptcha/api/siteverify";
        private readonly string _secretKey;

        public RecaptchaValidationService()
        {
            _secretKey = ConfigurationManager.AppSettings["RecaptchaSecretKey"];
        }

        public bool Validate(string response)
        {
            if (string.IsNullOrWhiteSpace(response)) return false;
            using (var client = new WebClient())
            {
                var result = client.DownloadString($"{ApiUrl}?secret={_secretKey}&response={response}");
                return ParseValidationResult(result);
            }

        }

        private static bool ParseValidationResult(string validationResult)
        {
            return (bool) JObject.Parse(validationResult).SelectToken("success");
        }
    }
}