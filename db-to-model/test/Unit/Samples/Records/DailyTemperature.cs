using FluentAssertions;
using Xunit;

namespace Unit.Samples.Records;

public readonly record struct DailyTemperature(double HighTemp, double LowTemp)
{
    public double Mean => (HighTemp + LowTemp) / 2.0;
}

public static class DataSet
{
    public static DailyTemperature[] DailyTemperatureData = new DailyTemperature[]
    {
    new DailyTemperature(HighTemp: 57, LowTemp: 30),
    new DailyTemperature(60, 35),
    new DailyTemperature(63, 33),
    new DailyTemperature(68, 29),
    new DailyTemperature(72, 47),
    new DailyTemperature(75, 55),
    new DailyTemperature(77, 55),
    new DailyTemperature(72, 58),
    new DailyTemperature(70, 47),
    new DailyTemperature(77, 59),
    new DailyTemperature(85, 65),
    new DailyTemperature(87, 65),
    new DailyTemperature(85, 72),
    new DailyTemperature(83, 68),
    new DailyTemperature(77, 65),
    new DailyTemperature(72, 58),
    new DailyTemperature(77, 55),
    new DailyTemperature(76, 53),
    new DailyTemperature(80, 60),
    new DailyTemperature(85, 66)
    };
}

public class LearningRecords
{


    [Fact]
    public void CheckValuesArePresent()
    {
        foreach (var item in DataSet.DailyTemperatureData)
        {
            item.Mean.Should().BeGreaterThan(item.LowTemp);
            item.Mean.Should().BeLessThan(item.HighTemp);
        }
    }
}

// Abstract class means that cannot be used directly, but only to extend from it, similar to an interface
// in this example `DegreeDays` is used as the base class for `HeatingDegreeDays` and `CoolingDegreeDays`.
public abstract record DegreeDays(double BaseTemperature, IEnumerable<DailyTemperature> TempRecords);

// sealed class means that cannot be inherited, we do this because we don't plan to use this class
// to be further extended somewhere else.
public sealed record HeatingDegreeDays(double BaseTemperature, IEnumerable<DailyTemperature> TempRecords)
    : DegreeDays(BaseTemperature, TempRecords)
{
    public double DegreeDays => TempRecords.Where(s => s.Mean < BaseTemperature).Sum(s => BaseTemperature - s.Mean);
}

public sealed record CoolingDegreeDays(double BaseTemperature, IEnumerable<DailyTemperature> TempRecords)
    : DegreeDays(BaseTemperature, TempRecords)
{
    public double DegreeDays => TempRecords.Where(s => s.Mean > BaseTemperature).Sum(s => s.Mean - BaseTemperature);
}

public class DegreeDaysTest
{
    [Fact]
    public void HeatingDegreeDaysTest()
    {
        var heatingDegreeDays = new HeatingDegreeDays(65, DataSet.DailyTemperatureData);
        Console.WriteLine(heatingDegreeDays);

        var coolingDegreeDays = new CoolingDegreeDays(65, DataSet.DailyTemperatureData);
        Console.WriteLine(coolingDegreeDays);
    }
}

/*
THE CRUX: record types are meant for storing read-only, init-once data. They serve the purpose of ValueTypes, but in reality they are reference types; they have best of both the worlds. They, being reference types, can be efficiently passed as function arguments(like class objects), and yet they carry data like structs [though read-only] but the data doesn't get copied during function calls - this promotes efficiency. They are 2-in-1: they are RefTypes but they perform the task of ValueTypes (though read-only, init-once). So with C# 9.0 we have a triad - class, struct and record. Two different instances of records are equal if they contain the same data (like structs, and un-like the classes!) The "positional record" syntax makes it easier to create their objects, and the newly introduced "with" expressions make it even easier to create their exact copies, possibly with just-in-time alterations to some of their properties.
(Rev. 13-Apr-2023)
*/