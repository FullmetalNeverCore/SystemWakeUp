using System;
namespace SystemWakeUp.Exceptions
{
	public class NoNetDevicesAvailableException : Exception
	{
		public NoNetDevicesAvailableException(string message) : base(message)
		{
		}
	}
}

