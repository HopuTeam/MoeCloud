using System.Net;
using System.Net.Mail;
using System.Text;

namespace MoeCloud.Common
{
    public class MailHelper
    {
        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="host">SMTP地址</param>
        /// <param name="account">发件人邮箱</param>
        /// <param name="password">SMTP密码</param>
        /// <param name="port">SMTP端口</param>
        /// <param name="from">发信邮箱用户名，一般与邮箱地址相同</param>
        /// <param name="ssl">是否使用加密连接</param>
        /// <param name="toMail">收件地址</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容(可包含HTML)</param>
        /// <returns>返回是否成功</returns>
        public static bool SendMail(string host, string account, string password, int port, string from, bool ssl, string toMail, string title, string content)
        {
            try
            {
                SmtpClient client = new SmtpClient
                {
                    //Host = "smtp.ym.163.com",//设置SMTP地址
                    Host = host,//设置SMTP地址
                    UseDefaultCredentials = true,
                    Port = port,
                    EnableSsl = ssl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(account, password)//发件人的邮箱和SMTP密码
                };
                MailMessage Message = new MailMessage
                {
                    From = new MailAddress(from)//发信邮箱用户名，一般与邮箱地址相同
                };
                Message.To.Add(toMail);//要发送的地址
                Message.Subject = title;//标题
                Message.Body = content;//内容
                Message.SubjectEncoding = Encoding.UTF8;
                Message.BodyEncoding = Encoding.UTF8;
                Message.Priority = MailPriority.High;
                Message.IsBodyHtml = true;//允许HTML格式
                client.Send(Message);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}