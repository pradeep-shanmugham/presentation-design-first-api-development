using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace user_interface.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public HttpClient Client { get; }

        public Settings Settings { get; }

        public IEnumerable<Car> Cars { get; set; } = new List<Car>();

        public IndexModel(ILogger<IndexModel> logger, IOptions<Settings> settings, IHttpClientFactory clientFactory)
        {
            _logger = logger;

            Client = clientFactory.CreateClient();
            Settings = settings.Value;
        }

        public async Task OnGet(bool onlySales = false, bool onlyInStock=false)
        {
            try
            {
                Client.DefaultRequestHeaders.Add("Key", Settings.Key);
                var response = await Client.GetAsync(Settings.BaseUri + "/cars");

                if (response.IsSuccessStatusCode)
                {
                    Cars = await JsonSerializer.DeserializeAsync
                                    <IEnumerable<Car>>
                                    (await response.Content.ReadAsStreamAsync());

                    if(onlySales)
                    {
                        Cars = Cars.Where(x => x.isOnSale);
                    }
                    if (onlyInStock)
                    {
                        Cars = Cars.Where(x => x.isInStock);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
            }
        }
    }
}

namespace user_interface
{
    public class Settings
    {
        public string BaseUri { get; set; }
        public string Key { get; set; }
    }

    public class Car
    {
        public int id { get; set; }
        public string vinNo { get; set; }
        public Type type { get; set; }
        public decimal regularPrice { get; set; }
        public bool isInStock { get; set; }
        public bool isOnSale { get; set; }
        public decimal saleDiscountPercentage { get; set; }

        public decimal savings =>
            !isOnSale ? 0 : (regularPrice) - (regularPrice * saleDiscountPercentage / 100);

    }

    public class Type
    {
        public int id { get; set; }
        public string make { get; set; }
        public string model { get; set; }
    }


}