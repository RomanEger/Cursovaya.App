using CursovayaApp.WPF.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.Models
{
    public static class MyExtensions
    {
        /// <summary>
        /// return 0 if user has been added;
        /// return 1 if user already exists;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public static int AddOrUpdate<T>(this DbSet<T> entities, T newEntity) where T : TableBase
        {
            var q = entities.Any(x => x.Id == newEntity.Id);
            if (!q)
            {
                entities.Add(newEntity);
                return 0;
            }
            else
            {
                newEntity = entities.FirstOrDefault(x => x.Id == newEntity.Id) ?? throw new InvalidOperationException();
                return 1;
            }
        }

        /// <summary>
        /// return 0 if user has been added;
        /// return 1 if user already exists;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public static async Task<int> AddOrUpdateAsync<T>(this DbSet<T> entities, T newEntity) where T : TableBase
        {
            var q = await entities.AnyAsync(x => x.Id == newEntity.Id);
            if (!q)
            {
                await entities.AddAsync(newEntity);
                return 0;
            }
            else
            {
                newEntity = await entities.FirstOrDefaultAsync(x => x.Id == newEntity.Id) ?? throw new InvalidOperationException();
                return 1;
            }
        }
    }
}
