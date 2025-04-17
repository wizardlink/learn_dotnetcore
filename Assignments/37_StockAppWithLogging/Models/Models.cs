using System.ComponentModel.DataAnnotations;

namespace Models;

public class Validations
{
    public static void ComponentModelValidation(object obj)
    {
        ValidationContext validationContext = new(obj);

        List<ValidationResult> validationResults = [];

        bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

        if (!isValid)
            throw new ArgumentException(
                string.Join(", ", validationResults.Select(result => result.ErrorMessage).ToArray())
            );
    }
}

public class AtLeastDateAttribute : ValidationAttribute
{
    private readonly DateTime _dateToCompare;

    public AtLeastDateAttribute(string dateToCompare)
    {
        _dateToCompare = Convert.ToDateTime(dateToCompare);
    }

    public override bool IsValid(object? value)
    {
        var date = Convert.ToDateTime(value);

        if (date == DateTime.MinValue)
            return false;

        if (date.CompareTo(_dateToCompare) < 1)
            return false;

        return true;
    }
}
