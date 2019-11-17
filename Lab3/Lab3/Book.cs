using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public enum Genres { Poetry, Classic, Novel, Modern, Science, Fantasy, Story }

    public class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publicher { get; set; }
        public Genres Genre { get; set; }
        public int Price { get; set; }

        public Book()
        {
            Name = "DefaultName";
            Author = "DefaultAuthor";
            Publicher = "DefaultPublisher";
            Price = 0;
        }

        public bool IsCorrect()
        {
            bool flag = true;
            if (String.IsNullOrWhiteSpace(Name))
            {
                flag = false;
            }
            else if (Name.Contains("Default"))
            {
                flag = false;
            }
            if (String.IsNullOrWhiteSpace(Author))
            {
                flag = false;
            }
            else if (Author.Contains("Default"))
            {
                flag = false;
            }
            if (String.IsNullOrWhiteSpace(Publicher))
            {
                flag = false;
            }
            else if (Publicher.Contains("Default"))
            {
                flag = false;
            }
            if (Price < 0)
            {
                flag = false;
            }
            return flag;
        }

        public override string ToString()
        {
            string genre = "";
            switch (Genre)
            {
                case Genres.Classic:
                    genre = "Классика";
                    break;
                case Genres.Modern:
                    genre = "Современное";
                    break;
                case Genres.Science:
                    genre = "Наука";
                    break;
                case Genres.Story:
                    genre = "Рассказ";
                    break;
                case Genres.Fantasy:
                    genre = "Фэнтези";
                    break;
                case Genres.Novel:
                    genre = "Роман";
                    break;
                case Genres.Poetry:
                    genre = "Поэззия";
                    break;
            }
            return Name + "&" + Author + "&" + Publicher + "&" + genre + "&" + Price;
        }
    }
}
