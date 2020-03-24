﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
        private NorthwindDbContext Context { get; }

        public DataProvider(DbContext context)
        {
            if (context is NorthwindDbContext cnt) 
                Context = cnt;
            else 
                WrongContextGiven(context);
        }


        public IItemsProvider<T> GetItemsProvider<T>()
        {
            return null;
        }

        public class ItemsProvider<T> : IItemsProvider<T>
        {
            private NorthwindDbContext _context;

            public ItemsProvider(NorthwindDbContext context)
            {
                _context = context;
            }
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
            public int FetchCount<T>()
            {
                MethodInfo countMethod = typeof(Queryable)
                    .GetMethods()
                    .Where(m => m.Name == nameof(Queryable.Count))
                    .Single(m => m.GetParameters().Length == 1)
                    .MakeGenericMethod(typeof(T));

                var dbSet = _context.GetType().GetProperty(typeof(T).Name).GetValue(_context);
                return (int)countMethod.Invoke(null, new object[] { dbSet });
            }

            public ObservableCollection<T> FetchRange<T>(int startIndex, int count)
            {
                return null;
            }
        }

        



        public IEnumerable<Employees> GetEmployees()
        {
            return Context.Employees.OrderBy(e => e.EmployeeId).Take(150).ToList();
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
