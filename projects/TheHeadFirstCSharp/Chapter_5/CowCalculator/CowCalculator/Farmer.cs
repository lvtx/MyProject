using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cow_calculator
{
    class Farmer
    {
        public Farmer (int numberOfCows, int feedMultiplier)
        {
            this.feedMultiplier = feedMultiplier;
            NumberOfCows = numberOfCows;
        }

        private int feedMultiplier;
        public int FeedMultiplier { get { return feedMultiplier; } }

        private int BagsOfFeed { get; set; }

        private int numberOfCows;
        public int NumberOfCows
        {
            get { return numberOfCows; }
            set
            {
                numberOfCows = value;
                BagsOfFeed = numberOfCows * FeedMultiplier;
            }
        }
    }
}
