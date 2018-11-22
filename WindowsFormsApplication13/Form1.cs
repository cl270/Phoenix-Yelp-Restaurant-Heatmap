using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace WindowsFormsApplication13
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataGridView dgv = new DataGridView();
        static SqlConnection cnn;
        string connectionString = null;
        string categoryquery = "Select * from Yelp;";
        List<double> latitude = new List<double>();
        List<double> longitude = new List<double>();

        private void Form1_Load(object sender, EventArgs e)
        {
            //Change server location below, and database if you changed name
            connectionString = "server=DESKTOP-5FNNB3C\\SQLEXPRESS;" +
                                       "Trusted_Connection=yes;" +
                                       "database=Yelp; " +
                                       "connection timeout=30";
            cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
                showall("Select * from [Yelp].[dbo].[Yelp]");
                filllist();
                listBox1.SelectedIndex = 0;
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void showall(String query)
        {
            try
            {
                //input variable is query, function refreshes view with new query.
                DataTable dt = new DataTable();
                dt.Rows.Clear();
                SqlCommand command = new SqlCommand(query, cnn);
                command.CommandType = CommandType.Text;
                SqlDataAdapter dataadapter = new SqlDataAdapter(command);
                dataadapter.Fill(dt);
                dataGridView1.DataSource = dt;
                dataadapter.Dispose();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void filllist()
        {
            listBox1.Items.Add("All Categories");
            for (int i = 0; i < 7; i++)
            {
                DataTable dt = new DataTable();
                dt.Rows.Clear();
                SqlCommand command = new SqlCommand("Select Distinct category#"+ i.ToString() +" from Yelp", cnn);
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if ((String)reader[0] != "NA")
                    {
                        listBox1.Items.Add(reader[0]);
                    }
                }
                reader.Close();
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            showall(textBox1.Text);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }


        public double degtorad(double degree)
        {
            return degree * Math.PI / 180;
        }

        public double distancehav(double onelat, double onelong, double twolat, double twolong)
        {
            double a = Math.Pow(Math.Sin(degtorad(twolat) - degtorad(onelat)) / 2,2) 
                + Math.Cos(degtorad(onelat))*Math.Cos(degtorad(twolat))
                * Math.Pow(Math.Sin(degtorad(twolong-onelong)) / 2,2);
            return 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a)) * 6371;
        }

        public double score(double latitude, double longitude, List<double[]> data)
        {
            List<double> scorelist = new List<double>();
            foreach (double[]dataline in data)
            {
                double dist = distancehav(latitude, longitude, dataline[0], dataline[1]);
                double numberrate = dataline[2];
                double star = dataline[3];
                //Modified Rayleigh Distribution, global max at 1
                double distscore = 4.083*((1.25*dist+60)/240)*Math.Exp(-0.00125*(0.0732*dist*dist+4.7*dist+200));
                //Sigmoidally growing function based on number of ratings
                double numratscore = 1/(1+Math.Exp(-0.05*(numberrate-20)));
                //Altered by the average review, the lower the better for a new restaurant
                double starscore = 1-1/(1+Math.Exp(-0.5*(star-6)));
                //Weights
                double distweight = Convert.ToDouble(textBox2.Text);
                double numweight = Convert.ToDouble(textBox4.Text);
                double starweight = Convert.ToDouble(textBox9.Text);
                scorelist.Add((distscore*distweight + numratscore*numweight + starscore*starweight) / (distweight + numweight + starweight));
            }
            return scorelist.Average();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label12.Text = Convert.ToString(score(Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox5.Text), initializedata()));
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox6.Text != "" && textBox7.Text != "")
                {
                    textBox8.Text = Convert.ToString(int.Parse(textBox6.Text) * int.Parse(textBox7.Text));
                }
            }
            catch
            {
                MessageBox.Show("Integers only.");
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox6.Text != "" && textBox7.Text != "")
                {
                    textBox8.Text = Convert.ToString(int.Parse(textBox6.Text) * int.Parse(textBox7.Text));
                }
            }
            catch
            {
                MessageBox.Show("Integers only.");
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((String)listBox1.SelectedItem == "All Categories")
            {
                categoryquery = "Select * from Yelp;";
                showall("Select * from Yelp;");
            }
            else
            {
                String cat = (String)listBox1.SelectedItem;
                categoryquery = "Select * from Yelp where category#0 = '" + cat +
                        "' OR category#1 = '" + cat +
                        "' OR category#2 = '" + cat +
                        "' OR category#3 = '" + cat +
                        "' OR category#4 = '" + cat +
                        "' OR category#5 = '" + cat +
                        "' OR category#6 = '" + cat + "';";
            }
            try
            {
                showall(categoryquery);
                SqlCommand command = new SqlCommand("Select latitude, longitude " + categoryquery.Substring(8), cnn);
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                latitude = new List<double>();
                longitude = new List<double>();                
                while (reader.Read())
                {
                    double templat = Convert.ToDouble(reader[0]);
                    double templo = Convert.ToDouble(reader[1]);
                    latitude.Add(templat);
                    longitude.Add(templo);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No category selected." + ex.Message);
            }
        }
        static DataTable ConvertListToDataTable(List<List<double>> list)
        {
            DataTable table = new DataTable();
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Count > columns)
                {
                    columns = array.Count;
                }
            }
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }
            foreach (var array in list)
            {
                DataRow d = table.NewRow();
                for(int i = 0; i<columns; i++)
                {
                    d[i] = array.ElementAt(i);
                }
                table.Rows.Add(d);
            }
            return table;
        }
        private List<double[]> initializedata()
        {
            List<double[]> storage = new List<double[]>();
            SqlCommand command = new SqlCommand("Select latitude, longitude, reviews, stars " + categoryquery.Substring(8), cnn);
            command.CommandType = CommandType.Text;
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                storage.Add(new double[] { Convert.ToDouble(reader[0]), Convert.ToDouble(reader[1]), Convert.ToDouble(reader[2]), Convert.ToDouble(reader[3]) });
            }
            reader.Close();
            return storage;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //between 33.1 33.8 -111.4 -112.8 
            List<List<double>> heat = new List<List<double>>();
            double latlen = Convert.ToDouble(textBox6.Text);
            double longwid = Convert.ToDouble(textBox7.Text);
            double incrementlat = 0.7 / latlen;
            double incrementlong = 1.4 / longwid;
            List<double[]> storage = initializedata();
            for (int i = 0; i < latlen; i++)
            {
                List<double> temp = new List<double>();
                double curlat = 33.1 + incrementlat*i +incrementlat / 2;
                for (int j = 0; j < longwid; j++)
                {
                    double curlong = -111.4 - incrementlong * j + incrementlong / 2;
                    double tempscore = score(curlat, curlong, storage);
                    temp.Add(tempscore);
                }
                heat.Add(temp);
            }
            dataGridView1.DataSource = ConvertListToDataTable(heat);
            Form2 frm = new Form2(heat, latitude, longitude);
            frm.Show();
        }
    }
}
