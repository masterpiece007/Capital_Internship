using Capital_Internship.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Capital_Internship.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Program_> Programs { get; set; }
        public DbSet<ApplicationRequirement> ApplicationRequirements { get; set; }
        public DbSet<AdditionalQuestion> AdditionalQuestions { get; set; }
        public DbSet<QuestionChoice> QuestionChoices { get; set; }
        public DbSet<CandidateApplication> CandidateApplications { get; set; }
        public DbSet<QuestionResponse> QuestionResponses { get; set; }
        public DbSet<SelectedMultiChoiceItem> SelectedDropdownItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAutoscaleThroughput(1000);

            modelBuilder.HasDefaultContainer("Program_");

            modelBuilder.Entity<Program_>()
              .HasNoDiscriminator()
              .HasPartitionKey(x => x.Id)
              .HasKey(x => x.Id);

            modelBuilder.Entity<ApplicationRequirement>()
               .HasNoDiscriminator()
               .ToContainer("ApplicationRequirements")
               .HasPartitionKey(x => x.Id)
               .HasKey(x => x.Id);

            modelBuilder.Entity<AdditionalQuestion>()
               .HasNoDiscriminator()
               .ToContainer("AdditionalQuestions")
               .HasPartitionKey(x => x.Id)
               .HasKey(x => x.Id);

            modelBuilder.Entity<QuestionChoice>()
               .HasNoDiscriminator()
               .ToContainer("QuestionChoices")
               .HasPartitionKey(x => x.Id)
               .HasKey(x => x.Id);


            modelBuilder.Entity<CandidateApplication>()
               .HasNoDiscriminator()
               .ToContainer("CandidateApplications")
               .HasPartitionKey(x => x.Id)
               .HasKey(x => x.Id);

            modelBuilder.Entity<QuestionResponse>()
               .HasNoDiscriminator()
               .ToContainer("QuestionResponses")
               .HasPartitionKey(x => x.Id)
               .HasKey(x => x.Id);

            modelBuilder.Entity<SelectedMultiChoiceItem>()
               .HasNoDiscriminator()
               .ToContainer("SelectedMultiChoiceItems")
               .HasPartitionKey(x => x.Id)
               .HasKey(x => x.Id);
        }
    }

}
