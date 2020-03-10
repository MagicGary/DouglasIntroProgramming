using System;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace week9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
            DataTable dt = await getData();
            listBox1.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                string name = Convert.ToString(row["F1"]);
                DateTime dateRaw= Convert.ToDateTime(row["F2"]);
                int day = Convert.ToInt16(row["F3"]);
                int guests = Convert.ToInt16(row["F4"]);
                string regionRaw = Convert.ToString(row["F5"]);
                string optionsRaw = Convert.ToString(row["F6"]);

                string date = dateRaw.ToString("yyyy-mm-dd");
                string endDate = dateRaw.AddDays(day).ToString("yyyy-mm-dd");
                string region = "";
                switch (regionRaw)
                {
                    case "A":
                        region = "Africa";
                        break;
                    case "N":
                        region = "New England";
                        break;
                    case "M":
                        region = "Mediterranean";
                        break;
                    default:
                        break;
                }

                string options = "";
                switch (optionsRaw)
                {
                    case "EDW":
                        options = "Excursion & Diing & Wifi";
                        break;
                    case "DW":
                        options = "Diing & Wifi";
                        break;
                    case "E":
                        options = "Excursion";
                        break;
                    case "W":
                        options = "Wifi";
                        break;
                    case "WE":
                        options = "Wifi & Dining";
                        break;
                    case "NA":
                        options = "None";
                        break;
                    default:
                        break;
                }

                string result = String.Format("{0,-20} {1, -12} {2, -12} {3,3} {4, 2} {5,15} {6,25}", name, date, endDate, day, guests, region, options);
                listBox1.Items.Add(result);

            }

        }

        public async Task<DataTable> getData()
        {
            string filePath = @"C:\Temp\LINQDemo2.csv";
            string header = "NO";
            string pathOnly = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);

            string sqlCustomer = @"SELECT * FROM [" + fileName + "]";

            using (OleDbConnection connection = new OleDbConnection(
              @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
              ";Extended Properties=\"Text;HDR=" + header + "\""))
            using (OleDbCommand command = new OleDbCommand(sqlCustomer, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.CurrentCulture;
                await Task.Run(() => adapter.Fill(dataTable));
                return dataTable;
            }
        }
    }
}
