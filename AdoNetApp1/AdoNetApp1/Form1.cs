using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdoNetApp1.Models;

namespace AdoNetApp1
{
    // Основное окно
    public partial class Form1 : Form
    {
        // Таблица данных
        private DbTable table;

        public Form1()
        {
            InitializeComponent();
            label3.Text = "";

            // Создаём строку подключения и таблицу, которая будет с ней работать
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\userDb.mdf;Integrated Security=True";
            table = new DbTable(connectionString);

            // Добавляем в таблицу значения по умолчанию
            Category c = new Category
            {
                Id = 1,
                Name = "Default"
            };
            Book b = new Book
            {
                Name = "DefaultBook1",
                Author = "Me",
                Publisher = "Program",
                Category = c
            };
            table.AddNewCategory(c);
            table.AddNewBook(b);
            table.Read();

            // Обновляем список
            RefreshList();

            // Запрещаем редактирование таблицы
            dataGridView1.ReadOnly = true;
            dataGridView2.ReadOnly = true;

            // Добавляем элементы в список фильтров
            comboBox1.Items.AddRange(new string[] { "Name", "Author", "Publisher" });
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        // Обновление списка
        private void RefreshList()
        {
            dataGridView1.DataSource = table.GetBooksDataSet().Tables[0];
            dataGridView2.DataSource = table.GetCategoriesDataSet().Tables[0];
        }

        // Обновление списка книг с фильтром
        private void RefreshList(int param, string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                dataGridView1.DataSource = table.GetBooksDataSet().Tables[0];
            else
                dataGridView1.DataSource = table.GetBooksDataSetFiltered(param, value).Tables[0];
        }

        // Добавление книги
        private void button1_Click(object sender, EventArgs e)
        {
            // Создаём пустую книгу
            Book b = new Book();

            // Создаём форму редактирования, передаём ей созданную пустую книгу и список категорий
            EditForm form = new EditForm(FormType.Create, b, table.Categories.ToArray());
            form.ShowDialog();

            // Если значения книги указаны корректно, добавляем её в таблицу и обновляем список
            if (b.IsCorrect())
            {
                table.AddNewBook(b);
                RefreshList();
            }
        }

        // Добавление категории
        private void button2_Click(object sender, EventArgs e)
        {
            // Создаём форму редактирования с пустой категорией
            Category c = new Category();
            EditCategoryForm form = new EditCategoryForm(FormType.Create, c);
            form.ShowDialog();

            // Если название корректно, добавляем, обновляем
            if (!String.IsNullOrWhiteSpace(c.Name))
            {
                table.AddNewCategory(c);
                RefreshList();
            }
        }

        // Изменение книги
        private void button3_Click(object sender, EventArgs e)
        {
            // Проверяем наличие выбранных ячеек
            if (dataGridView1.SelectedCells.Count == 0)
            {
                label3.Text = "Выберите элемент.";
                label3.ForeColor = Color.Black;
            }
            else if (dataGridView1.SelectedCells.Count >= 1)
            {
                // Проверяем выбор нескольких строк
                int num = dataGridView1.SelectedCells[0].RowIndex;
                foreach (DataGridViewCell c in dataGridView1.SelectedCells)
                {
                    if (c.RowIndex != num)
                    {
                        num = -1;
                        break;
                    }
                }
                if (num == -1)
                {
                    label3.Text = "Выберите один элемент.";
                    label3.ForeColor = Color.Black;
                }
                else
                {
                    // Если выбрана одна строка, получаем книгу, которая в ней записана
                    int rowNum = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow row = dataGridView1.Rows[rowNum];
                    Book b = table.Books.Find(x => x.Id == (int)row.Cells[0].Value);
                    b.Category = table.Categories.Find(x => x.Id == (int)row.Cells[4].Value);

                    // Создаём форму редактирования и передаём ей найденную книгу
                    EditForm form2 = new EditForm(FormType.Update, b, table.Categories.ToArray());
                    form2.ShowDialog();

                    // Обновляем книгу и список
                    table.ChangeBook(b);
                    RefreshList();
                }
            }
        }

        // Изменение категории
        private void button4_Click(object sender, EventArgs e)
        {
            // Проверяем наличие выбранных ячеек
            if (dataGridView2.SelectedCells.Count == 0)
            {
                label3.Text = "Выберите элемент.";
                label3.ForeColor = Color.Black;
            }
            else if (dataGridView2.SelectedCells.Count >= 1)
            {
                // Проверяем выбор нескольких строк
                int num = dataGridView2.SelectedCells[0].RowIndex;
                foreach (DataGridViewCell c in dataGridView2.SelectedCells)
                {
                    if (c.RowIndex != num)
                    {
                        num = -1;
                        break;
                    }
                }
                if (num == -1)
                {
                    label3.Text = "Выберите один элемент.";
                    label3.ForeColor = Color.Black;
                }
                else
                {
                    // Если выбрана одна строка, получаем категорию, которая в ней записана
                    int rowNum = dataGridView2.SelectedCells[0].RowIndex;
                    DataGridViewRow row = dataGridView2.Rows[rowNum];
                    Category c = table.Categories.Find(x => x.Id == (int)row.Cells[0].Value);

                    // Создаём форму для редактирования
                    EditCategoryForm form2 = new EditCategoryForm(FormType.Update, c);
                    form2.ShowDialog();

                    // Обновляем категорию и список
                    table.ChangeCategory(c);
                    RefreshList();
                }
            }
        }

        // Удаление книги
        private void button5_Click(object sender, EventArgs e)
        {
            // Проверяем выбор одной строки и т.д.
            if (dataGridView1.SelectedCells.Count == 0)
            {
                label3.Text = "Выберите элемент.";
                label3.ForeColor = Color.Black;
            }
            else if (dataGridView1.SelectedCells.Count >= 1)
            {
                int num = dataGridView1.SelectedCells[0].RowIndex;
                foreach (DataGridViewCell c in dataGridView1.SelectedCells)
                {
                    if (c.RowIndex != num)
                    {
                        num = -1;
                        break;
                    }
                }
                if (num == -1)
                {
                    label3.Text = "Выберите один элемент.";
                    label3.ForeColor = Color.Black;
                }
                else
                {
                    // Получаем выбранную книгу
                    int rowNum = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow row = dataGridView1.Rows[rowNum];
                    Book b = table.Books.Find(x => x.Id == (int)row.Cells[0].Value);
                    b.Category = table.Categories.Find(x => x.Id == (int)row.Cells[4].Value);

                    // Создаём форму с подтверждением удаления
                    DeleteBookForm form = new DeleteBookForm(b);
                    form.ShowDialog();

                    // Если удаление подтверждено, удаляем, обновляем
                    if (b.Name == "&&&")
                    {
                        table.DeleteBook(b);
                        RefreshList();
                    }
                }
            }
        }

        // Удаление категории
        private void button6_Click(object sender, EventArgs e)
        {
            // Проверяем выбор одной строки и т.д.
            if (dataGridView2.SelectedCells.Count == 0)
            {
                label3.Text = "Выберите элемент.";
                label3.ForeColor = Color.Black;
            }
            else if (dataGridView2.SelectedCells.Count >= 1)
            {
                int num = dataGridView2.SelectedCells[0].RowIndex;
                foreach (DataGridViewCell c in dataGridView2.SelectedCells)
                {
                    if (c.RowIndex != num)
                    {
                        num = -1;
                        break;
                    }
                }
                if (num == -1)
                {
                    label1.Text = "Выберите один элемент.";
                    label1.ForeColor = Color.Black;
                }
                else
                {
                    // Получаем выбранную категорию
                    int rowNum = dataGridView2.SelectedCells[0].RowIndex;
                    DataGridViewRow row = dataGridView2.Rows[rowNum];
                    Category c = table.Categories.Find(x => x.Id == (int)row.Cells[0].Value);

                    // Создаём форму с подтверждением удаления
                    DeleteCategoryForm form = new DeleteCategoryForm(c);
                    form.ShowDialog();

                    // Если удаление подтверждено, удаляем, обновляем
                    if (c.Name == "&&&")
                    {
                        table.DeleteCategory(c);
                        RefreshList();
                    }
                }
            }
        }

        // Фильтр книг
        private void button7_Click(object sender, EventArgs e)
        {
            // Обновляем список в зависимости от выбранного варианта фильтрации
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Name":
                    RefreshList(1, textBox1.Text);
                    break;
                case "Author":
                    RefreshList(2, textBox1.Text);
                    break;
                case "Publisher":
                    RefreshList(3, textBox1.Text);
                    break;
            }
        }
    }
}
