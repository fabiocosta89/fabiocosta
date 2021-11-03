namespace FabioCosta.Web.Services
{
    using FabioCosta.Web.Interfaces;

    using Microsoft.Extensions.Configuration;

    using Newtonsoft.Json.Linq;

    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ExternalService : IExternalService
    {
        private readonly HttpClient _captchaClient;
        private readonly IConfiguration _configuration;

        public ExternalService(HttpClient captchaClient, IConfiguration configuration)
        {
            _captchaClient = captchaClient;
            _configuration = configuration;
        }

        /// <summary>
        /// Validate Captcha
        /// </summary>
        /// <param name="captcha">Captcha response</param>
        /// <returns></returns>
        public async Task<bool> IsCaptchaValid(string captcha)
        {
            try
            {
                var secretKey = _configuration.GetValue(typeof(string), "Captcha:SecretKey");
                var postTask = await _captchaClient
                    .PostAsync($"?secret={secretKey}&response={captcha}", new StringContent(""));
                var result = await postTask.Content.ReadAsStringAsync();
                var resultObject = JObject.Parse(result);
                dynamic success = resultObject["success"];

                return (bool)success;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
