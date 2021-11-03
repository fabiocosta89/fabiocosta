namespace FabioCosta.Web.Interfaces
{
    using System.Threading.Tasks;

    public interface IExternalService
    {
        /// <summary>
        /// Validate Captcha
        /// </summary>
        /// <param name="captcha">Captcha response</param>
        /// <returns></returns>
        Task<bool> IsCaptchaValid(string captcha);
    }
}
