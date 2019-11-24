using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayParty_First
{
    class DinnerParty
    {
        public int NumberOfPeople { get; set; }
        public bool FancyDecorations { get; set; }
        public bool HealthyOption { get; set; }
        //private decimal CostOfDecorations { get; set; }
        public DinnerParty(int numberOfPeople, bool healthyOption, bool fancyDecorations)
        {
            NumberOfPeople = numberOfPeople;
            HealthyOption = healthyOption;
            FancyDecorations = fancyDecorations;
        }
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
        private decimal CalculateCostOfBeveragesPerPerson()//酒水的花销
        {
            if (HealthyOption)
            {
                return NumberOfPeople * 5M;
            }
            else
            {
                return NumberOfPeople * 20M;
            }
        }
        public decimal Cost//一个只读属性
        {
            get
            {
                if (HealthyOption)
                {
                    return ((NumberOfPeople * 25M) + CalculateCostOfBeveragesPerPerson()
                        + CalculateCostOfDecorations()) * 0.95M;
                }
                else
                {
                    return (NumberOfPeople * 25M) + CalculateCostOfDecorations()
                        + CalculateCostOfBeveragesPerPerson();
                }
            }
        }
    }
}
