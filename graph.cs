using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;
using Autodesk.AutoCAD.Geometry;

namespace TQCAD
{
    public partial class graph : UserControl
    {
        public graph()
        {
            InitializeComponent();
        }

        //nhập số liệu từ excel
        private void Button1_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();

            string path ="";
            OpenFileDialog result = new OpenFileDialog();
            result.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (result.ShowDialog() == DialogResult.OK)
            {
                path  = result.FileName.ToString();
            }

            if (path == "")
            {
                return;
            }

            using (var pck = new ExcelPackage(new FileInfo(path)))
            {
                int t = Convert.ToInt32(txt_column.Text);

                //redim array
                string[] col =null;
                var oldCol = col;
                col = new string[t];
                if (oldCol != null)
                    Array.Copy(oldCol, col, Math.Min(t, oldCol.Length));
                //

                dataGridView.ColumnCount = t;
                int j;
                ExcelWorksheet excelWorksheet = pck.Workbook.Worksheets[1];
                for (int i = excelWorksheet.Dimension.Start.Row; i <= excelWorksheet.Dimension.End.Row; i++)
                {
                    for(j=1; j<=t; j++)
                    {
                        col[j-1] = excelWorksheet.Cells[i, j].Value.ToString();
                    }
                    dataGridView.Rows.Add(col);
                }


            }
        }

        // vẽ biểu đồ trên autocad
        private void Button2_Click(object sender, EventArgs e)
        {
            int t = dataGridView.Rows.Count;
            int k = Convert.ToInt32(txt_column.Text);
            lib lib = new lib();
            
            for (int j = 0; j <= k-2; j++)
            {
                for (int i = 0; i <= t-2; i++)
                {
                    int x1 = Convert.ToInt32(dataGridView.Rows[i].Cells[j].Value);
                    int y1 = Convert.ToInt32(dataGridView.Rows[i].Cells[j+1].Value);
                    int x2 = Convert.ToInt32(dataGridView.Rows[i+1].Cells[j].Value);
                    int y2 = Convert.ToInt32(dataGridView.Rows[i+1].Cells[j+1].Value);
                    Point3d point3D1 = new Point3d(x1,y1,0);
                    Point3d point3D2 = new Point3d(x2, y2, 0);
                    lib.AddLine(point3D1,point3D2,"0",1,1);
                }
            }
        }
    }
}
