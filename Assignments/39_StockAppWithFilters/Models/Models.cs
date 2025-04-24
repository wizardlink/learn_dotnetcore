using System.ComponentModel.DataAnnotations;

namespace Models;

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
