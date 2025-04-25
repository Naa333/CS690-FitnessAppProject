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
        public void GetAvailableGoals_ShouldContainLoseWeightGoal()
        {
            var goals = manager.GetAvailableGoals();
            Assert.Contains("Lose Weight", goals);
        }

        [Fact]
        public void SetGoal_ShouldUpdateUserWeight()
        {
            var user = new UserInfo();
            manager.SetGoal(user, 90.2, "Maintain Fitness");
            Assert.Equal(90.2, user.Weight);
        }

        [Fact]
        public void GetAllWorkouts_ShouldReturnAtLeastOneWorkout()
        {
            var workouts = manager.GetAllWorkouts().ToList();
            Assert.NotEmpty(workouts);
        }

        [Fact]
        public void GetWorkoutsByTools_EmptyToolsList_ShouldReturnCardioAndBodyweight()
        {
            var workouts = manager.GetWorkoutsByTools(new List<string> { }).ToList();
            Assert.Contains(workouts, w => w.Name == "Cardio Blast");
            Assert.Contains(workouts, w => w.Name == "Bodyweight Basics");
        }

        [Fact]
        public void GetWorkoutsByTools_WithNoneTool_ShouldReturnNoRequiredToolsWorkouts()
        {
            var workouts = manager.GetWorkoutsByTools(new List<string> { "None" }).ToList();
            Assert.Contains(workouts, w => w.Name == "Cardio Blast");
            Assert.Contains(workouts, w => w.Name == "Bodyweight Basics");
        }
    }
}