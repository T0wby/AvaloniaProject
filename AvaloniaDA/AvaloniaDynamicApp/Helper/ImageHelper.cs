using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaDynamicApp.Helper
{
    public static class ImageHelper
    {
        public static Bitmap LoadFromResource(string path)
        {
            Uri uri;

            if (!path.StartsWith("avares://"))
            {
                var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
                uri = new Uri($"avares://{assemblyName}/{path.TrimStart('/')}");
            }
            else
            {
                uri = new Uri(path);
            }

            return new Bitmap(AssetLoader.Open(uri));
        }

        public static async Task<Bitmap?> LoadFromWeb(string path)
        {
            Uri uri = new Uri(path);
            using var httpClient = new HttpClient();

            try
            {
                httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AvaloniaDynamicApp", "0.1"));
                var data = await httpClient.GetByteArrayAsync(uri);
                return new Bitmap(new MemoryStream(data));
            }
            catch (HttpRequestException ex)
            {
                await Console.Out.WriteLineAsync($"An error occurd while you were trying to download the Image: '{uri}' : {ex.Message}");
                return null;
            }
        }
    }
}
