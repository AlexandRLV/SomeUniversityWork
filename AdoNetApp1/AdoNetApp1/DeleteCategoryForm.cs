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
    public partial class DeleteCategoryForm : Form
    {
        // Категория, которую удаляем
        public Category c;

        public DeleteCategoryForm(Category c)
        {
            // Сохраняем категорию
            this.c = c;
            InitializeComponent();
            textBox1.Text = c.Name;
            this.Text = "Удалить";
        }
        
        // Удаление
        private void button1_Click(object sender, EventArgs e)
        {
            c.Name = "&&&";
            Close();
        }

        // Отмена удаления
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
