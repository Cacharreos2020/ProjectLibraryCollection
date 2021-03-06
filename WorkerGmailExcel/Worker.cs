using System;
using System.Collections.Generic;
using System.IO;
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
            DateTime fechaUltimoMensaje = new DateTime();
            int intervalo = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var client = new ImapClient())
                {
                    try
                    {
                        using (var cancel = new CancellationTokenSource())
                        {
                            client.Connect("imap.gmail.com", 993, true, cancel.Token);
                            client.Authenticate("cacharreos2020@gmail.com", "Cacharreos.2020", cancel.Token);

                            // Miramos al inbox del correo.
                            var inbox = client.Inbox;
                            inbox.Open(FolderAccess.ReadWrite, cancel.Token);

                            Console.WriteLine("Total messages: {0}", inbox.Count);
                            Console.WriteLine("Recent messages: {0}", inbox.Recent);

                            // Descargamos los mensajes que se encuentren en el index
                            foreach(var mensaje in inbox.Where( x => x.Date.LocalDateTime > fechaUltimoMensaje))
                            {
                                Console.WriteLine("Asunto: {0}", mensaje.Subject);
                                Console.WriteLine("ResentDate: {0}", mensaje.ResentDate);
                                Console.WriteLine("Fecha: {0}", mensaje.Date.LocalDateTime);
                                intervalo++;

                                if (mensaje.Attachments.Count() != 0)
                                {
                                    foreach (var att in mensaje.Attachments)
                                    {
                                        if (!File.Exists(@"C:\Users\" + Environment.UserName + "\\Downloads\\" + att.ContentType.Name))
                                        {
                                            //Funci�n de crear
                                            CrearArchivo(att);
                                        }
                                        else
                                        {
                                            File.Delete(@"C:\Users\" + Environment.UserName + "\\Downloads\\" + att.ContentType.Name);
                                            //Funci�n de crear
                                            CrearArchivo(att);
                                        }
                                    }                                    
                                }

                                inbox.AddFlags(intervalo, MessageFlags.Seen, true);

                                if (fechaUltimoMensaje < mensaje.Date.LocalDateTime)
                                {
                                    fechaUltimoMensaje = mensaje.Date.LocalDateTime;
                                }
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

        private static void CrearArchivo(MimeEntity att)
        {
            using (FileStream stream = File.Create(@"C:\Users\" + Environment.UserName + "\\Downloads\\" + att.ContentType.Name))
            {
                if (att is MessagePart)
                {
                    MessagePart rfc822 = (MessagePart)att;
                    rfc822.Message.WriteTo(stream);
                }
                else
                {
                    var part = (MimePart)att;
                    part.Content.DecodeTo(stream);
                }

                FileInfo fileInfo = new FileInfo(stream.Name);
            }
        }
    }
}
