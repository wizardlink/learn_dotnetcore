namespace AssignmentWeather.Models;

public class CityWeather
{
    public DateTime DateAndTime { get; set; }
    public int TemperatureFahrenheit { get; set; }
    public string? CityName { get; set; }
    public string? CityUniqueCode { get; set; }
}
