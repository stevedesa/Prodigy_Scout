using ProdigyScout.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Reflection;
using ProdigyScout.Data;

namespace ProdigyScout.Data.SeedData
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider, Assembly assembly)
        {
            using var context = new ProdigyScoutContext(serviceProvider.GetRequiredService<DbContextOptions<ProdigyScoutContext>>());

            // Look for any Students.
            // NOTE:  Not robust enough yet.
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

                    // Logic to parse the line, separate by comma(s), and assign fields
                    // to the Student model.

                    string[] values = line.Split(",");

                    context.Prospect.Add(
                        new Prospect
                        {
                            FirstName = values[0],
                            LastName = values[1],
                            email = values[2],
                            Gender = values[3],
                            GPA = float.Parse(values[4])
                        }
                    );
                }
            }

            context.SaveChanges();
        }
    }
}
