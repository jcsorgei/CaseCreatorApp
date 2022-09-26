using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CaseCreatorApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<SvgData> SvgData { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Text> Texts { get; set; }
    }
}
