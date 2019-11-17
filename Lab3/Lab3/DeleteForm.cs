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
    public partial class DeleteForm : Form
    {
        private Book book;

        public DeleteForm(Book book)
        {
            this.book = book;
            InitializeComponent();
            textBox1.Text = book.Name;
            textBox2.Text = book.Author;
            textBox3.Text = book.Publicher;
            textBox4.Text = book.Genre.ToString();
            textBox5.Text = book.Price.ToString();
            this.Text = "Удалить";
        }

        // Да
        private void button1_Click(object sender, EventArgs e)
        {
            book.Name = "&&&";
            Close();
        }

        // Нет
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
