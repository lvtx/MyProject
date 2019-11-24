using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewDinnerParty
{
    class DinnerParty
    {
        public int NumberOfPerple { get; set; }
        public bool FancyDecorations { get; set; }
        public bool HealthyOption { get; set; }
        //private decimal CostOfDecorations { get; set; }
        public DinnerParty(int numberOfPeople,bool healthyOption,bool fancyDecorations)
        {
            NumberOfPerple = numberOfPeople;
            HealthyOption = healthyOption;
            FancyDecorations = fancyDecorations;
        }
        private decimal CalculateCostOfDecorations()//饰品的花销
        {
            if (FancyDecorations)
            {
                return (NumberOfPerple * 15M) + 50M;
            }
            else
            {
                return (NumberOfPerple * 7.5M) + 30M;
            }
        }
        private decimal CalculateCostOfBeveragesPerPerson()//酒水的花销
        {
            if (HealthyOption)
            {
                return NumberOfPerple * 5M;
            }
            else
            {
                return NumberOfPerple * 20M;
            }
        }
        public decimal Cost//一个只读属性
        {
            get
            {
                if (HealthyOption)
                {
                    return ((NumberOfPerple * 25M) + CalculateCostOfBeveragesPerPerson() 
                        + CalculateCostOfDecorations()) * 0.95M;
                }
                else
                {
                    return (NumberOfPerple * 25M) + CalculateCostOfDecorations() 
                        + CalculateCostOfBeveragesPerPerson();
                }
            }
        }
    }
}
