using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA_Rushhour_game
{
    internal abstract class Vehicle
    {
        //attributes
        protected string colour;
        protected int location;
        protected string direction;
        protected int size;
        
        

        public Vehicle(string colour, int location, string direction)
        {
            this.colour = colour;
            this.location = location;
            this.direction = direction;

        }
        //getters and setters
        public void ChangeLocation(int location1)
        {
            location = location1;
        }
        public int GetLocation()
        {
            return location;
        }
        public string GetDirection()
        {
            return direction;
        }
      
        public int GetSize()
        {
            return size;    
        }
        public string GetColour()
        {
            return colour;
        }
        public int GetColumn()
        {
            int col;
            col = location % 10;
            return col;
            //create a subroutine that gets the row
        }
        public int GetRow()
        {
            int row;
            row = location / 10;
            return row;
            //create a subroutine that gets the row
        }
        public int GetEndOfVehicle()
        {
            //returns the loaction that the vehicle ends
            int EndOfVehicle=0;
            if (size == 3)
            {
                if (direction == "v")
                {
                    EndOfVehicle = location + 20;
                     

                }
                else if (direction == "h")
                {
                    EndOfVehicle = location + 2;
                }


            }
            else if (size == 2)
            {
                if (direction == "v")
                {
                        EndOfVehicle = location + 10;
                }
                else if (direction == "h")
                {
                    EndOfVehicle = location + 1;
                }
            }
            return EndOfVehicle;
        }



    }
    class Car: Vehicle
    {
        //inherits from vehicle
        
        public Car(string colour, int location, string direction):base(colour, location,direction)
        {
            //size is 2 as it's a car
            size = 2;
            
        }
    }
    class Truck : Vehicle
    {
        //inherits from vehicle
        public Truck(string colour, int location, string direction):base(colour, location,direction)
        {
            //size is 3 as it's a truck
            size = 3;
        }
    }
}
