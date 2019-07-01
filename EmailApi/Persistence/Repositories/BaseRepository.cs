using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailApi.Persistence.Contexts;

//just an abstract class that all repositories will inherit
//receives an instance of dbcontext class through dependency injection
//and exposes a protected property called _context
//that gives access to all methods we need to handle operations

namespace EmailApi.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly AppDBContext _context;

        public BaseRepository(AppDBContext context)
        {
            _context = context;
        }
    }
}
