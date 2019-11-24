using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dog_Racing
{
    public class Guy
    {
        public string Name;
        public Bet MyBet;
        public int Cash;

        public RadioButton MyRadioButton;
        public Label MyLabel;

        public void UpdateLabels()
        {
            MyLabel.Text = MyBet.GetDescription();
            MyRadioButton.Text = Name + " has " + Cash + " bucks.";
        }

        public void ClearBet()
        {
            MyBet.Amount = 0;
            MyBet.Bettor = null;
            MyBet.Dog = 0;
        }

        public bool PlaceBet(int BetAmount, int DogToWin)
        {
            if(Cash >= BetAmount && BetAmount >= 5)
            {
                MyBet = new Bet()
                {
                    Amount = BetAmount,
                    Dog = DogToWin,
                    Bettor = this
                };
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Collect(int Winner)
        {
            Cash += MyBet.PayOut(Winner);
            this.UpdateLabels();
            this.ClearBet();
        }
    }
}
