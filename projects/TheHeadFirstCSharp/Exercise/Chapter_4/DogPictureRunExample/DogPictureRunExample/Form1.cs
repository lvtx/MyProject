using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DogPictureRunExample
{
    public partial class Form1 : Form
    {

        Dog[] dogArray = new Dog[4];
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            dogArray[0] = new Dog()
            {
                MyPictureBox = pictureBox1,
                StartingPosition = pictureBox1.Left,
                Location = pictureBox1.Left,
                RacetrackLength = pictureBox1.Parent.Width - pictureBox1.Width,
                Randomizer = random
            };
            dogArray[1] = new Dog()
            {
                MyPictureBox = pictureBox2,
                StartingPosition = pictureBox2.Left,
                Location = pictureBox2.Left,
                RacetrackLength = pictureBox2.Parent.Width - pictureBox2.Width,
                Randomizer = random
            };
            dogArray[2] = new Dog()
            {
                MyPictureBox = pictureBox3,
                StartingPosition = pictureBox3.Left,
                Location = pictureBox3.Left,
                RacetrackLength = pictureBox3.Parent.Width - pictureBox3.Width,
                Randomizer = random
            };
            dogArray[3] = new Dog()
            {
                MyPictureBox = pictureBox4,
                StartingPosition = pictureBox4.Left,
                Location = pictureBox4.Left,
                RacetrackLength = pictureBox4.Parent.Width - pictureBox4.Width,
                Randomizer = random
            };
            //MessageBox.Show(dog.MyPictureBox.Left.ToString());
            //MessageBox.Show(pictureBox1.Width.ToString());
            //MessageBox.Show(pictureBox1.Parent.Width.ToString());
            //MessageBox.Show(dog.RacetrackLength.ToString());
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < dogArray.Length; i++)
            {
                if (!dogArray[i].Run())
                {
                    timer1.Stop();
                }
            } 
        }
    }  
}
