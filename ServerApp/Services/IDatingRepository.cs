using System.Collections.Generic;
using System.Threading.Tasks;
using ServerApp.Helper;
using ServerApp.Models;

namespace ServerApp.Services
{
	public interface IDatingRepository
	{
		Task<T> Add<T>(T entity) where T : class;
		void Delete<T>(T entity) where T : class;
		Task<PageList<User>> GetUsers(UserParams userParams);
		Task<User> GetUser(int id);
		Task<bool> SaveAll();
		Task<Photo> GetPhoto(int id);
		Task<Photo> GetMainPhoto(int userId);

	}
}