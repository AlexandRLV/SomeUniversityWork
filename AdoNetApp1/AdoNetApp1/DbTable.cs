using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AdoNetApp1.Models;
using System.Data;

namespace AdoNetApp1
{
    // Промежуточный класс для работы с БД
    class DbTable
    {
        // Строка подключения
        private string connectionString;
        
        // Списки книг и категорий
        public List<Book> Books;
        public List<Category> Categories;

        // Пробуем подключиться к базе данных, инициализируем списки
        public DbTable(string connectionString)
        {
            this.connectionString = connectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            Categories = new List<Category>();
            Books = new List<Book>();
        }

        // Получение информации из БД и сохранение её в списках
        public void Read()
        {
            // Пробуем подключиться
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            Categories = new List<Category>();
            Books = new List<Book>();

            // Создаём запрос для получения категорий
            string commandString = "SELECT * FROM Categories";
            using (SqlCommand command = new SqlCommand(commandString, connection))
            {
                // Выполняем запрос, читаем результат и сохраняем его в список
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Category c = new Category
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        Categories.Add(c);
                    }
                }
                reader.Close();
            }

            // Создаём запрос для получения книг
            commandString = "SELECT * FROM Books";
            using (SqlCommand command = new SqlCommand(commandString, connection))
            {
                // Выполняем запрос, читаем результат и сохраняем его в список
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Book b = new Book
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Author = reader.GetString(2),
                            Publisher = reader.GetString(3),
                            Category = Categories.Find(x => x.Id == reader.GetInt32(4))
                        };
                        Books.Add(b);
                    }
                }
                reader.Close();
            }
        }

        // Получаем таблицу категорий
        public DataSet GetCategoriesDataSet()
        {
            // Пробуем подключиться
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            // Создаём запрос и записываем результаты в таблицу
            string commandString = "SELECT * FROM Categories";
            SqlDataAdapter adapter = new SqlDataAdapter(commandString, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        // Получаем таблицу книг
        public DataSet GetBooksDataSet()
        {
            // Пробуем подключиться
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            // СОздаём запрос и записываем результаты в таблицу
            string commandString = "SELECT * FROM Books";
            SqlDataAdapter adapter = new SqlDataAdapter(commandString, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        // Получаем таблицу книг с фильтром
        public DataSet GetBooksDataSetFiltered(int param, string value)
        {
            // Пробуем подключиться
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            // Создаём запрос, в зависимости от выбранного столбца для фильтрации
            string commandString = "";
            switch (param)
            {
                case 1:
                    commandString = $"SELECT * FROM Books WHERE Name LIKE '%{value}%'";
                    break;
                case 2:
                    commandString = $"SELECT * FROM Books WHERE Author LIKE '%{value}%'";
                    break;
                case 3:
                    commandString = $"SELECT * FROM Books WHERE Publisher LIKE '%{value}%'";
                    break;
            }

            // Выполняем запрос и записываем результаты в таблицу
            SqlDataAdapter adapter = new SqlDataAdapter(commandString, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        // Получаем объект категории по имени
        public Category GetCategoryByName(string name)
        {
            // Пробуем подключиться
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            // Создаём запрос
            string commandString = $"SELECT * FROM Categories WHERE Name = '{name}'";
            SqlCommand command = new SqlCommand(commandString, connection);

            // Выполняем запрос и читаем категорию
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Category c = new Category
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
            return c;
        }

        // Добавление новой книги
        public void AddNewBook(Book b)
        {
            // Пробуем подключиться
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            // Создаём запрос со значениями книги и выполняем его
            string commandString = $"INSERT INTO Books (Name, Author, Publisher, Category) VALUES ('{b.Name}', '{b.Author}', '{b.Publisher}', '{b.Category.Id}')";
            SqlCommand command = new SqlCommand(commandString, connection);
            int lines = command.ExecuteNonQuery();

            // Если книга добавилась, обновляем список
            if (lines == 1)
            {
                Read();
            }
        }

        // Добавление новой категории
        public void AddNewCategory(Category c)
        {
            // Пробуем подключиться
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            // Создаём запрос со значениями новой категории и выполняем его
            string commandString = $"INSERT INTO Categories (Name) VALUES ('{c.Name}')";
            SqlCommand command = new SqlCommand(commandString, connection);
            int lines =  command.ExecuteNonQuery();

            // Если категория добавилась, обновляем список
            if (lines == 1)
            {
                Read();
            }
        }

        // Изменение книги
        public void ChangeBook(Book b)
        {
            // Пробуем подключиться
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            // Создаём запрос изменения книги по Id, выполняем и обновляем список
            string commandString = $"UPDATE Books SET Name = '{b.Name}', Author = '{b.Author}', Publisher = '{b.Publisher}', Category = '{b.Category.Id}' WHERE Id = {b.Id}";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.ExecuteNonQuery();
            Read();
        }

        // Изменение категории
        public void ChangeCategory(Category c)
        {
            // Пробуем подключиться
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            // Создаём запрос изменения категории по Id, выполняем и обновляем список
            string commandString = $"UPDATE Categories SET Name = '{c.Name}' WHERE Id = {c.Id}";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.ExecuteNonQuery();
            Read();
        }

        // Удаление категории
        public void DeleteCategory(Category c)
        {
            // Пробуем подключиться
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            // Создаём запрос - вызов хранимой процедуры, которая всем книгам удаляемой категории назначит категорию по умолчанию
            // Если таковой нет, создаст её
            // Выполняем запрос и обновляем список
            string commandString = $"EXECUTE DeleteCategory @CategoryId = {c.Id}";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.ExecuteNonQuery();
            Read();
        }

        // Удаление книги
        public void DeleteBook(Book b)
        {
            // Пробуем подключиться
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }

            // Создаём запрос об удалении книги по Id, выполняем и обновляем список
            string commandString = $"DELETE FROM Books WHERE Id = {b.Id}";
            SqlCommand command = new SqlCommand(commandString, connection);
            command.ExecuteNonQuery();
            Read();
        }
    }
}
