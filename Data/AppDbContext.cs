using AuthPracticeAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthPracticeAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
           public AppDbContext(DbContextOptions<AppDbContext>options)
            : base(options)
        {

        }
    }
}
