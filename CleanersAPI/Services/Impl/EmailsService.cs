using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CleanersAPI.Services.Impl
{
    public class EmailsService : CleanersService<Email>, IEmailsService
    {
        private readonly IEmailsRepository _emailsRepository;
        private readonly IConfiguration _configuration;

        public EmailsService(IEmailsRepository emailsRepository, IConfiguration configuration)
        {
            _emailsRepository = emailsRepository ?? throw new ArgumentNullException(nameof(emailsRepository));
            _configuration = configuration;
        }

        protected override ICleanersRepository<Email> GetRepository()
        {
            return _emailsRepository;
        }

        public new Task<Email> Create(Email t)
        {
            var newEmail = SendEmail(t);
            return _emailsRepository.Create(newEmail);
        }

        public async Task notifyUsersOnReservationCreation(Reservation reservation)
        {
            var apiKey = _configuration.GetValue<string>("SendGrid:apiKey");
            var client = new SendGridClient(apiKey);
            var fromEmail = _configuration.GetValue<string>("SendGrid:senderEmail");
            var fromName = _configuration.GetValue<string>("SendGrid:senderName");

            var emailToCustomer = new Email
            {
                From = fromEmail,
                To = reservation.Customer.Email,
                SenderNames = fromName,
                ReplyTo = fromEmail,
                Subject = "A new reservation has been created",
                Body = "<html><head><meta charset='UTF-8'><meta http-equiv='x-ua-compatible' content='IE=edge'>" +
                       "<meta name='viewport' content='width=device-width, initial-scale=1'><title>Notification</title></head>" +
                       $"<body><table><tr><td>Professional:</td><td>{reservation.Expertise.Professional.FirstName}, {reservation.Expertise.Professional.LastName}</td></tr>" +
                       $"<tr><td>Service:</td><td>{reservation.Expertise.Service.Title}</td></tr>" +
                       $"<tr><td>Date:</td><td>{reservation.StartTime} à {reservation.EndTime}</td></tr></table></body></html>"
            };
            var plainTextBodyCustomer =
                $"Customer:{reservation.Expertise.Professional.FirstName}, {reservation.Expertise.Professional.LastName} " +
                $"Service:{reservation.Expertise.Service.Title} Date:{reservation.StartTime} à {reservation.EndTime}";

            var emailToProfessional = new Email
            {
                From = fromEmail,
                To = reservation.Expertise.Professional.Email,
                SenderNames = fromName,
                ReplyTo = fromEmail,
                Subject = "A new reservation has been created",
                Body = "<html><head><meta charset='UTF-8'><meta http-equiv='x-ua-compatible' content='IE=edge'>" +
                       "<meta name='viewport' content='width=device-width, initial-scale=1'><title>Notification</title></head>" +
                       $"<body><table><tr><td>Customer:</td><td>{reservation.Customer.FirstName}, {reservation.Customer.LastName}</td></tr>" +
                       $"<tr><td>Service:</td><td>{reservation.Expertise.Service.Title}</td></tr>" +
                       $"<tr><td>Date:</td><td>{reservation.StartTime} à {reservation.EndTime}</td></tr></table></body></html>"
            };
            var plainTextBodyProfessional =
                $"Customer:{reservation.Customer.FirstName}, {reservation.Customer.LastName} " +
                $"Service:{reservation.Expertise.Service.Title} Date:{reservation.StartTime} à {reservation.EndTime}";

            var msgToCustomer = MailHelper.CreateSingleEmail(new EmailAddress(emailToCustomer.From),
                new EmailAddress(emailToCustomer.To), emailToCustomer.Subject, plainTextBodyCustomer,
                emailToCustomer.Body);
            var responseForCustomer = await client.SendEmailAsync(msgToCustomer);

            var msgToProfessional = MailHelper.CreateSingleEmail(new EmailAddress(emailToProfessional.From),
                new EmailAddress(emailToProfessional.To), emailToProfessional.Subject, plainTextBodyProfessional,
                emailToProfessional.Body);
            var responseForProfessional = await client.SendEmailAsync(msgToProfessional);

            if (responseForCustomer.StatusCode.Equals(HttpStatusCode.Accepted))
            {
                emailToCustomer.Sent = true;
            }

            if (responseForProfessional.StatusCode.Equals(HttpStatusCode.Accepted))
            {
                emailToProfessional.Sent = true;
            }

            await Create(emailToCustomer);
            await Create(emailToProfessional);
        }

        private Email SendEmail(Email email)
        {
            email.Sent = false;
            var client = new SmtpClient(_configuration["SmtpSettings:server"])
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration["SmtpSettings:username"],
                    _configuration["SmtpSettings:password"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(email.From),
                Sender = new MailAddress(email.From, email.SenderNames),
                Body = email.Body,
                Subject = email.Subject
            };
            mailMessage.To.Add(email.To);
            mailMessage.ReplyToList.Add(email.ReplyTo);
            try
            {
                client.SendAsync(mailMessage, "CleanersAppEmail:" + DateTime.Now);
                email.Sent = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return email;
        }
    }
}