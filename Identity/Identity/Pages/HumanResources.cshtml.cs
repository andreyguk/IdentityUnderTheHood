using Identity.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Pages
{
    [Authorize(Policy = "HRDepartment")]
    public class HumanResourcesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        [BindProperty]
        public List<WeatherForecastDto> WeatherForecastItems { get; set; }

        public HumanResourcesModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task OnGet()
        {
            var httpClient = _httpClient.CreateClient("OurWebApi");
            WeatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDto>>("WeatherForecast");

        }
    }
}
