using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.Geometry;

namespace TQCAD
{
    public partial class uct_main : UserControl
    {
        public uct_main()
        {
            InitializeComponent();
        }

        private void Uct_main_Load(object sender, EventArgs e)
        {

        }

        private void TínhTổngChiềuDàiPolylineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var total_Length = new total_length();
            pnl.Controls.Add(total_Length);
            pnl.Dock = DockStyle.Fill;
        }

        private void LineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point3d point3D = new Point3d(0, 0, 0);
            double pi = 3.14159265359;
            lib lib = new lib();
            _ = lib.AddArc(point3D, 5, 0, pi, "0");
            for(double i = 1; i < 100; i++)
            {
                lib.AddCircle(point3D, 2 * i, "0");
            }
            
        }

        private void LIBToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void VẽBiểĐồToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var graph = new graph();
            pnl.Controls.Add(graph);
            pnl.Dock = DockStyle.Fill;
        }
    }
}
