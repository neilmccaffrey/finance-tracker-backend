using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


[Route("api/[controller]")]
[ApiController]
public class NewsController : ControllerBase
{
    private readonly AppDbContext _context;

    public NewsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GuardianNews>>> GetFinancialNews()
    {
        // Fetch news stories from the database
        var news = await _context.GuardianNews.ToListAsync();

        // Return the list of news stories
        return Ok(news);
    }
}