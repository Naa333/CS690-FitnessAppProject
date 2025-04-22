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
        public void GetWorkoutDetails_ShouldReturnCorrectWorkoutIfExists()
        {
            var workout = manager.GetWorkoutDetails("Morning Yoga");
            Assert.NotNull(workout);
            Assert.Equal("Morning Yoga", workout.Name);
            Assert.Contains("Cat-Cow Pose", workout.Instructions.First()); // Check the first instruction
            Assert.Equal("A gentle yoga flow to start your day with flexibility and mindfulness.", workout.Description);
            Assert.Contains("Yoga Mat", workout.RequiredTools);
        }

        [Fact]
        public void GetWorkoutDetails_ShouldReturnNullIfNotExists()
        {
            var workout = manager.GetWorkoutDetails("NonExistentWorkout");
            Assert.Null(workout);
        }

        [Fact]
        public void GetWorkoutsByTools_WithPartialMatch_ShouldNotReturn()
        {
            var tools = new List<string> { "Dumbbells" };
            var workouts = manager.GetWorkoutsByTools(tools).ToList();
            Assert.Contains(workouts, w => w.Name == "Dumbbell Workout");
            Assert.DoesNotContain(workouts, w => w.Name == "Full Body Circuit"); // Requires more than just dumbbells
        }

        [Fact]
        public void GetWorkoutsByTools_CaseInsensitiveMatch()
        {
            var tools = new List<string> { "dumbbells" };
            var workouts = manager.GetWorkoutsByTools(tools).ToList();
            Assert.Contains(workouts, w => w.Name == "Dumbbell Workout");
        }

        [Fact]
        public void GetWorkoutsByTools_WithNoneTool_ShouldReturnBodyweightOnly()
        {
            var workouts = manager.GetWorkoutsByTools(new List<string> { "None" }).ToList();
            Assert.Contains(workouts, w => w.Name == "Cardio Blast");
            Assert.Contains(workouts, w => w.Name == "Bodyweight Basics");
            Assert.DoesNotContain(workouts, w => w.RequiredTools.Any()); // Ensure only no-tool workouts
        }
                
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
