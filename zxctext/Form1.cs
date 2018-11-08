using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zxctext
{

    public partial class Form1 : Form
    {
        tblMain model = new tblMain();
        //Connection String  
        string cs = "data source=mylaptop\\sqlexpress;initial catalog=MultipleLogin;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework;multisubnetfailover=False";
        SqlConnection con;
        SqlDataAdapter adapt;
        DataTable dt;
        public Form1()
        {
            InitializeComponent();
            Clear();
            PopulateDataGridView();
        }

      
        void Clear()
        {
            txtUserName.Text = txtPersonName.Text = txtPhoneNumber.Text = txtType.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.Id = 0;
        }

        private void dgvMain_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMain.CurrentRow.Index != -1)
            {
                model.Id = Convert.ToInt32(dgvMain.CurrentRow.Cells["Id"].Value);
                using (DBEntities db = new DBEntities())
                {
                    model = db.tblMains.Where(x => x.Id == model.Id).FirstOrDefault();
                    txtUserName.Text = model.UserName;
                    txtPersonName.Text = model.PersonName;
                    txtPhoneNumber.Text = model.PhoneNumber;
                    txtType.Text = model.Type;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }
        void PopulateDataGridView()
        {
            dgvMain.AutoGenerateColumns = false;
            using (DBEntities db = new DBEntities())
            {
                dgvMain.DataSource = db.tblMains.ToList();

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure to Delete this Record ?", txtUserName.Text + "'s Phonebook", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.tblMains.Attach(model);
                    db.tblMains.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Deleted Successfully");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            model.UserName = txtUserName.Text;
            model.PersonName = txtPersonName.Text;
            model.PhoneNumber = txtPhoneNumber.Text;
            model.Type = txtType.Text;
            using (DBEntities db = new DBEntities())
            {
                if (model.Id == 0) //Insert
                {
                    db.tblMains.Add(model);
                    db.SaveChanges();
                    MessageBox.Show("Added New Contact.");
                }

                else //Update
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    MessageBox.Show("Updated Successfully");
                }
                    
            }
            Clear();
            PopulateDataGridView();
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            model.UserName = txtUserName.Text;
            model.PersonName = txtPersonName.Text;
            model.PhoneNumber = txtPhoneNumber.Text;
            model.Type = txtType.Text;
            using (DBEntities db = new DBEntities())
            {
                if (model.Id != 0) //Update
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    MessageBox.Show("Updated Successfully");
                }
            }
        }

       

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            con = new SqlConnection(cs);
            con.Open();
            
            //    SearchDirectionHint by Person Name Only
           adapt = new SqlDataAdapter("select * from tblMain where PersonName like '" + txtSearch.Text + "%'", con);
            dt = new DataTable();
            adapt.Fill(dt);
            dgvMain.DataSource = dt;
            con.Close();
        }
    }
}
