using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string type="";
                Console.WriteLine("Введите название:");
                string ans=Console.ReadLine();
                Console.WriteLine("Введите тип :\n 1.Фильм\n 2.Сериал");
                int typ= int.Parse(Console.ReadLine());
                switch (typ)
                {
                    case 1:
                        type = "movie";
                        break;
                    case 2:
                        type = "serial";
                        break;
                    default:
                        Console.WriteLine("Введено неверное значение!");
                        break;

                }
                ans.Replace(' ', '+');
                Task<Rezalt[]> f =null;
                 f=Find.Search.GetResalt(type, ans);
                foreach(var i in f.Result)
                {
                    Console.WriteLine($"Название:{i.title}");
                    Console.WriteLine($"Популярность:{i.popularity}");
                    Console.WriteLine($"Рейтинг:{i.vote_average}");
                    Console.WriteLine($"Описание:{i.overview}");
                    Console.WriteLine($"Жанр:{i.genre_ids}");
                    Console.WriteLine($"Дата реализации:{i.release_date}");
                }
            }
                
                    
        }
    }
}
