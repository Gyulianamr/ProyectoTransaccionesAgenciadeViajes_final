using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TransaccionAgenciadeViajesDevExtreme.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace TransaccionAgenciadeViajesDevExtreme.Controllers {
    public class SampleDataController : ApiController {

        private static readonly HttpClient client = new HttpClient();
        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44304/api/Cliente";
            var respuestaJson = await GetAsync(apiUrl);
            //System.Diagnostics.Debug.WriteLine(respuestaJson); imprimir info
            List<Cliente> listacliente = JsonConvert.DeserializeObject<List<Cliente>>(respuestaJson);
            return Request.CreateResponse(DataSourceLoader.Load(listacliente, loadOptions));
        }

        public static async Task<string> GetAsync(string uri)
        {
            try
            {

                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(handler))
                {
                    var response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }

                //var response = await client.GetAsync(uri);

            }
            catch (Exception e)
            {
                var m = e.Message;
                return null;
            }

        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {

            var values = form.Get("values");

            var httpContent = new StringContent(values, System.Text.Encoding.UTF8, "application/json");

            var url = "https://localhost:44304/api/Cliente";
            var response = await client.PostAsync(url, httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            return Request.CreateResponse(HttpStatusCode.Created);
        }

    }
}