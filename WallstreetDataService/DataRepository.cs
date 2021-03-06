﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallstreetDataService.Model;

namespace WallstreetDataService
{
    public class DataRepository
    {
        public DataRepository()
        {
            Exchanges = new Dictionary<string, Exchange>();
            Exchanges.Add("ATX", new Exchange());
            Exchanges.Add("DAX", new Exchange());
        }

        public Dictionary<string, Exchange> Exchanges { get; private set; }

        public class Exchange
        {
            public Exchange()
            {
                Brokers = new ConcurrentBag<IBroker>();
                ShareInformation = new ConcurrentDictionary<string, ShareInformation>();
                InvestorDepots = new ConcurrentDictionary<string, InvestorDepot>();
                FundDepots = new ConcurrentDictionary<string, FundDepot>();
                Orders = new ConcurrentDictionary<string, Order>();
                Transactions = new ConcurrentBag<Transaction>();
                FirmDepots = new ConcurrentDictionary<string, FirmDepot>();
                PendingFirmRegistrationRequests = new ConcurrentBag<FirmRegistration>();
                PendingFundRegistrationRequests = new ConcurrentBag<FundRegistration>();
                ShareInformationCallbacks = new ConcurrentBag<Action<ShareInformation>>();
                FundCallbacks = new ConcurrentBag<Action<FundDepot>>();
                OrderCallbacks = new ConcurrentBag<Action<Order>>();
                InvestorCallbacks = new ConcurrentBag<Action<InvestorDepot>>();
                TransactionCallbacks = new ConcurrentBag<Action<Transaction>>();
            }

            public ConcurrentBag<IBroker> Brokers { get; set; }
            public ConcurrentDictionary<string, ShareInformation> ShareInformation { get; set; }
            public ConcurrentDictionary<string, InvestorDepot> InvestorDepots { get; set; }
            public ConcurrentDictionary<string, FundDepot> FundDepots { get; set; }
            public ConcurrentDictionary<string, Order> Orders { get; set; }
            public ConcurrentBag<Transaction> Transactions { get; set; }
            public ConcurrentDictionary<string, FirmDepot> FirmDepots { get; set; }
            public ConcurrentBag<FirmRegistration> PendingFirmRegistrationRequests { get; set; }
            public ConcurrentBag<FundRegistration> PendingFundRegistrationRequests { get; set; }
            public ConcurrentBag<Action<ShareInformation>> ShareInformationCallbacks { get; set; }
            public ConcurrentBag<Action<FundDepot>> FundCallbacks { get; set; }
            public ConcurrentBag<Action<Order>> OrderCallbacks { get; set; }
            public ConcurrentBag<Action<InvestorDepot>> InvestorCallbacks { get; set; }
            public ConcurrentBag<Action<Transaction>> TransactionCallbacks { get; set; }
        }
    }
}
