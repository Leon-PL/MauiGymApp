using MauiGymApp.Calculations;

namespace MauiGymApp.Models
{
    public class FunctionChoice
    {   
        public bool IncludeFunction { get; set; }
        public OneRepMaxFunction Function { get; set; }

        public FunctionChoice(bool includeFunction, OneRepMaxFunction function)
        {
            IncludeFunction = includeFunction;
            Function = function;
        }
    }
}
