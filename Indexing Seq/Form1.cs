using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Indexing_Seq
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            write(0);
        }
        SortedDictionary<int, int> d = new SortedDictionary<int, int>();
        FileStream fs;
        StreamReader sr;
        StreamWriter sw;
        FileStream fs1;
        StreamReader sr1;
        StreamWriter sw1;
        public void write(int x)//write test
        {
            d.Clear();
            fs = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            sr = new StreamReader(fs);
            string l;
            int c = 0;
            while((l=sr.ReadLine())!=null)
            {
                if (l[0] == '*')
                {
                    c += l.Length + 2; continue;
                }
                string[] arr = l.Split('|');
                d.Add(int.Parse(arr[0]), c);
                c += l.Length+2;
            }
            sr.Close();
            fs.Close();
            write();
        }
        public void write()//write index
        {
            fs1 = new FileStream("ind.txt", FileMode.Truncate, FileAccess.ReadWrite);
            sw1 = new StreamWriter(fs1);
            foreach(var i in d)
            {
                sw1.WriteLine(i.Key + "\t" + i.Value);
                sw1.Flush();
            }
            sw1.Close();
            fs1.Close();
        }
        private void button7_Click(object sender, EventArgs e)//INSERT
        {
            int[] arr = d.Keys.ToArray();
            if (!bs(arr, int.Parse(textBox2.Text)))
            {
                fs = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                sw = new StreamWriter(fs);
                fs.Seek(0, SeekOrigin.End);
                int c = Convert.ToInt32(fs.Position);
                d.Add(int.Parse(textBox2.Text), c);
                sw.Write(textBox2.Text + "|" + textBox3.Text + "\r\n");
                sw.Flush();
                sw.Close();
                fs.Close();
                write();
            }
            else
            {
                MessageBox.Show("Already Added!");
            }
        }

        private void button6_Click(object sender, EventArgs e)//LOADFILE
        {
            fs = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            sr=new StreamReader(fs);
            string l;
            string f = "";
            while((l=sr.ReadLine())!=null)
            {
                if (l[0]!='*')
                    f += l + "\r\n";
            }
            textBox1.Text = f;
            sr.Close();
            fs.Close();
        }

        private void button5_Click(object sender, EventArgs e)//LOADINDEX
        {
            write();
            fs1 = new FileStream("ind.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            sr1 = new StreamReader(fs1);
            string l,f="";
            while ((l=sr1.ReadLine())!=null)
            {
                if (l[0] == '*') continue;
                f += l+ "\r\n";
            }
            textBox1.Text = f;
            sr1.Close();
            fs1.Close();
        }
        public bool bs(int[] arr,int n)
        {
            int st = 0, en = arr.Length - 1;
            int mid;
            while(st<=en)
            {
                mid = (st + en) / 2;
                if (arr[mid] == n) return true;
                if (arr[mid] < n) st = mid + 1;
                else if (arr[mid] > n) en = mid - 1;
            }
            return false;
        }
        private void button4_Click(object sender, EventArgs e)//Search By ID
        {
            int[] arr = d.Keys.ToArray();
            if(bs(arr, int.Parse(textBox2.Text)))
            {
                MessageBox.Show("Found!");
                int h = Convert.ToInt32(textBox2.Text);
                int m = d[h];
                textBox1.Text = "ID = " + h.ToString()+" at Position = "+ m.ToString();
            }
            else
            {
                MessageBox.Show("Not Found!");
            }
        }
        public void delind()
        {
            sw1 = new StreamWriter("ind.txt");
            int[] arr = d.Keys.ToArray();
            if (bs(arr, int.Parse(textBox2.Text)))
            {
                d.Remove(int.Parse(textBox2.Text));
                foreach (var i in d)
                {
                    sw1.WriteLine(i.Key + "\t" + i.Value);
                    sw1.Flush();
                }
                MessageBox.Show("Deleted");
            }
            else MessageBox.Show("Not Found!");
            sw1.Close();
        }
        public void deltxt()
        {
            fs = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            sr = new StreamReader(fs);
            sw = new StreamWriter(fs);
            string l;
            int c = 0;
            while ((l=sr.ReadLine())!=null)
            {
                string[] arr = l.Split('|');
                if (arr[0]==textBox2.Text)
                {
                    fs.Seek(c, SeekOrigin.Begin);
                    sw.Write("*");
                    sw.Flush();
                    break;
                }
                c += l.Length + 2;
            }
            fs.Close();
        }
        private void button3_Click(object sender, EventArgs e)//Delete By ID
        {
            delind();
            deltxt();
        }

        private void button1_Click(object sender, EventArgs e)//Close
        {
            Environment.Exit(0);
        }

        private void btnrr_Click(object sender, EventArgs e)//Rewrite
        {
            write(1);
        }
    }
}
