using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
using AwesomeLibrary;
using Ninject;

namespace P
{

   public class DIExperiment
    {
        ILogger log;
        public DIExperiment(ILogger log)
        {
            this.log = log;
        }
        public void UseLogger(Object obj)
        {
            log.Log(obj.ToString(), Messages.WARNING);
        }
    }
    public class Program
    {
        static void ConsoleCase()
        {
            string type = "";
            Console.WriteLine("Введите название:");
            string ans = Console.ReadLine();
            Console.WriteLine("Введите тип :\n 1.Фильм\n 2.Сериал");
            try
            {
                int typ = int.Parse(Console.ReadLine());
                switch (typ)
                {
                    case 1:
                        type = "movie";
                        break;
                    case 2:
                        type = "tv";
                        break;
                    default:
                        Console.WriteLine("Введено неверное значение!");
                        break;
                }
            }
            catch (Exception e) {
                IKernel kernel = new StandardKernel();
                ModuleLoader.Load(kernel, LoggerType.Console);
                DIExperiment dI = kernel.Get<DIExperiment>();
                dI.UseLogger("Неверное значение!");
            }
            ShowRezult(type, ans, Find.Search.Show,(e) => Console.WriteLine(e.Message));
            Thread.Sleep(2000);
            Console.ReadLine();
        }
        
     public   static async Task<string> ShowRezult(string type, string question,Find.Sh sh,Action<Exception> a)
        {
            string content="";
            try
            {
                question.Replace(' ', '+');
                
                string url = $"https://api.themoviedb.org/3/search/{type}?api_key={Find.Search.API_key}&query={question}";
                if ((content=await Find.Cache.ReturnCash(question, type,sh))==null)
                {
                    if (type == "movie")
                    {
                        Find.AnswerMovie rez = await Find.Search.GetRezaltAsync<Find.AnswerMovie>(url);
                        Find.Genres genres = await Find.Search.GetRezaltAsync<Find.Genres>($"https://api.themoviedb.org/3/genre/movie/list?api_key={Find.Search.API_key}&language=en-US");
                       content= sh(rez, null, genres);
                        Find.Cache cache = new Find.Cache(question, genres, rez);
                        cache.CreateCache();
                    }
                    else
                    {
                        Find.AnswerTV reza = await Find.Search.GetRezaltAsync<Find.AnswerTV>(url);
                        Find.Genres genress = await Find.Search.GetRezaltAsync<Find.Genres>($"https://api.themoviedb.org/3/genre/tv/list?api_key={Find.Search.API_key}&language=en-US");
                        content= sh(null, reza, genress);
                        Find.Cache cache = new Find.Cache(question, genress, reza);
                        cache.CreateCache();
                    }
                }
                
            }
            catch (Exception e) { a(e);
                IKernel kernel = new StandardKernel();
                ModuleLoader.Load(kernel, LoggerType.File);
                DIExperiment dI = kernel.Get<DIExperiment>();
                dI.UseLogger(e.Message);
            }
            return content;
           
    }

   
        [STAThread]
        static void Main(string[] args)
        { 

            if (args.Length > 0 && args[0].Equals("/console", StringComparison.OrdinalIgnoreCase)
            )
            {
                console.Kernel32.AllocConsole();
                ConsoleCase();
                console.Kernel32.FreeConsole();
                return;
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }



    } }

    