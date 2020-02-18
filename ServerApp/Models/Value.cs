using System;

namespace ServerApp.Models
{
	public class Value
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime? DateCreated { get; set; }
		public Value()
		{
				DateCreated = DateTime.Now;
		}
	}
}