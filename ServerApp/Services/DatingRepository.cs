using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;
using ServerApp.Helper;
using ServerApp.Models;

namespace ServerApp.Services
{
	public class DatingRepository:IDatingRepository
	{
		private readonly DataContext _context;


		public DatingRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<T> Add<T>(T entity) where T : class
		{
			await _context.AddAsync(entity);
			return entity;
		}

		public void Delete<T>(T entity) where T : class
		{
			_context.Remove(entity);
		}

		public async Task<PageList<User>> GetUsers(UserParams userParams)
		{
			var users = _context.Users.Include(p => p.Photos)
				.OrderByDescending(x=>x.LastActive);
				users = users.Where(x=>x.Id != userParams.UserId).OrderByDescending(x => x.LastActive);
				users = users.Where(x=>x.Gender == userParams.Gender).OrderByDescending(x => x.LastActive);
				if (userParams.Likers)
				{
					var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
					users = users.Where(x => userLikers.Contains(x.Id)).OrderByDescending(x=>x.LastActive);
				}

				if (userParams.Likees)
				{
					var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
					users = users.Where(x => userLikees.Contains(x.Id)).OrderByDescending(x => x.LastActive);
				}
				if(userParams.MinAge != 18 || userParams.MaxAge != 99){
					var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
					var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

					users = users.Where(x=>x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob).OrderByDescending(x => x.LastActive);
				}
			if (!string.IsNullOrEmpty(userParams.OrderBy))
			{
				switch (userParams.OrderBy)
				{
					case "created":
						users = users.OrderByDescending(x => x.Created);
						break;
					default:
						users = users.OrderByDescending(x => x.LastActive);
						break;
				}
			}
			return await PageList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
		}

		private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
		{

			var user = await _context.Users.Include(l => l.Likees)
				.Include(l => l.Likers).FirstOrDefaultAsync(x=>x.Id == id);
			if (likers)
			{
				return user.Likers.Where(x => x.LikeeId == id).Select(x => x.LikerId);
			}
			else
			{
				return user.Likees.Where(x => x.LikerId == id).Select(x => x.LikeeId);
			}


		}

		public async Task<User> GetUser(int id)
		{
			var user = await _context.Users
				.Include(p=>p.Photos)
				.FirstOrDefaultAsync(x => x.Id == id);
			return user;
		}

		public async Task<bool> SaveAll()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<Photo> GetPhoto(int id)
		{
			var photo = await _context.Photos.FirstOrDefaultAsync(x => x.Id == id);
			return photo;
		}

		public async Task<Photo> GetMainPhoto(int userId)
		{
			var mainPhoto = await _context.Photos.Where(x => x.UserId == userId)
				.FirstOrDefaultAsync(x => x.IsMain);
			return mainPhoto;
		}

		public async Task<Like> GetLike(int userId, int recepientId)
		{
			return await _context.Likes.FirstOrDefaultAsync(x => x.LikerId == userId && x.LikeeId == recepientId);	

		}

		public async Task<Message> GetMessage(int id)
		{
			return await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<PageList<Message>> GetMessagesForUser(MessagesParam messagesParam)
		{
			var messages = _context.Messages
				.Include(u => u.Sender)
				.ThenInclude(p => p.Photos)
				.Include(u => u.Recipient)
				.ThenInclude(p => p.Photos).AsQueryable();
			switch (messagesParam.MessageContainer)
			{
				case "Inbox":
					messages = messages.Where(m => m.RecipientId == messagesParam.UserId && m.RecipientDelete == false);
					break;
				case "Outbox":
					messages = messages.Where(m => m.SenderId == messagesParam.UserId && m.SenderDelete == false);
					break;
				default:
					messages = messages.Where(m => m.RecipientId == messagesParam.UserId 
					                               && m.IsRead == false && m.RecipientDelete ==false);
					break;
			}

			messages = messages.OrderByDescending(i => i.MessageSent);
			return await PageList<Message>.CreateAsync(messages, messagesParam.PageNumber, messagesParam.PageSize);

		}

		public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
		{
			var messages =await _context.Messages
				.Include(u => u.Sender)
				.ThenInclude(p => p.Photos)
				.Include(u => u.Recipient)
				.ThenInclude(p => p.Photos)
				.Where(m=>m.RecipientId ==userId && m.SenderId == recipientId && m.RecipientDelete ==false
				          || m.RecipientId == recipientId && m.SenderId == userId && m.SenderDelete ==false)
				.OrderBy(x => x.MessageSent)
				.ToListAsync();
			return messages;
		}
	}
}