using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

// Имя неймспейса изменено вручную на то, где лежат сгенерированные EF классы сущностей,
// потому что partial классы должны быть в одном неймспейсе
namespace NorthwindDesktopClientCore.Model.Entities
{
    public partial class Employees : IDataErrorInfo
    {
        string IDataErrorInfo.Error { get { return null; } }

        string IDataErrorInfo.this[string propertyName] {
            get { return this.GetValidationError(propertyName); }
        }

        private static readonly string[] ValidatedProperties =
        {
            "LastName",
            "FirstName"
        };

        public bool IsValid { 
            get {
                foreach (var property in ValidatedProperties)
                    if (GetValidationError(property) != null)
                        return false;

                return true;
            }
        }

        string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                throw new ArgumentException("Property is not in validation list", propertyName);

            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                throw new MissingMemberException("Non exist property in Employees class", propertyName);

            string methodName = $"Validate{propertyName}";
            // Для вызова метода такой второй параметр обязателен, без него не работает
            var method = typeof(Employees).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (string)method.Invoke(this, null);
        }

        private string ValidateLastName()
        {
            string err = null;

            if (this.LastName == null)
                return "Фамилия не заполнена";

            int maxLen = 20;
            if (TooLong(this.LastName, maxLen))
                err += $"Фамилия не может быть длиннее {maxLen} символов\n";

            if (ContainsNonLetters(this.LastName))
                err += "Фамилия может состоять только из букв\n";

            return err?.Trim();
        }

        private string ValidateFirstName()
        {
            string err = null;

            if (this.FirstName == null)
                return "Имя не заполнено";

            int maxLen = 10;
            if (TooLong(this.FirstName, maxLen))
                err += $"Имя не может быть длиннее {maxLen} символов\n";

            if (ContainsNonLetters(this.FirstName))
                err += "Имя может состоять только из букв\n";

            return err?.Trim();
        }

        private bool TooLong(string str, int len)
        {
            return str.Length > len;
        }

        private bool ContainsNonLetters(string str)
        {
            //return Regex.IsMatch(str, @"[а-яА-Яa-zA-Z]+");
            return false;
        }
    }
}
