using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using SystemWakeUp.Exceptions;
using SystemWakeUp.Scripts;

namespace SystemWakeUp.Network
{
	public static class MagicPacket
	{

        public static string? GetMacByIP(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return null;
            }

            Console.WriteLine($"Trying to get mac of {ipAddress}....");
            //use ScriptRunner to obtain mac of master pc
            return ScriptRunner.RunProcess(ipAddress);
        }

        public static void Broadcast(string macAddress, int port = 9)
        {
            UdpClient client = new UdpClient() { EnableBroadcast = true };
            client.Connect(IPAddress.Broadcast, port);

            int counter = 0;
            byte[] bytes = new byte[102];

            //brewing magic packet
            //6 times 255(FF) 
            for (int x = 0; x < 6; x++)
                bytes[counter++] = 0xFF; //FF 255
            //16 times device mac
            for (int macPackets = 0; macPackets < 16; macPackets++)
                for (int macBytes = 0; macBytes < 12; macBytes += 2)
                    bytes[counter++] = byte.Parse(macAddress.Substring(macBytes, 2), NumberStyles.HexNumber);

            client.Send(bytes, bytes.Length);
        }




    }
}

