using System;

namespace ServerApp.Helper
{
	public static class Extensions
	{
		public static int CalculateAge(this DateTime dateOfBirth)
		{
			var age = DateTime.Today.Year - dateOfBirth.Year;
			if (dateOfBirth.AddYears(age) > DateTime.Today)
				age--;
			return age;
		}
	}
}