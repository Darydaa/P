using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using P;

namespace Find
{
   static class Search
    {
        public static string API_key = "fa44fe25198fecb879ad580ed2a067b5";
         static  async public Task<Rezalt[]> GetResalt(string type,string ask)
        {
            using (var http = new HttpClient())
            {
                HttpResponseMessage result = await http.GetAsync(new Uri($"https://api.themoviedb.org/3/search/{type}?api_key={API_key}&query={ask}"));
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsAsync<Rezalt[]>();
            }

        }
    }
}
