﻿// using FluentEmail.Smtp;
// using System.Net.Mail;
// using ConversionHive.Models.Mail;
//
// namespace ConversionHive.Services.Implementations;
//
// public class LocalEmailSender : IMailer
// {
//     private readonly SmtpSender _sender;
//     
//     public LocalEmailSender()
//     {
//         _sender = new SmtpSender(() => new SmtpClient("localhost")
//         {
//             EnableSsl = false,
//             DeliveryMethod = SmtpDeliveryMethod.Network,
//             Port = 25
//         });
//     }
//
//     public async Task<bool> SendMail(Mail mail)
//     {
//         // Using Razor templating package (or set using AddRazorRenderer in services)
//         Email.DefaultRenderer = new RazorRenderer();
//
//         var template = $"""
//                             <h1 style="color:blue;">{mail.Name}</h1>
//                             Dear @Model.Name,
//
//                             <p>@Model.Body</p>
//                        """;
//
//         var email = Email
//             .From("bob@hotmail.com")
//             .To("somedude@gmail.com")
//             .Subject("woo nuget")
//             .UsingTemplate(template, new Contact { FirstName = "Luke", LastName = "Awesome" });
//
//
//          Email.DefaultSender = _sender;
//         
//          var sendResponse = new SendResponse();
//         
//          foreach (var receiver in mail.Receivers)
//          {
//              sendResponse = await Email
//              .From(mail.Sender, mail.Name)
//              .To(receiver.EmailAddress)
//              .Subject(mail.Subject)
//              //.Body(mail.Body)
//              .UsingTemplate(template, mail)
//              .SendAsync();
//          }
//         
//          return sendResponse.Successful;
//     }
// }
