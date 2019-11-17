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
    public partial class MainForm : Form
    {
        private BookList books;

        // Главная форма с таблицей
        public MainForm()
        {
            books = new BookList("Main Group");
            books.Read();

            InitializeComponent();

            dataGridView1.DataSource = books.Books;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;

            label1.Text = "";

            // Главный ряд кнопок в меню
            ToolStripMenuItem fileItem = new ToolStripMenuItem("Файл");
            ToolStripMenuItem editItem = new ToolStripMenuItem("Правка");
            ToolStripMenuItem tableOrderItem = new ToolStripMenuItem("Порядок");

            // Кнопки в подменю файл
            ToolStripMenuItem fileGroupItem = new ToolStripMenuItem("Группа");
            ToolStripMenuItem fileSaveItem = new ToolStripMenuItem("Сохранить");

            fileGroupItem.Click += ChangeGroupItemClisk;
            fileSaveItem.Click += SaveItemClick;

            fileItem.DropDownItems.Add(fileGroupItem);
            fileItem.DropDownItems.Add(fileSaveItem);
            
            // Кнопки в подменю порядок
            ToolStripMenuItem tableOrderNameItem = new ToolStripMenuItem("По названию");
            ToolStripMenuItem tableOrderAuthorItem = new ToolStripMenuItem("По имени автора");
            ToolStripMenuItem tableOrderPublisherItem = new ToolStripMenuItem("По издателю");
            ToolStripMenuItem tableOrderGenreItem = new ToolStripMenuItem("По жанру");
            ToolStripMenuItem tableOrderPriceItem = new ToolStripMenuItem("По цене");

            // Упорядочивание
            tableOrderNameItem.Click += delegate
            {
                label1.Text = "";
                books.Books = books.Books.OrderBy(x => x.Name).ToList();
                dataGridView1.DataSource = new Book();
                dataGridView1.DataSource = books.Books;
            };

            tableOrderAuthorItem.Click += delegate
            {
                label1.Text = "";
                books.Books = books.Books.OrderBy(x => x.Author).ToList();
                dataGridView1.DataSource = new Book();
                dataGridView1.DataSource = books.Books;
            };

            tableOrderPublisherItem.Click += delegate
            {
                label1.Text = "";
                books.Books = books.Books.OrderBy(x => x.Publicher).ToList();
                dataGridView1.DataSource = new Book();
                dataGridView1.DataSource = books.Books;
            };

            tableOrderPriceItem.Click += delegate
            {
                label1.Text = "";
                books.Books = books.Books.OrderBy(x => x.Price).ToList();
                dataGridView1.DataSource = new Book();
                dataGridView1.DataSource = books.Books;
            };

            tableOrderGenreItem.Click += delegate
            {
                label1.Text = "";
                books.Books = books.Books.OrderBy(x => x.Genre).ToList();
                dataGridView1.DataSource = new Book();
                dataGridView1.DataSource = books.Books;
            };

            tableOrderItem.DropDownItems.Add(tableOrderNameItem);
            tableOrderItem.DropDownItems.Add(tableOrderAuthorItem);
            tableOrderItem.DropDownItems.Add(tableOrderPublisherItem);
            tableOrderItem.DropDownItems.Add(tableOrderGenreItem);
            tableOrderItem.DropDownItems.Add(tableOrderPriceItem);
            
            // Кнопки в подменю правка
            ToolStripMenuItem editAddItem = new ToolStripMenuItem("Добавить");
            ToolStripMenuItem editUpdateItem = new ToolStripMenuItem("Изменить");
            ToolStripMenuItem editDeleteItem = new ToolStripMenuItem("Удалить");

            editAddItem.Click += button1_Click;
            editUpdateItem.Click += button2_Click;
            editDeleteItem.Click += button3_Click;

            editItem.DropDownItems.Add(editAddItem);
            editItem.DropDownItems.Add(editUpdateItem);
            editItem.DropDownItems.Add(editDeleteItem);

            // Добавление элементов в меню
            menuStrip1.Items.Add(fileItem);
            menuStrip1.Items.Add(editItem);
            menuStrip1.Items.Add(tableOrderItem);

            // Кнопки в контекстном меню
            ToolStripMenuItem contextEditAddItem = new ToolStripMenuItem("Добавить");
            ToolStripMenuItem contextEditUpdateItem = new ToolStripMenuItem("Изменить");
            ToolStripMenuItem contextEditDeleteItem = new ToolStripMenuItem("Удалить");

            contextEditAddItem.Click += button1_Click;
            contextEditUpdateItem.Click += button2_Click;
            contextEditDeleteItem.Click += button3_Click;

            // Добавление элементов в контекстное меню
            contextMenuStrip1.Items.Add(contextEditAddItem);
            contextMenuStrip1.Items.Add(contextEditUpdateItem);
            contextMenuStrip1.Items.Add(contextEditDeleteItem);

            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.CellMouseDown += dataGridView1_CellMouseDown;
        }

        // Добавление
        private void button1_Click(object sender, EventArgs e)
        {
            Book b = new Book();
            EditForm form2 = new EditForm(FormType.Create, b);
            form2.ShowDialog();
            if (b.IsCorrect())
            {
                if (books.AddBook(b))
                {
                    label1.Text = "Успешно!";
                    this.Text = "Библиотека*";
                    label1.ForeColor = Color.Green;
                    dataGridView1.DataSource = new Book();
                    dataGridView1.DataSource = books.Books;
                }
                else
                {
                    label1.Text = "Неудача.";
                    label1.ForeColor = Color.Red;
                }
            }            
        }

        // Изменение
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0)
            {
                label1.Text = "Выберите элемент.";
                label1.ForeColor = Color.Black;

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
                    label1.Text = "Выберите один элемент.";
                    label1.ForeColor = Color.Black;
                }
                else
                {
                    int rowNum = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow row = dataGridView1.Rows[rowNum];
                    Book b = books.Books.Find(x => x.Name == row.Cells[0].Value.ToString());
                    EditForm form2 = new EditForm(FormType.Update, b);
                    form2.ShowDialog();
                    dataGridView1.DataSource = new Book();
                    dataGridView1.DataSource = books.Books;
                    this.Text = "Библиотека*";
                }
            }
        }

        // Удаление
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0)
            {
                label1.Text = "Выберите элемент.";
                label1.ForeColor = Color.Black;
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
                    label1.Text = "Выберите один элемент.";
                    label1.ForeColor = Color.Black;
                }
                else
                {
                    int rowNum = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow row = dataGridView1.Rows[rowNum];
                    Book b = new Book
                    {
                        Name = (string)row.Cells[0].Value,
                        Author = (string)row.Cells[1].Value,
                        Publicher = (string)row.Cells[2].Value,
                        Genre = (Genres)row.Cells[3].Value,
                        Price = int.Parse(row.Cells[4].Value.ToString())
                    };
                    DeleteForm form3 = new DeleteForm(b);
                    form3.ShowDialog();
                    if (b.Name == "&&&")
                    {
                        if (books.DeleteBook((string)row.Cells[0].Value))
                        {
                            label1.Text = "Успешно!";
                            this.Text = "Библиотека*";
                            label1.ForeColor = Color.Green;
                            dataGridView1.DataSource = new Book();
                            dataGridView1.DataSource = books.Books;
                        }
                        else
                        {
                            label1.Text = "Неудача.";
                            label1.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }

        // Сохранение изменений
        private void SaveItemClick(object sender, EventArgs e)
        {
            this.Text = "Библиотека";
            books.Write();
        }

        // Смена группы
        private void ChangeGroupItemClisk(object sender, EventArgs e)
        {
            label1.Text = "";
            books.Write();
            ChangeGroupForm form = new ChangeGroupForm(books);
            form.ShowDialog();
            books.Read();
            dataGridView1.DataSource = new Book();
            dataGridView1.DataSource = books.Books;
        }

        // Выделение ячейки при щелчке правой кнопкой
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.RowIndex > -1) && (e.Button == MouseButtons.Right))
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
        }
    }
}
