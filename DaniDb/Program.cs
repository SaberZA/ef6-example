using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DaniDb
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var ctx = new DaniContext())
            {
                var savedApplicants = ctx.Applicants.Count();
                Console.WriteLine(savedApplicants);
                
                AddApplicantWithJobs(ctx);

                FindApplicantByJob(ctx);
            } 
            
        }

        private static void FindApplicantByJob(DaniContext ctx)
        {
            var jobId = 2;

            var applicants = ctx.JobListings.Where(p => p.JobID == jobId).SelectMany(x => x.Applicants);
            
            Console.WriteLine(String.Join(",",applicants.Select(p=>p.Name)));
            
        }

        private static void AddApplicantWithJobs(DaniContext ctx)
        {
            var applicant = new Applicant
            {
                Name = "Steven van der Merwe"
            };

            var jobListing1 = new JobListing {JobTitle = "Job1"};
            var jobListing2 = new JobListing {JobTitle = "Job2"};

            applicant.JobListings.Add(jobListing1);
            applicant.JobListings.Add(jobListing2);

            ctx.Applicants.Add(applicant);
            ctx.SaveChanges();
        }
    }

    public class DaniContext : DbContext
    {
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<JobListing> JobListings { get; set; }
        
        public DaniContext() : base("Dani")
        { }
    }
    
    [Table("Applicant")]
    public class Applicant
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        
        public virtual ICollection<JobListing> JobListings { get; set; }
        
        public Applicant()
        {
            JobListings = new List<JobListing>();
        }
    }

    [Table("JobListing")]
    public class JobListing
    {
        [Key]
        public int JobID { get; set; }
        [Required]
        public string JobTitle { get; set; }
        
        public virtual ICollection<Applicant> Applicants { get; set; }
        
        public JobListing()
        {
            Applicants = new List<Applicant>();
        }
    }
    
    
}