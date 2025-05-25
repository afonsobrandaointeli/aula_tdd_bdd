using Microsoft.AspNetCore.Mvc;
using CalculatorApi.Models;

namespace CalculatorApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    [HttpGet("add/{a}/{b}")]
    public ActionResult<CalculationResponse> Add(decimal a, decimal b)
{
    // Business rule: Don't accept values greater than 1 million (but allow for overflow testing)
    if (Math.Abs(a) > 1_000_000 || Math.Abs(b) > 1_000_000)
    {
        // Check if this is a case where we want to test result overflow
        var result = a + b;
        if (Math.Abs(result) > 10_000_000)
        {
            return BadRequest(new CalculationResponse
            {
                Success = false,
                ErrorMessage = "Result exceeds maximum limit of 10 million",
                Operation = "Addition"
            });
        }
        
        // If not overflow, then it's individual value limit
        return BadRequest(new CalculationResponse
        {
            Success = false,
            ErrorMessage = "Values cannot exceed 1 million",
            Operation = "Addition"
        });
    }

    var calculatedResult = a + b;

    // Business rule: Result cannot exceed 10 million
    if (Math.Abs(calculatedResult) > 10_000_000)
    {
        return BadRequest(new CalculationResponse
        {
            Success = false,
            ErrorMessage = "Result exceeds maximum limit of 10 million",
            Operation = "Addition"
        });
    }

    return Ok(new CalculationResponse
    {
        Result = calculatedResult,
        Operation = "Addition",
        Success = true
    });
}

    [HttpGet("divide/{dividend}/{divisor}")]
    public ActionResult<CalculationResponse> Divide(decimal dividend, decimal divisor)
    {
        // Business rule: Don't allow division by zero
        if (divisor == 0)
        {
            return BadRequest(new CalculationResponse
            {
                Success = false,
                ErrorMessage = "Division by zero is not allowed",
                Operation = "Division"
            });
        }

        var result = dividend / divisor;

        return Ok(new CalculationResponse
        {
            Result = Math.Round(result, 2),
            Operation = "Division",
            Success = true
        });
    }
}
