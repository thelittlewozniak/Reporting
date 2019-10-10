using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Reporting.services.GoogleMapsMatrixService
{
    class GoogleMapsMatrixService
    {
        private readonly string _endpoint;
        private readonly string _apiKey;
        private readonly string _units;
        private readonly string _mode;
        private readonly string _language;
        private readonly string _format;
        private readonly HttpClient _httpClient;
        public GoogleMapsMatrixService(string apiKey, string units = "metric", string mode = "driving", string language = "en", string format = "json")
        {
            _endpoint = "https://maps.googleapis.com/maps/api/distancematrix/";
            _apiKey = apiKey;
            _units = units;
            _mode = mode;
            _language = language;
            _format = format;
            _httpClient = new HttpClient();
        }
        public async Task<double> GetKilometers(string a, string b)
        {
            var url = new StringBuilder(_endpoint);
            url.Append(_format);
            url.Append("?origins=");
            url.Append(a);
            url.Append("&destinations=");
            url.Append(b);
            url.Append("&mode=");
            url.Append(_mode);
            url.Append("&units=");
            url.Append(_units);
            url.Append("&language=");
            url.Append(_language);
            url.Append("&key=");
            url.Append(_apiKey);
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url.ToString());
                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<MatrixResult>(responseBody);
                    return result.rows[0].elements[0].distance.value / 1000;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return 0;
            }
        }
    }

}
