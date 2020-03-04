using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using NorthwindDesktopClientCore.Model.Entities;
using System.Reflection;

namespace NorthwindDesktopClientCore.Helpers.Validation
{
    // Добавил валидацию через partial класс. Этот оставил пока что.
    public static class EmployeeValidator2
    {
        /*
        private static readonly string[] ValidatedProperties =
        {
            "LastName",
            "FirstName",
            "Title",
            "TitleOfCourtesy",
            "BirthDate",
            "HireDate",
            "Address",
            "City",
            "Region",
            "PostalCode",
            "Country",
            "HomePhone",
            "Extension",
            "ReportsTo"
        };

        public static bool IsValid(this Employees emp)
        {
            foreach (var prop in ValidatedProperties)
            {
                ValidateProperty(emp, prop);
            }

            return true;
        }

        private static void ValidateProperty(Employees emp, string prop)
        {
            if (Array.IndexOf(ValidatedProperties, prop) < 0)
                throw new ArgumentException("Property is not in validation list", prop);

            if (TypeDescriptor.GetProperties(emp)[prop] == null)
                throw new MissingMemberException("Non exist property in Employees class", prop);

            string methodName = $"Validate{prop}";
            // Для вызова static метода такой второй параметр обязателен, без него не работает
            var method = typeof(EmployeeValidator).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, new object[] { emp });
        }


        private static void ValidateLastName(Employees emp)
        {
            int maxLen = 20;

            if (TooLong(emp.LastName, maxLen))
                throw new ArgumentException($"LastName must be {maxLen} symbols of less");
        }

        private static void ValidateFirstName(Employees emp)
        {
            int maxLen = 10;

            if (TooLong(emp.FirstName, maxLen))
                throw new ArgumentException($"FirstName must be {maxLen} symbols of less");
        }

        private static void ValidateTitle(Employees emp)
        {

        }

        private static void ValidateTitleOfCourtesy(Employees emp)
        {

        }

        private static void ValidateBirthDate(Employees emp)
        {

        }

        private static void ValidateHireDate(Employees emp)
        {

        }

        private static void ValidateAddress(Employees emp)
        {

        }

        private static void ValidateCity(Employees emp)
        {

        }

        private static void ValidateRegion(Employees emp)
        {

        }

        private static void ValidatePostalCode(Employees emp)
        {

        }

        private static void ValidateCountry(Employees emp)
        {

        }

        private static void ValidateHomePhone(Employees emp)
        {

        }

        private static void ValidateExtension(Employees emp)
        {

        }

        private static void ValidateReportsTo(Employees emp)
        {

        }

        private static bool TooLong(string value, int maxLen)
        {
            return value.Length > maxLen;
        }
        */
    }
}
