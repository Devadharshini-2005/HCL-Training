using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagementSystem.Models
{
    internal class FeeCalculator
    {
        public decimal baseFee;
        public decimal BaseFee { get; set; }

        public decimal discount;
        public decimal Discount { get; set; }

        public decimal totalFee;
        public decimal TotalFee { get; set; }

        public FeeCalculator(decimal baseFee, decimal discount, decimal totalFee)
        {
            this.baseFee = baseFee;
            this.discount = discount;
            this.totalFee = totalFee;
        }

        public FeeCalculator()
        {

        }
        public List<FeeCalculator> BaseFeeCalculation(List<Course> courseList)
        {

            List<FeeCalculator> feeCalculationTuple = new List<FeeCalculator>();
            Console.WriteLine("Enter the Course Code : ");
            string? courseCode = Console.ReadLine();

            for(int i = 0; i < courseList.Count(); i++)
            {
                if (courseList[i].courseCode == courseCode)
                {
                    feeCalculationTuple.Add(new FeeCalculator(courseList[i].fee, 10,courseList[i].fee - (courseList[i].fee/10)));
                }
            }
            return feeCalculationTuple;
        }
    }
}
