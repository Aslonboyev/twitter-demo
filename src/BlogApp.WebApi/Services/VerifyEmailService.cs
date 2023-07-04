using BlogApp.Service.ViewModels.Users;
using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Exceptions;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.ViewModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace BlogApp.WebApi.Services
{
    public class VerifyEmailService : IVerifyEmailService
    {
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;

        public VerifyEmailService(IMemoryCache cache, IEmailService email, AppDbContext appDbContext)
        {
            _cache = cache;
            _emailService = email;
            _context = appDbContext;
        }

        public async Task SendCodeAsync(SendCodeToEmailViewModel email)
        {
            int code = new Random().Next(1000, 9999);

            _cache.Set(email.Email, code, TimeSpan.FromMinutes(3));

            var message = new EmailMessageViewModel()
            {
                To = email.Email,
                Subject = "Verification code",
                Body = MessageGenerator(code),
            };

            await _emailService.SendAsync(message);
        }

        public async Task VerifyEmail(EmailVerifyViewModel emailVerify)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(user => user.Email == emailVerify.Email)
                ?? throw new StatusCodeException(HttpStatusCode.NotFound, message: "User not found!");
            
            if (_cache.TryGetValue(emailVerify.Email, out int exceptedCode))
            {
                if (exceptedCode != emailVerify.Code)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is wrong!");

                entity.IsEmailConfirmed = true;

                _context.Update(entity);

                await _context.SaveChangesAsync();
            }
            else
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is expired");
        }

        public async Task VerifyPasswordAsync(UserResetPasswordViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == model.Email)
                ?? throw new StatusCodeException(HttpStatusCode.NotFound, message: "user not found");
            
            if (_cache.TryGetValue(model.Email, out int code))
            {
                if (code != model.Code)
                    throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is wrong");
            }
            else
                throw new StatusCodeException(HttpStatusCode.BadRequest, message: "Code is expired");
        }

        private string MessageGenerator(int code)
            => $"<!DOCTYPE html>\r\n" +
            $"<html lang=\"en\" >\r\n\r\n" +
            $"<head>\r\n  " +
            $"<meta charset=\"UTF-8\">\r\n  " +
            $"<title>Please Verify Your Email Address</title>\r\n  \r\n  \r\n  \r\n  \r\n  \r\n" +
            $"</head>\r\n\r\n" +
            $"<body>\r\n\r\n  " +
            $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" +
            $"\r\n<html>\r\n  \r\n  " +
            $"<head>\r\n    " +
            $"<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n    " +
            $"<title>RUAPB</title>\r\n  " +
            $"</head>\r\n  \r\n  " +
            $"<body leftmargin=\"0\" marginwidth=\"0\" topmargin=\"0\" marginheight=\"0\" offset=\"0\"\r\n  " +
            $"style=\"margin: 0pt auto; padding: 0px; background:#F4F7FA;\">\r\n    <table id=\"main\" width=\"100%\" " +
            $"height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"\r\n    bgcolor=\"#F4F7FA\">\r\n      " +
            $"<tbody>\r\n        " +
            $"<tr>\r\n          " +
            $"<td valign=\"top\">\r\n            " +
            $"<table class=\"innermain\" cellpadding=\"0\" width=\"580\" cellspacing=\"0\" border=\"0\"\r\n            " +
            $"bgcolor=\"#F4F7FA\" align=\"center\" style=\"margin:0 auto; table-layout: fixed;\">\r\n              " +
            $"<tbody>\r\n                " +
            $"<!-- START of MAIL Content -->\r\n                " +
            $"<tr>\r\n                  " +
            $"<td colspan=\"4\">\r\n                    " +
            $"<!-- Logo start here -->\r\n                    " +
            $"<table class=\"logo\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n                      " +
            $"<tbody>\r\n                        " +
            $"<tr>\r\n                          " +
            $"<td colspan=\"2\" height=\"30\"></td>\r\n                        " +
            $"</tr>\r\n                        \r\n                        " +
            $"<tr>\r\n                          " +
            $"<td colspan=\"2\" height=\"30\"></td>\r\n                        " +
            $"</tr>\r\n                      " +
            $"</tbody>\r\n                    " +
            $"</table>\r\n                    " +
            $"<!-- Logo end here -->\r\n                    " +
            $"<!-- Main CONTENT -->\r\n                    " +
            $"<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" bgcolor=\"#ffffff\"\r\n                    " +
            $"style=\"border-radius: 4px; box-shadow: 0 2px 8px rgba(0,0,0,0.05);\">\r\n                      " +
            $"<tbody>\r\n                        " +
            $"<tr>\r\n                          " +
            $"<td height=\"40\"></td>\r\n                        " +
            $"</tr>\r\n                        " +
            $"<tr style=\"font-family: -apple-system,BlinkMacSystemFont,&#39;Segoe UI&#39;,&#39;Roboto&#39;," +
            $"&#39;Oxygen&#39;,&#39;Ubuntu&#39;,&#39;Cantarell&#39;,&#39;Fira Sans&#39;,&#39;Droid Sans&#39;,&#39;Helvetica Neue&#39;,sans-serif; " +
            $"color:#4E5C6E; font-size:14px; line-height:20px; margin-top:20px;\">\r\n                          " +
            $"<td class=\"content\" colspan=\"2\" valign=\"top\" align=\"center\" style=\"padding-left:90px; padding-right:90px;\">\r\n                            " +
            $"<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" bgcolor=\"#ffffff\">\r\n                              " +
            $"<tbody>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td align=\"center\" valign=\"bottom\" colspan=\"2\" cellpadding=\"3\">\r\n                                    <img alt=\"Coinbase\" " +
            $"width=\"80\" src=\"https://www.coinbase.com/assets/app/icon_email-e8c6b940e8f3ec61dcd56b60c27daed1a6f8b169d73d9e79b8999ff54092a111.png\"\r\n" +
            $"                                    />\r\n                                  " +
            $"</td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td height=\"30\" &nbsp;=\"\"></td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td align=\"center\"> <span style=\"color:#48545d;font-size:22px;line-height: 24px;\">\r\n     " +
            $"     Проверьте свой адрес электронной почты\r\n        " +
            $"</span>\r\n\r\n                                  " +
            $"</td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td height=\"24\" &nbsp;=\"\"></td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td height=\"1\" bgcolor=\"#DAE1E9\"></td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td height=\"24\" &nbsp;=\"\"></td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td align=\"center\"> <span style=\"color:#48545d;font-size:14px;line-height:24px;\">\r\n          " +
            $"Чтобы начать пользоваться учетной записью Республиканской универсальной агро-промышленной биржи, вам необходимо подтвердить свой адрес электронной почты.\r\n        " +
            $"</span>\r\n\r\n                                  " +
            $"</td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td height=\"20\" &nbsp;=\"\"></td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td valign=\"top\" width=\"48%\" align=\"center\"> " +
            $"<span>\r\n          <a  style=\"display:block; padding:15px 15px;font-size:50px; color:#000000; text-decoration:none;\">{code}</a>\r\n        " +
            $"</span>\r\n\r\n                                  " +
            $"</td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td height=\"20\" &nbsp;=\"\"></td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td align=\"center\">\r\n                                    " +
            $"<img src=\"https://s3.amazonaws.com/app-public/Coinbase-notification/hr.png\" width=\"54\"\r\n                                    height=\"2\" border=\"0\">\r\n                                  " +
            $"</td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td height=\"20\" &nbsp;=\"\"></td>\r\n                                " +
            $"</tr>\r\n                                " +
            $"<tr>\r\n                                  " +
            $"<td align=\"center\">\r\n                                    <p style=\"color:#a2a2a2; font-size:12px; line-height:17px; font-style:italic;\">Если вы не зарегистрировали" +
            $" эту учетную запись, вы можете игнорировать это электронное письмо и учетную запись.\r\n                                      will be deleted.</p>\r\n                                  " +
            $"</td>\r\n                                " +
            $"</tr>\r\n                              " +
            $"</tbody>\r\n                            " +
            $"</table>\r\n                          " +
            $"</td>\r\n                        " +
            $"</tr>\r\n                        " +
            $"<tr>\r\n                          " +
            $"<td height=\"60\"></td>\r\n                        " +
            $"</tr>\r\n                      " +
            $"</tbody>\r\n                    " +
            $"</table>\r\n                    " +
            $"<!-- Main CONTENT end here -->\r\n                    " +
            $"<!-- PROMO column start here -->\r\n                    " +
            $"<!-- Show mobile promo 75% of the time -->\r\n                    " +
            $"<table id=\"promo\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"margin-top:20px;\">\r\n                      " +
            $"<tbody>\r\n                        " +
            $"<tr>\r\n                          " +
            $"<td colspan=\"2\" height=\"20\"></td>\r\n                        " +
            $"</tr>\r\n                        " +
            $"<tr>\r\n                          " +
            $"<td colspan=\"2\" align=\"center\"> <span style=\"font-size:14px; font-weight:500; margin-bottom:10px; color:#7E8A98; " +
            $"font-family: -apple-system,BlinkMacSystemFont,&#39;Segoe UI&#39;,&#39;Roboto&#39;,&#39;Oxygen&#39;,&#39;Ubuntu&#39;" +
            $",&#39;Cantarell&#39;,&#39;Fira Sans&#39;,&#39;Droid Sans&#39;,&#39;Helvetica Neue&#39;,sans-serif;\">" +
            $"Республиканская универсальная агро-промышленная биржа</span>\r\n\r\n                          " +
            $"</td>\r\n                        " +
            $"</tr>\r\n                        " +
            $"<tr>\r\n                          " +
            $"<td colspan=\"2\" height=\"20\"></td>\r\n                        " +
            $"</tr>\r\n                       \r\n                        " +
            $"<tr>\r\n                          " +
            $"<td colspan=\"2\" height=\"20\"></td>\r\n                        " +
            $"</tr>\r\n                      " +
            $"</tbody>\r\n                    " +
            $"</table>\r\n                    " +
            $"<!-- PROMO column end here -->\r\n                    " +
            $"<!-- FOOTER start here -->\r\n                    <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n                      " +
            $"<tbody>\r\n                        " +
            $"<tr>\r\n                          " +
            $"<td height=\"10\">&nbsp;</td>\r\n                        " +
            $"</tr>\r\n                        \r\n                        " +
            $"<tr>\r\n                          " +
            $"<td height=\"50\">&nbsp;</td>\r\n                        " +
            $"</tr>\r\n                      " +
            $"</tbody>\r\n                    " +
            $"</table>\r\n                    " +
            $"<!-- FOOTER end here -->\r\n                  " +
            $"</td>\r\n                " +
            $"</tr>\r\n              " +
            $"</tbody>\r\n            " +
            $"</table>\r\n          " +
            $"</td>\r\n        " +
            $"</tr>\r\n      " +
            $"</tbody>\r\n    " +
            $"</table>\r\n  " +
            $"</body>\r\n\r\n" +
            $"</html>\r\n  \r\n  \r\n\r\n" +
            $"</body>\r\n\r\n" +
            $"</html>\r\n";
    }
}
