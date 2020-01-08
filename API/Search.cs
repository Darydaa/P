using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Find
{
    public  class Search
    {
        public static string API_key = "fa44fe25198fecb879ad580ed2a067b5";
        static HttpClient client = new HttpClient();
        public static async Task<T> GetRezaltAsync<T>(string path) where T : class
        {
            T rezalt = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                rezalt = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            return rezalt;
        }
        public static string Show(AnswerMovie mo, AnswerTV tV, Genres genres)
        {
            if (mo != null)
            {
                if (mo.total_results != 0)
                    for (int i = 0; i < mo.total_results && i < 20; i++)
                    {
                        Console.WriteLine($"Название:{mo.results[i].original_title}");
                        Console.WriteLine($"Популярность:{mo.results[i].popularity}");
                        Console.WriteLine($"Рейтинг:{mo.results[i].vote_average}");
                        Console.WriteLine($"Описание:{mo.results[i].overview}");
                        Console.Write($"Жанр:");
                        for (int t = 0; t < mo.results[i].genre_ids.Length; t++)
                        {
                            Console.Write(genres.genres.Find(x => x.id == mo.results[i].genre_ids[t]).name);
                            if (mo.results[i].genre_ids.Length - 1 != t)
                            {
                                Console.Write(",");
                            }
                        }

                        Console.WriteLine($"\nДата реализации:{mo.results[i].release_date}");
                        Console.WriteLine("\n" + new string('-', Console.BufferWidth));
                    }
                else Console.WriteLine("Ничего не найдено!");
            }
            else if (tV != null)
            {
                if (tV.total_results != 0)
                    for (int i = 0; i < tV.total_results && i < 20; i++)
                    {
                        Console.WriteLine($"Название:{tV.results[i].original_name}");
                        Console.WriteLine($"Популярность:{tV.results[i].popularity}");
                        Console.WriteLine($"Рейтинг:{tV.results[i].vote_average}");
                        Console.WriteLine($"Описание:{tV.results[i].overview}");
                        Console.Write($"Жанр:");
                        for (int t = 0; t < tV.results[i].genre_ids.Length; t++)
                        {
                            Console.Write(genres.genres.Find(x => x.id == tV.results[i].genre_ids[t]).name);
                            if (tV.results[i].genre_ids.Length - 1 != t)
                            {
                                Console.Write(",");
                            }
                        }

                        Console.WriteLine($"\nДата реализации:{tV.results[i].first_air_date}");
                        Console.WriteLine("\n" + new string('-', Console.BufferWidth));
                    }
                else Console.WriteLine("Ничего не найдено!");
            }
            return null;
        }

    }

}
