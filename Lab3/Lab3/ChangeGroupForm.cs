using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class ChangeGroupForm : Form
    {
        private BookList list;

        public ChangeGroupForm(BookList list)
        {
            InitializeComponent();
            label1.Text = "";
            this.Text = "Выбор группы";
            this.list = list;
            
            using (StreamReader sr = new StreamReader("Groups.txt"))
            {
                List<string> groups = new List<string>();
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        groups.Add(s);
                    }
                }
                listBox1.Items.AddRange(groups.ToArray());
            }

            listBox1.SetSelected(listBox1.Items.IndexOf(list.Group), true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            list.Group = listBox1.SelectedItem.ToString();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string group = textBox1.Text;
            if (listBox1.Items.Contains(group))
            {
                label1.Text = "Название уже занято.";
                label1.ForeColor = Color.Red;
            }
            else if (String.IsNullOrWhiteSpace(group))
            {
                label1.Text = "Некорректное название.";
                label1.ForeColor = Color.Red;
            }
            else
            {
                try
                {
                    if (Directory.Exists(group))
                    {
                        Directory.Delete(group);
                    }
                    Directory.CreateDirectory(group);
                    File.Create(group + "/Books.txt");
                    using (StreamWriter sw = new StreamWriter("Groups.txt"))
                    {
                        sw.WriteLine(group);
                    }
                    listBox1.Items.Add(group);
                    label1.Text = "Успешно!";
                    label1.ForeColor = Color.Green;
                }
                catch
                {
                    label1.Text = "Неудача.";
                    label1.ForeColor = Color.Red;
                }
            }
        }
    }
}
