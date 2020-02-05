using System;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;

namespace ClassExercise
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private OleDbConnection returnConnection(string fileName)
        {
            if (Path.GetExtension(fileName) == ".xls")
            {
                return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + "; Jet OLEDB:Engine Type=5;Extended Properties=\"Excel 8.0;\"");
            }
            else
            {
                return new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=Excel 12.0;");
            }
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "C:\\";
            //openFileDialog1.Filter = "Excel Files|*.xls|CSV Files|*.csv|Txt Files|*.txt";
            openFileDialog1.Filter = "Data Sheets|*.xls;*.xlsx;*.csv;*.txt";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog1.FileName;
                var fileType = Path.GetExtension(path);

                Dictionary<string, int> results= new Dictionary<string, int>();

                if (fileType == ".csv" || fileType == ".txt")
                {
                    using (var reader = new StreamReader(path))
                    {
                        while (!reader.EndOfStream)
                        {
                            var name = reader.ReadLine();
                            if (results.ContainsKey(name))
                            {
                                results[name] += 1;
                            }
                            else
                            {
                                results.Add(name, 1);
                            }
                        }
                    }
                }
                else
                {
                    DataTable datasheet = new DataTable();
                    using (OleDbConnection conn = this.returnConnection(path))
                    {
                        conn.Open();
                        // retrieve the data using data adapter
                        OleDbDataAdapter sheetAdapter = new OleDbDataAdapter("select * from [Sheet1$]", conn);
                        sheetAdapter.Fill(datasheet);
                        conn.Close();
                    }
                    var rows = datasheet.Rows;
                    foreach (DataRow row in rows)
                    {
                        if (results.ContainsKey(row["1"].ToString()))
                        {
                            results[row["1"].ToString()] += 1;
                        }
                        else
                        {
                            results.Add(row["1"].ToString(), 1);
                        }
                    } 
                }

                foreach (var key in results.Keys)
                {
                    listBox1.Items.Add(String.Format("{0}: {1}", key, results[key].ToString())); 
                }


            }

            
        }
    }
}
