﻿namespace InsuranceQuoter.Acl.Insurer.Service.MessageHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using InsuranceQuoter.Infrastructure.Message.Requests;
    using InsuranceQuoter.Infrastructure.Message.Responses;
    using NServiceBus;

    public class JklInsurerQuoteRequestHandler : IHandleMessages<JklInsurerQuoteRequest>
    {
        public Task Handle(JklInsurerQuoteRequest message, IMessageHandlerContext context)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(3000);

            return context.Reply(
                new QuoteResponse
                {
                    Uid = Guid.NewGuid(),
                    Insurer = "Jkl Insurer PLC",
                    Premium = 1100M,
                    StartDate = DateTime.Now,
                    PremiumTax = 110M,
                    Addons = new List<string>()
                    {
                        "Personal accident cover",
                    }
                });
        }
    }
}