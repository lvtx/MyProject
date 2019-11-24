using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dog_Racing
{
    public class Greyhound
    {
        public int StartingPosition;
        public int RacetrackLength;
        public PictureBox MyPictureBox;
        public int Location = 0;
        public Random Randomizer;
        public PictureBox racetrackPictureBox;

        public bool Run()
        {
            Location += Randomizer.Next(1, 5);
            MyPictureBox.Left = StartingPosition + Location;
            if (MyPictureBox.Left < RacetrackLength)
            {    
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TakeStartingPosition()//将狗放回起点0
        {
            Location = 0;
            MyPictureBox.Left = StartingPosition;
        }
    }
}
