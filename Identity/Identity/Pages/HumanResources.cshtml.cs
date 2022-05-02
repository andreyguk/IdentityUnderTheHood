using Identity.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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
            var response = await httpClient.PostAsJsonAsync("auth", new CreadentialsDto { UserName = "admin", Password = "admin" });
            response.EnsureSuccessStatusCode();
            var authProps = await response.Content.ReadAsStringAsync();
            var jwtToken = JsonConvert.DeserializeObject<JwtTokenDto>(authProps).AccessToken;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            WeatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDto>>("WeatherForecast");

        }
    }

    class CreadentialsDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    class JwtTokenDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expired_at")]
        public string ExpiredAt { get; set; }
    }
}
