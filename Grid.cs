using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA_Rushhour_game
{
    internal class Grid
    {
        //attributes
        private List<Vehicle> grid;

        

        


        public Grid()
        {
            grid = new List<Vehicle>();
        }
        //getters and setters
        public List<Vehicle> GetVehicles()
        {
            return grid;
        }
        public void SetGrid(List<Vehicle> grid)
        {
            this.grid = grid;
        }

        public void DisplayGrid()
        {
            
            string[] gridArray = new string[36];
            //creates an array of 36 elements
            CreateGrid(gridArray);
            //make it a nested loop
            for(int i = 0; i < 6; i++)
            {
                //displays line 1
                ColourToDisplay(gridArray[i]);
                Console.Write(gridArray[i]+"");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");

            }
            Console.WriteLine("\n");
            
            for (int i = 6; i < 12; i++)
            {
                //displays line 2
                ColourToDisplay(gridArray[i]);
                Console.Write(gridArray[i] + "");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");

            }
            Console.WriteLine("\n");
            for (int i = 12; i < 18; i++)
            {
                //displays line 3
                ColourToDisplay(gridArray[i]);
                Console.Write(gridArray[i] + "");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");
            }
            Console.WriteLine("\n");
            for (int i = 18; i < 24; i++)
            {
                //displays line 4 
                ColourToDisplay(gridArray[i]);
                Console.Write(gridArray[i] + "");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");
            }
            Console.WriteLine("\n");
            for (int i = 24; i < 30; i++)
            {
                //displays line 5
                ColourToDisplay(gridArray[i]);
                Console.Write(gridArray[i] + "");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");

            }
            Console.WriteLine("\n");
            for (int i = 30; i < 36; i++)
            {
                //displays line 6
                ColourToDisplay(gridArray[i]);
                Console.Write(gridArray[i] + "");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");
            }
            Console.ForegroundColor = ConsoleColor.White;
           


            //displays grid
            //goes through grid and displays the value in the correct colour
        }
        public void CreateGrid(string[] gridArray)
        {
            int location;
            for (int i = 0; i < gridArray.Length; i++)
            {
                gridArray[i] = "00";
            }
            //creates an empty array of 36 with 00
            foreach (var item in grid)
            {
                location = item.GetLocation();
                //gets the location of the vehicle


                if (item.GetSize() == 3)
                {
                    //for all trucks as they take up 3 spaces in array
                    if (item.GetDirection() == "v")
                    {
                        //puts the colour code of the vehicle in the array at the correct place, according to the size and direction
                        gridArray[GetIndexOfSquare(location)] = item.GetColour();
                       
                        gridArray[GetIndexOfSquare(location) + 6] = item.GetColour();
                        //adds 6 to location as its vertical and its the second space going down
                        gridArray[GetIndexOfSquare(location) + 12] = item.GetColour();
                        //adds 12 to location as its vertical and its the third space going down
                    }
                    else if (item.GetDirection() == "h")
                    {
                        gridArray[GetIndexOfSquare(location)] = item.GetColour();
                        gridArray[GetIndexOfSquare(location) + 1] = item.GetColour();
                        //adds 1 to location as its horizontal and its the second space going right
                        gridArray[GetIndexOfSquare(location) + 2] = item.GetColour();
                        //adds 2 to location as its horizontal and its the third space going right
                    }
                }
                else if (item.GetSize() == 2)
                {
                    //for all cars that take up 2 spaces
                    if (item.GetDirection() == "v")
                    {
                        gridArray[GetIndexOfSquare(location)] = item.GetColour();
                        gridArray[GetIndexOfSquare(location) + 6] = item.GetColour();
                        //adds 6 to location as its vertical and its the second space going down
                    }
                    else if (item.GetDirection() == "h")
                    {
                        gridArray[GetIndexOfSquare(location)] = item.GetColour();
                        gridArray[GetIndexOfSquare(location) + 1] = item.GetColour();
                        //adds 1 to location as its horizontal and its the second space going right
                    }
                }

            }
        }
        public string GetColourInArray(int SquareReference)
        {
            //returns the colour code in the array, given the location cooridinate
            string[] gridArray = new string[36];
            
            CreateGrid(gridArray);

            return gridArray[GetIndexOfSquare(SquareReference)];
        }
        public int GetIndexOfSquare(int SquareReference)//problem when 1 is parameter
        {
            //returns the index in the array of the location coordinate
            int Row = SquareReference / 10;
            int Col = SquareReference % 10;
            return (Row - 1) * 6 + (Col - 1);
        }
        
        public void ColourToDisplay(string colour)
        {   
            //this simply changes the colour being displayed depending on the colour code of the vehicle
            
            switch (colour)
            {
                case "00":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "01":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case "02":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "03":
                    Console.ForegroundColor  = ConsoleColor.DarkGray;
                    break;
                case "04":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case "05":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "06":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case "07":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "08":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "09":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "10":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case "11":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "12":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "13":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "14":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "15":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "16":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
            }
            

        }
            
    }
}
