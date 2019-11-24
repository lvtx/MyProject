using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Racing
{
    public class Bet
    {
        public int Amount;
        public int Dog;
        public Guy Bettor;

        public string GetDescription()
        {
            if(this.Amount > 0  && this.Dog > 0)
            {
                return Bettor.Name + " bets " + this.Amount + " on dog" + " #" + this.Dog + ".";
            }
            else
            {
                return Bettor.Name + " hasn't placed a bet.";
            }
        }

        public int PayOut(int Winner)
        {
            if(this.Dog == Winner)
            {
                return this.Amount;
            }
            else
            {
                return (-(this.Amount));
            }
        }
    }
}
