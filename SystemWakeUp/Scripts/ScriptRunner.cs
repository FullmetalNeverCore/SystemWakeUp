﻿using System;
using System.Diagnostics;
using System.Net;
using SystemWakeUp.Exceptions;
using SystemWakeUp.Network.Misc;

namespace SystemWakeUp.Scripts
{
	public class ScriptRunner
	{

        public static string SearchForMaster(string mastermac)
        {
            Console.WriteLine("Searching for master");
            // Get the main project folder
            string projectFolder = AppDomain.CurrentDomain.BaseDirectory;

            string scriptFolder = Path.Combine(projectFolder, "Scripts");
            string script = Path.Combine(scriptFolder, "macaddr.py");




            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = "python3",
                Arguments = $"\"{script}\" {mastermac}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardError)
                {
                    string err = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(err))
                    {
                        Console.WriteLine($"Error: {err}");
                    }
                }

                using (StreamReader reader = process.StandardOutput)
                {
                    string res = reader.ReadToEnd();
                    if (res.Trim() == "None")
                    {
                        return "None";
                    }
                    Console.WriteLine($"Master has been found: {res}");
                    return "True";
                }
            }

        }

        public static string? RunProcess(string ip)
		{
			Console.WriteLine("Reached ScriptRunner.");
            // Get the main project folder
            string projectFolder = AppDomain.CurrentDomain.BaseDirectory;

			string scriptFolder = Path.Combine(projectFolder, "Scripts");
			string script = Path.Combine(scriptFolder, "macaddr.py");

			ProcessStartInfo start = new ProcessStartInfo
			{
				FileName = "python3",
				Arguments = $"\"{script}\" {ip}",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			};

			using(Process process = Process.Start(start))
			{
                using (StreamReader reader = process.StandardError)
                {
                    string err = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(err))
                    {
                        Console.WriteLine($"Error: {err}");
                    }
                }

                using (StreamReader reader = process.StandardOutput)
				{
					string res = reader.ReadToEnd();
					if(res == "None")
					{
						throw new InvalidMacException("Script returns None value");
					}
					Console.WriteLine($"Obtained MAC: {res}");
					return res; 
				}
			}

		}
	}
}

