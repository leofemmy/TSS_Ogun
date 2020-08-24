using System;
using System.Windows.Forms;
using TaxSmartSuite.Class ;

namespace Normalization.Forms
{
    public partial class MDIMain : Form
    {
        private int childFormNumber = 0;

        public MDIMain()
        {
            InitializeComponent();
        }

        private void ShowNewForm(EventArgs e)
        {
            Form childForm = new Form { MdiParent = this, Text = "Window " + childFormNumber++ };
            childForm.Show();
        }

        private void OpenFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog { InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal), Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*" })
            {
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string FileName = openFileDialog.FileName;
                }
            }
        }

        private void SaveAsToolStripMenuItem_Click(EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog { InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal), Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*" })
            {
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string FileName = saveFileDialog.FileName;
                }
            }
        }

        private void ExitToolsStripMenuItem_Click()
        {
            Close();
        }

        private static void CutToolStripMenuItem_Click()
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private static void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }


        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void _004_001_001_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void _004_004_001_Click(object sender, EventArgs e)
        {
            if (sender == null || e == null)
                return;
            new FrmNormalize { MdiParent = this }.Show();
        }

        private void _004_002_001_Click(object sender, EventArgs e)
        {
            if (sender == null || e == null)
                return;
            new FrmDownload {MdiParent = this  }.Show();
        }

        private void _004_003_002_Click(object sender, EventArgs e)
        {
            if (sender == null || e == null)
                return;
            FrmImports.tableType = 2;
                new FrmImports { MdiParent = this }.Show ();
        }

        private void _004_003_003_Click(object sender, EventArgs e)
        {

            if (sender == null || e == null)
                return;
            FrmImports.tableType = 3;
            new FrmImports { MdiParent = this }.Show();
        }

        private void _004_003_001_Click(object sender, EventArgs e)
        {

            if (sender == null || e == null)
                return;
            FrmImports.tableType = 1;
            new FrmImports { MdiParent = this }.Show();
        }
    }
}
