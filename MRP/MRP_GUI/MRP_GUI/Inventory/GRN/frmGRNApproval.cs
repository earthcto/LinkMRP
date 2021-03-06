using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DL;
using SESD.MRP.REF;

namespace MRP_GUI
{
    public partial class frmGRNApproval : System.Windows.Forms.Form
    {

        public frmGRNApproval(User objUser, string _StoreID)
        {
            InitializeComponent();
            CurrentUser = objUser;
            StoreID = _StoreID;
        }


        //------------------------ Variables ----------------------------
        private User _objCurrentUser;

        public User CurrentUser
        {
            get { return _objCurrentUser; }
            set { _objCurrentUser = value; }
        }

        private GRN objGRN;
        private GRN_Payment objGRN_Payment;
        private Store objStore = new Store();
        private GRN_DL objGRNDL = new GRN_DL(ConnectionStringClass.GetConnection());
        private GRN_Payment_DL objGRN_Payment_DL = new GRN_Payment_DL(ConnectionStringClass.GetConnection());
        private Department_DL objDepartment_DL = new Department_DL(ConnectionStringClass.GetConnection());
        private GRNCollec objGRNCollec = new GRNCollec();

        string StoreID = "";

        private Store_DL objStoreDL = new Store_DL(ConnectionStringClass.GetConnection());


        //---------------------------------------------------------------

      

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(this, "Are you sure you want to close? Click Yes to close", "Confirm Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void frmGRNApproval_Load(object sender, EventArgs e)
        {
            try
            {

                objStore = objStoreDL.Get(StoreID);

                txtStore.Text = objStore.StoreDescription;

                DataTable dtGRN = new DataTable();

                dtGRN = objGRNDL.GetDataView(objStore.StoreID, GRN.Status.Initial);
                bindGRNList.DataSource = dtGRN;

                gvGRNList.AutoGenerateColumns = false;
                gvGRNList.DataSource = bindGRNList;
                bindGRNList.ResetBindings(true);
                
            }
            catch (Exception ex)
            {
                
               MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      

        private void gvGRNList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    objGRN = new GRN();
                    bindItemList.DataSource = null;
                   objGRN =objGRNDL.Get(Convert.ToInt64(gvGRNList.Rows[e.RowIndex].Cells["ColGRNNO"].Value));
                    if (objGRN != null)
                    {
                        bindItemList.DataSource = objGRNDL.GetDataView_Items(objGRN.GRNNo);

                        objGRN_Payment = objGRN_Payment_DL.GetByGRNNO(objGRN.GRNNo);

                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {

                if (objGRN != null)
                {

                    DialogResult dr = MessageBox.Show(this, "Are you sure you want Approve Selected GRN? Click Yes to Approve", "Confirm Approve", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dr == DialogResult.Yes)
                    {


                        objGRN = objGRNDL.Get(objGRN.GRNNo);
                        if (objGRN != null && objGRN.GRNStatus == GRN.Status.Initial)
                        {


                            objGRN.GRNApprovedBy = CurrentUser.UserEmp.EmployeeID;
                            int y = objGRNDL.Approve(objGRN);


                            objGRN = null;
                            bindItemList.DataSource = null;
                            bindGRNList.DataSource = objGRNDL.GetDataView(objStore.StoreID, GRN.Status.Initial);
                        }
                        else
                        {
                            MessageBox.Show(this,"Another user changed the Status of selected GRN","Action Stoped",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                            bindGRNList.DataSource = objGRNDL.GetDataView(objStore.StoreID, GRN.Status.Initial);
                            bindItemList.DataSource = null;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                if (objGRN != null)
                {
                    DialogResult dr = MessageBox.Show(this,"Are you sure you want Reject Selected GRN? Click Yes to Reject","Confirm Reject",MessageBoxButtons.YesNo,MessageBoxIcon.Information);


                    if (dr == DialogResult.Yes)
                    {
                        objGRN = objGRNDL.Get(objGRN.GRNNo);
                        if (objGRN != null && objGRN.GRNStatus == GRN.Status.Initial)
                        {
                            objGRNDL.Update(objGRN.GRNNo, GRN.Status.Reject);

                            objGRN = null;
                            bindItemList.DataSource = null;
                            bindGRNList.DataSource = objGRNDL.GetDataView(objStore.StoreID, GRN.Status.Initial);
                        }
                        else
                        {
                            MessageBox.Show(this, "Another user changed the Status of selected GRN", "Action Stoped", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            bindGRNList.DataSource = objGRNDL.GetDataView(objStore.StoreID, GRN.Status.Initial);
                            bindItemList.DataSource = null;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}