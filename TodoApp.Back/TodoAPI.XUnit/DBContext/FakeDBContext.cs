using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.DAL.DBContext;

namespace TodoAPI.XUnit.DBContext
{
    public static class FakeDBContext
    {
        public static AppDBContext MakeTestInstance()
        {
            return new AppDBContext(new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options, true);
        }
    }
}
