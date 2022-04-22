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
        public DbSet<Picture> Picture  { get; set; }
        public DbSet<Video> Video { get; set; } 
        public DbSet<VideoFile> VideoFile { get; set; } 
        public DbSet<Document> Document { get; set; }
        public DbSet<DocFile> DocFile { get; set; }
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
            modelBuilder.Entity<Guide>().HasData(new ERS.Model.Guide[] { new ERS.Model.Guide { Id=1, MainGuideId=1, Name="Главное окно" } }) ;
            modelBuilder.Entity<Picture>().ToTable("Picture");
            modelBuilder.Entity<Video>().ToTable("Video");
            modelBuilder.Entity<VideoFile>().ToTable("VideoFile");
            modelBuilder.Entity<Document>().ToTable("Document");
            modelBuilder.Entity<DocFile>().ToTable("DocFile");
        }
    }
}
