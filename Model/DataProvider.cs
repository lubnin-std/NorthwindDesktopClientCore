using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NorthwindDesktopClientCore.Model.DataContext;
using NorthwindDesktopClientCore.Model.Entities;
using NorthwindDesktopClientCore.Helpers.DataVirtualization;
using System.Linq;
using System.Reflection;

namespace NorthwindDesktopClientCore.Model
{
    public class DataProvider
    {
        public NorthwindDbContext Context { get; }

        public DataProvider(DbContext context)
        {
            if (context is NorthwindDbContext cnt) 
                Context = cnt;
            else 
                WrongContextGiven(context);
        }


        public IItemsProvider<T> GetItemsProvider<T>() where T : class
        {
            return new ItemsProvider<T>(Context);
        }

        private class ItemsProvider<T> : IItemsProvider<T> where T : class
        {
            private NorthwindDbContext _context;

            public ItemsProvider(NorthwindDbContext context)
            {
                _context = context;
            }

            public int FetchCount()
            {
                return _context.Set<T>().Count();
            }

            public IList<T> FetchRange(int startIndex, int count)
            {
                return _context.Set<T>().Skip(startIndex).Take(count).ToList();
            }

            // Выбор количества - интересное решение через рефлексию
            //
            // Метод Count - это статический метод класса Queryable, а не DbSet'а, поэтому искать его
            // нужно именно в Queryable. Там их два, в данном случае нужен тот, что с одним параметром.
            // Это дженерик-метод, поэтому надо сообщить ему тип, с которым он будет работать.
            //
            // При работе с рефлексией, GetType().GetProperty("Foobar") возвращаемт тип и непосредственно
            // само свойство, а не его значение в каком-то конкретном объекте. Поэтому после получения
            // свойства нужно вызвать метод вытаскивания значения из этого свойства и передать этому методу
            // конкретный объект, для которого извлекается значение свойства.
            //
            // В конце вызываем этот метод. Поскольку он статический, первый параметр null, а второй -
            // источник записей, т.е., например, значение свойства Context.Employees, коим является DbSet<Employees>   
            private int FetchCountViaReflection<T>()
            {
                MethodInfo countMethod = typeof(Queryable)
                    .GetMethods()
                    .Where(m => m.Name == nameof(Queryable.Count))
                    .Single(m => m.GetParameters().Length == 1)
                    .MakeGenericMethod(typeof(T));

                var dbSet = _context.GetType().GetProperty(typeof(T).Name).GetValue(_context);
                return (int)countMethod.Invoke(null, new object[] { dbSet });
            }
        }



        

        


        public Employees GetNewEmployee()
        {
            return new Employees();
        }

       
        private void WrongContextGiven(DbContext context)
        {
            var msg = string.Format($"Data context must be of {0} type. {1} given",
                    typeof(NorthwindDbContext).Name,
                    context.GetType().Name);
            throw new ArgumentException(msg);
        }
    }
}
