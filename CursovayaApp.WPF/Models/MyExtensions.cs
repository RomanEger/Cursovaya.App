using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var ent = entities.Where(x => x.Id == newEntity.Id).FirstOrDefault();
                ent = newEntity;
                return 1;
            }
        }
    }
}
