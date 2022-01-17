using College.App_Code;
using System;
using System.Data;
using System.Windows.Forms;
namespace College
{
    public partial class PayFee : Form
    {
        string id;
        public PayFee(string id)
        {
            InitializeComponent();
            this.id = id;
            loadData();
        }
        public void loadData()
        {
            try
            {
                Student std = new Student();
                std.studentId = this.id;
                DataTable dt = std.getStudentById();

                lblStudentId.Text = this.id;
                int cCode = Int32.Parse(dt.Rows[0]["classCode"].ToString());
                int bCode = Int32.Parse(dt.Rows[0]["batchCode"].ToString());
                lblStudentName.Text = dt.Rows[0]["name"].ToString();

                Batch b = new Batch();
                b.seriel = bCode;
                string batchName = b.getBatchName();
                ClassList cl = new ClassList();
                cl.seriel = cCode;
                string cName = cl.getClassName();

                lblBatch.Text = batchName + ", " + cName;

                Fee f = new Fee();
                f.studentId = this.id;

                double totalAmount = f.getTotaAmount();
                lblTotalAmount.Text = totalAmount.ToString() + " ( Exc Fine )";


                DataTable dtx = f.getAllUnpaid();
                studentGrid.DataSource = dtx;
            }
            catch (System.Exception ex)
            {

            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            YesNo y = new YesNo();
            y.ShowDialog();
            if (Common.command == true)
            {
                double fine = double.Parse(txtFine.Text);
                double admissionFee = double.Parse(txtAdmissionFee.Text);
                string month = studentGrid.SelectedRows[0].Cells[0].Value.ToString();
                int year = Int32.Parse(studentGrid.SelectedRows[0].Cells[1].Value.ToString());
                double tAmount = double.Parse(studentGrid.SelectedRows[0].Cells[8].Value.ToString());
                tAmount += fine;
                tAmount += admissionFee;

                Ledger l = new Ledger();
                l.type = "Credit";
                l.amount = tAmount;
                l.reason = "Monthly Fee";
                l.note = "--";
                l.date = DateTime.Parse(DateTime.Now.ToShortDateString());
                int lcode = l.addLedger();

                Fee f = new Fee();
                f.ledgerCode = lcode;
                f.studentId = this.id;
                f.month = month;
                f.year = year;
                f.fine = fine;
                f.admissionFee = admissionFee;
                f.totalAmount = tAmount;
                f.receivingDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                f.receivedBy = "Baqir";
                f.pay();

                ReportPayFee rp = new ReportPayFee(this.id, month, year);
                rp.ShowDialog();

                this.Close();
            }
            else
            {

            }
        }

        private void chkFine_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFine.Checked)
            {
                txtFine.Text = settings.fine.ToString();
            }
            else
            {
                txtFine.Text = "0";
            }
        }

        private void admi(object sender, EventArgs e)
        {
            if (chkAdmission.Checked)
            {
                txtAdmissionFee.Text = settings.admissionFee.ToString();
            }
            else
            {
                txtAdmissionFee.Text = "0";
            }
        }
    }
}
