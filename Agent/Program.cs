using Agent.localhost;
using System;
using System.Threading;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Agent
{
    class Program
    {
        static private IObservable<long> timer;
        static int counter = 0;

        static private WallstreetDataServiceClient wallstreetClient;
        static private Order[] orders;
        static private ShareInformation[] stockPrices;

        static bool exitSystem = false;
        
        private static void RandomlyUpdateASingleStock()
        {
            counter++;
            if (stockPrices.Length > 0 && counter % 3 == 0)
            {
                counter = 0;
                Random rrd = new Random();
                int index = rrd.Next(0, stockPrices.Length - 1);
                ShareInformation oldPrice = stockPrices[rrd.Next(0, stockPrices.Length - 1)];
                Console.WriteLine("UPDATE stock price {0} randomly: {1}", oldPrice.FirmName, DateTime.Now);

                double x = Math.Max(1, oldPrice.PricePerShare * (1 + (rrd.Next(-3, 3) / 100.0)));
                ShareInformation newPrice = new ShareInformation()
                {
                    ExchangeName = oldPrice.ExchangeName,
                    FirmName = oldPrice.FirmName,
                    NoOfShares = oldPrice.NoOfShares,
                    PricePerShare = x,
                    IsFund = oldPrice.IsFund
                };
                Console.WriteLine("Update {0} from {1} to {2}.", newPrice.FirmName, oldPrice.PricePerShare, newPrice.PricePerShare);

                stockPrices[index] = newPrice;
            }
        }

        static private long PendingOrders(string stockName, OrderType orderType)
        {
            long stocks = 0;

            for (int i = 0; i < orders.Length; i++)
            {
                Order order = orders[i];
                if (order.ShareName == stockName && (order.Status == OrderStatus.OPEN || order.Status == OrderStatus.PARTIAL) && order.Type == orderType)
                {
                    stocks += order.NoOfOpenShares;
                }
            }

            return stocks;
        }


        static void Main(string[] args)
        {
            wallstreetClient = new WallstreetDataServiceClient(new InstanceContext(new WallstreetHandlerDummy()));

            timer = Observable.Interval(TimeSpan.FromSeconds(2));
            timer.Subscribe(_ => UpdateStockPrices());


            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        static void UpdateStockPrices()
        {

            stockPrices = wallstreetClient.GetOverallMarketInformation();
            orders = wallstreetClient.GetOverallOrders();

            for (int i = 0; i < stockPrices.Length; i++)
            {
                ShareInformation oldPrice = stockPrices[i];
                long pendingBuyOrders = PendingOrders(oldPrice.FirmName, OrderType.BUY);
                long pendingSellOrders = PendingOrders(oldPrice.FirmName, OrderType.SELL);
                double x = ComputeNewPrice(oldPrice.PricePerShare, pendingBuyOrders, pendingSellOrders);
                ShareInformation newPrice = new ShareInformation()
                {
                    ExchangeName = oldPrice.ExchangeName,
                    FirmName = oldPrice.FirmName,
                    NoOfShares = oldPrice.NoOfShares,
                    IsFund = oldPrice.IsFund,
                    PricePerShare = x
                };
                Console.WriteLine("Update {0} from {1} to {2}.", newPrice.FirmName, oldPrice.PricePerShare, newPrice.PricePerShare);

                stockPrices[i] = newPrice;
            }

            RandomlyUpdateASingleStock();

            for (int i = 0; i < stockPrices.Length; i++)
            {
                wallstreetClient.PutShareInformation(stockPrices[i], stockPrices[i].ExchangeName);
            }
        }

        static double ComputeNewPrice(double oldPrice, long pendingBuyOrders, long pendingSellOrders)
        {
            double d = Math.Max(1, pendingBuyOrders + pendingSellOrders);
            double n = (double)(pendingBuyOrders - pendingSellOrders);
            double x = (1 + ((n / d) * (1.0 / 16.0)));

            return Math.Max(1, oldPrice * x);
        }
    }
}
