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
    public partial class EditCategoryForm : Form
    {
        // Категория, с которой работаем
        private Category category;

        public EditCategoryForm(FormType type, Category c)
        {
            // Сохраняем полученную категорию
            this.category = c;
            InitializeComponent();
            label6.Text = "";
            if (category == null)
            {
                category = new Category();
            }

            // Записываем значения полей
            if (type == FormType.Update)
            {
                textBox1.Text = c.Name;
                button1.Text = "Применить";
                this.Text = "Изменить";
            }
            if (type == FormType.Create)
            {
                button1.Text = "Добавить";
                this.Text = "Добавить";
            }
        }

        // Сохранение изменений
        private void button1_Click(object sender, EventArgs e)
        {
            // Флаг для проверки корректности полей
            bool flag = true;

            // Проверка названия
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

            // Если все поля корректны, сохраняем изменения
            if (flag)
            {
                category.Name = textBox1.Text;
                this.Close();
            }
        }
    }
}
