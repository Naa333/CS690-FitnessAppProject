using Xunit;
using Fitness;
using System.Collections.Generic;
using System.Linq;

namespace Fitness.Tests
{
    public class WorkoutManagerTests
    {
        private readonly WorkoutManager manager = new WorkoutManager();

        [Fact]
        public void GetAvailableGoals_ShouldReturnExpectedGoals()
        {
            var goals = manager.GetAvailableGoals();
            Assert.Contains("Lose Weight", goals);
            Assert.Contains("Build Muscle", goals);
            Assert.Contains("Improve Endurance", goals);
            Assert.Contains("Maintain Fitness", goals);
            Assert.Equal(4, goals.Count);
        }

        [Fact]
        public void SetGoal_ShouldUpdateUserWorkoutGoalAndWeight()
        {
            var user = new UserInfo();
            manager.SetGoal(user, 75.5, "Lose Weight");

            Assert.Equal("Lose Weight", user.WorkoutGoal);
            Assert.Equal(75.5, user.Weight);
        }

        [Fact]
        public void GetAllWorkouts_ShouldReturnAllWorkouts()
        {
            var workouts = manager.GetAllWorkouts().ToList();
            Assert.True(workouts.Count >= 8); // Ensure there's a solid set
            Assert.Contains(workouts, w => w.Name == "Cardio Blast");
        }

        [Fact]
        public void GetWorkoutsByTools_ShouldReturnCorrectWorkouts()
        {
            var workouts = manager.GetWorkoutsByTools(new List<string> { "Dumbbells", "Yoga Mat", "Resistance Bands" }).ToList();
            Assert.Contains(workouts, w => w.Name == "Full Body Circuit");
            Assert.Contains(workouts, w => w.Name == "Dumbbell Workout");
            Assert.DoesNotContain(workouts, w => w.Name == "Cardio Blast"); // Has no tools
        }

        [Fact]
        public void GetWorkoutsByTools_WithNoTools_ShouldReturnBodyweightOnly()
        {
            var workouts = manager.GetWorkoutsByTools(new List<string> { }).ToList();
            Assert.Contains(workouts, w => w.Name == "Cardio Blast");
            Assert.Contains(workouts, w => w.Name == "Bodyweight Basics");
        }
    }
}
