using System.ComponentModel.DataAnnotations;

namespace BooksManagementSystem.Infra
{
    public class CheckDateRangeAttribute : ValidationAttribute
    {
        public bool CanBeNull { get; set; }
        public string FieldName { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (CanBeNull && value == null) return ValidationResult.Success;

            if(value is null) return new ValidationResult(ErrorMessage ?? $"{FieldName} cannot be empty. Please set a valid date.");

            DateTime dt = (DateTime)value;
            if (dt.ToUniversalTime() <= DateTime.UtcNow)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? $"You cannot return a book in future date. Please enter a valid {FieldName}");
        }
    }
}
