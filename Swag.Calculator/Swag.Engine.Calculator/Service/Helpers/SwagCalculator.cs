namespace Swag.Engine.Calculator.Service.Helpers;

public class SwagCalculator
{

    /// <summary>
    /// SWAG stands for Scientific Wild-ass Guess.
    /// https://projectmanagers.net/swag-estimates-in-project-management/
    /// </summary>
    /// <param name="optimistic"></param>
    /// <param name="mostLikely"></param>
    /// <param name="pessimistic"></param>
    /// <param name="precision"></param>
    /// <returns></returns>
    public async Task<decimal> Calculate(int optimistic, int mostLikely, int pessimistic, int precision = 4)
    {
        var numerator = (optimistic + (mostLikely * 4) + pessimistic);
        var result =  numerator / (decimal) 6;
        result = Math.Round(result, precision);
        return await Task.FromResult(result);
    }

}