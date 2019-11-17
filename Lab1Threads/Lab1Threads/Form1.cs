using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1Threads
{
    public partial class Form1 : Form
    {
        private long timeNonParallel;
        private long[] times;

        public Form1()
        {
            // Записываем размер массива
            const int n = 5000;

            InitializeComponent();

            // Создаём массивы
            double[,] a = Initialize(n);
            double[,] b = Initialize(n);

            // Запоминаем начальное время
            long timeStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            // Считаем последовательно
            CalculateNonParallel(a, b, n);

            // Запоминаем конечное время и находим разницу
            long timeStop = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            timeNonParallel = timeStop - timeStart;
            
            // Считаем параллельно и запоминаем время для кол-ва потоков от 1 до 8
            times = new long[8];
            for (int i = 0; i < 8; i++)
            {
                // Создаём массивы заново
                a = Initialize(n);
                b = Initialize(n);

                // Считаем
                timeStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                CalculateParallel(a, b, n, i + 1);
                timeStop = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                times[i] = timeStop - timeStart;
            }

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.ColumnCount = 2;
            dataGridView1.RowCount = 9;
            dataGridView1.Rows[0].Cells[0].Value = "Число потоков";
            dataGridView1.Rows[0].Cells[1].Value = "Время выполнения";

            for (int i = 0; i < 8; i++)
            {
                dataGridView1.Rows[i + 1].Cells[0].Value = i + 1;
                dataGridView1.Rows[i + 1].Cells[1].Value = times[i];
            }
        }

        // Инициализация массива рандомными числами
        public double[,] Initialize(int n)
        {
            double[,] result = new double[n, n];
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = r.Next();
                }
            }
            return result;
        }

        // Функция над элементом массива
        public double Function(double x)
        {
            // Обычная сигмоида
            return 1 / (1 + Math.Pow(Math.E, -x));
        }

        // Метод параллельного подсчёта
        public void CalculateParallel(double[,] a, double[,] b, int n, int threadCount)
        {
            // Массив потоков
            Thread[] threads = new Thread[threadCount];

            // По сколько строк считает каждый поток
            int m = n / threadCount;

            // Запускаем потоки, передавая им границы
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(() => ThreadWork(a, b, i * m, Math.Min((i + 1) * m, n), n));
                threads[i].Start();
            }

            // Ждём, пока все потоки закончат считать
            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }
        }

        // Метод подсчёта элементов в отдельном потоке
        public void ThreadWork(double[,] a, double[,] b, int start, int end, int n)
        {
            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a[i, j] = Function(a[i, j] + b[i, j]);
                }
            }
        }

        // Метод последовательного счёта
        public void CalculateNonParallel(double[,] a, double[,] b, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a[i, j] = Function(a[i, j] + b[i, j]);
                }
            }
        }

        // Рисуем график
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics(); // Создаём объект рисования
            g.TranslateTransform(50, 320); // Смещение начала координат (в пикселях)
            g.ScaleTransform(0.4f, 0.4f); // Масштабирование

            PointF[] points = new PointF[8]; // Список точек графика

            Pen pen = new Pen(Color.Black, 0.5f);// Перо для отрисовки графика
            Pen penCO = new Pen(Color.Green, 2f); // Перо для отрисовки осей

            // Рисуем координатные оси
            g.DrawLine(penCO, new Point(-5000, 0), new Point(5000, 0));
            g.DrawLine(penCO, new Point(0, -5000), new Point(0, 5000));

            Font fo = new System.Drawing.Font(FontFamily.GenericSerif, 20f); // Шрифт для вывода текста

            double k = 0.5; // Коэффициент масштабирования значений

            // Запоминаем точки
            for (int i = 0; i < 8; i++)
            {
                points[i] = new PointF((i + 1) * 100f, -float.Parse((times[i] * k).ToString()));
            }
            
            // Рисуем линии графика
            g.DrawLines(pen, points);

            // Выводим значения на оси и легенду
            g.DrawString("0", fo, Brushes.Black, -20, 20);
            for (int i = 1; i <= 8; i++)
            {
                g.DrawString("" + i, fo, Brushes.Black, i * 100f - 10, 20);
                g.DrawString("" + (points[i - 1].Y * -2), fo, Brushes.Black, -80, points[i - 1].Y - 10);
            }

            g.DrawString("Число потоков", fo, Brushes.Black, 1000, 20);
            g.DrawString("Время работы\n в мс", fo, Brushes.Black, 10, -800);
            g.DrawString("Время работы последовательной программы, мс: " + timeNonParallel, fo, Brushes.Black, 200, 80);
        }
    }
}
