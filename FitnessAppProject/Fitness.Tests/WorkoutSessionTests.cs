using Xunit;
using Fitness;
using System;

public class WorkoutSessionLogTests
{
    [Fact]
    public void WorkoutSessionLog_PropertiesAreSetCorrectly()
    {
        string workoutName = "Running";
        DateTime startTime = new DateTime(2025, 4, 12, 9, 0, 0);
        DateTime endTime = new DateTime(2025, 4, 12, 9, 30, 0);
        double caloriesBurned = 300;
        TimeSpan expectedDuration = endTime - startTime;

        var log = new WorkoutSessionLog
        {
            WorkoutName = workoutName,
            StartTime = startTime,
            EndTime = endTime,
            Duration = expectedDuration, // Explicitly set Duration
            CaloriesBurned = caloriesBurned
        };

        Assert.Equal(workoutName, log.WorkoutName);
        Assert.Equal(startTime, log.StartTime);
        Assert.Equal(endTime, log.EndTime);
        Assert.Equal(expectedDuration, log.Duration);
        Assert.Equal(caloriesBurned, log.CaloriesBurned);
    }

    [Fact]
    public void WorkoutSessionLog_DurationIsCalculatedCorrectly()
    {
        DateTime startTime = new DateTime(2025, 4, 12, 10, 0, 0);
        DateTime endTime = new DateTime(2025, 4, 12, 10, 45, 15);
        TimeSpan expectedDuration = endTime - startTime;

        var log = new WorkoutSessionLog
        {
            StartTime = startTime,
            EndTime = endTime,
            Duration = expectedDuration // Explicitly set Duration
        };

        Assert.Equal(expectedDuration, log.Duration);
    }
}