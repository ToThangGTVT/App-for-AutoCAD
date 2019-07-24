namespace TQCAD
{
    partial class uct_main
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.chứcNăngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tínhTổngChiềuDàiPolylineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lIBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hướngDẫnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnl = new System.Windows.Forms.Panel();
            this.vẽBiểĐồToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chứcNăngToolStripMenuItem,
            this.hướngDẫnToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(242, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // chứcNăngToolStripMenuItem
            // 
            this.chứcNăngToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tínhTổngChiềuDàiPolylineToolStripMenuItem,
            this.lIBToolStripMenuItem,
            this.vẽBiểĐồToolStripMenuItem});
            this.chứcNăngToolStripMenuItem.Name = "chứcNăngToolStripMenuItem";
            this.chứcNăngToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.chứcNăngToolStripMenuItem.Text = "Chức năng";
            // 
            // tínhTổngChiềuDàiPolylineToolStripMenuItem
            // 
            this.tínhTổngChiềuDàiPolylineToolStripMenuItem.Name = "tínhTổngChiềuDàiPolylineToolStripMenuItem";
            this.tínhTổngChiềuDàiPolylineToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.tínhTổngChiềuDàiPolylineToolStripMenuItem.Text = "Tính tổng chiều dài polyline";
            this.tínhTổngChiềuDàiPolylineToolStripMenuItem.Click += new System.EventHandler(this.TínhTổngChiềuDàiPolylineToolStripMenuItem_Click);
            // 
            // lIBToolStripMenuItem
            // 
            this.lIBToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lineToolStripMenuItem});
            this.lIBToolStripMenuItem.Name = "lIBToolStripMenuItem";
            this.lIBToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.lIBToolStripMenuItem.Text = "LIB";
            this.lIBToolStripMenuItem.Click += new System.EventHandler(this.LIBToolStripMenuItem_Click);
            // 
            // lineToolStripMenuItem
            // 
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            this.lineToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.lineToolStripMenuItem.Text = "Line";
            this.lineToolStripMenuItem.Click += new System.EventHandler(this.LineToolStripMenuItem_Click);
            // 
            // hướngDẫnToolStripMenuItem
            // 
            this.hướngDẫnToolStripMenuItem.Name = "hướngDẫnToolStripMenuItem";
            this.hướngDẫnToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.hướngDẫnToolStripMenuItem.Text = "Hướng dẫn";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // pnl
            // 
            this.pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl.Location = new System.Drawing.Point(0, 24);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(242, 315);
            this.pnl.TabIndex = 2;
            // 
            // vẽBiểĐồToolStripMenuItem
            // 
            this.vẽBiểĐồToolStripMenuItem.Name = "vẽBiểĐồToolStripMenuItem";
            this.vẽBiểĐồToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.vẽBiểĐồToolStripMenuItem.Text = "Vẽ biể đồ";
            this.vẽBiểĐồToolStripMenuItem.Click += new System.EventHandler(this.VẽBiểĐồToolStripMenuItem_Click);
            // 
            // uct_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl);
            this.Controls.Add(this.menuStrip1);
            this.Name = "uct_main";
            this.Size = new System.Drawing.Size(242, 339);
            this.Load += new System.EventHandler(this.Uct_main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem chứcNăngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hướngDẫnToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tínhTổngChiềuDàiPolylineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lIBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineToolStripMenuItem;
        private System.Windows.Forms.Panel pnl;
        private System.Windows.Forms.ToolStripMenuItem vẽBiểĐồToolStripMenuItem;
    }
}
