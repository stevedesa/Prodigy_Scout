using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProdigyScout.Models;
using System.Diagnostics;
using System.Reflection;

namespace ProdigyScout.Data.SeedData
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider, Assembly assembly)
        {
            using var context = new ProdigyScoutContext(serviceProvider.GetRequiredService<DbContextOptions<ProdigyScoutContext>>());

            // Look for any Students.
            if (!context.Users.Any())
            {
                SeedUser(context, "recruiter1@example.com", "Test_1234", "Recruiter", "One");
                SeedUser(context, "recruiter2@example.com", "Test_1234", "Recruiter", "Two");
                SeedUser(context, "recruiter3@example.com", "Test_1234", "Recruiter", "Three");
                context.SaveChanges();
            }

            if (!context.Prospect.Any())
            {
                SeedStudents(context, assembly);
                context.SaveChanges();
            }
        }

        private static void SeedUser(ProdigyScoutContext context, string email, string password, string firstName, string lastName)
        {
            var user = new IdentityUser
            {
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true
            };

            PasswordHasher<IdentityUser> passwordHasher = new();
            string passwordHash = passwordHasher.HashPassword(user, password);
            user.PasswordHash = passwordHash;

            context.Users.Add(user);
        }

        // var assembly = Assembly.GetExecutingAssembly();
        private static void SeedStudents(ProdigyScoutContext context, Assembly assembly)
        {
            string resourceName = "ProdigyScout.Data.SeedData.Prospects.csv";
            string line;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                // Eat the header row.
                reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    // Writes to the Output Window.
                    Debug.WriteLine(line);

                    string[] values = line.Split(",");

                    var prospect = new Prospect
                    {
                        FirstName = values[0],
                        LastName = values[1],
                        Email = values[2],
                        Gender = values[3],
                        GPA = float.Parse(values[4]),
                        GraduationDate = DateTime.Parse(values[5]),
                        Degree = values[6],
                    };

                    var complexDetails = new ComplexDetails
                    {
                        IsWatched = false, // Set IsWatched to false
                        IsPipeline = false, // Set Is Pipeline to false
                        Comment = null, // Set Comment String to null
                        Prospect = prospect // Associate ComplexDetails with Prospect
                    };

                    prospect.ComplexDetails = complexDetails; // Associate Prospect with ComplexDetails

                    context.Prospect.Add(prospect);
                    context.ComplexDetails.Add(complexDetails);
                }
            }
            context.SaveChanges();
        }
    }
}
