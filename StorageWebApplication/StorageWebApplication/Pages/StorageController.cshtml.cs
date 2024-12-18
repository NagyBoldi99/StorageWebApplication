using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StorageWebApplication.Pages
{
    public class StorageControllerModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public StorageControllerModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7102/api/storage/");
        }

        public List<StorageDto> Storages { get; set; } = new List<StorageDto>();
        public string Message { get; set; }

        public async Task OnGetAsync()
        {
            await LoadStoragesAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync(string Sname, string Sdescription, int Sarea, string OwnerId)
        {
            var dto = new CreateStorageDto
            {
                Sname = Sname,
                Sdescription = Sdescription,
                Sarea = Sarea,
                OwnerId = string.IsNullOrEmpty(OwnerId) ? (Guid?)null : Guid.Parse(OwnerId)
            };

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("create", content);

                if (response.IsSuccessStatusCode)
                {
                    Message = "T�rol� sikeresen l�trehozva!";
                }
                else
                {
                    Message = "Nem siker�lt l�trehozni a t�rol�t.";
                }
            }
            catch (Exception ex)
            {
                Message = $"Hiba t�rt�nt: {ex.Message}";
            }

            await LoadStoragesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string StorageId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(StorageId);

                if (response.IsSuccessStatusCode)
                {
                    Message = "T�rol� sikeresen t�r�lve!";
                }
                else
                {
                    Message = "Nem siker�lt t�r�lni a t�rol�t.";
                }
            }
            catch (Exception ex)
            {
                Message = $"Hiba t�rt�nt: {ex.Message}";
            }

            await LoadStoragesAsync();
            return Page();
        }

        private async Task LoadStoragesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("all");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Storages = JsonSerializer.Deserialize<List<StorageDto>>(json) ?? new List<StorageDto>();
                }
                else
                {
                    Message = "Nem siker�lt bet�lteni a t�rol�kat.";
                }
            }
            catch (Exception ex)
            {
                Message = $"Hiba t�rt�nt a t�rol�k bet�lt�sekor: {ex.Message}";
            }
        }

        public class StorageDto
        {
            public Guid Id { get; set; }
            public string Sname { get; set; }
            public string Sdescription { get; set; }
            public int Sarea { get; set; }
            public Guid? OwnerId { get; set; }
        }

        public class CreateStorageDto
        {
            public string Sname { get; set; }
            public string Sdescription { get; set; }
            public int Sarea { get; set; }
            public Guid? OwnerId { get; set; }
        }
    }
}
