
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemWakeUp.Controllers.Structures;
using SystemWakeUp.DBHandler.Entity;
using SystemWakeUp.Exceptions;
using SystemWakeUp.Network;
using SystemWakeUp.Network.Misc;
using SystemWakeUp.Scripts;
using SystemWakeUp.Services;

public class GetNetwork : IHostedService
{
    private Timer _timer;
    private readonly IServiceScopeFactory _scopeFactory; 
    private readonly List<string> _devices = new List<string> { "192.168.8.186" }; //devices to send magic packet to.
    private readonly LastStatus _lastStatus;


    public GetNetwork(IServiceScopeFactory scopeFactory, LastStatus lastStatus)
    {
        _scopeFactory = scopeFactory;
        _lastStatus = lastStatus;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Start the timer to check the network every 30 seconds
        _timer = new Timer(async _ => await CheckNetwork(), null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async Task CheckNetwork()
    {
        string mastermac;


        mastermac = "None";
        Console.WriteLine(@"


                        MySystemWakeUP
------------------------------------------------------------------


");
        Console.WriteLine($"mastermac: {mastermac}");
        Console.WriteLine("Obtaining Master's device...");
        string master = ReadWriteConfig.ReadMaster();

        if (master == "None")
        {
            throw new InvalidMasterException("Create .txt file with master's device ip (masterip.txt)");
        }

        Console.WriteLine("Reached ScriptRunner (Network Lookup)");

        mastermac = ScriptRunner.RunProcess(ReadWriteConfig.ReadMaster()); //trying to get master ip

        mastermac = mastermac.Trim();

        Console.WriteLine($"Master's MAC status: {mastermac}");

        if (mastermac != "None")
        {
            //Todo: put master mac into database.
            ReadWriteConfig.Write(mastermac); //writing master mac to the json
        }
        else
        {
            mastermac = ScriptRunner.SearchForMaster(ReadWriteConfig.ReadMaster(1));
            mastermac = mastermac.Trim();
            Console.WriteLine($"Master's MAC status after search: {mastermac}");
        }

        if (mastermac != "None")
        {
            if (_lastStatus.lastStatus == false)
            {
                Console.WriteLine("Previous Status was False,sending magic packet...");
                List<string> macs = Enumerable.Repeat(0, 2)
                .Select(i => MagicPacket.GetMacByIP(_devices[i]))
                .ToList();


                if (macs.Any(i => i.Trim() != "None"))
                {
                    ReadWriteConfig.Write(macs);//writing to inner config
                }
                else
                {
                    macs = ReadWriteConfig.Read();
                }

                //foreach(var x in macs)
                //{
                //    Console.WriteLine(x);
                //}

                //MagicPacket.Broadcast("309c23e1c450");
                for (int x = 0; x < macs.Count(); x++)
                {
                    if (macs[x].Trim() != "None")
                    {
                        _lastStatus.lastStatus = true; //for further iterations.
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var entityService = scope.ServiceProvider.GetRequiredService<EntityService>();
                            await entityService.AddEntityAsync(UpdateDB(mastermac)); //sending data to db
                        }
                        string trimmedmac = macs[x].Replace(":", "").Replace("-", "");
                        Console.WriteLine($"Trying to send magic package to {trimmedmac}");
                        MagicPacket.Broadcast(trimmedmac);
                        mastermac = "None";
                    }
                    else
                    {
                        Console.WriteLine($"Unable to obtain MAC for: {_devices[x]} : {macs[x]}");
                    }
                }
            }

        }
        else
        {
            Console.WriteLine("Master Device offline.");
            _lastStatus.lastStatus = false;
            mastermac = "None";
        }
        Console.WriteLine("Sleeping for 10secs...");

    }

    private DBEntity UpdateDB(string masterMac)
    {
        return new DBEntity
        {
            mac = masterMac,
            lastlogin = DateTime.UtcNow
        };
    }
}

