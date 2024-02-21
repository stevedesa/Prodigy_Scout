using Team09.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
using Team09.Data;

namespace Team09.Data.SeedData
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using var context = new Team09Context(serviceProvider.GetRequiredService<DbContextOptions<Team09Context>>());

            // Look for any Students.
            // NOTE:  Not robust enough yet.
            if (context.Prospect.Any())
            {
                return;
            }

            // Reference:
            // https://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file
            // 
            //

            var assembly = Assembly.GetExecutingAssembly();

            // NOTE:
            // Use the following to get the exact resource name
            // to be assigned to the resourceName variable below.
            //
            //string[] resourceNames = assembly.GetManifestResourceNames();

            string resourceName = "Team09.Data.SeedData.Prospects.csv";
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
                            first_Name = values[0],
                            last_Name = values[1],
                            email = values[2],
                            gender = values[3],
                            GPA = float.Parse(values[4])
                        }
                    );
                }
            }

            context.SaveChanges();
        }
    }
}
