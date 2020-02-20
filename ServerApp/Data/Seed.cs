using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using ServerApp.Models;

namespace ServerApp.Data
{
	public class Seed
	{
		private readonly DataContext _context;

		public Seed(DataContext context)
		{
			_context = context;
		}

		public void SeedData()
		{
			if (!_context.Users.Any())
			{
				var userData = File.ReadAllText("Data/UserSeedData.json");
				var users = JsonConvert.DeserializeObject<List<User>>(userData);
				foreach (var user in users)
				{
					byte[] passwordHash, passwordSalt;
					CreatePasswordHash("password", out passwordHash, out passwordSalt);
					user.PasswordHash = passwordHash;
					user.PasswordSalt = passwordSalt;
					user.Username = user.Username.ToLower();

					_context.Add(user);
				}

				_context.SaveChanges();
			}
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			}
		}
	}
}