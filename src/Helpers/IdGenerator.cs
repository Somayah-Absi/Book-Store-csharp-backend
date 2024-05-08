using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Helpers
{
    public class IdGenerator
    {
        public static async Task<int> GenerateIdAsync<T>(EcommerceSdaContext dbContext) where T : class
        {
            try
            {
                // Get the DbSet for the specified entity type
                var dbSet = dbContext.Set<T>();

                // Get the count of existing entities
                var count = await dbSet.CountAsync();

                // Increment the count to get the next ID
                int nextId = count + 1;

                return nextId;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while generating a new ID.", ex);
            }
        }
    }
}
