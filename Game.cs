using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace NEA_Rushhour_game
{
    internal class Game
    {
        //private string player;
        private Grid grid;
        private Level level;
        //private Vehicle vehicle;
        private Login login;
        private Score score;
        List<List<Vehicle>> history;
        private int currentlevel;
        int numOfTimes = 0;



        public Game()
        {
            this.history = new List<List<Vehicle>>();
            this.score = new Score();
            this.login = new Login();
            level = new Level(GetLevel().ToString());
            this.grid = new Grid();
            grid.SetGrid(level.LoadGame());
            //Bactracking();


        }
        public Grid GetGrid()
        {
            return grid;
        }

        public bool Solver(List<Vehicle> vehicles)
        {
            if (CheckIfUserHasWon() == true)
            {
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                grid.DisplayGrid();
                Console.WriteLine("\n" + "we have a solution!!!");
                return true;
            }



            List<Vehicle> stamp = new List<Vehicle>();

            stamp = StampGrid(vehicles);

            int countmatching = 0;


            foreach (List<Vehicle> ListOfVehicles in history)
            {
                countmatching = 0;
                foreach (Vehicle vehicle in ListOfVehicles)
                {
                    foreach (Vehicle stampVehicle in stamp)
                    {
                        if (vehicle.GetColour() == stampVehicle.GetColour() && vehicle.GetLocation() == stampVehicle.GetLocation() && vehicle.GetDirection() == stampVehicle.GetDirection() && vehicle.GetSize() == stampVehicle.GetSize())
                        {
                            countmatching++;
                        }
                    }
                }
                if (countmatching == grid.GetVehicles().Count)
                {
                    break;
                }
            }

            if (countmatching < grid.GetVehicles().Count)//double loop
            {
                history.Add(stamp);

            }
            else if (history.Count == 0) { history.Add(stamp); }
            else { return false; }


            ApplyHeuristics(vehicles);

            Console.WriteLine("\n");
            Console.WriteLine("\n");
            grid.DisplayGrid();
            // vehicles = grid.GetVehicles();
            for (int i = 0; i < vehicles.Count; i++)
            {

                if (vehicles[i].GetDirection() == "h")
                {
                    if (vehicles[i].GetColour() == "01")
                    {
                        if (CalcMaxAmountOfMoves(vehicles[i], "R") != 0)
                        {

                            //cancel move
                            MoveVehicleInDirectionChosen1("R", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " right");

                            if (Solver(vehicles) == true)
                            {
                                return true;
                            }
                            //cancel move
                            MoveVehicleInDirectionChosen1("L", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " left");




                        }
                        if (CalcMaxAmountOfMoves(vehicles[i], "L") != 0)
                        {
                            //move left
                            MoveVehicleInDirectionChosen1("L", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " left");

                            if (Solver(vehicles) == true)
                            {
                                return true;
                            }
                            //cancel move
                            MoveVehicleInDirectionChosen1("R", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " right");



                        }

                    }
                    else
                    {
                        //for other horizontal cars try moving left
                        if (CalcMaxAmountOfMoves(vehicles[i], "L") != 0)
                        {

                            //move left
                            MoveVehicleInDirectionChosen1("L", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " left");



                            if (Solver(vehicles) == true)
                            {
                                return true;
                            }
                            //cancel move
                            MoveVehicleInDirectionChosen1("R", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " right");



                        }
                        if (CalcMaxAmountOfMoves(vehicles[i], "R") != 0)
                        {//move right


                            MoveVehicleInDirectionChosen1("R", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " right");

                            if (Solver(vehicles) == true)
                            {
                                return true;
                            }
                            //cancel move
                            MoveVehicleInDirectionChosen1("L", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " left");



                        }
                    }
                }
                else//vertical cars
                {
                    if (vehicles[i].GetSize() == 3)
                    {
                        if (CalcMaxAmountOfMoves(vehicles[i], "D") != 0)
                        {
                            //move down
                            MoveVehicleInDirectionChosen1("D", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " down");


                            if (Solver(vehicles) == true)
                            {
                                return true;
                            }
                            //cancel move
                            MoveVehicleInDirectionChosen1("U", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " up");





                        }
                        if (CalcMaxAmountOfMoves(vehicles[i], "U") != 0)
                        {



                            //move up
                            MoveVehicleInDirectionChosen1("U", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " up");

                            if (Solver(vehicles) == true)
                            {
                                return true;
                            }
                            //cancel move
                            MoveVehicleInDirectionChosen1("D", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " down");



                        }


                    }
                    else
                    {
                        if (CalcMaxAmountOfMoves(vehicles[i], "U") != 0)
                        {
                            //move up                           
                            MoveVehicleInDirectionChosen1("U", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " up");


                            if (Solver(vehicles) == true)
                            {
                                return true;
                            }
                            //cancel move
                            MoveVehicleInDirectionChosen1("D", 1, vehicles[i]);
                            Console.WriteLine("Moved: " + vehicles[i].GetColour() + " down");
                        }



                    }
                    if (CalcMaxAmountOfMoves(vehicles[i], "D") != 0)
                    {
                        //move down
                        MoveVehicleInDirectionChosen1("D", 1, vehicles[i]);
                        Console.WriteLine("Moved: " + vehicles[i].GetColour() + " down");






                        if (Solver(vehicles) == true)
                        {
                            return true;
                        }
                        //cancel move
                        MoveVehicleInDirectionChosen1("U", 1, vehicles[i]);
                        Console.WriteLine("Moved: " + vehicles[i].GetColour() + " up");

                    }

                }
            }

            //grid.DisplayGrid();

            return false;


        }

        public void SaveStampAndRestartRecursion(List<Vehicle> stamp)
        {
            List<Vehicle> stamp1 = stamp;
            
            grid.SetGrid(stamp);
            history = new List<List<Vehicle>>();
            Bactracking();
        }



        public void Bactracking()
        {
            // List<List<Vehicle>> history = new List<List<Vehicle>>();
            //history.Add(grid.GetVehicles());
            List<Vehicle> vehicles = new List<Vehicle>();
            Vehicle v;
            foreach (Vehicle gridv in grid.GetVehicles())
            {
                if (gridv.GetSize() == 3)
                { v = new Truck(gridv.GetColour(), gridv.GetLocation(), gridv.GetDirection()); }
                else { v = new Car(gridv.GetColour(), gridv.GetLocation(), gridv.GetDirection()); }
                vehicles.Add(v);
            }
            if (Solver(vehicles) == true)
            {
                Console.WriteLine("problem is solved");
            }
            else { Console.WriteLine("This problem cannot be solved"); }

            // bool solved = false;
            // (solved, List<Vehicle> stamp) = Solver(vehicles);
            // while (!solved)
            {
             //   (solved, stamp) = Solver(stamp);
              //  if (solved == true)
                {
                //    Console.WriteLine("problem is solved");
                }
            }

            string response = "";
            Console.WriteLine("Would you like to play again? y/n");
            do
            {
                Console.Write("Enter 'y' for yes or 'n' for no: ");
                response = Console.ReadLine().ToLower();
                Console.WriteLine();

                if (response != "y" && response != "n")
                {
                    Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                }

            } while (response != "y" && response != "n");
            if (response == "n")
            {
                Console.WriteLine("Press any key to exit the console...");
                Console.ReadKey();

                // Exit the console application
                Environment.Exit(0);
            }
            else if (response == "y")
            {
                string response1 = "";
                string NextLevel = "";
                if (login.GetIsLoggedIn() == true)
                {
                    Console.WriteLine("You are still logged in as " + login.GetCurrentUser());
                    Console.WriteLine("Would you like to continue on to the next level? y/n");
                    do
                    {
                        Console.Write("Enter 'y' for yes or 'n' for no: ");
                        response1 = Console.ReadLine().ToLower();
                        Console.WriteLine();

                        if (response1 != "y" && response1 != "n")
                        {
                            Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                        }

                    } while (response1 != "y" && response1 != "n");
                    if (response1 == "n")
                    {
                        NextLevel = GetLevel().ToString();
                    }
                    else if (response1 == "y")
                    {
                        NextLevel = (currentlevel + 1).ToString();
                    }



                    this.history = new List<List<Vehicle>>();
                    level.Setlevel(NextLevel);
                    this.grid = new Grid();
                    grid.SetGrid(level.LoadGame());
                    GetGrid().DisplayGrid();
                    Rushhour();
                }
                else
                {
                    Console.WriteLine("Would you like to continue on to the next level? y/n");
                    do
                    {
                        Console.Write("Enter 'y' for yes or 'n' for no: ");
                        response1 = Console.ReadLine().ToLower();
                        Console.WriteLine();

                        if (response1 != "y" && response1 != "n")
                        {
                            Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                        }

                    } while (response1 != "y" && response1 != "n");
                    if (response1 == "n")
                    {
                        NextLevel = GetLevel().ToString();
                    }
                    else if (response1 == "y")
                    {
                        NextLevel = (currentlevel + 1).ToString();
                    }
                    this.score = new Score();
                    this.login = new Login();
                    this.history = new List<List<Vehicle>>();
                    level.Setlevel(NextLevel);
                    this.grid = new Grid();
                    grid.SetGrid(level.LoadGame());
                    GetGrid().DisplayGrid();
                    Rushhour();
                }
            }
        }

        public bool IsInHistory(List<Vehicle> vehicles)
        {
            int countmatching = 0;
            foreach (List<Vehicle> ListOfVehicles in history)
            {
                countmatching = 0;
                foreach (Vehicle vehicle in ListOfVehicles)
                {
                    foreach (Vehicle stampVehicle in vehicles)
                    {
                        if (vehicle.GetColour() == stampVehicle.GetColour() && vehicle.GetLocation() == stampVehicle.GetLocation() && vehicle.GetDirection() == stampVehicle.GetDirection() && vehicle.GetSize() == stampVehicle.GetSize())
                        {
                            countmatching++;
                        }
                    }
                }
                if (countmatching == grid.GetVehicles().Count)
                {
                    break;
                }
            }
            if (countmatching == vehicles.Count)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public List<Vehicle> SendToFront(Vehicle vehicle, List<Vehicle> vehicles)
        {
            // Temporary variable to hold the vehicle being moved
            Vehicle tempVehicle;

            // Loop through the list of vehicles
            for (int i = 0; i < vehicles.Count; i++)
            {
                // Check if the current vehicle matches the one to be moved
                if (vehicle == vehicles[i])
                {
                    // Store the vehicle in a temporary variable
                    tempVehicle = vehicles[i];

                    // Remove the vehicle from its current position
                    vehicles.RemoveAt(i);

                    // Insert the vehicle at the front of the list
                    vehicles.Insert(0, tempVehicle);
                }
            }

            // Return the updated list of vehicles
            return vehicles;
        }

        public List<Vehicle> SendToBack(Vehicle vehicle, List<Vehicle> vehicles)
        {
            // Temporary variable to hold the vehicle being moved
            Vehicle tempVehicle;

            // Loop through the list of vehicles
            for (int i = 0; i < vehicles.Count; i++)
            {
                // Check if the current vehicle matches the one to be moved
                if (vehicle == vehicles[i])
                {
                    // Store the vehicle in a temporary variable
                    tempVehicle = vehicles[i];

                    // Remove the vehicle from its current position
                    vehicles.RemoveAt(i);

                    // Add the vehicle to the end of the list
                    vehicles.Add(tempVehicle);
                }
            }

            // Return the updated list of vehicles
            return vehicles;
        }

        public void DisplayListOfVehicles(List<Vehicle> vehicles)//this subroutine was used to help see whats going on throughout the heuristics
        {
            foreach (Vehicle vehicle in vehicles)
            {
                Console.WriteLine("\n");
                Console.WriteLine("Colour:" + vehicle.GetColour() + " Location:" + vehicle.GetLocation() + " direction:" + vehicle.GetDirection() + " size:" + vehicle.GetSize());
            }
        }
        public void ApplyHeuristics(List<Vehicle> vehicles)
        {


            List<Vehicle> NewVehicles = vehicles;
            List<Vehicle> OGVehicles = vehicles;


            Vehicle tempVehicle;

            for (int i = 0; i < OGVehicles.Count; i++)
            {
                //priority 3 lowest- vertical cars in the way
                if (OGVehicles[i].GetDirection() == "v")
                {


                    if (OGVehicles[i].GetSize() == 3)
                    {



                        if (OGVehicles[i].GetEndOfVehicle() / 10 == 3 || OGVehicles[i].GetRow() == 3 || OGVehicles[i].GetEndOfVehicle() - 10 / 10 == 3)//if any part of a vertical truck is on row 3
                        {
                            tempVehicle = OGVehicles[i];
                            NewVehicles = SendToFront(tempVehicle, NewVehicles);
                            //it must be blocking
                        }

                    }
                    else if (OGVehicles[i].GetSize() == 2)
                    {
                        if (OGVehicles[i].GetRow() == 3 || OGVehicles[i].GetEndOfVehicle() / 10 == 3)
                        {
                            tempVehicle = OGVehicles[i];
                            NewVehicles = SendToFront(tempVehicle, NewVehicles);
                            //it must be blocking
                        }

                    }
                }

            }
            // DisplayListOfVehicles(NewVehicles);
            OGVehicles = NewVehicles;

            for (int j = 0; j < OGVehicles.Count; j++)
            {
                //priority 2 horizontal cars that can move left medium
                if (OGVehicles[j].GetDirection() == "h" && OGVehicles[j].GetColour() != "01")
                {
                    if (CalcMaxAmountOfMoves(OGVehicles[j], "L") != 0)
                    {
                        tempVehicle = OGVehicles[j];
                        NewVehicles = SendToFront(tempVehicle, NewVehicles);
                        //it can move left
                    }
                    else //if (CalcMaxAmountOfMoves(OGVehicles[j], "L") == 0)
                    {
                        tempVehicle = OGVehicles[j];
                        NewVehicles = SendToBack(tempVehicle, NewVehicles);
                        //it cant move left
                    }


                }
            }
            // DisplayListOfVehicles(NewVehicles);
            OGVehicles = NewVehicles;

            for (int k = 0; k < OGVehicles.Count; k++)
            {
                //priority 1- red car can move right highest
                if (OGVehicles[k].GetDirection() == "h")
                {
                    if (OGVehicles[k].GetColour() == "01")
                    {
                        if (CalcMaxAmountOfMoves(OGVehicles[k], "R") != 0)
                        {
                            tempVehicle = OGVehicles[k];
                            NewVehicles = SendToFront(tempVehicle, NewVehicles);
                            //put it at front of queue
                            //red car can move right
                        }
                        else //if (CalcMaxAmountOfMoves(OGVehicles[k], "R") == 0)
                        {
                            tempVehicle = OGVehicles[k];
                            NewVehicles = SendToBack(tempVehicle, NewVehicles);
                            //put it at the back
                        }
                    }
                }
            }
            //DisplayListOfVehicles(NewVehicles);
            grid.SetGrid(NewVehicles);



        }
        public void ApplyHeuristics1(List<Vehicle> vehicles)//IGNORE THIS SUBROUTINE
        {
            //DisplayListOfVehicles (vehicles);
            // Create new lists to hold manipulated vehicles
            // NewVehicles will be modified based on the heuristics
            List<Vehicle> NewVehicles = new List<Vehicle>();

            List<Vehicle> OGVehicles = new List<Vehicle>(); // OGVehicles will be used for iteration
            Vehicle v;
            foreach (Vehicle gridv in vehicles)
            {
                if (gridv.GetSize() == 3)
                { v = new Truck(gridv.GetColour(), gridv.GetLocation(), gridv.GetDirection()); }
                else { v = new Car(gridv.GetColour(), gridv.GetLocation(), gridv.GetDirection()); }
                NewVehicles.Add(v);
                OGVehicles.Add(v);
            }
          //  DisplayListOfVehicles(OGVehicles);
          //  DisplayListOfVehicles(NewVehicles);

            Vehicle tempVehicle;// Temporary variable to hold a vehicle during manipulation


            // Apply heuristic for priority 3: Move vertical cars blocking the way to the front
            for (int i = 0; i < OGVehicles.Count; i++)
            {
                //priority 3 lowest- vertical cars in the way
                if (OGVehicles[i].GetDirection() == "v")
                {


                    if (OGVehicles[i].GetSize() == 3)
                    {



                        if (OGVehicles[i].GetEndOfVehicle() / 10 == 3 || OGVehicles[i].GetRow() == 3 || OGVehicles[i].GetEndOfVehicle() - 10 / 10 == 3)//if any part of a vertical truck is on row 3
                        {
                            tempVehicle = OGVehicles[i];
                            NewVehicles = SendToFront(tempVehicle, NewVehicles);
                            //it must be blocking
                            // Move the blocking vertical car to the front
                        }

                    }
                    else if (OGVehicles[i].GetSize() == 2)
                    {
                        if (OGVehicles[i].GetRow() == 3 || OGVehicles[i].GetEndOfVehicle() / 10 == 3)
                        {
                            tempVehicle = OGVehicles[i];
                            NewVehicles = SendToFront(tempVehicle, NewVehicles);
                            //it must be blocking
                            // Move the blocking vertical car to the front
                        }

                    }
                }

            }
            // DisplayListOfVehicles(NewVehicles);
            // Update OGVehicles after applying the first heuristic
          //  Console.WriteLine("after first heuristic");
          //  DisplayListOfVehicles(NewVehicles);
            OGVehicles.Clear();
            Vehicle v1;
            foreach (Vehicle gridv in NewVehicles)
            {
                if (gridv.GetSize() == 3)
                { v1 = new Truck(gridv.GetColour(), gridv.GetLocation(), gridv.GetDirection()); }
                else { v1 = new Car(gridv.GetColour(), gridv.GetLocation(), gridv.GetDirection()); }
                
                OGVehicles.Add(v1);
            }

            // Apply heuristic for priority 2: Move horizontal cars that can move left to the front

            for (int j = 0; j < OGVehicles.Count; j++)
            {
                //priority 2 horizontal cars that can move left - medium
                if (OGVehicles[j].GetDirection() == "h" && OGVehicles[j].GetColour() != "01")
                {
                    if (CalcMaxAmountOfMoves(OGVehicles[j], "L") != 0)
                    {
                        tempVehicle = OGVehicles[j];
                        NewVehicles = SendToFront(tempVehicle, NewVehicles);
                            //it can move left
                            // Move the horizontal car that can move left to the front
                        }
                        else //if (CalcMaxAmountOfMoves(OGVehicles[j], "L") == 0)
                    {
                        tempVehicle = OGVehicles[j];
                        NewVehicles = SendToBack(tempVehicle, NewVehicles);
                            //it cant move left
                            // Move the horizontal car that cannot move left to the back
                        }


                    }
            }
            // DisplayListOfVehicles(NewVehicles);
            // Update OGVehicles after applying the second heuristic
          //  Console.WriteLine("after second heuristic");
            //DisplayListOfVehicles(NewVehicles);

            OGVehicles.Clear();
            Vehicle v2;
            foreach (Vehicle gridv in NewVehicles)
            {
                if (gridv.GetSize() == 3)
                { v2 = new Truck(gridv.GetColour(), gridv.GetLocation(), gridv.GetDirection()); }
                else { v2 = new Car(gridv.GetColour(), gridv.GetLocation(), gridv.GetDirection()); }

                OGVehicles.Add(v2);
            }
            // Apply heuristic for priority 1: Move the red car that can move right to the front
            for (int k = 0; k < OGVehicles.Count; k++)
            {
                //priority 1- red car can move right highest
                if (OGVehicles[k].GetDirection() == "h")
                {
                    if (OGVehicles[k].GetColour() == "01")
                    {
                        if (CalcMaxAmountOfMoves(OGVehicles[k], "R") != 0)
                        {
                            tempVehicle = OGVehicles[k];
                            NewVehicles = SendToFront(tempVehicle, NewVehicles);
                            //put it at front of queue
                            //red car can move right
                        }
                        else //if (CalcMaxAmountOfMoves(OGVehicles[k], "R") == 0)
                        {
                            tempVehicle = OGVehicles[k];
                            NewVehicles = SendToBack(tempVehicle, NewVehicles);
                            //put it at the back
                        }
                    }
                }
            }
          //  Console.WriteLine("after last heuristic");
           // DisplayListOfVehicles(NewVehicles);
            //DisplayListOfVehicles(NewVehicles);
            // Set the grid with the updated list of vehicles
            grid.SetGrid(NewVehicles);
            



        }
        public List<Vehicle> StampGrid(List<Vehicle> CurrentVehicle)
        {
            // Create a new list to hold stamped vehicles
            List<Vehicle> stamp = new List<Vehicle>();

            // Variable to hold a new vehicle instance
            Vehicle newVehicle = null;

            // Iterate through each vehicle in the current list of vehicles
            foreach (Vehicle vehicle in CurrentVehicle)
            {
                // Retrieve attributes of the current vehicle
                string c = vehicle.GetColour(); // Color of the vehicle
                int l = vehicle.GetLocation(); // Location of the vehicle
                int s = vehicle.GetSize(); // Size of the vehicle
                string d = vehicle.GetDirection(); // Direction of the vehicle

                // Check the size of the vehicle and create a new instance accordingly
                if (s == 3)
                {
                    // Create a new Truck instance with the attributes
                    newVehicle = new Truck(c, l, d);
                }
                else
                {
                    // Create a new Car instance with the attributes
                    newVehicle = new Car(c, l, d);
                }

                // Add the new vehicle instance to the stamped list
                stamp.Add(newVehicle);
            }

            // Return the stamped list of vehicles
            return stamp;
        }

        public void DisplayMenu()
        {
            string option = "";
            Console.WriteLine("Welcome to Rushhour game:");
            Console.WriteLine("To play, shift the cars and trucks up and down, left and right, until the path is cleared to slide the red car out the exit.");
            while (true)
            {
                try
                {
                    Console.Write("Please enter 1 to continue or 0 to exit: ");
                    string input = Console.ReadLine();

                    if (input == "1")
                    {
                        Console.WriteLine("Continuing...");
                        break; // Exit the loop
                    }
                    else if (input == "0")
                    {
                        Console.WriteLine("Exiting the program...");
                        Environment.Exit(0);

                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid input. Please enter either 1 to continue or 0 to exit.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        
        Console.WriteLine("Would you like to log in? y/n");

            do
            {
                Console.Write("Enter 'y' for yes or 'n' for no: ");
                option = Console.ReadLine().ToLower();
                Console.WriteLine();

                if (option != "y" && option != "n")
                {
                    Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                }

            } while (option != "y" && option != "n");
            login.GetDetails(option);





            //give user instructions and display options of logginig in



        }
        public int GetLevel()
        {
            int level1 = 0;
            DisplayMenu();
            if(login.GetIsLoggedIn()==true)
            {
                Console.WriteLine("Would you like to carry on that level? y/n");
                string input;
                // Loop until valid input is received
                do
                {
                    
                    input = Console.ReadLine();

                    if (input != "y" && input != "n")
                    {
                        Console.WriteLine("Error: Invalid input. Please enter 'y' or 'n'.");
                    }
                } while (input != "y" && input != "n");
                // If user wants to continue the current level
                if (input == "y")
                {
                    level1 = login.GetCurrentLevel();
                    Console.WriteLine("Okay! Here is level "+level1);
                    return level1;
                   
;                }
                // If user wants to choose a new level
                else if (input == "n")
                {
                    Console.WriteLine("Okay! Please choose your level");
                }
            }
            
            string userinput;
            bool valid = false;
            Console.WriteLine("Would you like beginner, intermediate,advanced or expert? B/I/A/E");
            // Loop until valid input is received for the level difficulty
            do
            {

                userinput = Console.ReadLine().ToUpper();
                if (userinput == "B" || userinput == "I" || userinput == "A" || userinput == "E")
                {
                    valid = true;
                }

                else
                {

                    Console.WriteLine("you must enter either B,I,A or E");
                }


            }
            while (valid == false);
            bool valid1 = false;
            // Based on user's choice, prompt for a level within the specified range
            if (userinput == "B")
            {
                Console.WriteLine("Please choose a level between 1 and 10");
                do
                {
                    try
                    {
                        level1 = Convert.ToInt32(Console.ReadLine());
                        if (level1 <= 10 && level1 >= 1)
                        {
                            valid1 = true;
                        }
                        else
                        {
                            Console.WriteLine("number must be between 1 and 10");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("you must enter an integer");
                        valid1 = false;
                    }
                }
                while (valid1 == false);
            }
            else if (userinput == "I")
            {
                Console.WriteLine("Please choose a level between 11 and 20");
                do
                {
                    try
                    {
                        level1 = Convert.ToInt32(Console.ReadLine());
                        if (level1 <= 20 && level1 >= 11)
                        {
                            valid1 = true;
                        }
                        else
                        {
                            Console.WriteLine("number must be between 11 and 20");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("you must enter an integer");
                        valid1 = false;
                    }
                }
                while (valid1 == false);
            }
            else if (userinput == "A")
            {
                Console.WriteLine("Please choose a level between 21 and 30");
                do
                {
                    try
                    {
                        level1 = Convert.ToInt32(Console.ReadLine());
                        if (level1 <= 30 && level1 >= 21)
                        {
                            valid1 = true;
                        }
                        else
                        {
                            Console.WriteLine("number must be between 21 and 30");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("you must enter an integer");
                        valid1 = false;
                    }
                }
                while (valid1 == false);
            }
            else if (userinput == "E")
            {
                Console.WriteLine("Please choose a level between 31 and 40");
                do
                {
                    try
                    {
                        level1 = Convert.ToInt32(Console.ReadLine());
                        if (level1 <= 40 && level1 >= 31)
                        {
                            valid1 = true;
                        }
                        else
                        {
                            Console.WriteLine("number must be between 31 and 40");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("you must enter an integer");
                        valid1 = false;
                    }
                }
                while (valid1 == false);
            }
            // Store the chosen level
            StoreLevel(level1);


            return level1;
        }
        public void StoreLevel(int level)
        {
            currentlevel = level;
        }
        public void Rushhour()
        {
            int userInput = 0;
            bool isValidInput = false;

            Console.WriteLine("\n" + "Would you like to play game or use the solver? press 1 for solver and 2 to play");

            // Loop until valid input is received
            do
            {
                Console.Write("Please enter either 1 or 2: ");
                string input = Console.ReadLine();

                try
                {
                    userInput = int.Parse(input);

                    // Check if the input is either 1 or 2
                    if (userInput == 1 || userInput == 2)
                    {
                        isValidInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter either 1 or 2.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }

            } while (!isValidInput);

            // Determine whether to use the solver or play the game based on user input
            if (userInput == 1)
            {
                Bactracking();
            }
            else
            {
                PlayGame();
            }
        }
        public void PlayGame()
        {


            int numOfMoves = 0;
            score.StartTimer(); // Start the timer to track game duration

            // Loop until the user wins the game
            do
            {
                MoveVehicle(); // Move vehicles in the game
                numOfMoves++; // Increment the number of moves made by the user
            } while (CheckIfUserHasWon() == false);

            Console.WriteLine("WELL DONE! YOU HAVE WON!");

            // Display the number of moves made by the user
            if (numOfMoves == 1)
            {
                Console.WriteLine("It took you " + numOfMoves + " move");
            }
            else
            {
                Console.WriteLine("It took you " + numOfMoves + " moves");
            }

            score.StopTimer(); // Stop the timer
            TimeSpan duration = score.GetCurrentDuration(); // Get the duration of the game
            int score1 = score.CalculateScore(duration, numOfMoves, currentlevel); // Calculate the score based on game duration, number of moves, and level
            Console.WriteLine("Your score is " + score1);

            // Update user's level and score if logged in
            if (login.GetIsLoggedIn() == true)
            {
                login.UpdateLevel(login.GetCurrentUser(), currentlevel.ToString());
                login.UpdateScore(login.GetCurrentUser(), score1);
            }

            string response = "";

            // Ask the user if they want to play again
            Console.WriteLine("Would you like to play again? y/n");
            do
            {
                Console.Write("Enter 'y' for yes or 'n' for no: ");
                response = Console.ReadLine().ToLower();
                Console.WriteLine();

                if (response != "y" && response != "n")
                {
                    Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                }
            } while (response != "y" && response != "n");

            // If the user chooses not to play again, exit the application
            if (response == "n")
            {
                Console.WriteLine("Press any key to exit the console...");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else if (response == "y")
            {
                string response1 = "";
                string NextLevel = "";

                // If the user is logged in, prompt to continue to the next level
                if (login.GetIsLoggedIn() == true)
                {
                    Console.WriteLine("You are still logged in as " + login.GetCurrentUser());
                    Console.WriteLine("Would you like to continue on to the next level? y/n");
                    do
                    {
                        Console.Write("Enter 'y' for yes or 'n' for no: ");
                        response1 = Console.ReadLine().ToLower();
                        Console.WriteLine();

                        if (response1 != "y" && response1 != "n")
                        {
                            Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                        }
                    } while (response1 != "y" && response1 != "n");

                    // Determine the next level based on user response
                    if (response1 == "n")
                    {
                        NextLevel = GetLevel().ToString(); // Get the level from user input
                    }
                    else if (response1 == "y")
                    {
                        NextLevel = (currentlevel + 1).ToString(); // Proceed to the next level
                    }
                
            




            this.history = new List<List<Vehicle>>();
                    level.Setlevel(NextLevel);
                    this.grid = new Grid();
                    grid.SetGrid(level.LoadGame());
                    GetGrid().DisplayGrid();
                    Rushhour();
                }
                else
                {
                    Console.WriteLine("Would you like to continue on to the next level? y/n");
                    do
                    {
                        Console.Write("Enter 'y' for yes or 'n' for no: ");
                        response1 = Console.ReadLine().ToLower();
                        Console.WriteLine();

                        if (response1 != "y" && response1 != "n")
                        {
                            Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                        }

                    } while (response1 != "y" && response1 != "n");
                    if (response1 == "n")
                    {
                        NextLevel = GetLevel().ToString();
                    }
                    else if (response1 == "y")
                    {
                        NextLevel = (currentlevel + 1).ToString();
                    }
                    this.score = new Score();
                    this.login = new Login();
                    this.history = new List<List<Vehicle>>();
                    level.Setlevel(NextLevel);
                    this.grid = new Grid();
                    grid.SetGrid(level.LoadGame());
                    GetGrid().DisplayGrid();
                    Rushhour();
                }
            }








            //display the grid
            //ask user which move
            //check its valid
            //movevhicle
        }
    
        public bool CheckMoveIsValid()//is this necassary?
        {
            return false;
        }
        public int CalcMaxAmountOfMoves(Vehicle SelectedVehicle, string way)
        {



            // Initialize variables
            int MaxAmountOfMoves = 0; // Maximum amount of moves
            int place; // Placeholder variable
            List<Vehicle> vehiclesInTheWay = new List<Vehicle>(); // List to store vehicles in the way
            int value = 0; // Value variable

            // Check the direction of movement
            if (way == "U") // If moving up
            {
                // Loop through all vehicles on the grid
                foreach (Vehicle v in grid.GetVehicles())
                {
                    // Determine value based on vehicle direction
                    if (v.GetDirection() == "v") // If the vehicle is vertical
                    {
                        value = 10; // Set value to 10 (representing rows)
                    }
                    else if (v.GetDirection() == "h") // If the vehicle is horizontal
                    {
                        value = 1; // Set value to 1 (representing columns)
                    }

                    // Check if the vehicle size is 2 or 3
                    if (v.GetSize() == 2) // If the vehicle size is 2
                    {
                        // Check if SelectedVehicle is in the same column as v
                        if (SelectedVehicle.GetColumn() == v.GetColumn() || SelectedVehicle.GetColumn() == v.GetEndOfVehicle() % 10)
                        {
                            // Check if SelectedVehicle is above v
                            if (SelectedVehicle.GetRow() > v.GetRow() || SelectedVehicle.GetRow() > v.GetEndOfVehicle() / 10)
                            {
                                vehiclesInTheWay.Add(v); // Add v to vehiclesInTheWay
                            }
                        }
                    }
                    else if (v.GetSize() == 3) // If the vehicle size is 3
                    {
                        // Check if SelectedVehicle is in the same column as v or its adjacent columns
                        if (SelectedVehicle.GetColumn() == v.GetColumn() || SelectedVehicle.GetColumn() == v.GetEndOfVehicle() % 10 || SelectedVehicle.GetColumn() == (v.GetEndOfVehicle() - value) % 10)
                        {
                            // Check if SelectedVehicle is above v or its adjacent rows
                            if (SelectedVehicle.GetRow() > v.GetRow() || SelectedVehicle.GetRow() > v.GetEndOfVehicle() / 10 || SelectedVehicle.GetRow() > (v.GetEndOfVehicle() - value) / 10)
                            {
                                vehiclesInTheWay.Add(v); // Add v to vehiclesInTheWay
                            }
                        }
                    }

                    // Calculate MaxAmountOfMoves based on vehiclesInTheWay
                    if (vehiclesInTheWay.Count == 0)
                    {
                        MaxAmountOfMoves = SelectedVehicle.GetRow() - 1; // Set MaxAmountOfMoves to the difference between SelectedVehicle's row and the top of the grid
                    }
                    else
                    {
                        // Initialize MaxAmountOfMoves to a default value
                        MaxAmountOfMoves = 4;
                        // Loop through vehiclesInTheWay to find the nearest vehicle
                        foreach (Vehicle vehicle in vehiclesInTheWay)
                        {
                            // Calculate the distance between SelectedVehicle and the vehicle in the way
                            int distance = SelectedVehicle.GetRow() - vehicle.GetEndOfVehicle() / 10 - 1;
                            // Update MaxAmountOfMoves if distance is smaller
                            if (distance < MaxAmountOfMoves)
                            {
                                MaxAmountOfMoves = distance;
                            }
                        }
                    }
                    
                
            

        }
        //up works

        //go through all vehicles chack if its on that column and the one thats nearest 
    }

            // Check if the movement direction is down (D)
            if (way == "D")
            {
                // Loop through all vehicles on the grid
                foreach (Vehicle v in grid.GetVehicles())
                {
                    // Determine the value based on the direction of the vehicle
                    if (v.GetDirection() == "v") // If the vehicle is vertical
                    {
                        value = 10; // Set value to 10 (representing rows)
                    }
                    else if (v.GetDirection() == "h") // If the vehicle is horizontal
                    {
                        value = 1; // Set value to 1 (representing columns)
                    }

                    // Check if the vehicle size is 2 or 3
                    if (v.GetSize() == 2) // If the vehicle size is 2
                    {
                        // Check if SelectedVehicle is in the same column as v
                        if (SelectedVehicle.GetColumn() == v.GetColumn() || SelectedVehicle.GetColumn() == v.GetEndOfVehicle() % 10)
                        {
                            // Check if SelectedVehicle's end row is above v or its end row
                            if (SelectedVehicle.GetEndOfVehicle() / 10 < v.GetRow() || SelectedVehicle.GetEndOfVehicle() / 10 < v.GetEndOfVehicle() / 10)
                            {
                                vehiclesInTheWay.Add(v); // Add v to vehiclesInTheWay list
                            }
                        }
                    }
                    else if (v.GetSize() == 3) // If the vehicle size is 3
                    {
                        // Check if SelectedVehicle is in the same column as v or its adjacent columns
                        if (SelectedVehicle.GetColumn() == v.GetColumn() || SelectedVehicle.GetColumn() == v.GetEndOfVehicle() % 10 || SelectedVehicle.GetColumn() == (v.GetEndOfVehicle() - value) % 10)
                        {
                            // Check if SelectedVehicle's end row is above v or its adjacent rows
                            if (SelectedVehicle.GetEndOfVehicle() / 10 < v.GetRow() || SelectedVehicle.GetEndOfVehicle() / 10 < v.GetEndOfVehicle() / 10 || SelectedVehicle.GetEndOfVehicle() / 10 < (v.GetEndOfVehicle() - value) / 10)
                            {
                                vehiclesInTheWay.Add(v); // Add v to vehiclesInTheWay list
                            }
                        }
                    }

                    // Calculate MaxAmountOfMoves based on vehiclesInTheWay
                    if (vehiclesInTheWay.Count == 0)
                    {
                        MaxAmountOfMoves = 6 - SelectedVehicle.GetEndOfVehicle() / 10; // Set MaxAmountOfMoves to the difference between the bottom of the grid and SelectedVehicle's end row
                    }
                    else
                    {
                        // Initialize MaxAmountOfMoves to a default value
                        MaxAmountOfMoves = 4;
                        // Loop through vehiclesInTheWay to find the nearest vehicle
                        foreach (Vehicle vehicle in vehiclesInTheWay)
                        {
                            // Calculate the distance between SelectedVehicle's end row and the vehicle in the way
                            int distance = (vehicle.GetRow() - SelectedVehicle.GetEndOfVehicle() / 10) - 1;
                            // Update MaxAmountOfMoves if distance is smaller
                            if (distance < MaxAmountOfMoves)
                            {
                                MaxAmountOfMoves = distance;
                            }
                        }
                    }
                
            


        }

        //go through all vehicles chack if its on that column and the one thats nearest 
        //down works
    }
            // Check if the movement direction is left (L)
            if (way == "L")
            {
                // Loop through all vehicles on the grid
                foreach (Vehicle v in grid.GetVehicles())
                {
                    // Determine the value based on the direction of the vehicle
                    if (v.GetDirection() == "v") // If the vehicle is vertical
                    {
                        value = 10; // Set value to 10 (representing rows)
                    }
                    else if (v.GetDirection() == "h") // If the vehicle is horizontal
                    {
                        value = 1; // Set value to 1 (representing columns)
                    }

                    // Check if the vehicle size is 2 or 3
                    if (v.GetSize() == 2) // If the vehicle size is 2
                    {
                        // Check if SelectedVehicle is in the same row as v
                        if (SelectedVehicle.GetRow() == v.GetRow() || SelectedVehicle.GetRow() == v.GetEndOfVehicle() / 10)
                        {
                            // Check if SelectedVehicle's column is greater than v's or its end column
                            if (SelectedVehicle.GetColumn() > v.GetColumn() || SelectedVehicle.GetColumn() > v.GetEndOfVehicle() % 10)
                            {
                                vehiclesInTheWay.Add(v); // Add v to vehiclesInTheWay list
                            }
                        }
                    }
                    else if (v.GetSize() == 3) // If the vehicle size is 3
                    {
                        // Check if SelectedVehicle is in the same row as v or its adjacent rows
                        if (SelectedVehicle.GetRow() == v.GetRow() || SelectedVehicle.GetRow() == v.GetEndOfVehicle() / 10 || SelectedVehicle.GetRow() == (v.GetEndOfVehicle() - value) / 10)
                        {
                            // Check if SelectedVehicle's column is greater than v's or its adjacent columns
                            if (SelectedVehicle.GetColumn() > v.GetColumn() || SelectedVehicle.GetColumn() > v.GetEndOfVehicle() % 10 || SelectedVehicle.GetColumn() > (v.GetEndOfVehicle() - value) % 10)
                            {
                                vehiclesInTheWay.Add(v); // Add v to vehiclesInTheWay list
                            }
                        }
                    }

                    // Calculate MaxAmountOfMoves based on vehiclesInTheWay
                    if (vehiclesInTheWay.Count == 0)
                    {
                        MaxAmountOfMoves = SelectedVehicle.GetColumn() - 1; // Set MaxAmountOfMoves to the difference between SelectedVehicle's column and the left boundary
                    }
                    else
                    {
                        // Initialize MaxAmountOfMoves to a default value
                        MaxAmountOfMoves = 4;
                        // Loop through vehiclesInTheWay to find the nearest vehicle
                        foreach (Vehicle vehicle in vehiclesInTheWay)
                        {
                            // Calculate the distance between SelectedVehicle's end column and the vehicle in the way
                            int distance = SelectedVehicle.GetColumn() - (vehicle.GetEndOfVehicle() % 10 + 1);
                            // Update MaxAmountOfMoves if distance is smaller
                            if (distance < MaxAmountOfMoves)
                            {
                                MaxAmountOfMoves = distance;
                            }
                        }
                    }
                }
            }

            // Check if the movement direction is right (R)
            if (way == "R")
            {
                // Loop through all vehicles on the grid
                foreach (Vehicle v in grid.GetVehicles())
                {
                    // Determine the value based on the direction of the vehicle
                    if (v.GetDirection() == "v") // If the vehicle is vertical
                    {
                        value = 10; // Set value to 10 (representing rows)
                    }
                    else if (v.GetDirection() == "h") // If the vehicle is horizontal
                    {
                        value = 1; // Set value to 1 (representing columns)
                    }

                    // Check if the vehicle size is 2 or 3
                    if (v.GetSize() == 2) // If the vehicle size is 2
                    {
                        // Check if SelectedVehicle is in the same row as v
                        if (SelectedVehicle.GetRow() == v.GetRow() || SelectedVehicle.GetRow() == v.GetEndOfVehicle() / 10)
                        {
                            // Check if SelectedVehicle's end column is less than v's or its end column
                            if (SelectedVehicle.GetEndOfVehicle() % 10 < v.GetColumn() || SelectedVehicle.GetEndOfVehicle() % 10 < v.GetEndOfVehicle() % 10)
                            {
                                vehiclesInTheWay.Add(v); // Add v to vehiclesInTheWay list
                            }
                        }
                    }
                    else if (v.GetSize() == 3) // If the vehicle size is 3
                    {
                        // Check if SelectedVehicle is in the same row as v or its adjacent rows
                        if (SelectedVehicle.GetRow() == v.GetRow() || SelectedVehicle.GetRow() == v.GetEndOfVehicle() / 10 || SelectedVehicle.GetRow() == (v.GetEndOfVehicle() - value) / 10)
                        {
                            // Check if SelectedVehicle's end column is less than v's or its adjacent columns
                            if (SelectedVehicle.GetEndOfVehicle() % 10 < v.GetColumn() || SelectedVehicle.GetEndOfVehicle() % 10 < v.GetEndOfVehicle() % 10 || SelectedVehicle.GetEndOfVehicle() % 10 < (v.GetEndOfVehicle() - value) % 10)
                            {
                                vehiclesInTheWay.Add(v); // Add v to vehiclesInTheWay list
                            }
                        }
                    }

                    // Calculate MaxAmountOfMoves based on vehiclesInTheWay
                    if (vehiclesInTheWay.Count == 0)
                    {
                        MaxAmountOfMoves = 6 - SelectedVehicle.GetEndOfVehicle() % 10; // Set MaxAmountOfMoves to the difference between the right boundary and SelectedVehicle's end column
                    }
                    else
                    {
                        // Initialize MaxAmountOfMoves to a default value
                        MaxAmountOfMoves = 4;
                        // Loop through vehiclesInTheWay to find the nearest vehicle
                        foreach (Vehicle vehicle in vehiclesInTheWay)
                        {
                            // Calculate the distance between SelectedVehicle's end column and the vehicle in the way
                            int distance = (vehicle.GetColumn() - SelectedVehicle.GetEndOfVehicle() % 10) - 1;
                            // Update MaxAmountOfMoves if distance is smaller
                            if (distance < MaxAmountOfMoves)
                            {
                                MaxAmountOfMoves = distance;
                            }
                        }
                    }
                }
            }



            return MaxAmountOfMoves;

        }

        // This method facilitates the movement of a vehicle on the game board.
        public void MoveVehicle()
        {
            bool valid4 = false; // Flag to validate user input
            int userInput = 0; // Variable to store user input
            bool isValidInput = false; // Flag to validate user input
            string direction = ""; // Variable to store the direction of movement
            bool valid = false; // Flag to validate user input
            bool valid2 = false; // Flag to validate user input
            string colour = ""; // Variable to store the colour code of the vehicle to be moved
            Vehicle SelectedVehicle = null; // Variable to store the selected vehicle
            List<string> PossibleColourCodes = new List<string>(); // List to store possible colour codes of vehicles on the grid

            // Populate PossibleColourCodes with the colour codes of vehicles on the grid
            foreach (Vehicle i in grid.GetVehicles())
            {
                PossibleColourCodes.Add(i.GetColour());
            }

            // Prompt the user to enter the colour code of the vehicle to be moved
            Console.WriteLine("");
            Console.WriteLine("Enter the two-digit colour code for the vehicle you would like to move:");
            Console.WriteLine("E.g., If red, enter 01");
            Console.WriteLine("If you have given up, press 0 for the solver");
            Console.WriteLine("If you would like to exit the game, enter 8");
            Console.WriteLine("If you would like to pause, press 5");

            // Validate the user's input for the colour code
            do
            {
                colour = Console.ReadLine();
                if (PossibleColourCodes.Contains(colour) || colour == "0" || colour == "8" || colour == "5")
                {
                    valid2 = true;
                }
                else
                {
                    Console.WriteLine("The colour you entered does not exist on this game board");
                    Console.WriteLine("Please re-enter");
                }
            } while (valid2 == false);

            // Check if the user wants to use the solver
            if (colour == "0")
            {
                Bactracking(); // Call the Bactracking method for solving
            }

            // Check if the user wants to exit the game
            if (colour == "8")
            {
                Environment.Exit(0); // Exit the game
            }

            // Check if the user wants to pause the game
            if (colour == "5")
            {
                // Pause the timer
                score.PauseTimer();
                Console.WriteLine("Timer paused");
                Console.WriteLine("Press 1 to resume or 8 to exit the game");

                // Validate user input to resume or exit the game
                while (!isValidInput)
                {
                    Console.WriteLine("Please enter 1 or 8:");
                    string inputString = Console.ReadLine();

                    if (int.TryParse(inputString, out userInput))
                    {
                        if (userInput == 1 || userInput == 8)
                        {
                            isValidInput = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter either 1 or 8.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer.");
                    }
                }

                // If user chooses to exit, exit the game
                if (userInput == 8)
                {
                    Environment.Exit(0);
                }
                // If user chooses to resume, resume the timer and prompt for colour code again
                else if (userInput == 1)
                {
                    score.ResumeTimer();
                    Console.WriteLine("Timer resumed. You can carry on playing...");
                    Console.WriteLine("Please enter your colour code: ");
                    do
                    {
                        colour = Console.ReadLine();
                        if (PossibleColourCodes.Contains(colour))
                        {
                            valid4 = true;
                        }
                        else
                        {
                            Console.WriteLine("The colour you entered does not exist on this game board");
                            Console.WriteLine("Please re-enter");
                        }
                    } while (valid4 == false);
                }
            }

            // Identify the selected vehicle based on the entered colour code
            foreach (Vehicle v in grid.GetVehicles())
            {
                if (v.GetColour() == colour)
                {
                    SelectedVehicle = v;
                }
            }

            // Prompt the user to choose the direction of movement based on the vehicle's orientation
            if (SelectedVehicle.GetDirection() == "v") // If the vehicle is vertical
            {
                Console.WriteLine("Would you like to move up or down? U/D");

                // Validate user input for the direction of movement
                do
                {
                    direction = Console.ReadLine().ToUpper();
                    if (direction == "U" || direction == "D")
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine("You must enter either U or D");
                    }
                } while (valid == false);
            }
            else if (SelectedVehicle.GetDirection() == "h") // If the vehicle is horizontal
            {
                Console.WriteLine("Would you like to move left or right? L/R");

                // Validate user input for the direction of movement
                do
                {
                    direction = Console.ReadLine().ToUpper();
                    if (direction == "L" || direction == "R")
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine("You must enter either L or R");
                    }
                } while (valid == false);
            }

            int NumOfSpaces = 0; // Variable to store the number of spaces to move
            bool valid3 = false; // Flag to validate user input
            int max = CalcMaxAmountOfMoves(SelectedVehicle, direction); // Calculate the maximum number of spaces the vehicle can move in the chosen direction

            // Display the maximum number of spaces the vehicle can move
            if (max == 0)
            {
                Console.WriteLine("You cannot move in the " + DirectionToMove(direction) + " direction");
            }
            else
            {
                Console.WriteLine("The max amount of spaces is " + max);
                Console.WriteLine("How many spaces would you like to move in the " + DirectionToMove(direction) + " direction?");

                // Validate user input for the number of spaces to move
                do
                {
                    try
                    {
                        NumOfSpaces = Convert.ToInt32(Console.ReadLine());
                        if (NumOfSpaces <= max && NumOfSpaces >= 0)
                        {
                            valid3 = true;
                        }
                        else
                        {
                            Console.WriteLine("Number must be between 0 and " + max);
                            valid3 = false;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("You must enter an integer");
                        Console.WriteLine("Please re-enter");
                        valid3 = false;
                    }
                } while (valid3 == false);

                // Move the vehicle in the chosen direction by the specified number of spaces
                MoveVehicleInDirectionChosen(direction, NumOfSpaces, SelectedVehicle);

                // Display the updated game grid
                grid.DisplayGrid();
            }
        }






        // This method moves the specified vehicle in the chosen direction by the specified number of spaces.
        public void MoveVehicleInDirectionChosen1(string way, int NumberOfSpaces, Vehicle vehicle)
        {
            int distancepos = CalcMaxAmountOfMoves(vehicle, way);
            if (distancepos == 0) { return; }
            int currentLocation;
            int PlaceInArray;
            currentLocation = vehicle.GetLocation();



            PlaceInArray = grid.GetIndexOfSquare(currentLocation);
            if (way == "L")
            {
                PlaceInArray = PlaceInArray - NumberOfSpaces;
            }
            else if (way == "R")
            {
                PlaceInArray = PlaceInArray + NumberOfSpaces;
            }
            else if (way == "U")
            {
                PlaceInArray = PlaceInArray - (NumberOfSpaces * 6);
            }
            else if (way == "D")
            {
                PlaceInArray = PlaceInArray + (NumberOfSpaces * 6);
            }
            for (int i = 0; i < grid.GetVehicles().Count; i++)
            {
                if (grid.GetVehicles()[i] == vehicle)
                {
                    grid.GetVehicles()[i].ChangeLocation(IndexToSquareReference(PlaceInArray));
                }
            }
            grid.SetGrid(grid.GetVehicles());
        }

            // This method moves the specified vehicle in the chosen direction by the specified number of spaces.
            public void MoveVehicleInDirectionChosen(string way, int NumberOfSpaces, Vehicle vehicle)
        {
            // Declare variables to store the current location of the vehicle and its new position in the game grid
            int currentLocation;
            int PlaceInArray;

            // Retrieve the current location of the vehicle
            currentLocation = vehicle.GetLocation();

            // Get the index of the square in the game grid where the vehicle is located
            PlaceInArray = grid.GetIndexOfSquare(currentLocation);

            // Determine the new position of the vehicle in the game grid based on the chosen direction and number of spaces to move
            if (way == "L")
            {
                // If moving left, decrement the index by the specified number of spaces
                PlaceInArray = PlaceInArray - NumberOfSpaces;
            }
            else if (way == "R")
            {
                // If moving right, increment the index by the specified number of spaces
                PlaceInArray = PlaceInArray + NumberOfSpaces;
            }
            else if (way == "U")
            {
                // If moving up, decrement the index by the product of the specified number of spaces and the width of the grid
                PlaceInArray = PlaceInArray - (NumberOfSpaces * 6);
            }
            else if (way == "D")
            {
                // If moving down, increment the index by the product of the specified number of spaces and the width of the grid
                PlaceInArray = PlaceInArray + (NumberOfSpaces * 6);
            }

            // Update the location of the vehicle in the game grid based on the new index
            vehicle.ChangeLocation(IndexToSquareReference(PlaceInArray));
        }





    
    public int IndexToSquareReference(int index)
        {
            //returns the loaction coordinate, given the index in array
            int row = index / 6 + 1;
            int col = index % 6 + 1;
            return row * 10 + col;
        }
        public string DirectionToMove(string way)
        {
            string FullWordDirection = "";
            switch (way)
            {
                case "U":
                    FullWordDirection = "up";
                    break;
                case "D":
                    FullWordDirection = "down";
                    break;
                case "L":
                    FullWordDirection = "left";
                    break;
                case "R":
                    FullWordDirection = "right";
                    break;
            }
            return FullWordDirection;


        }
        public bool CheckIfUserHasWon()
        {
            //checks if red car is in location 35 and if it is then user has won!
            bool UserHasWon = false;
            foreach (Vehicle i in grid.GetVehicles())
            {
                if (i.GetColour() == "01" && i.GetLocation() == 35)
                {
                    UserHasWon = true;

                }

            }
            return UserHasWon;


        }
    }
}
