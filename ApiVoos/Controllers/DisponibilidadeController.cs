using Microsoft.AspNetCore.Mvc;

namespace ApiVoos.Controllers;

[ApiController]
[Route("[controller]")]
public class DisponibilidadeController : ControllerBase
{

    private readonly ILogger<DisponibilidadeController> _logger;
    private readonly string _golUrl = "https://dev.reserve.com.br/airapi/gol/getavailability";
    private readonly string _latamUrl = "https://dev.reserve.com.br/airapi/latam/flights";

    public DisponibilidadeController(ILogger<DisponibilidadeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string origin, string destination, string date)
    {
        var queryGol = new Dictionary<string, string>()
        {
            {"origin", origin},
            {"destination" , destination},
            {"date", date}
        };

        var queryLatam = new Dictionary<string, string>() {
            {"departureCity", origin},
            { "arrivalCity", destination},
            {"departureDate", date }
        };

        var voosGolResponse = await BuscaVoos(_golUrl, queryGol);
        var voosLatamResponse = await BuscaVoos(_latamUrl, queryLatam);

        

        // Mapear retorno em classe VooModel



        return Ok();
    }

    private async Task<IActionResult> BuscaVoos(string endpoint, Dictionary<string, string> queryParams)
    {
        HttpClient client = new HttpClient();
        UriBuilder url = new UriBuilder(endpoint);

        var queryString = string.Join('&', queryParams.Select(q => $"{q.Key}={q.Value}"));

        using HttpResponseMessage response = await client.GetAsync($"{url}?{queryString.Replace(" ", "%")}");

        return Ok(response.Content);
    }
}
