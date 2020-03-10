using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
//using System.Data.DataSetExtensions;

namespace AS2GaryYutonBao
{
    public partial class Form1 : Form
    {
        private DataTable customerDataTable = new DataTable();
        private DataTable rentalDataTable = new DataTable();
        private double discount = 0; //a really bad way to declare the variable, but given the scope of this project ... 
        private double totalAmt = 0; //a really bad way to decare this as well..... 
        private DataTable customerRentalDataTable = new DataTable();
        private DataTable priceDataTable = new DataTable();
        //correct way to do so is 
            //1. make sure csv data at source is correctly formatted as output
            //2. if not, then create ETL jobs and move all csv data into db in already formatted versions 
            //3. create models in backend and use EF and Linq to directly query data from db and present in MVC fashion
            //4. app developers wasting all precious in cleaning data to prsented its required formmat is a failure in system design
            //5. discount factor should be stored in its independent table instead of getting assigned from the view
            //6. totalAmt should also be a property of a temporary table queried by sql
            //7. all these work should have been done in the database side rather than winform side


        public Form1()
        {
            InitializeComponent();
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            var customerFilePath = @"C:\AS2customers.csv";
            var rentalFilePath = @"C:\AS2rentals.csv";
            
            DataCollection dc = new DataCollection();
            
            customerDataTable = await dc.GetDataAsync(customerFilePath);
            customerDataTable.Columns["F1"].ColumnName = "dlNumber";
            customerDataTable.Columns["F2"].ColumnName = "discount";

            customerDataTable.Columns["F3"].ColumnName = "name";


            List<string> customerDataList = new List<String>();
            customerDataList.Add(" ");
            
            foreach (DataRow row in customerDataTable.Rows)
            {
                var data = String.Format("{0} {1}", row["name"], row["dlNumber"]);
                customerDataList.Add(data);
            }


            rentalDataTable = await dc.GetDataAsync(rentalFilePath);
            rentalDataTable.Columns["F1"].ColumnName = "dlNumber";
            rentalDataTable.Columns["F2"].ColumnName = "startDate";
            rentalDataTable.Columns["F3"].ColumnName = "vType";
            rentalDataTable.Columns["F4"].ColumnName = "rType";
            rentalDataTable.Columns["F5"].ColumnName = "days";
            rentalDataTable.Columns["F6"].ColumnName = "options";

            comboBox1.DataSource = customerDataList.OrderBy(x => x).ToList(); ;
           

        }

        private void ComboBox_SelectionChanged(object sender, System.EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            string selectedEmployee = (string)comboBox1.SelectedItem;
            string myname = String.Format("{0} {1}", selectedEmployee.Split(' ')[0], selectedEmployee.Split(' ')[1]);
            int dlNumber = selectedEmployee == " "? 0 : int.Parse(selectedEmployee.Split(' ')[2]);
            

            var row = from myRow in customerDataTable.AsEnumerable()
                          where myRow.Field<string>("name") == myname
                      select myRow.Field<string>("discount");


            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            string r = row.FirstOrDefault();
            switch (r)
            {
                case "B":
                    radioButton1.Checked = true;
                    discount = 0.1;
                    break;
                case "S":
                    radioButton2.Checked = true;
                    discount = 0.7;
                    break;
                case "D":
                    radioButton3.Checked = true;
                    discount = 0.15;
                    break;
                case "N":
                    radioButton4.Checked = true;
                    discount = 0;
                    break;
                default:
                    break;
            }

            CalculatePrice(dlNumber);

        }

        public void CalculatePrice(int dlNumber)
        {
            listBox1.Items.Clear();
            double totalAmt = 0;

            var table = from DataRow myRow in rentalDataTable.Rows
                          where (int)myRow["dlNumber"] == dlNumber
                          select myRow;
            
            var results = from table1 in table.AsEnumerable()                                      
                                       select new
                                       {
                                           startDate = table1["startDate"],
                                           vType = table1["vType"],
                                           rType = table1["rType"],
                                           days = table1["days"],
                                           options = table1["options"]
                                       };

            if ( ! results.Any())
            {
                listBox1.Items.Add("sorry, no records found");
            }
            else
            {
                listBox1.Items.Add(String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", "Start Date", "Vehicle", "Return Type",
                    "Days", "Option", "Rental Amout", "Dicount Amout", "Local Amt", "Option Amt", "Net Amt"));
                foreach (var item in results)
                {
                    string vehType = "";
                    double rentalAmt = 0;
                    switch (item.vType)
                    {
                        case "C":
                            vehType = "Compact";
                            rentalAmt = 30;
                            break;
                        case "I":
                            vehType = "Intermediate";
                            rentalAmt = 45.5;
                            break;
                        case "S":
                            vehType = "SUV";
                            rentalAmt = 55;
                            break;
                        case "T":
                            vehType = "Truck";
                            rentalAmt = 60.5;
                            break;
                        default:
                            break;

                    }
                    string reType = "";
                    double locaAmt = 0;
                    switch (item.rType)
                    {
                        case "O":
                            reType = "Ony-Way";
                            locaAmt = 45.5;
                            break;
                        case "S":
                            reType = "Same Location";
                            locaAmt = 15;
                            break;
                        default:
                            break;

                    }
                    string option = "";
                    double optionAmt = 0;
                    switch (item.options)
                    {
                        case "G":
                            option = "GPS";
                            optionAmt = 9.99;
                            break;
                        case "D":
                            option = "Dash Cam";
                            optionAmt = 15;
                            break;
                        case "R":
                            option = "GasRefill";
                            optionAmt = 50;
                            break;
                        //needs more clarification from project requirment
                        case "GD":
                            option = "GPS & Dash Cam & GasRefill";
                            optionAmt = 50 + 15 + 9.99;
                            break;
                        default:
                            break;

                    }

                    double discountAmt = (rentalAmt + locaAmt + optionAmt) * discount;
                    double netAmt = (rentalAmt + locaAmt + optionAmt) * (1 - discount);
                    totalAmt += netAmt; 

                    listBox1.Items.Add(item.startDate + vehType + reType + item.days + option + rentalAmt + locaAmt + optionAmt + discountAmt + netAmt);

                }
                listBox1.Items.Add("total Amt: " + totalAmt.ToString());
                listBox1.Items.Add("coded by magic gary" );

            }

        }

    }
}
