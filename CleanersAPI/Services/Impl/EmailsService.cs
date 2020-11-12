using System;
using System.Net;
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

        public async Task<Response> SendEmail(Email email)
        {
            var apiKey = _configuration.GetValue<string>("SendGrid:apiKey");
            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(new EmailAddress(email.From),
                new EmailAddress(email.To), email.Subject,
                string.IsNullOrEmpty(email.PlainTextBody) ? email.Body : email.PlainTextBody, email.Body);
            return await client.SendEmailAsync(msg);
        }

        public async Task<Response> SendEmail(Email email, string filename, string attachmentInBase64)
        {
            var apiKey = _configuration.GetValue<string>("SendGrid:apiKey");
            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(new EmailAddress(email.From),
                new EmailAddress(email.To), email.Subject,
                string.IsNullOrEmpty(email.PlainTextBody) ? email.Body : email.PlainTextBody, email.Body);
            msg.AddAttachment(new Attachment
            {
                Content = attachmentInBase64,
                Filename = filename,
                Type = "application/pdf"
            });
            return await client.SendEmailAsync(msg);
        }

        public new Task<Email> Create(Email t)
        {
            return _emailsRepository.Create(t);
        }

        public async Task notifyUsersOnReservationCreation(Reservation reservation)
        {
            var fromEmail = _configuration.GetValue<string>("SendGrid:senderEmail");
            var fromName = _configuration.GetValue<string>("SendGrid:senderName");

            var emailToCustomer = new Email
            {
                From = fromEmail,
                To = reservation.Customer.User.Email,
                SenderNames = fromName,
                ReplyTo = fromEmail,
                Subject = "A new reservation has been created",
                Body = "<html><head><meta charset='UTF-8'><meta http-equiv='x-ua-compatible' content='IE=edge'>" +
                       "<meta name='viewport' content='width=device-width, initial-scale=1'><title>Notification</title></head>" +
                       $"<body><table><tr><td>Professional:</td><td>{reservation.Expertise.Professional.User.FirstName}, {reservation.Expertise.Professional.User.LastName}</td></tr>" +
                       $"<tr><td>Service:</td><td>{reservation.Expertise.Service.Title}</td></tr>" +
                       $"<tr><td>Date:</td><td>{reservation.StartTime} à {reservation.EndTime}</td></tr></table></body></html>",
                PlainTextBody =
                    $"Customer:{reservation.Expertise.Professional.User.FirstName}, {reservation.Expertise.Professional.User.LastName} " +
                    $"Service:{reservation.Expertise.Service.Title} Date:{reservation.StartTime} à {reservation.EndTime}"
            };

            var emailToProfessional = new Email
            {
                From = fromEmail,
                To = reservation.Expertise.Professional.User.Email,
                SenderNames = fromName,
                ReplyTo = fromEmail,
                Subject = "A new reservation has been created",
                Body = "<html><head><meta charset='UTF-8'><meta http-equiv='x-ua-compatible' content='IE=edge'>" +
                       "<meta name='viewport' content='width=device-width, initial-scale=1'><title>Notification</title></head>" +
                       $"<body><table><tr><td>Customer:</td><td>{reservation.Customer.User.FirstName}, {reservation.Customer.User.LastName}</td></tr>" +
                       $"<tr><td>Service:</td><td>{reservation.Expertise.Service.Title}</td></tr>" +
                       $"<tr><td>Date:</td><td>{reservation.StartTime} à {reservation.EndTime}</td></tr></table></body></html>",
                PlainTextBody =
                    $"Customer:{reservation.Customer.User.FirstName}, {reservation.Customer.User.LastName} " +
                    $"Service:{reservation.Expertise.Service.Title} Date:{reservation.StartTime} à {reservation.EndTime}"
            };

            var responseForCustomer = await SendEmail(emailToCustomer);
            var responseForProfessional = await SendEmail(emailToProfessional);

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
    }
}