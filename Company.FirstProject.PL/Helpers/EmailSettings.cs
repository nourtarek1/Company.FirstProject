using System.Net;
using System.Net.Mail;

namespace Company.FirstProject.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            // Mail Server : Gmail
            // SMTP
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("nourtarek541@gmail.com", "uuabmurflsilqdym");
                client.Send("nourtarek541@gmail.com", email.To, email.Subject, email.Body);

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
