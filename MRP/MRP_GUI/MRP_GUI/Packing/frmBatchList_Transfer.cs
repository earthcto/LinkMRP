﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DL;
using SESD.MRP.REF;
using MRP_GUI.Production;


namespace MRP_GUI
{
    public partial class frmBatchList_Transfer : System.Windows.Forms.Form
    {

        User CurrentUser = new User();

        public frmBatchList_Transfer(User objUser)
        {
            InitializeComponent();
            CurrentUser = objUser;
        }

        Batch_DL objBatch_DL = new Batch_DL(ConnectionStringClass.GetConnection());
        MainActivity_DL objMainActivity_DL = new MainActivity_DL(ConnectionStringClass.GetConnection());
        BatchActivity_DL objBatchActivity_DL = new BatchActivity_DL(ConnectionStringClass.GetConnection());
        MachineActivity_DL objMachineActivity_DL = new MachineActivity_DL(ConnectionStringClass.GetConnection());
        BatchLabourDetails_DL objBatchLabourDetails_DL = new BatchLabourDetails_DL(ConnectionStringClass.GetConnection());
        BatchMachineDetails_DL objBatchMachineDetails_DL = new BatchMachineDetails_DL(ConnectionStringClass.GetConnection());
        Machine_DL objMachine_DL = new Machine_DL(ConnectionStringClass.GetConnection());
        Employee_DL objEmployee_DL = new Employee_DL(ConnectionStringClass.GetConnection());
        FinishProduct_DL objFinishProduct_DL = new FinishProduct_DL(ConnectionStringClass.GetConnection());
        QCReport_DL objQCReport_DL = new QCReport_DL(ConnectionStringClass.GetConnection());
        MTN_DL objMTN_DL = new MTN_DL(ConnectionStringClass.GetConnection());
        Department_DL objDepartment_DL = new Department_DL(ConnectionStringClass.GetConnection());

        Batch objBatch = new Batch();
        BatchActivity objBatchActivity = new BatchActivity();
        MainActivity objActivity = new MainActivity();

        bool Loaded = false;

        private void frmBatchList_Load(object sender, EventArgs e)
        {

            Load_Activity();

            objActivity = objMainActivity_DL.GetByName("Packing");

            
        }

       


        private void Load_Activity()
        {
            try
            {
                    DataTable dt = objBatchActivity_DL.GetView_ToTransfer_ToSecondary(Convert.ToInt32(BatchActivity.Status.Finished));
                    //objSourceActivityList.DataSource = dt;
                    dgvActivityz.AutoGenerateColumns = false;
                    dgvActivityz.DataSource = dt; //objSourceActivityList;
                    objSourceActivityList.ResetBindings(true);


               // SetColors();
            }
            catch (Exception)
            {

                MessageBox.Show(this, "Error occured while loading Batch List", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetColors()
        {
            for (int i = 0; i < dgvActivityz.Rows.Count; i++)
            {
                if ((dgvActivityz.Rows[i].Cells["BatchActStatus"].Value).ToString() == "2")
                {
                    dgvActivityz.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
                else
                {
                    dgvActivityz.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }

        private void dgvActivity_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {

                FinishProduct objFinishProduct = new FinishProduct();

                objFinishProduct = objFinishProduct_DL.Get(dgvActivityz.CurrentRow.Cells["PrimaryFinishProduct"].Value.ToString());


            objBatchActivity = objBatchActivity_DL.GetByID(Convert.ToInt32(dgvActivityz.CurrentRow.Cells["BatchActID"].Value));


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //private void btnFinishPacking_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DialogResult dr = MessageBox.Show(this,"Do you want to finish the Activity","Confirmation",MessageBoxButtons.YesNo);

        //        if(dr== DialogResult.Yes)
        //        {

        //        objBatchActivity = objBatchActivity_DL.GetByID(Convert.ToInt32(dgvActivity.CurrentRow.Cells["BatchActID"].Value));

        //        objBatchActivity.BatchActStatus = BatchActivity.Status.Finished;

        //        objBatchActivity_DL.Update(objBatchActivity);


        //        }

        //        Load_Activity();

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}


        private void btnSendtoPacking_Click(object sender, EventArgs e)
        {

            try
            {
                int theCount = 0;
                 DataTable dtReports=  objQCReport_DL.Get_IsPackingQCPass(dgvActivityz.CurrentRow.Cells["BatchID"].Value.ToString(), Convert.ToInt32(QCReport.ReportStatus.Accept),"Packing");

                 theCount=objFinishProduct_DL.Get_SecondaryProductList(dgvActivityz.CurrentRow.Cells["PrimaryFinishProduct"].Value.ToString()).Rows.Count;

                 if (dtReports.Rows.Count > 0)
                 {
                     if (theCount > 0)
                     {
                         objBatchActivity.BatchActStatus = BatchActivity.Status.SecondaryPacking;
                     }
                     else
                     {
                         objBatchActivity.BatchActStatus = BatchActivity.Status.Finished;
                     }

                     objBatchActivity_DL.Update(objBatchActivity);
                     objBatch_DL.Update_BatchPackingCost(objBatchActivity.BatchActID);

                     Load_Activity();
                 }
                 else
                 {
                     MessageBox.Show(this, "Packing QC is Not Passed", "Can not Proceed", MessageBoxButtons.OK);
                 }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

       


    }
}
