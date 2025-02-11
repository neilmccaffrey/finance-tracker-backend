using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionsController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/transactions
    [HttpPost]
    public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
    {
       
        // Make sure the UUID is being passed from frontend
        if (transaction.Id == Guid.Empty)
        {
            return BadRequest("UUID is required.");
        }
        // Make sure Transaction Type is being passed from rontend
        if (string.IsNullOrEmpty(transaction.Type))
        {
            return BadRequest("Transaction type is required.");
        }

        // Add the transaction to the database
        _context.Transactions.Add(transaction);

        // Save the changes to the database
        await _context.SaveChangesAsync();

        // Return the created transaction
        return Ok(transaction); // Sends back the transaction data as a response
    }

 // Get all expense transactions for a user
[HttpGet("user/{userId}/expenses")]
public async Task<ActionResult<IEnumerable<Transaction>>> GetUserExpenses(int userId)
{
    var expenses = await _context.Transactions
        .Where(t => t.UserId == userId && t.Type == "Expense") // Check Type as string
        .ToListAsync();

    return Ok(expenses);
}

// Get all income transactions for a user
[HttpGet("user/{userId}/income")]
public async Task<ActionResult<IEnumerable<Transaction>>> GetUserIncome(int userId)
{
    var income = await _context.Transactions
        .Where(t => t.UserId == userId && t.Type == "Income") // Check Type as string
        .ToListAsync();

    return Ok(income);
}

// DELETE: api/transactions/{id}
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteTransaction(int id)
{
    var transaction = await _context.Transactions.FindAsync(id);
    
    if (transaction == null)
    {
        return NotFound(new { message = "Transaction not found." });
    }

    _context.Transactions.Remove(transaction);
    await _context.SaveChangesAsync();

    return Ok(new { success = true, message = "Transaction deleted successfully." });
}


}