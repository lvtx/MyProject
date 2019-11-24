using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dog_Racing
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        Greyhound[] GreyhoundArray = new Greyhound[4];
        Guy[] guys = new Guy[3];
        int index = 0;

        public Form1()
        {
            InitializeComponent();
            GreyhoundArray[0] = new Greyhound()
            {
                MyPictureBox = pictureBox1,
                
                StartingPosition = pictureBox1.Left,
                RacetrackLength = raceTrackPicture.Width + raceTrackPicture.Left - pictureBox1.Width - pictureBox1.Width,
                Randomizer = random
            };

            GreyhoundArray[1] = new Greyhound()
            {
                MyPictureBox = pictureBox2,
                
                StartingPosition = pictureBox2.Left,
                RacetrackLength = raceTrackPicture.Width + raceTrackPicture.Left - pictureBox2.Width - pictureBox1.Width ,
                Randomizer = random
            };

            GreyhoundArray[2] = new Greyhound()
            {
                MyPictureBox = pictureBox3,
                
                StartingPosition = pictureBox3.Left,
                RacetrackLength = raceTrackPicture.Width + raceTrackPicture.Left - pictureBox3.Width - pictureBox1.Width,
                Randomizer = random
            };

            GreyhoundArray[3] = new Greyhound()
            {
                MyPictureBox = pictureBox4,
                
                StartingPosition = pictureBox4.Left,
                RacetrackLength = raceTrackPicture.Width + raceTrackPicture.Left - pictureBox4.Width - pictureBox1.Width,
                Randomizer = random
            };

            guys[0] = new Guy()
            {
                Name = "Joe",
                MyBet = new Bet(),
                Cash = 50,
                MyLabel = joeBetLabel,
                MyRadioButton = radioButton1
            };

            guys[1] = new Guy()
            {
                Name = "Bob",
                MyBet = new Bet(),
                Cash = 75,
                MyLabel = bobBetLabel,
                MyRadioButton = radioButton2
            };

            guys[2] = new Guy()
            {
                Name = "Al",
                MyBet = new Bet(),
                Cash = 45,
                MyLabel = alBetLabel,
                MyRadioButton = radioButton3
            };

            guys[0].PlaceBet(random.Next(5, 15), random.Next(1, 4));
            guys[1].PlaceBet(random.Next(5, 15), random.Next(1, 4));
            guys[2].PlaceBet(random.Next(5, 15), random.Next(1, 4));

            for (int i = 0; i < guys.Length; i++)
            {
                guys[i].UpdateLabels();
            }
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                GreyhoundArray[i].TakeStartingPosition();
            }
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < GreyhoundArray.Length; i++)
            {
                if (!GreyhoundArray[i].Run())
                {
                    timer1.Stop();
                    for (int j = 0; j < guys.Length; j++)
                    {
                        guys[j].Collect(i + 1);
                    }
                }
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            index = 0;
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            index = 1;
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            index = 2;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (guys[index].PlaceBet((int)numericUpDown1.Value, (int)numericUpDown2.Value))
            {
                guys[index].UpdateLabels();
            }
            else
            {
                MessageBox.Show("你的钱用光了，不能下注了");
            }
        }
    }
}
