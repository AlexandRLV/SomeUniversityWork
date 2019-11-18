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
    public partial class DeleteBookForm : Form
    {
        // Книга, которую удаляем
        public Book b;

        public DeleteBookForm(Book book)
        {
            // Сохраняем книгу
            b = book;
            InitializeComponent();

            // Записываем данные в поля
            textBox1.Text = book.Name;
            textBox2.Text = book.Author;
            textBox3.Text = book.Publisher;
            textBox4.Text = book.Category.Name;
            this.Text = "Удалить";
        }

        // Отмена удаления
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Удаление
        private void button1_Click(object sender, EventArgs e)
        {
            b.Name = "&&&";
            Close();
        }
    }
}
