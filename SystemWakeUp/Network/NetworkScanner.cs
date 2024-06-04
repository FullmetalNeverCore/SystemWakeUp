using System;
using System.Net;
using System.Net.NetworkInformation;
namespace SystemWakeUp.Network
{
	public class NetworkScanner
	{
		public static void PingCompleteCallback(object sender,PingCompletedEventArgs pe)
		{
			if(pe.Reply.Status == IPStatus.Success)
			{
				Console.WriteLine($"Find accessable ip {pe.Reply.Address}");
			}
		}
	}
}

