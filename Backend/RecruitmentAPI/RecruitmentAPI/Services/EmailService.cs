using System.Net;
using System.Net.Mail;
using RecruitmentAPI.Services.Interfaces;

namespace RecruitmentAPI.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;


        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string message)
        {

            var emailSettings =
                _configuration.GetSection("EmailSettings");



            var smtpServer =
                emailSettings["SmtpServer"];



            var port =
                int.Parse(
                    emailSettings["Port"]!
                );



            var email =
                emailSettings["Email"];



            var password =
                emailSettings["Password"];




            using var smtpClient =
                new SmtpClient(smtpServer)
                {
                    Port = port,

                    Credentials =
                    new NetworkCredential(
                        email,
                        password
                    ),

                    EnableSsl = true
                };





            using var mail =
                new MailMessage
                {

                    From =
                    new MailAddress(
                        email!
                    ),


                    Subject = subject,


                    Body = message,


                    IsBodyHtml = true

                };



            mail.To.Add(toEmail);



            await smtpClient.SendMailAsync(mail);

        }

    }
}