using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace StorageWebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public DateTime BirthDate { get; set; }

        [BindProperty]
        public string LoginEmail { get; set; }

        [BindProperty]
        public string LoginPassword { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            var client = _httpClientFactory.CreateClient("BackendApi");

            var registrationDto = new
            {
                Name = Name,
                Email = Email,
                Password = Password,
                BirthDate = BirthDate
            };

            var response = await client.PostAsJsonAsync("User", registrationDto);
            if (response.IsSuccessStatusCode)
            {
                Message = "Sikeres regisztr�ci�!";
            }
            else
            {
                Message = "Hiba t�rt�nt a regisztr�ci� sor�n.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            var client = _httpClientFactory.CreateClient("BackendApi");

            var loginDto = new
            {
                Email = LoginEmail,
                Password = LoginPassword
            };

            var response = await client.PutAsJsonAsync("User", loginDto);
            if (response.IsSuccessStatusCode)
            {
                Message = "Sikeres bejelentkez�s!";
            }
            else
            {
                Message = "Helytelen email vagy jelsz�.";
            }

            return Page();
        }
    }
}
