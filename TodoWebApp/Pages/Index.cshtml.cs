using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TodoWebApp.Models;

namespace TodoWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        public List<WeatherForecast>? weatherForecasts { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task OnGet()
        {
            await GetWeatherForecasts();
        }

        public async Task<List<WeatherForecast>> GetWeatherForecasts()
        {
            
            string? baseAddress = _configuration.GetValue<string>("BaseApiAddress");
            
            Console.WriteLine($"BaseApiAddress : {baseAddress}");

            using (var httpClient = new HttpClient())
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(baseAddress+ "/WeatherForecast"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    weatherForecasts = apiResponse != null? JsonConvert.DeserializeObject<List<WeatherForecast>>(apiResponse): new List<WeatherForecast>();
                }
            }
            return weatherForecasts ?? new List<WeatherForecast>();
        }
    }
}