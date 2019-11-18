using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetApp1.Models
{
    // Класс книги
    public class Book
    {
        public int Id;
        public string Name;
        public string Author;
        public string Publisher;
        public Category Category;

        // Проверка корректности данных
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
            if (String.IsNullOrWhiteSpace(Publisher))
            {
                flag = false;
            }
            else if (Publisher.Contains("Default"))
            {
                flag = false;
            }
            if (Category == null)
            {
                flag = false;
            }
            return flag;
        }

        public override string ToString()
        {
            return $"{Id}: {Name}, {Author}, {Publisher}, {Category.Name}";
        }
    }
}
