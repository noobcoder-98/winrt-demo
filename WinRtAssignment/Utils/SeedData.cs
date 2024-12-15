using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRtAssignment.Models;

namespace WinRtAssignment.Utils
{
    public class SeedData
    {
        private static readonly string[] SampleTitles = new[]
    {
        "Meeting Notes", "Shopping List", "Project Ideas", "Daily Journal",
        "Workout Plan", "Recipe Notes", "Travel Itinerary", "Book Summary",
        "Random Thoughts", "Dream Log", "Event Plan", "Task List",
        "Budget Plan", "Movie Reviews", "Study Schedule"
    };

        private static readonly string[] SampleContents = new[]
        {
        "Remember to buy milk, bread, and eggs.",
        "Brainstorm ideas for the upcoming project.",
        "Jogging at 6 AM and strength training at 7 PM.",
        "A quick summary of the book I read today.",
        "Plan the itinerary for the upcoming trip.",
        "Reflections on today's activities.",
        "Prepare a list of tasks for the week.",
        "A dream I had last night was so vivid!",
        "Key points from the meeting with the team.",
        "Write down some random thoughts to reflect on later.",
        "Review the latest movie watched.",
        "Organize my study materials and plan sessions."
    };

        public static List<Note> GenerateRandomNotes(int count)
        {
            var random = new Random();
            var notes = new List<Note>();

            for (int i = 0; i < count; i++)
            {
                var note = new Note
                {
                    Title = SampleTitles[random.Next(SampleTitles.Length)],
                    Content = SampleContents[random.Next(SampleContents.Length)],
                    CreatedAt = DateTime.Now.AddDays(-random.Next(0, 30)) // Ngày tạo trong 30 ngày gần nhất
                };

                notes.Add(note);
            }

            return notes;
        }
    }
}
