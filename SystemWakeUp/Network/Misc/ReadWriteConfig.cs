using System;
using System.IO;
using Newtonsoft.Json;
using SystemWakeUp.Exceptions;

namespace SystemWakeUp.Network.Misc
{
	public static class ReadWriteConfig
	{
        public static string[] ReadDBCreds()
        {
            try
            {
                return File.ReadAllLines("dbconfig.txt");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return new string[2] {"root","toor"};
            }
        }

		public static List<string> Read()
		{
            string json = File.ReadAllText("config.json");

            return JsonConvert.DeserializeObject<List<string>>(json);
        }

		public static string ReadMaster(byte x = 0)
		{
			if(x == 0)
			{
                try
                {
                    //obtaining master's ip and returning it.
                    string[] lines = File.ReadAllLines("masterip.txt");
                    return lines[0];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return "None";
                }
            }
			else
			{
                try
                {
                    //obtaining master's ip and returning it.
                    string[] lines = File.ReadAllLines("masterdevice.json");
                    return lines[0];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return "None";
                }
            }

		}

		public static void Write(List<string> macs)
		{
			if(macs.Count < 1)
			{
                throw new InvalidMacException("No MAC's to write.");
			}
			else
			{
                string json = JsonConvert.SerializeObject(macs, Formatting.Indented);
                File.WriteAllText("config.json", json);

                Console.WriteLine("Successfully written!");
            }
        }

		//Writing Master's device. 
		public static void Write(string mastermac)
		{
			if((mastermac.Split(":")).Count() != 6)
			{
				throw new InvalidMacException("Invalid master's mac");
			}

			Dictionary<string, string> masterdict = new Dictionary<string, string>()
			{
				{"Master",mastermac}
			};

			string json = JsonConvert.SerializeObject(masterdict, Formatting.Indented);

			File.WriteAllText("masterdevice.json", json);

			Console.WriteLine("Master device created");
		}

    }
}

