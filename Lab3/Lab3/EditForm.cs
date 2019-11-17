using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public enum FormType { Create, Update }

    public partial class EditForm : Form
    {
        private Book book;

        public EditForm(FormType type, Book book)
        {
            this.book = book;
            InitializeComponent();
            comboBox1.Items.Add("Поэззия");
            comboBox1.Items.Add("Классика");
            comboBox1.Items.Add("Роман");
            comboBox1.Items.Add("Современное");
            comboBox1.Items.Add("Наука");
            comboBox1.Items.Add("Фэнтези");
            comboBox1.Items.Add("Рассказ");
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label10.Text = "";
            if (book == null)
            {
                this.book = new Book();
            }
            if (type == FormType.Update)
            {
                textBox1.Text = book.Name;
                textBox2.Text = book.Author;
                textBox3.Text = book.Publicher;
                switch (book.Genre)
                {
                    case Genres.Classic:
                        comboBox1.SelectedItem = "Классика";
                        break;
                    case Genres.Fantasy:
                        comboBox1.SelectedItem = "Фэнтези";
                        break;
                    case Genres.Modern:
                        comboBox1.SelectedItem = "Современное";
                        break;
                    case Genres.Novel:
                        comboBox1.SelectedItem = "Роман";
                        break;
                    case Genres.Poetry:
                        comboBox1.SelectedItem = "Поэззия";
                        break;
                    case Genres.Science:
                        comboBox1.SelectedItem = "Наука";
                        break;
                    case Genres.Story:
                        comboBox1.SelectedItem = "Рассказ";
                        break;
                    default:
                        comboBox1.SelectedItem = "Рассказ";
                        break;
                }
                textBox4.Text = book.Price.ToString();
                button1.Text = "Применить.";
                this.Text = "Изменить";
            }
            if (type == FormType.Create)
            {
                button1.Text = "Добавить.";
                this.Text = "Добавить";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = true;

            if (String.IsNullOrWhiteSpace(textBox1.Text))
            {
                label5.Text = "Некорректное название.";
                flag = false;
            }
            else if (textBox1.Text.Length > 50)
            {
                label5.Text = "Максимум 50 символов";
                flag = false;
            }
            else
            {
                label5.Text = "";
            }

            if (String.IsNullOrWhiteSpace(textBox2.Text))
            {
                label6.Text = "Некорректное имя автора.";
                flag = false;
            }
            else if (textBox2.Text.Length > 50)
            {
                label6.Text = "Максимум 50 символов";
                flag = false;
            }
            else
            {
                label6.Text = "";
            }

            if (String.IsNullOrWhiteSpace(textBox3.Text))
            {
                label7.Text = "Некорректное имя издателя.";
                flag = false;
            }
            else if (textBox3.Text.Length > 30)
            {
                label7.Text = "Максимум 30 символов";
                flag = false;
            }
            else
            {
                label7.Text = "";
            }

            if (String.IsNullOrWhiteSpace(textBox4.Text))
            {
                label10.Text = "Некорректное название жанра.";
                flag = false;
            }
            else if (textBox4.Text.Length > 15)
            {
                label10.Text = "Максимум 15 символов";
                flag = false;
            }
            else
            {
                try
                {
                    int.Parse(textBox4.Text);
                    label10.Text = "";
                }
                catch
                {
                    flag = false;
                    label10.Text = "Некорректное значение.";
                }
            }

            if (flag)
            {
                book.Name = textBox1.Text;
                book.Author = textBox2.Text;
                book.Publicher = textBox3.Text;
                book.Genre = (Genres)comboBox1.SelectedIndex;
                book.Price = int.Parse(textBox4.Text);
                this.Close();
            }
        }
    }
}
