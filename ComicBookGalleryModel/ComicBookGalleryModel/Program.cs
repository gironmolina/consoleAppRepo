using System;
using System.Linq;
using System.Data.Entity;
using System.Diagnostics;

namespace ComicBookGalleryModel
{
    internal class Program
    {
        private static void Main()
        {
            using (var context = new Context())
            {
                // Show Database Logs in the Output Panel
                context.Database.Log = message => Debug.WriteLine(message);
                
                // Query to DbSet context
                var comicBooks = context.ComicBooks
                    .Include(cb => cb.Series)
                    .Include(cb => cb.Artist.Select(a => a.Artist))
                    .Include(cb => cb.Artist.Select(a => a.Role))
                    .ToList();

                foreach (var comicBook in comicBooks)
                {
                    // How to force an entity to be loaded
                    if (comicBook.Series == null)
                    {
                        context.Entry(comicBook)
                            .Reference(cb => cb.Series)
                            .Load();
                    }

                    // Show data
                    var artistRoleNames = comicBook.Artist.Select(a => $"{a.Artist.Name} - {a.Role.Name}").ToList();
                    var artistRolesDisplayTest = string.Join(", ", artistRoleNames);

                    Console.WriteLine(comicBook.DisplayText);
                    Console.WriteLine(artistRolesDisplayTest);
                }

                Console.ReadLine();
            }
        }
    }
}
