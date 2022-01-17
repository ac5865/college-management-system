using College.App_Code;
using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;

namespace College
{
    public partial class ReportClassAttendance : Form
    {
        public ReportClassAttendance()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReportClassAttendance_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'collegeDataSet.settings' table. You can move, or remove it, as needed.
            SelectBatch obj = new SelectBatch();
            obj.ShowDialog();
            if (Common.command == true)
            {
                try
                {
                    // TODO: This line of code loads data into the 'PlantDataSet.CurrentStockByCategory' table. You can move, or remove it, as needed.
                    this.classAttendanceTableAdapter.Fill(this.collegeDataSet.ClassAttendance, Common.month, Common.year.ToString(), Common.batchCode, Common.classCode);
                    this.settingsTableAdapter.Fill(this.collegeDataSet.settings);
                    reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                    reportViewer1.ZoomMode = ZoomMode.Percent;
                    reportViewer1.ZoomPercent = 100;
                    this.reportViewer1.RefreshReport();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
