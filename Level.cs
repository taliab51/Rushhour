using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NEA_Rushhour_game
{
    internal class Level
    {
        private string level;

        public Level(string level)
        {
            this.level = level;

        }
        public void Setlevel(string level1)
        {
            level = level1;
        }
        public List<Vehicle> LoadGame()
        {

            List<Vehicle> vehicles = new List<Vehicle>();
            //read the text file
            StreamReader reader = new StreamReader("card" + level + ".txt");
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                String[] fields = line.Split(',');
                if (fields[0] == "Truck")
                {
                    vehicles.Add(new Truck(fields[1], Convert.ToInt32(fields[2]), fields[3]));
                }
                else if (fields[0] == "Car")
                {
                    vehicles.Add(new Car(fields[1], Convert.ToInt32(fields[2]), fields[3]));
                }

            }
            reader.Close();
            return vehicles;
            //read throght he every line and put it into an istance of truck/car
            //add the truck/car to vehicles

        }

       


    }
}
