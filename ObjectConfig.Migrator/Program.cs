namespace ObjectConfig.Migrator
{
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                new ObjectConfigContextFactory().CreateDbContext(args).Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
