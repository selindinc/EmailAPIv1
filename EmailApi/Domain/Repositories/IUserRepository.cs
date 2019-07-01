using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailApi.Models;


namespace EmailApi.Services
{
    public interface IUserRepository
    {
        User Add(User user);
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Delete(User user);
        void Update(User user);
    }
}
