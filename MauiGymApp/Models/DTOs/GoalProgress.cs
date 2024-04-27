using MauiGymApp.Calculations;
using UnitsNet;

namespace MauiGymApp.Models
{
    public record GoalProgress
    {
        public GoalProgress(IQuantity goalQuantity, IQuantity currentQuantity)
        {
            GoalQuantity = goalQuantity;
            CurrentQuantity = currentQuantity;

            Progress = CalculateProgress(GoalQuantity, CurrentQuantity);
            Remaining = CalculateRemaining(GoalQuantity, CurrentQuantity);
        }
        
        public IQuantity GoalQuantity { get; }
        public IQuantity CurrentQuantity { get; }

        /// <summary>
        /// Current value / Goal value. 
        /// </summary>
        public double Progress { get; }
        /// <summary>
        /// Goal value - Current value.
        /// </summary>
        public IQuantity Remaining { get; }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="goalQuantity"></param>
        /// <param name="currentQuantity"></param>
        /// <returns></returns>
        public static double CalculateProgress(IQuantity goalQuantity, IQuantity currentQuantity)
        {
            UnitsNetHelpers.CheckSameBaseUnits(goalQuantity, currentQuantity);
            
            return Math.Min(currentQuantity.AsBaseUnit(), goalQuantity.AsBaseUnit()) / Math.Max(currentQuantity.AsBaseUnit(), goalQuantity.AsBaseUnit());
        }

        /// <summary>
        /// Returns postitive difference between 2 quantities
        /// </summary>
        /// <param name="q1">Quantity 1</param>
        /// <param name="q2">Quantity 2</param>
        /// <returns></returns>
        public static IQuantity CalculateRemaining(IQuantity q1, IQuantity q2)
        {   
            if (q1.AsBaseUnit() > q2.AsBaseUnit()){
                return q1.Substract(q2);
            }
            return q2.Substract(q1);
        }
    }
}
