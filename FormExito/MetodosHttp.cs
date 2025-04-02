using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FormExito
{
    //Metodos para realizar peticiones HTTP

    public class MetodosHttp
    {
        // Metodo para Get
        public async Task<T?> GetObject<T>(string link)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(link);

            var json = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(json);
            else
                return default;
        }

        // Metodo para Post
        public async Task<T?> PostObject<T>(string link, T obj)
        {
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(link, obj);
            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(json);
            else
            {
                MessageBox.Show($"Error: {json}");
                return default;
            }
        }

        //Metodo para Put (update)
        public async Task<bool> PutObject<T>(string link, T obj)
        {
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(link, obj);
            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return true;
            else
            {
                MessageBox.Show($"Error: {json}");
                return false;
            }
        }

        public async Task<bool> DeleteObject(string link)
        {
            var client = new HttpClient();
            var response = await client.DeleteAsync(link);
            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return true;
            else
            {
                MessageBox.Show($"Error: {json}");
                return false;
            }
        }
    }
}
