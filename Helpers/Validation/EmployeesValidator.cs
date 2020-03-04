using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Model.Entities;
using System.ComponentModel;
using System.Reflection;

namespace NorthwindDesktopClientCore.Helpers.Validation
{
    public class EmployeesValidator : IValidation
    {
        private Employees _emp;
        private readonly string[] ValidatedProperties =
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

        public List<string> LastNameErrors { get; private set; } = new List<string>();

        public EmployeesValidator(Employees emp)
        {
            _emp = emp;
        }

        private bool _isValid;
        public bool IsValid()
        {
            _isValid = true;
            ClearPreviousErrors();

            foreach (var prop in ValidatedProperties)
            {
                ValidateProperty(prop);
            }

            return _isValid;
        }

        private void ValidateProperty(string prop)
        {
            if (Array.IndexOf(ValidatedProperties, prop) < 0)
                throw new ArgumentException("Property is not in validation list", prop);

            if (TypeDescriptor.GetProperties(_emp)[prop] == null)
                throw new MissingMemberException("Non exist property in Employees class", prop);

            string methodName = $"Validate{prop}";
            // Для вызова static метода такой второй параметр обязателен, без него не работает
            var method = typeof(EmployeesValidator).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(this, null);
        }

        private void ValidateLastName()
        {
            int maxLen = 20;

            if (TooLong(_emp.LastName, maxLen))
            {
                _isValid = false;
                LastNameErrors.Add("Значение не может быть больше 20 символов");
            }
                //throw new ArgumentException($"LastName must be {maxLen} symbols of less");
        }

        private void ValidateFirstName()
        {
            int maxLen = 10;

            if (TooLong(_emp.FirstName, maxLen))
            {
                _isValid = false;
                //FirstNameErrors.Add("Значение не может быть больше 10 символов");
            }
            //throw new ArgumentException($"FirstName must be {maxLen} symbols of less");
        }

        private void ValidateTitle()
        {

        }

        private void ValidateTitleOfCourtesy()
        {

        }

        private void ValidateBirthDate()
        {

        }

        private void ValidateHireDate()
        {

        }

        private void ValidateAddress()
        {

        }

        private void ValidateCity()
        {

        }

        private void ValidateRegion()
        {

        }

        private void ValidatePostalCode()
        {

        }

        private void ValidateCountry()
        {

        }

        private void ValidateHomePhone()
        {

        }

        private void ValidateExtension()
        {

        }

        private void ValidateReportsTo()
        {

        }

        private void ClearPreviousErrors()
        {
            LastNameErrors.Clear();
        }

        private bool TooLong(string value, int maxLen)
        {
            return value.Length > maxLen;
        }
    }
}
