using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

namespace AS2GaryYutonBao
{
    class DataCollection
    {
        public DataCollection()
        {

        }
        public async Task<DataTable> GetDataAsync(string filePath)
        {
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
