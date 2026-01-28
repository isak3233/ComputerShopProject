using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;

namespace webbshop.Database
{
    public enum DbAction
    {
        Add,
        Update,
        Remove
    }
    // Används i AdminController
    public class DatabaseActions<T> where T : class
    {
        public async Task Execute(DbAction action, T entity)
        {
            using (var db = new ShopDbContext())
            {
                switch (action)
                {
                    case DbAction.Add:
                        db.Set<T>().Add(entity);
                        break;

                    case DbAction.Update:
                        db.Set<T>().Update(entity);
                        break;

                    case DbAction.Remove:
                        db.Set<T>().Remove(entity);
                        break;
                }

                await db.SaveChangesAsync();
            }

        }

        public async Task<T?> GetById(int id)
        {
            using (var db = new ShopDbContext())
            {
                return await db.Set<T>().FindAsync(id);
            }
                
        }

        public async Task<List<T>> GetAll()
        {
            using (var db = new ShopDbContext())
            {
                return await db.Set<T>().ToListAsync();
            }
        }
    }
}
