using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace WorkerGmailExcel
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using (var client = new ImapClient())
                {
                    try
                    {
                        using (var cancel = new CancellationTokenSource())
                        {
                            client.Connect("imap.gmail.com", 993, true, cancel.Token);

                            // If you want to disable an authentication mechanism,
                            // you can do so by removing the mechanism like this:
                            client.AuthenticationMechanisms.Remove("XOAUTH");

                            client.Authenticate("cacharreos2020@gmail.com", "Cacharreos.2020", cancel.Token);

                            // The Inbox folder is always available...
                            var inbox = client.Inbox;
                            inbox.Open(FolderAccess.ReadOnly, cancel.Token);

                            Console.WriteLine("Total messages: {0}", inbox.Count);
                            Console.WriteLine("Recent messages: {0}", inbox.Recent);

                            // download each message based on the message index
                            for (int i = 0; i < inbox.Count; i++)
                            {
                                var message = inbox.GetMessage(i, cancel.Token);
                                Console.WriteLine("Asunto: {0}", message.Subject);
                                Console.WriteLine("ResentDate: {0}", message.ResentDate);
                                Console.WriteLine("Fecha: {0}", message.Date.LocalDateTime);
                                if (message.Attachments.Count() != 0)
                                {
                                    Console.WriteLine("Adjunto: {0}", message.Attachments.FirstOrDefault().IsAttachment);
                                    MimeEntity adjunto = message.Attachments.FirstOrDefault();
                                }
                            }

                            // let's try searching for some messages...
                            var query = SearchQuery.DeliveredAfter(DateTime.Parse("2013-01-12"))
                                .And(SearchQuery.SubjectContains("MailKit"))
                                .And(SearchQuery.Seen);

                            foreach (var uid in inbox.Search(query, cancel.Token))
                            {
                                var message = inbox.GetMessage(uid, cancel.Token);
                                Console.WriteLine("[match] {0}: {1}", uid, message.Subject);
                            }

                            client.Disconnect(true, cancel.Token);
                        }
                    }
                    catch (Exception e)
                    {

                    }

                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}