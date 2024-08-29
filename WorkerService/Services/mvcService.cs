using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService.Context;
using WorkerService.Models;

namespace WorkerService.Services
{

    public class mvcService
    {
        private readonly MVCContext _mvcContext;
        private readonly IMapper _mapper;
        public  mvcService(MVCContext mvcContext, IMapper mapper)
        {
            _mvcContext = mvcContext;
            _mapper = mapper;
        }
        public async Task AddUser(User dbUser, CancellationToken stoppingToken)
        {
            var userDto = new UserDto() { Avatar = dbUser.Avatar, Email = dbUser.Email, FirstName = dbUser.FirstName, LastName = dbUser.LastName };
            _mvcContext.Users.Add(_mapper.Map<User>(userDto));
            await _mvcContext.SaveChangesAsync(stoppingToken);
            Console.WriteLine("Adding user to MVCContext");

        }
        public async Task UpdateUser(User mvcExistingUser, User dbUser, CancellationToken stoppingToken)
        {
            mvcExistingUser.Avatar = dbUser.Avatar;
            mvcExistingUser.FirstName = dbUser.FirstName;
            mvcExistingUser.LastName = dbUser.LastName;
            mvcExistingUser.Email = dbUser.Email;
            _mvcContext.Update(mvcExistingUser);
            await _mvcContext.SaveChangesAsync(stoppingToken);
            Console.WriteLine("Updating MVCContext");
        } 
        public List<User> GetUsers()
        {
            return _mvcContext.Users.ToList();
        }
        public async Task SaveChangesAsync(CancellationToken stoppingToken)
        {
            await _mvcContext.SaveChangesAsync(stoppingToken);
        }
    }
}
