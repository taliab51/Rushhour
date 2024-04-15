using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace NEA_Rushhour_game
{
    internal class Login
    {
        private List<string> UserNames = new List<string>();
        private List<string> Passwords = new List<string>();
        private List<string> LevelAt = new List<string>();
        private List<string> Scores = new List<string>(); // Added Scores list
        private string currentLevel;
        private string currentUser;
        private bool IsLoggedIn = false;

        public Login()
        {
            AppendWordsToLists("login.txt", UserNames, Passwords, LevelAt, Scores);
            //gets usernames and pwords from text file and appends to the list
        }
        public int GetCurrentLevel()
        {
            //returns users current level
            return int.Parse(currentLevel);
        }

        public bool GetIsLoggedIn()
        {
            //returns true if user is logged in, otherwise false
            return IsLoggedIn;
        }
        public string GetCurrentUser()
        { return currentUser; }
        public void AppendWordsToLists(string file, List<string> usernames, List<string> passwords, List<string> levels, List<string> scores)
        {
            try
            {
                // Read the first three lines from the file
                string[] lines = File.ReadLines(file).Take(4).ToArray();

                // Iterate through each line and split into words
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] words = lines[i].Split(',');

                    // Append words to the respective list based on the line number
                    switch (i)
                    {
                        case 0:
                            usernames.AddRange(words);
                            break;
                        case 1:
                            passwords.AddRange(words);
                            break;
                        case 2:
                            levels.AddRange(words);
                            break;
                        case 3:
                            scores.AddRange(words);
                            break;
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error reading from the file: " + ex.Message);
            }

        }

        public void GetDetails(string option)
        {
            string usernameInput;
            string PasswordInput;
            string choice;
            if (option == "y")
            {
                Console.WriteLine("Please enter your user name");
                usernameInput = Console.ReadLine();
                Console.WriteLine("Please enter your password");
                PasswordInput = Console.ReadLine();
                //uses username and password to login to account, passes them as parameters to LogIntoAccount
                LogIntoAccount(usernameInput, PasswordInput);


            }
            else if (option == "n")
            {
                Console.WriteLine("Would you like to create an account? y/n");
                choice = Console.ReadLine();
                if (choice == "y")
                {
                    //if user wants to create an account, calls CreateAccount
                    CreateAccount();
                }



            }
        }
        public void LogIntoAccount(string userName, string Password)
        {
            string level;
            string score;
            if (VerifyDetails(userName, Password) == true)
            {
                //checks if details are correct
                IsLoggedIn = true;
                currentUser = userName;

                score = Scores[UserNames.IndexOf(userName)];
                //gets the users current score from the scores list as its at the same index as their username
                Console.WriteLine("You have logged in!");
                level = LevelAt[UserNames.IndexOf(userName)];
                currentLevel = level;
                //gets users current level from the LevelAt list as its at the same index as their username
                Console.WriteLine("Just to let you know! Last time you played, you were on level: " + level);
                Console.WriteLine("Your score is " + score);



            }
            else
            {
                Console.WriteLine("The details you have entered do not exist:");
                //makes them enter their details again
                GetDetails("y");
            }
        }
        public void CreateAccount()
        {
            //add conditions, data validation for pword e.g must contain certain characters, length etc.
            string username;
            string password;
            bool validUsername = false;
            bool validPassword = false;
            do
            {
                Console.WriteLine("Please enter the username you would like:");
                username = Console.ReadLine();

                if (!UserNames.Contains(username))
                {
                    //checks if username doesnt exist already
                    validUsername = true;
                }
                else
                {
                    Console.WriteLine("Username already exists. Please choose a different username.");
                }
            } while (!validUsername);
            //check if the username exists
            do
            {
                Console.WriteLine("Please enter the password you would like (must be at least 4 characters long):");
                password = Console.ReadLine();

                if (password.Length >= 4)
                {
                    //ensures the password is more then 4 characters
                    validPassword = true;
                }
                else
                {
                    Console.WriteLine("Password must be at least 4 characters long. Please enter a valid password.");
                }
            } while (!validPassword);
            //add check if pword exists
            //adds the users details to text file and  the lists
            AddDetailsToTextFile("login.txt", 1, username);
            UserNames.Add(username);
            AddDetailsToTextFile("login.txt", 2, password);
            Passwords.Add(password);
            AddDetailsToTextFile("login.txt", 3, "1");
            LevelAt.Add("1");//intializes their level to 1
            AddDetailsToTextFile("login.txt", 4, "0");
            Scores.Add("0"); // Initialize score as 0



            Console.WriteLine("Your account has been created.");
            LogIntoAccount(username, password);
            //logs the user in



        }

        public void AddDetailsToTextFile(string file, int lineNumber, string word)
        {
            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(file);

                // Check if the specified line number is valid
                if (lineNumber > 0 && lineNumber <= lines.Length)
                {
                    // Insert or modify the content of the specified line
                    lines[lineNumber - 1] += word + ",";

                    // Write the updated content back to the file
                    File.WriteAllLines(file, lines);
                }
                else
                {
                    Console.WriteLine("Invalid line number.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error appending text to the file: " + ex.Message);
            }
        }
        // Method to update a text file with new text at a specified line number
        private void UpdateTextFile(string file, int lineNumber, string newText)
        {
            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(file);

                // Check if the specified line number is valid
                if (lineNumber > 0 && lineNumber <= lines.Length)
                {
                    // Update the line at the specified index with the new text
                    lines[lineNumber - 1] = newText;

                    // Write the updated lines back to the file
                    File.WriteAllLines(file, lines);
                }
                else
                {
                    // Print error message if line number is invalid
                    Console.WriteLine("Invalid line number.");
                }
            }
            catch (Exception ex)
            {
                // Print error message if an exception occurs during file operations
                Console.WriteLine("Error updating text file: " + ex.Message);
            }
        }

        // Method to update the level associated with a user
        public void UpdateLevel(string userName, string newLevel)
        {
            // Find the index of the user in the list of usernames
            int index = UserNames.IndexOf(userName);
            if (index != -1)
            {
                // Update the level of the user at the found index
                LevelAt[index] = newLevel;

                // Update the text file with the updated levels
                UpdateTextFile("login.txt", 3, string.Join(",", LevelAt));
            }
            else
            {
                // Print error message if user is not found
                Console.WriteLine("User not found.");
            }
        }

        // Method to update the score associated with a user
        public void UpdateScore(string userName, int scoreToAdd)
        {
            // Find the index of the user in the list of usernames
            int index = UserNames.IndexOf(userName);
            if (index != -1)
            {
                // Parse the current score of the user
                int currentScore = int.Parse(Scores[index]);

                // Calculate the new score by adding the provided scoreToAdd
                int newScore = currentScore + scoreToAdd;

                // Update the score of the user at the found index
                Scores[index] = newScore.ToString();

                // Update the text file with the updated scores
                UpdateTextFile("login.txt", 4, string.Join(",", Scores));
            }
            else
            {
                // Print error message if user is not found
                Console.WriteLine("User not found.");
            }
        }

        // Method to verify the provided username and password combination
        public bool VerifyDetails(string userName, string Password)
        {
            int pos;
            bool verified = false;

            // Check if the username exists in the list of usernames
            if (UserNames.Contains(userName) == true)
            {
                // Find the index of the username in the list
                pos = UserNames.IndexOf(userName);

                // Check if the provided password matches the password at the found index
                if (Passwords[pos] == Password)
                {
                    // Set verified to true if password matches
                    verified = true;
                }
            }
            // Return whether verification was successful or not
            return verified;
        }
    }
}
