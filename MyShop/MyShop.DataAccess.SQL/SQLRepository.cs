using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;
using MyShop.Core.models;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext    context;
        internal DbSet<T>       dbSet;

        public SQLRepository(DataContext context)
        {
            this.context    = context;
            dbSet           = context.Set<T>();
        }

        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var t = Find(Id);//use the Find() below to get the correct object

            //see if the found object is in a detached state.. (whatever that is.)
            if (context.Entry(t).State == EntityState.Detached)
            {//if so, attach it.....
                dbSet.Attach(t);
            }

            dbSet.Remove(t);
        }

        public T Find(string Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void Update(T t)
        {
            dbSet.Attach(t);//you have to 'attach' the object... which is not the Entry
            //tell EF the entry is modified and it will automatically update the correct object in the DB
            context.Entry(t).State = EntityState.Modified;

        }
    }
}
