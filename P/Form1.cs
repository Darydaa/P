using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P
{
    public partial class Form1 : Form
    {
        bool ch = false;
        public Form1()
        {
            InitializeComponent();
            textBox1.Click += TextBox1_Click;
            button1.Click += Button1_Click;
            textBox1.KeyUp += Form1_KeyUp;
            KeyUp += Form1_KeyUp;
            radioButton1.Checked = true;
            panel1.KeyUp += Form1_KeyUp;
            button1.KeyUp += Form1_KeyUp;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                Button1_Click(null, null);
            }

        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            string content = "";
         
                if (radioButton1.Checked)
                {
                   content= await Program.ShowRezult("movie", textBox1.Text, Display,WriteExeption);
                }
                else if(radioButton2.Checked)
                {
                    content= await Program.ShowRezult("tv", textBox1.Text, Display, WriteExeption);

                }
            webBrowser1.DocumentText = content;



        }
        void WriteExeption(Exception exc) {
            webBrowser1.DocumentText = $"<html><body><h1 style=\"color:red;\">ERROR:{exc.Message}</h1></body></html>";
        }
       
        string Display(Find.AnswerMovie x, Find.AnswerTV tV, Find.Genres z)
        {
            string content= "<html><body>";
            if (x != null)
                {
                    if (x.total_results != 0)

                        for (int i = 0; i < x.total_results && i < 20; i++)
                        {
                       
                            content += $"<div><img width=200 height=300 style=\"display:inline-block;\" src=\"https://image.tmdb.org/t/p/w300_and_h450_bestv2{x.results[i].poster_path}\">" +
                               $"<p>Название:{x.results[i].original_title}</p>" +
                                $"<p>Популярность:{x.results[i].popularity}</p>" +
                                $"<p>Рейтинг:{x.results[i].vote_average}</p>" +
                                $"<p>Описание:{x.results[i].overview}</p>" +
                                $"<p>Жанp:";
                 
                        for (int t = 0; t < x.results[i].genre_ids.Length; t++)
                            {
                            content += z.genres.Find(o => o.id == x.results[i].genre_ids[t]).name; 
                                if (x.results[i].genre_ids.Length - 1 != t)
                                {
                               content += ",";
                                }
                            }

                       content += $"</p><p>Дата реализации:{x.results[i].release_date}</p></div><br><hr></ body ></ html > ";

                    }
                else 
                    content= ("<html><body><h1>Ничего не найдено!</h1></body><html>");
            
        }
              if (tV != null)
                {
                    if (tV.total_results != 0)
                        for (int i = 0; i < tV.total_results && i < 20; i++)
                        {
                        content += $"<div><img width=200 height=300 style=\"display:inline-block;\" src=\"https://image.tmdb.org/t/p/w300_and_h450_bestv2{tV.results[i].poster_path}\">" +
                                $"<p>Название:{tV.results[i].original_name}</p>" +
                                $"<p>Популярность:{tV.results[i].popularity}</p>" +
                                $"<p>Рейтинг:{tV.results[i].vote_average}</p>" +
                                $"<p>Описание:{tV.results[i].overview}</p>" +
                                $"<p>Жанр: ";
                           
                            for (int t = 0; t < tV.results[i].genre_ids.Length; t++)
                            {
                               content += z.genres.Find(o => o.id == tV.results[i].genre_ids[t]).name;
                                if (tV.results[i].genre_ids.Length - 1 != t)
                                {
                                content += ",";
                                }
                            }

                        content += $"</p><p>Дата реализации:{tV.results[i].first_air_date}</p></div><br><hr></ body ></ html > ";
                    }
                    else content = ("<html><body><h1>Ничего не найдено!</h1></body><html>");
                }

            return content;
   
        }
        private void TextBox1_Click(object sender, EventArgs e)
        {
            if (!ch)
            {
                textBox1.Text = "";
                ch = true;
                textBox1.ForeColor = Color.Black;
            }
           
        }

       
    }
}
