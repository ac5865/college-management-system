using College.App_Code;
using System;
using System.Data;
using System.Windows.Forms;

namespace College
{
    public partial class PaySalary : Form
    {
        string empId;
        int yyyy = 2017;
        public PaySalary(string id)
        {
            InitializeComponent();
            this.empId = id;


            for (int i = yyyy; i <= (Int32.Parse(DateTime.Now.ToString("yyyy")) + 2); i++)
            {
                cboYear.Items.Add(i + "");
            }
            cboMonth.SelectedIndex = Int32.Parse(DateTime.Now.ToString("MM")) - 1;
            int year = Int32.Parse(DateTime.Now.ToString("yyyy"));

            int c = 0;
            for (int x = 0; x < cboYear.Items.Count; x++)
            {
                string str = cboYear.Items[x].ToString();
                if (str.Equals(year.ToString()))
                {
                    cboYear.SelectedIndex = c;
                    break;
                }
                c++;
            }

            this.empId = id;
            loadData();
            bindGrid();
        }
        public void bindGrid()
        {
            try
            {
                Payroll obj = new Payroll();
                obj.Emp = empId;
                obj.Year = DateTime.Now.Year;
                DataTable dt = obj.getPayrollOfYear();
                salaryGrid.DataSource = dt;
            }
            catch (Exception ex)
            {
                ErrorBox obj = new ErrorBox();
                obj.ShowDialog();
            }
        }
        double salary = 0.0;
        double grandTotal = 0.0;
        public void loadData()
        {
            //------------------------------------- get name
            Employee emp = new Employee();
            emp.EmployeeId = this.empId;
            string empName = emp.getName();
            lblName.Text = empName;
            lblId.Text = this.empId;
            salary = emp.getSalary();
            lblSalary.Text = salary.ToString();
            //-------------------------------------
            lblDate.Text = DateTime.Now.ToShortDateString();
            //--------------
            grandTotal = salary;
            lblGrandTotal.Text = grandTotal.ToString();
        }
        private void label8_Click(object sender, System.EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //------------ check for already paid salary


                Ledger l = new Ledger();
                l.type = "Debit";
                l.amount = grandTotal;
                l.reason = "Salary";
                l.note = "--";
                l.date = DateTime.Parse(DateTime.Now.ToShortDateString());
                int lcode = l.addLedger();

                Payroll p = new Payroll();
                p.ledgerCode = lcode;
                p.Month = cboMonth.Text;
                p.Year = Int32.Parse(cboYear.Text);
                p.Emp = empId;
                bool x = p.checkPaiment();
                if (x)
                {
                    ErrorBox err = new ErrorBox();
                    err.ShowDialog();
                }
                else
                {
                    p.Amount = salary;
                    p.TotalAmount = grandTotal;
                    p.Received = true;
                    p.Date = DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy"));
                    int id = p.paySalary();


                    ReportPayrollSlip ob = new ReportPayrollSlip(id);
                    ob.ShowDialog();


                    //-------------------
                    Done d = new Done();
                    d.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorBox obj = new ErrorBox();
                obj.ShowDialog();
            }
        }

        private void txtYear_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                Payroll obj = new Payroll();
                obj.Emp = empId;
                obj.Year = Int32.Parse(txtYear.Text);
                DataTable dt = obj.getPayrollOfYear();
                salaryGrid.DataSource = dt;
            }
            catch (Exception ex)
            {

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Int32.Parse(salaryGrid.SelectedRows[0].Cells[0].Value.ToString());
                ReportPayrollSlip obj = new ReportPayrollSlip(id);
                obj.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
