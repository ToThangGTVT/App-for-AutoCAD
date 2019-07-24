using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace TQCAD
{
    public partial class total_length : UserControl
    {
        double sum_lenght=0;
        public total_length()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
            int nfilter = 1;
            if (cbo_linetype.Text != "ALL")
                nfilter = 2;
            TypedValue[] acTypValAr = new TypedValue[nfilter];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, comboBox1.Text), 0);
            if (cbo_linetype.Text != "ALL")
            {
                acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, cbo_linetype.Text), 1);
            }
            
            SelectionFilter filter = new SelectionFilter(acTypValAr);

            var selection = ed.GetSelection(filter);

            if (selection.Status != PromptStatus.OK)
                return;

            if (txt_tl.Text != "" & comboBox1.Text!="")
            {
                int tl = Convert.ToInt32(txt_tl.Text);
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    // use Linq queries to get lengths by type in a dictionary
                    var lengthes = selection.Value
                        .Cast<SelectedObject>()
                        .Select(so => (Curve)tr.GetObject(so.ObjectId, OpenMode.ForRead))
                        .ToLookup(curve => curve.GetType().Name, curve => curve.GetDistanceAtParameter(curve.EndParam)*tl)
                        .ToDictionary(group => group.Key, group => group.Sum());

                    // print results
                    foreach (var entry in lengthes)
                    {
                        ed.WriteMessage($"\n{entry.Key,-12} = {entry.Value}");
                        string[] row = { comboBox1.Text, Math.Round((entry.Value), 2).ToString() };
                        dgv.Rows.Add(row);
                    }

                    ed.WriteMessage($"\nTotal Length = {lengthes.Values.Sum()}");
                    sum_lenght = lengthes.Values.Sum();
                    tr.Commit();
                }
                Application.DisplayTextScreen = true;

            }
            else
            {
                MessageBox.Show("Bạn phải nhập tỉ lệ bản vẽ!");
            }
            
            txt_toolstrip.Text = "SUM = "+sum_lenght.ToString(); 

        }

        public void TestDisplayLayers()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            List<string> info = LayersToList(db);
            foreach (string lname in info)
                comboBox1.Items.Add(lname);

        }

        public List<string> LayersToList(Database db)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            List<string> lstlay = new List<string>();

            LayerTableRecord layer;
            
            using (Transaction tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                LayerTable lt = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                foreach (ObjectId layerId in lt)
                {
                    doc.LockDocument();
                    layer = tr.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                    lstlay.Add(layer.Name);
                }

            }
            return lstlay;
        }

        private void Total_length_Load(object sender, EventArgs e)
        {
            TestDisplayLayers();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            TestDisplayLayers();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            sum_lenght = 0;
        }
    }
}
