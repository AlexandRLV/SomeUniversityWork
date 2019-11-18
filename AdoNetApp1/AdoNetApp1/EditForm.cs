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
    // Тип формы - Создать, Изменить
    public enum FormType { Create, Update }

    public partial class EditForm : Form
    {
        // Объект книги, с которым работаем
        private Book book;

        // Список до
        private Category[] categories;

        public EditForm(FormType type, Book book, Category[] categories)
        {
            // Сохраняем полученную книгу и список категорий
            this.book = book;
            this.categories = categories;
            InitializeComponent();

            // Записываем доступные категории в выпадающий список
            foreach (Category c in categories)
            {
                comboBox1.Items.Add(c.ToString());
            }
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            if (book == null)
            {
                this.book = new Book();
            }

            // Записываем данные из книги в соответствующие поля
            if (type == FormType.Update)
            {
                textBox1.Text = book.Name;
                textBox3.Text = book.Author;
                textBox2.Text = book.Publisher;
                comboBox1.SelectedItem = book.Category.ToString();
                button1.Text = "Применить";
                this.Text = "Изменить";
            }
            if (type == FormType.Create)
            {
                comboBox1.SelectedItem = comboBox1.Items[0];
                button1.Text = "Добавить";
                this.Text = "Добавить";
            }
        }

        // Сохранение изменений
        private void button1_Click(object sender, EventArgs e)
        {
            // Создаём флаг для проверки корректности всех полей
            bool flag = true;

            // Проверяем название
            if (String.IsNullOrWhiteSpace(textBox1.Text))
            {
                label6.Text = "Некорректное название";
                flag = false;
            }
            else if (textBox1.Text.Length > 50)
            {
                label6.Text = "Максимум 50 символов";
                flag = false;
            }
            else
            {
                label6.Text = "";
            }

            // Проверяем имя автора
            if (String.IsNullOrWhiteSpace(textBox2.Text))
            {
                label7.Text = "Некорректное имя автора.";
                flag = false;
            }
            else if (textBox2.Text.Length > 50)
            {
                label7.Text = "Максимум 50 символов";
                flag = false;
            }
            else
            {
                label7.Text = "";
            }

            // Проверяем имя издателя
            if (String.IsNullOrWhiteSpace(textBox3.Text))
            {
                label8.Text = "Некорректное имя издателя.";
                flag = false;
            }
            else if (textBox3.Text.Length > 30)
            {
                label8.Text = "Максимум 30 символов";
                flag = false;
            }
            else
            {
                label8.Text = "";
            }

            // Если все проверки пройдёны, сохраняем изменения
            if (flag)
            {
                book.Name = textBox1.Text;
                book.Author = textBox3.Text;
                book.Publisher = textBox2.Text;
                book.Category = categories.FirstOrDefault(x => x.Id == int.Parse(comboBox1.SelectedItem.ToString().Split(':')[0]));
                this.Close();
            }
        }
    }
}
