using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayParty
{
    class DinnerParty:Party
    {
        //private decimal CostOfDecorations { get; set; }
        public bool HealthyOption { get; set; }
        public DinnerParty(int numberOfPeople, bool healthyOption, bool fancyDecorations)
        {
            NumberOfPeople = numberOfPeople;
            FancyDecorations = fancyDecorations;
            HealthyOption = healthyOption;
        }
        
        private decimal CalculateCostOfBeveragesPerPerson()//酒水的花销
        {
            decimal costOfBeveragesPerPerson;
            if (HealthyOption)
            {
                return costOfBeveragesPerPerson = 5M;
            }
            else
            {
                return costOfBeveragesPerPerson = 20M;
            }
            return costOfBeveragesPerPerson;
        }
        override public decimal Cost//一个只读属性
        {
            get
            {
                decimal totalCost = base.Cost;
                totalCost += CalculateCostOfBeveragesPerPerson() * NumberOfPeople;
                if (HealthyOption)
                {
                    return totalCost * 0.95M;
                }
                else
                {
                    return totalCost;
                }
            }
        }
    }
}
