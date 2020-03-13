using System;
using System.Collections.Generic;
using System.ComponentModel;  // ICollectionView
using System.Text;
using System.Windows;
using System.Windows.Controls;  // GridView
using System.Windows.Data;  // CollectionViewSource
using System.Reflection;

namespace NorthwindDesktopClientCore.Helpers
{
    public static class GridViewColumns
    {
        // propa - снипет для создания Attached Property, tab-tab чтобы вставить шаблон

        // Список с названиями колонок
        public static object GetColumnsSource(DependencyObject obj)
        {
            return obj.GetValue(ColumnsSourceProperty);
        }

        public static void SetColumnsSource(DependencyObject obj, object value)
        {
            obj.SetValue(ColumnsSourceProperty, value);
        }

        public static readonly DependencyProperty ColumnsSourceProperty =
            DependencyProperty.RegisterAttached("ColumnsSource", typeof(object), typeof(GridViewColumns), new UIPropertyMetadata(default, SourceChanged));  // new UIPropertyMetadata(default) что это?


        // Какой текст отображать в заголовке
        public static string GetHeader(DependencyObject obj)
        {
            return (string)obj.GetValue(HeaderProperty);
        }

        public static void SetHeader(DependencyObject obj, string value)
        {
            obj.SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.RegisterAttached("Header", typeof(string), typeof(GridViewColumns), new UIPropertyMetadata(default));


        // Какое свойство объекта данных отображать в колонке
        public static string GetDisplayMember(DependencyObject obj)
        {
            return (string)obj.GetValue(DisplayMemberProperty);
        }

        public static void SetDisplayMember(DependencyObject obj, string value)
        {
            obj.SetValue(DisplayMemberProperty, value);
        }

        public static readonly DependencyProperty DisplayMemberProperty =
            DependencyProperty.RegisterAttached("DisplayMember", typeof(string), typeof(GridViewColumns), new UIPropertyMetadata(default));


        private static void SourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is GridView gridView)
            {
                if (e.NewValue != null)
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(e.NewValue);
                    if (view != null)
                    {
                        CreateColumns(gridView, view);
                    }
                }
            }
        }

        private static void CreateColumns(GridView gridView, ICollectionView view)
        {
            foreach (var item in view)
            {
                var column = CreateColumn(gridView, item);
                gridView.Columns.Add(column);
            }
        }

        private static T GetPropertyValue<T>(object obj, string propertyName)
        {
            if (obj == null) return default;
            PropertyInfo prop = obj.GetType().GetProperty(propertyName);
            return (T)prop?.GetValue(obj, null);
        }

        private static GridViewColumn CreateColumn(GridView gridView, object columnSource)
        {
            var column = new GridViewColumn();
            string header = GetHeader(gridView);
            string displayMember = GetDisplayMember(gridView);

            if (!string.IsNullOrEmpty(header))
                column.Header = GetPropertyValue<string>(columnSource, header);

            if (!string.IsNullOrEmpty(displayMember))
            {
                string propertyName = GetPropertyValue<string>(columnSource, displayMember);
                column.DisplayMemberBinding = new Binding(propertyName);
            }

            return column;
        }
    }
}
