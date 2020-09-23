using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CleanersAPI.Helpers;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos.Email;
using CleanersAPI.Repositories;
using CleanersAPI.Templates;
using HandlebarsDotNet;
using IronPdf;
using Microsoft.Extensions.Configuration;

namespace CleanersAPI.Services.Impl
{
    public class BillingsService : CleanersService<Billing>, IBillingsService
    {
        private readonly IBillingsRepository _billingsRepository;
        private readonly IReservationsRepository _reservationsRepository;
        private readonly IEmailsService _emailsService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public BillingsService(IBillingsRepository billingsRepository, 
            IReservationsRepository reservationsRepository, IEmailsService emailsService, IMapper mapper, IConfiguration configuration)
        {
            _billingsRepository = billingsRepository;
            _reservationsRepository = reservationsRepository;
            _emailsService = emailsService;
            _mapper = mapper;
            _configuration = configuration;
        }

        protected override ICleanersRepository<Billing> GetRepository()
        {
            return _billingsRepository;
        }

        public async Task<Billing> Create(ReservationSearchCriteria reservationSearchCriteria)
        {
            var reservations = _reservationsRepository.Search(reservationSearchCriteria).Result.ToList();
            if (!reservations.Any()) return null;
            var billing = new Billing {Date = DateTime.Now, TotalPrice = reservations.Sum(r => r.TotalCost)};
            var invoice = await GenerateInvoice(reservationSearchCriteria.Customer, reservations);

            var customer = reservationSearchCriteria.Customer;
            var newEmail = _mapper.Map<Email>(new EmailForSend
            {
                Subject = "House Cleaners Invoice",
                To = customer.Email,
                ReplyTo = _configuration.GetValue<string>("SendGrid:senderEmail"),
                Body = "<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8'><title>Invoice</title></head><body>" +
                       $"<div>Dear {customer.FirstName},</div>" +
                       "<div>Please find attached your invoice of the latest services.</div><div style='margin-top: 20px'>HouseCleaners</div></body></html>"
            });
            newEmail.PlainTextBody =
                $"Dear {customer.FirstName}, Please find attached your invoice of the latest services. HouseCleaners.";
            var response = await _emailsService.SendEmail(newEmail, "HouseCleanersInvoice.pdf", Convert.ToBase64String(invoice.BinaryData));
            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                newEmail.Sent = true;
            }
            await _emailsService.Create(newEmail);
            
            return await _billingsRepository.Create(billing, reservations);
        }

        public async Task<PdfDocument> GenerateInvoice(Person person, IEnumerable<Reservation> reservations)
        {
            var pageTemplate = await System.IO.File.ReadAllTextAsync("Templates/invoice.html");
            var rowTemplate = await System.IO.File.ReadAllTextAsync("Templates/invoiceRow.html");
            Handlebars.RegisterTemplate("invoiceRow", rowTemplate);

            var invoice = new Invoice
            {
                Names = $"{person.LastName}, {person.FirstName}",
                Address =
                    $"{person.Address.StreetName} {person.Address.PlotNumber}, {person.Address.PostalCode} {person.Address.City}",
                Email = person.Email,
                Telephone = person.PhoneNumber,
                InvoiceLines = new List<InvoiceLine>()
            };
            foreach (var reservation in reservations)
            {
                invoice.InvoiceLines.Add(new InvoiceLine
                {
                    Service = reservation.Expertise.Service.Title,
                    DateTime = reservation.StartTime,
                    HourlyRate = reservation.Expertise.HourlyRate,
                    NumberOfHours = reservation.EndTime.Subtract(reservation.StartTime).Hours,
                    TotalCost = reservation.TotalCost,
                });
            }

            var template = Handlebars.Compile(pageTemplate);
            var result = template(invoice);

            using var renderer = new HtmlToPdf
            {
                PrintOptions =
                {
                    FirstPageNumber = 1,
                    Footer = new SimpleHeaderFooter
                    {
                        DrawDividerLine = true, RightText = "Page {page} of {total-pages}"
                    },
                    CustomCssUrl = "Templates/invoice.css"
                }
            };
            return await renderer.RenderHtmlAsPdfAsync(result);
        }
    }
}