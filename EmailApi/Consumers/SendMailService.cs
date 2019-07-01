using EmailApi.Contracts.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailApi.Domain.Services;
using EmailApi.Contracts;

namespace EmailApi.Consumers
{
    public class SendMailService : IConsumer<MailScheduledEvent>
    {
        private readonly IMailService _mailService;
        private readonly IBusControl _consumerBusControl;
        public SendMailService(IMailService mailService,IBusControl busControl)
        {
            _mailService = mailService;
            //_consumerBusControl = busControl;

        }
        public Task Consume(ConsumeContext<MailScheduledEvent> context)
        {
            var mailScheduledEvent = context.Message;

            Console.WriteLine($"MailId : {mailScheduledEvent.MailId}{Environment.NewLine}" +
                $"Schedule Date : {mailScheduledEvent.ScheduleDate}");

            var a = (mailScheduledEvent.ScheduleDate - DateTime.UtcNow);
           

            var value = DateTime.Compare(mailScheduledEvent.ScheduleDate, DateTime.UtcNow); 

            //// checking 
            if (value > 0) {
                Console.Write("ScheduleDate is later than UtcNow. ");

                var MailInfo = _mailService.GetMailById(mailScheduledEvent.MailId);


               if(_mailService.UpdateStatus(mailScheduledEvent.MailId, EmailTaskStatuses.Ongoing)== true)// eğer ongoinge çekilebildiyse
                {
                    Console.WriteLine($"MailId : {MailInfo.MailId}{Environment.NewLine}" +
                   $"Receiver Address : {MailInfo.ReceiverAddress}{Environment.NewLine}" +
                   $"Subject : {MailInfo.Subject}{Environment.NewLine}" +
                   $"Content : {MailInfo.Content}{Environment.NewLine}" +
                   $"Status : {MailInfo.EmailTaskStatus}{Environment.NewLine}");

                    _mailService.UpdateStatus(mailScheduledEvent.MailId, EmailTaskStatuses.Sent);
                    return Task.CompletedTask;

                }
                else //çekilemediyse
                {
                    if (_mailService.UpdateStatus(mailScheduledEvent.MailId, EmailTaskStatuses.Ongoing) == false) {
                        //ve beklenen bir hata dönerse
                        //    Console.Write("While mail status is cancel conversion to another status type is not possible.");
                        //    return Task.CompletedTask;

                        //throw new CustomExceptions.ValidationException("While mail status is cancel conversion to another status type is not possible.");
                        throw new CustomExceptions.ValidationException("This mail status conversion is not valid.");

                    }


                    else
                    {
                        //bir exception mı atmalıyım, error queue ya otomatik atılır mı o zaman
                        throw new CustomExceptions.NotFoundException(" Error");
                        //return Task.CompletedTask;
                    }//beklenmeyen bir hata dönerse
                }
            }

            else if (value <= 0) {

                Console.Write("ScheduleDate is earlier than UtcNow. ");

                //context.Redeliver(a);
                context.Redeliver(a);

                var MailInfo = _mailService.GetMailById(mailScheduledEvent.MailId);

                Console.WriteLine($"MailId : {MailInfo.MailId}{Environment.NewLine}" +
                    $"Receiver Address : {MailInfo.ReceiverAddress}{Environment.NewLine}" +
                    $"Subject : {MailInfo.Subject}{Environment.NewLine}" +
                    $"Content : {MailInfo.Content}{Environment.NewLine}" +
                    $"Status : {MailInfo.EmailTaskStatus}{Environment.NewLine}");
                return Task.CompletedTask;

            }
            return Task.CompletedTask;
        }  
        
        
    }
}
