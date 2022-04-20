using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERS.Model
{
    public class ConnectDB : DbContext
    {   

        public DbSet<Guide> Guide { get; set; }
        public ConnectDB()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            //Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite($"Filename=ERS.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Guide>().ToTable("Guide");
            modelBuilder.Entity<Guide>().HasData(new ERS.Model.Guide[] { new ERS.Model.Guide { Id=1, MainGuideId=1, Name="" }, new ERS.Model.Guide { Id=2, MainGuideId=1, Name="21" } , new ERS.Model.Guide { Id=3, MainGuideId=2, Name="32" } 
    ,new ERS.Model.Guide { Id=4, MainGuideId=2, Name="42" }
,new ERS.Model.Guide { Id=5, MainGuideId=2, Name="52" },new ERS.Model.Guide { Id=6, MainGuideId=3, Name="63" } }) ;
        }
    }
}
