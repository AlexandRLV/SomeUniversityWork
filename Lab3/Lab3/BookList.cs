using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Lab3
{
    // Класс для чтения и записи информации в файл
    public class BookList
    {
        public List<Book> Books { get; set; }
        public string Group { get; set; }

        public BookList(string group)
        {
            Books = new List<Book>();
            Group = group;
        }

        // Чтение информации из файла
        public void Read()
        {
            Books = new List<Book>();
            using (StreamReader sr = new StreamReader(Group + "/Books.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string[] s = sr.ReadLine().Split('&');
                    Book b = new Book
                    {
                        Name = s[0],
                        Author = s[1],
                        Publicher = s[2],
                        Price = int.Parse(s[4])
                    };
                    switch ((string)s[3])
                    {
                        case "Поэззия":
                            b.Genre = Genres.Poetry;
                            break;
                        case "Классика":
                            b.Genre = Genres.Classic;
                            break;
                        case "Современное":
                            b.Genre = Genres.Modern;
                            break;
                        case "Роман":
                            b.Genre = Genres.Novel;
                            break;
                        case "Фэнтези":
                            b.Genre = Genres.Fantasy;
                            break;
                        case "Наука":
                            b.Genre = Genres.Science;
                            break;
                        case "Рассказ":
                            b.Genre = Genres.Story;
                            break;
                    }
                    Books.Add(b);
                }
            }
        }

        // Запись информации в файл
        public void Write()
        {
            if (Books != null)
            {
                using (StreamWriter sw = new StreamWriter(Group + "/Books.txt", false))
                {
                    foreach (var b in Books)
                    {
                        sw.WriteLine(b.ToString());
                    }
                }
            }            
        }

        // Добавление книги, возвращает, успешно или нет
        public bool AddBook(Book b)
        {
            if (b == null)
            {
                return false;
            }
            if (Books.Find(x=>x.Name==b.Name) != null)
            {
                return false;
            }
            else
            {
                Books.Add(b);
                return true;
            }
        }

        // Удаление книги
        public bool DeleteBook(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return false;
            }
            Book b = Books.Find(x => x.Name == name);
            if (b == null)
            {
                return false;
            }
            else
            {
                Books.Remove(b);
                return true;
            }
        }
    }
}
