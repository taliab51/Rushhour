using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA_Rushhour_game
{
    internal class Score
    {
        private int HighScore;
        
        private Timer timer;
        DateTime startTime;
        private DateTime pauseStartTime;
        private bool isPaused;
        public void SetHighScore(int highScore)
        {
            HighScore = highScore;
        }
        public int GetHighScore()
        {
            return HighScore;
        }
        public int CalculateScore(TimeSpan duration, int moves, int level)
        {
            // Calculate duration score inversely proportional to duration
            int durationScore = (int)(1000 * (1 - (duration.TotalSeconds / (level * 20)))); // Adjust as needed

            // Calculate moves score inversely proportional to moves
            int movesScore = (int)(1000 * (1 - (moves / (level * 10.0)))); // Adjust as needed

            // Calculate level score linearly proportional to level with 10% increase per level
            int levelScore = (int)(1000 * level * Math.Pow(1.1, level - 1)); // Adjust as needed

            // Total score calculation
            int totalScore = durationScore + movesScore + levelScore;

            return totalScore;
        }


        

        public void StopTimer()
        {
            DateTime Time;
            DateTime stopTime;
            if (timer != null)
            {
                // Stop the timer
                timer.Dispose();
                timer = null;
                // Record the time when the timer stops
                //stopTime = DateTime.Now;
                // Display the time when the timer stopped
                //Console.WriteLine($"Timer stopped at {stopTime.ToString("HH:mm:ss")}. Press any key to exit.");
                TimeSpan duration = GetCurrentDuration();

                // Display the start and stop times
                //Console.WriteLine($"Timer started at {startTime.ToString("HH:mm:ss")} and stopped at {DateTime.Now.ToString("HH:mm:ss")}. ");
                Console.WriteLine($"Time taken: {duration.Hours} hours, {duration.Minutes} minutes, {duration.Seconds} seconds.");
                // Console.ReadKey();
            }
            
        }
        
        public void StartTimer()
        {

            startTime = DateTime.Now;
            // Create a timer that fires every second
            timer = new Timer(TimerCallback, null, 0, 1000);
        }
        public void PauseTimer()
        {
            if (timer != null && !isPaused)
            {
                isPaused = true;
                timer.Change(Timeout.Infinite, Timeout.Infinite); // Stop the timer temporarily
            }
        }

        public void ResumeTimer()
        {
            if (timer != null && isPaused)
            {
                isPaused = false;
                timer.Change(0, 1000); // Resume the timer
            }
        }

        private void TimerCallback(object state)
        {
            if (!isPaused)
            {
                // Your timer callback logic here
            }
        }

        public TimeSpan GetCurrentDuration()
        {
            TimeSpan runningTime = DateTime.Now - startTime;
            return runningTime;
        }
    }
}
    

