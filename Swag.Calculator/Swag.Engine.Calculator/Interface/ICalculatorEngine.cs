namespace Swag.Engine.Calculator.Interface;

public interface ICalculatorEngine
{
    Task<decimal> CalculateEstimate(int optimistic, int mostLikely, int pessimistic);
}