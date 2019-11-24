using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DogPictureRunExample
{
    public class Dog
    {
        public int StartingPosition;
        public int RacetrackLength;
        public PictureBox MyPictureBox;
        public int Location;
        public Random Randomizer;

        public bool Run()
        {
            Location = Location + Randomizer.Next(5, 10);
            MyPictureBox.Left = Location;
            if (MyPictureBox.Left < RacetrackLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
