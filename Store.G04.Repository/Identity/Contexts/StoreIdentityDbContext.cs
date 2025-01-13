using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.G04.core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Repository.Identity.Contexts
{
    public class StoreIdentityDbContext : IdentityDbContext<AppUser>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext>options) : base(options)
        {

        }
    }
}
//Add-Migration AddIdentityTables -Context StoreIdentityDbContext -OutputDir Identity/Migrations

