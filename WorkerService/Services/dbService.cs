using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService.Context;
using WorkerService.Models;

namespace WorkerService.Services
{
    public class dbService
    {
        private readonly dbContext _dbContext;
        private readonly IMapper _mapper;
        public dbService(dbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task AddUser(User user, CancellationToken stoppingToken) {
            var userDto = new UserDto() { Avatar = user.Avatar, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName };

            _dbContext.Users.Add(_mapper.Map<User>(userDto));
            await _dbContext.SaveChangesAsync(stoppingToken);

            Console.WriteLine("Adding user to dbContext");

        }
        public async Task UpdateUser(User existingUser, User user, CancellationToken stoppingToken)
        {
            existingUser.Avatar = user.Avatar;
            existingUser.Email = user.Email;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            _dbContext.Users.Update(existingUser);
            Console.WriteLine("Updating the dbContext");
            await _dbContext.SaveChangesAsync(stoppingToken);

        }
        public List<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }
        public User FirstOrDefault(User user)
        {
           return _dbContext.Users.FirstOrDefault(u => u.UserId == user.UserId);
        }
        public async Task SaveChangesAsync(CancellationToken stoppingToken)
        {
            await _dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}
