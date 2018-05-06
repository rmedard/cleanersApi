using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;
using Microsoft.Extensions.Configuration;

namespace CleanersAPI.Services.Impl
{
    public class EmailsService : IEmailsService
    {

        private readonly IEmailsRepository _emailsRepository;
        private readonly IConfiguration _configuration;

        public EmailsService(IEmailsRepository emailsRepository, IConfiguration configuration)
        {
            _emailsRepository = emailsRepository ?? throw new ArgumentNullException(nameof(emailsRepository));
            _configuration = configuration;
        }

        public Task<IEnumerable<Email>> GetAll()
        {
            return _emailsRepository.GetAll();
        }

        public Task<Email> GetOneById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool DoesExist(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Email> Create(Email t)
        {
            var newEmail = SendEmail(t);
            return _emailsRepository.Create(newEmail);
        }

        public Task<bool> Update(Email t)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
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