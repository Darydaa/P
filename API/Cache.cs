using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Drawing;

namespace Find
{
    public delegate string Sh(Find.AnswerMovie mo, Find.AnswerTV tV, Find.Genres genres);

    public class Cache
    {
        public Cache(string req,Genres gen,object result)
        {
            request = req;
            genres = gen;
            
            if(result is AnswerMovie answer) {
                movie = answer;
                tv = null;
             
            }
            if (result is AnswerTV ans)
            {
                tv = ans;
                movie = null;
            }
            data = DateTime.Now;
        }
       
        public string request;
        public AnswerMovie movie;
        public AnswerTV tv;
        public Genres genres;
        public DateTime data;
        
        public  void CreateCache()
        {
            
            List<Cache> caches = new List<Cache>();
            var thread = new Thread(start: () =>
            {
                if (new FileInfo("Cache.txt").Exists)
                {
                    using (var file = File.Open("Cache.txt", FileMode.Open))
                    {
                        using (StreamReader sw = new StreamReader(file))
                        {
                            caches = JsonConvert.DeserializeObject<List<Cache>>(sw.ReadToEnd());
                        }
                    }
                    DeleteCash(caches);
                      
                    caches.Insert(0,this);
                    using (var file = File.Open("Cache.txt", FileMode.Create))
                    {
                        using (StreamWriter sw = new StreamWriter(file))
                        {
                            sw.Write(JsonConvert.SerializeObject(caches));
                        }
                    }
                }
                else
                {
                    using (var file = File.Open("Cache.txt", FileMode.Create))
                    {
                        using (StreamWriter sw = new StreamWriter(file))
                        {
                            sw.Write(JsonConvert.SerializeObject(caches));
                        }
                    }
                }

            });
            thread.Start();
        }
        public static Task<string>   ReturnCash(string ask,string type,Sh sh)
        {
            Task<string> task = new Task<string>(() =>
              {
                  FileInfo info = new FileInfo("Cache.txt");
                  if (!info.Exists)
                  {
                      List<Cache> caches = new List<Cache>();
                      using (var file = File.Open("Cache.txt", FileMode.Create))
                      {
                          using (StreamWriter sw = new StreamWriter(file))
                          {
                              sw.Write(JsonConvert.SerializeObject(caches));
                          }
                      }
                  }
                  
                  List<Cache> des;
                  using (var file = File.Open("Cache.txt", FileMode.Open))
                  {
                      using (StreamReader sw = new StreamReader(file))
                      {
                           des = JsonConvert.DeserializeObject<List<Cache>>(sw.ReadToEnd());
                      }
                  }
                  DeleteCash(des);
                          var rez = des.FirstOrDefault(x => x.request == ask && ((type == "movie" && x.movie != null) || (type == "tv" && x.tv != null)));
                          if (rez != null)
                          {
                      return sh(rez.movie, rez.tv, rez.genres)??"";
                             
                          }

                  return null;
              });
            task.Start();
                return task;
           
        }

        public static void DeleteCash(List<Cache> caches)
        {
           
            DateTime dayAgo = DateTime.Now;
            dayAgo=dayAgo.AddDays(-1);
            for (int i = 0; i < caches.Count; i++)
            {
                if (caches[i].data <= dayAgo)
                {
                    caches.Remove(caches[i]);
                }
              
            }
            using (var file = File.Open("Cache.txt", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(JsonConvert.SerializeObject(caches));
                }
            }
        }

    }
}
