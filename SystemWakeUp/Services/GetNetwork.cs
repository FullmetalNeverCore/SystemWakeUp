using System;
using System.Net.NetworkInformation;
using SystemWakeUp.Network;

namespace SystemWakeUp.Services
{
    public class GetNetwork : IHostedService, IDisposable
    {
        private Timer _timer;
        private List<string> _devices = new List<string>() { "192.168.8.186", "192.168.8.176" };

        public void Dispose()
        {
            //desposing of timer
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //starting a new task
            _timer = new Timer(CheckNetwork,null,TimeSpan.Zero,TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        public void CheckNetwork(object state)
        {
            List<string> macs = Enumerable.Repeat(0, 2)
                .Select(i => MagicPacket.GetMacByIP(_devices[i]))
                .ToList();


            for(int x = 0; x < macs.Count(); x++)
            {
                if (macs[x] != null)
                {
                    Console.WriteLine($"Trying to send magic package to {macs[x]}");
                    MagicPacket.WakeOnLan("30:9c:23:e1:c4:50");
                }
                else
                {
                    Console.WriteLine($"Unable to obtain MAC for: {_devices[x]} : {macs[x]}");
                }
            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}

