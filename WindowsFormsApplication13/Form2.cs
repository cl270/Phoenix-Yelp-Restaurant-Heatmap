using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication13
{
    public partial class Form2 : Form
    {
        public List<List<double>> heatmap;
        public int rowlen;
        public int collen;
        public double max;
        public double min;
        public Bitmap bit;
        public Bitmap bitunder;
        public List<double> mylats;
        public List<double> mylongs;
        private void Form2_Load(object sender, EventArgs e)
        {
            //1389x833
            int piclen = pictureBox1.Size.Width;
            int picwid = pictureBox1.Size.Height;
            double iterlen = piclen/rowlen;
            double iterwid = picwid/collen;
            for(double i = 0; i<piclen; i++)
            {
                int row = Convert.ToInt32(Math.Floor(i/iterlen));
                for(double j = 0; j<picwid; j++)
                {
                    int col = Convert.ToInt32(Math.Floor(j/iterwid));
                    Color pixcolor = bit.GetPixel(Convert.ToInt32(i), Convert.ToInt32(j));
                        int r = 0;
                        int b = 0;
                        int g = 0;
                        if (col >= collen && row >= rowlen)
                        {
                            r = Convert.ToInt32(scalered(heatmap.ElementAt(rowlen - 1).ElementAt(collen - 1)) * 255);
                            g = Convert.ToInt32(scalegreen(heatmap.ElementAt(rowlen - 1).ElementAt(collen - 1)) * 255);
                            b = Convert.ToInt32(scaleblue(heatmap.ElementAt(rowlen - 1).ElementAt(collen - 1)) * 255);
                        }
                        else if (col >= collen)
                        {
                            r = Convert.ToInt32(scalered(heatmap.ElementAt(row).ElementAt(collen - 1)) * 255);
                            g = Convert.ToInt32(scalegreen(heatmap.ElementAt(row).ElementAt(collen - 1)) * 255);
                            b = Convert.ToInt32(scaleblue(heatmap.ElementAt(row).ElementAt(collen - 1)) * 255);

                        }
                        else if (row >= rowlen)
                        {
                            r = Convert.ToInt32(scalered(heatmap.ElementAt(rowlen - 1).ElementAt(col)) * 255);
                            g = Convert.ToInt32(scalegreen(heatmap.ElementAt(rowlen - 1).ElementAt(col)) * 255);
                            b = Convert.ToInt32(scaleblue(heatmap.ElementAt(rowlen - 1).ElementAt(col)) * 255);
                        }
                        else
                        {
                            r = Convert.ToInt32(scalered(heatmap.ElementAt(row).ElementAt(col)) * 255);
                            g = Convert.ToInt32(scalegreen(heatmap.ElementAt(row).ElementAt(col)) * 255);
                            b = Convert.ToInt32(scaleblue(heatmap.ElementAt(row).ElementAt(col)) * 255);
                        }
                        pixcolor = Color.FromArgb(r, pixcolor.G, 255-b);
                        bitunder.SetPixel(Convert.ToInt32(i), Convert.ToInt32(j), pixcolor);
                        pixcolor = Color.FromArgb(r, g, 255-b);
                        bit.SetPixel(Convert.ToInt32(i), Convert.ToInt32(j), pixcolor);
                    
                }
            }
            //between 33.1 33.8 -111.4 -112.8 
            double longwid = 1.4;
            double latlen = 0.7;
            for (int i = 0; i < mylats.Count; i++)
            {
                double dotlat = Math.Floor(1389 * (mylats.ElementAt(i) - 33.1) / latlen);
                double dotlong = Math.Floor(833 * (Math.Abs(mylongs.ElementAt(i)) - 111.4) / longwid);
                if(dotlat >=  1388) { dotlat = 1387; }
                if(dotlat < 1) { dotlat = 1; }
                if(dotlong>=832) { dotlong = 831; }
                if(dotlong<1) { dotlong = 1; }
                restaurantlocation(dotlat, dotlong);
            }
            pictureBox1.Image = bitunder;
        }

        public void restaurantlocation(double lat, double longit)
        {
            bitunder.SetPixel(Convert.ToInt32(lat), Convert.ToInt32(longit), Color.White);
            bit.SetPixel(Convert.ToInt32(lat), Convert.ToInt32(longit), Color.White);
            bitunder.SetPixel(Convert.ToInt32(lat)-1, Convert.ToInt32(longit), Color.Black);
            bit.SetPixel(Convert.ToInt32(lat)-1, Convert.ToInt32(longit), Color.Black);
            bitunder.SetPixel(Convert.ToInt32(lat) - 1, Convert.ToInt32(longit)-1, Color.Black);
            bit.SetPixel(Convert.ToInt32(lat) - 1, Convert.ToInt32(longit)-1, Color.Black);
            bitunder.SetPixel(Convert.ToInt32(lat) - 1, Convert.ToInt32(longit)+1, Color.Black);
            bit.SetPixel(Convert.ToInt32(lat) - 1, Convert.ToInt32(longit)+1, Color.Black);
            bitunder.SetPixel(Convert.ToInt32(lat), Convert.ToInt32(longit)+1, Color.Black);
            bit.SetPixel(Convert.ToInt32(lat), Convert.ToInt32(longit)+1, Color.Black);
            bitunder.SetPixel(Convert.ToInt32(lat), Convert.ToInt32(longit)-1, Color.Black);
            bit.SetPixel(Convert.ToInt32(lat), Convert.ToInt32(longit)-1, Color.Black);
            bitunder.SetPixel(Convert.ToInt32(lat) + 1, Convert.ToInt32(longit)+1, Color.Black);
            bit.SetPixel(Convert.ToInt32(lat) + 1, Convert.ToInt32(longit)+1, Color.Black);
            bitunder.SetPixel(Convert.ToInt32(lat) + 1, Convert.ToInt32(longit)-1, Color.Black);
            bit.SetPixel(Convert.ToInt32(lat) + 1, Convert.ToInt32(longit)-1, Color.Black);
            bitunder.SetPixel(Convert.ToInt32(lat) + 1, Convert.ToInt32(longit), Color.Black);
            bit.SetPixel(Convert.ToInt32(lat) + 1, Convert.ToInt32(longit), Color.Black);
        }
        public double scaleblue(double col)
        {
            double mid = min + (max - min) / 2;
            if (col <= mid) { return 0; }
            else { return (col - mid) / (max - mid); }
        }
        public double scalered(double col)
        {
            double mid = min + 3 * (max - min) / 4;
            if (col <= mid) { return 0; }
            else { return (col - mid) / (max - mid); }
        }
        public double scalegreen(double col)
        {
            double mid = min + 7 * (max - min) / 8;
            if (col <= mid) { return col; }
            else
            {
                double scale = (col - mid) / (max - mid);
                return 0.5 + scale / 2;
            }
        }
        public Form2(List<List<double>> heat, List<double> latitude, List<double> longitude)
        {
            heatmap = heat;
            rowlen = heat.Count;
            collen = heat[0].Count;
            mylats = latitude;
            mylongs = longitude;
            max = heat.SelectMany(x => x).Max();
            min = heat.SelectMany(x => x).Min();
            bit = new Bitmap(WindowsFormsApplication13.Properties.Resources.phoenix);
            bitunder = new Bitmap(WindowsFormsApplication13.Properties.Resources.phoenix);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image == bitunder)
            {
                pictureBox1.Image = bit;
            }
            else { pictureBox1.Image = bitunder; }
        }
    }
}
