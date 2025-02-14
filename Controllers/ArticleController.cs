using Microsoft.AspNetCore.Mvc; 
using System; 
using System.Threading.Tasks; 
using MySql.Data.MySqlClient; // For MySQL database operations
using System.Net.Http; // For HttpClient to call The Guardian API
using Newtonsoft.Json.Linq; // For parsing JSON responses from the API
using Microsoft.Extensions.Configuration; 

[ApiController]
[Route("api/[controller]")]
public class CronJobController : ControllerBase
{
    private readonly string _connectionString = Environment.GetEnvironmentVariable("MYSQL_URL");

    [HttpPost("update-database")]
    public async Task<IActionResult> UpdateDatabase()
    {
        var apiKey = Environment.GetEnvironmentVariable("GUARDIAN_API_KEY");
        using (var httpClient = new HttpClient())
        {
            try
            {
                // Fetch data from The Guardian API
                var apiUrl = $"https://content.guardianapis.com/search?section=business&production-office=us&page-size=5&order-by=newest&api-key={apiKey}";
                var response = await httpClient.GetStringAsync(apiUrl);
                var json = JObject.Parse(response);
                var articles = json["response"]?["results"];

                // Check if API response is valid
                if (articles == null)
                {
                    return StatusCode(500, "Invalid API response format.");
                }

                // Update database
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var deleteCommand = new MySqlCommand("DELETE FROM guardian_news LIMIT 5", connection);
                    await deleteCommand.ExecuteNonQueryAsync();

                    var insertCommand = new MySqlCommand(
                        "INSERT INTO guardian_news (Id, Title, Url) VALUES (@Id, @Title, @Url)",
                        connection);

                    foreach (var article in articles)
                    {
                        insertCommand.Parameters.Clear();
                        insertCommand.Parameters.AddWithValue("@Id", article["id"].ToString());
                        insertCommand.Parameters.AddWithValue("@Title", article["webTitle"].ToString());
                        insertCommand.Parameters.AddWithValue("@Url", article["webUrl"].ToString());
                        await insertCommand.ExecuteNonQueryAsync();
                    }
                }

                return Ok("Database updated successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update database: {ex.Message}");
            }
        }
    }
}