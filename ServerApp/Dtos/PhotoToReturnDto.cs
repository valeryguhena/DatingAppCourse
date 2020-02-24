using System;

namespace ServerApp.Dtos
{
	public class PhotoToReturnDto
	{
		public int Id { get; set; }
		public string Url { get; set; }
		public string Description { get; set; }
		public bool IsMain { get; set; }
		public DateTime DateAdded { get; set; }
		public string PublicId { get; set; }
	}
}