//class to log a workout session

public class WorkoutSessionLog
{
    public string WorkoutName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }
    public double CaloriesBurned { get; set; } 
}