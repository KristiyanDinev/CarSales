using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarSales.Utilities
{
    public class ErrorUtility
    {
        public static void HandleModelErrors(IEnumerable<string> Errors, 
           ref ModelStateDictionary ModelState)
        {
            foreach (string error in Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}
