using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

// Имя неймспейса изменено вручную на то, где лежат сгенерированные EF классы сущностей,
// потому что partial классы должны быть в одном неймспейсе
namespace NorthwindDesktopClientCore.Model.Entities
{
    public partial class Employees
    {
        private  readonly string[] ValidatedProperties =
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

        public bool IsValid()
        {
            foreach (var prop in ValidatedProperties)
            {
                ValidateProperty(prop);
            }

            return true;
        }

        private void ValidateProperty(string prop)
        {
            if (Array.IndexOf(ValidatedProperties, prop) < 0)
                throw new ArgumentException("Property is not in validation list", prop);

            if (TypeDescriptor.GetProperties(this)[prop] == null)
                throw new MissingMemberException("Non exist property in Employees class", prop);

            string methodName = $"Validate{prop}";
            // Для вызова  метода такой второй параметр обязателен, без него не работает
            var method = typeof(Employees).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(this, null);
        }


        private void ValidateLastName()
        {
            int maxLen = 20;

            if (TooLong(this.LastName, maxLen))
                throw new ArgumentException($"LastName must be {maxLen} symbols of less");
        }

        private void ValidateFirstName()
        {
            int maxLen = 10;

            if (TooLong(this.FirstName, maxLen))
                throw new ArgumentException($"FirstName must be {maxLen} symbols of less");
        }

        private void ValidateTitle()
        {

        }

        private void ValidateTitleOfCourtesy()
        {

        }

        private  void ValidateBirthDate()
        {

        }

        private  void ValidateHireDate()
        {

        }

        private  void ValidateAddress()
        {

        }

        private  void ValidateCity()
        {

        }

        private  void ValidateRegion()
        {

        }

        private  void ValidatePostalCode()
        {

        }

        private  void ValidateCountry()
        {

        }

        private  void ValidateHomePhone()
        {

        }

        private  void ValidateExtension()
        {

        }

        private  void ValidateReportsTo()
        {

        }

        private  bool TooLong(string value, int maxLen)
        {
            return value.Length > maxLen;
        }
    }
}
