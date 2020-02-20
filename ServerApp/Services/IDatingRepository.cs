using System.Collections.Generic;
using System.Threading.Tasks;
using ServerApp.Models;

namespace ServerApp.Services
{
	public interface IDatingRepository
	{
		Task<T> Add<T>(T entity) where T : class;
		void Delete<T>(T entity) where T : class;
		Task<IEnumerable<User>> GetUsers();
		Task<User> GetUser(int id);
		Task<bool> SaveAll();
		
	}
}