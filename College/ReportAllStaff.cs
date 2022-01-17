using System;
using System.Windows.Forms;

namespace College
{
    public partial class ReportAllStaff : Form
    {
        public ReportAllStaff()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReportStaffProfile_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
