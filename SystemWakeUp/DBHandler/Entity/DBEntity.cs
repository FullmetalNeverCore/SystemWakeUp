using System;
using System.Net.NetworkInformation;

namespace SystemWakeUp.DBHandler.Entity
{
	public class DBEntity
	{
		public int Id { get; set; }
		public string mac { get; set; }
		public DateTime lastlogin { get; set; }
    }
}

