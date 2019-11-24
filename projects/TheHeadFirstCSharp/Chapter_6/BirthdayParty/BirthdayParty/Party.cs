using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayParty
{
    class Party
    {
        public const int CostOfFoodPerPerson = 25;
        public int NumberOfPeople { get; set; }
        public bool FancyDecorations { get; set; }
        private decimal CalculateCostOfDecorations()//饰品的花销
        {
            if (FancyDecorations)
            {
                return (NumberOfPeople * 15M) + 50M;
            }
            else
            {
                return (NumberOfPeople * 7.5M) + 30M;
            }
        }

        virtual public decimal Cost
        {
            get
            {
                decimal totalCost;
                totalCost = CalculateCostOfDecorations();
                totalCost += NumberOfPeople * CostOfFoodPerPerson;
                if(NumberOfPeople > 12)
                {
                    totalCost += 100;
                }
                return totalCost;
            }     
        }
    }
}
