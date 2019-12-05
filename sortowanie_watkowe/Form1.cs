using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace sortowanie_watkowe
{
    public partial class Form1 : Form
    {
        const int SEQUENTIAL_THRESHOLD = 5000;
        List<int> Numbers;
        int zakres;
        Random rnd;
        int w = 5;
        int ile;
        Stopwatch stopwatch1;
        Stopwatch stopwatch2;
        public Form1()
        {
            InitializeComponent();
            rnd = new Random();
            Numbers = new List<int>();
            stopwatch1 = new Stopwatch();
            stopwatch2 = new Stopwatch();
            lblTime1.Text = "";
            lblTime2.Text = "";
            //ile = pictureBox1.Width / w;
            timer1.Stop();
            timer2.Stop();
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            int nextTo = 1;
            if(Numbers.Count > pictureBox1.Width / w)
            {
                nextTo = Numbers.Count / (pictureBox1.Width / w);
            }
            for (int i = 0; i < Numbers.Count; i += nextTo)
            {
                int scaleH = Numbers[i];
                if(zakres > pictureBox1.Height)
                {
                    scaleH = (int)(((float)Numbers[i] / (float)zakres) * pictureBox1.Height);
                }
                Brush rectColor = Brushes.White;
                Pen border = new Pen(Color.Black);
                Rectangle rect = new Rectangle
                    (w * (i / nextTo), pictureBox1.Height - scaleH, w, scaleH);
                canvas.FillRectangle
                    (rectColor, w * (i / nextTo), (pictureBox1.Height - scaleH), w, scaleH);
                canvas.DrawRectangle(border, rect);
            }
        }
        private void Sortuj()
        {
            Numbers.Sort();
        }
        private void QuickSort(int start, int end)
        {
            if (start >= end) return;
            int index = Partition(start, end);
            QuickSort(start, index - 1);
            QuickSort(index + 1, end);
        }
        private int Partition(int start, int end)
        {
            int pivotIndex = start; //to guess
            float pivotValue = Numbers[end];
            for (int i = start; i < end; i++)
            {
                if (Numbers[i] < pivotValue)
                {
                    Swap(i, pivotIndex);
                    pivotIndex++;
                }
            }
            Swap(pivotIndex, end);
            return pivotIndex;
        }
        private void Swap(int a, int b)
        {
            var pom = Numbers[a];
            Numbers[a] = Numbers[b];
            Numbers[b] = pom;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            ShowTime(stopwatch1, lblTime1);
        }
        private void ShowTime(Stopwatch sw, object sender)
        {
            var lbl = (Label)sender;
            lbl.Text = String.Format
                ("{0,00}h:{1,00}m:{2,00}s:{3,00}ms", sw.Elapsed.Hours, sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10);
        }
        private void GenerateNumbers()
        {
                zakres = int.Parse(textBox2.Text);
                Numbers.Clear();
                for (long i = 0; i < ile; i++)
                {
                    Numbers.Add(rnd.Next(zakres));
                }
        }
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            try
            {
                ChangeEnabledBtn(btnStart, true);
                ChangeEnabledBtn(btnStartMultiThread, true);
                ile = int.Parse(textBox1.Text);
                GenerateNumbers();
                pictureBox1.Refresh();
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(btnGenerate, ex.Message);
            }
        }

        async private void btnStart_Click(object sender, EventArgs e)
        {
            ChangeEnabledBtn(sender, false);
            ChangeEnabledBtn(btnStartMultiThread, false);
            Task task = new Task(() => QuickSort(0, Numbers.Count - 1));
            stopwatch1.Restart();
            stopwatch1.Start();
            timer1.Start();
            task.Start();
            //czeka na watek ale tym samym nie blokuje interfejsu
            await task;
            stopwatch1.Stop();           
            ShowOnList(stopwatch1, "Zwykłe");
            timer1.Stop();
            pictureBox1.Refresh();
        }
        private void QuickSortMultiThread(int start, int end)
        {
            if (start >= end) return;
            if((end - start) < SEQUENTIAL_THRESHOLD)
            {
                QuickSort(start, end);
            }
            else
            {
                int index = Partition(start, end);
                //metoda Invoke czeka aż zakończą pracę
                Parallel.Invoke(
                () => QuickSortMultiThread(start, index - 1),
                () => QuickSortMultiThread(index + 1, end));
            }
        }

        async private void btnStartMultiThread_Click(object sender, EventArgs e)
        {
            ChangeEnabledBtn(sender, false);
            ChangeEnabledBtn(btnStart, false);
            Task task = new Task( () => QuickSortMultiThread(0, Numbers.Count - 1));
            stopwatch2.Restart();
            stopwatch2.Start();
            timer2.Start();
            task.Start();
            //zaczekaj na wątek
            await task;
            stopwatch2.Stop();
            ShowOnList(stopwatch2, "Wątkowe");
            timer2.Stop();
            pictureBox1.Refresh();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ShowTime(stopwatch2, lblTime2);
        }
        private void ChangeEnabledBtn(object obj, bool b)
        {
            var btn = (Button)obj;
            btn.Enabled = b;
        }
        private void ShowOnList(Stopwatch sw, string text)
        {
            listBox1.Items.Add
                (String.Format
                (text+ " Ilość: {0}, zakres: {1}, czas: {2, 00}h:{3, 00}m:{4, 00}s:{5,00}ms",
                Numbers.Count, zakres, sw.Elapsed.Hours,
                sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10));
        }
    }
}
