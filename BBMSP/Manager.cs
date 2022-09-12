using System;
using System.Windows.Forms;

namespace BBMSP
{
    public partial class Manager : Form
    {
        public static string CIN;
        public Manager()
        {
            InitializeComponent();
        }

        private void Manager_Load(object sender, EventArgs e)
        {
            aDMINBindingSource.DataSource = ADMIN.DisplayAdmins();
            uSERBindingSource.DataSource = USER.DisplayUsers();
            iNFOSUSERBindingSource.DataSource = INFOS_USER.DisplayUsers();
            txtCIN.Text = CIN;

            P1txtPsw.UseSystemPasswordChar = true;
            P1txtCon.UseSystemPasswordChar = true;
        }
        private void P1chk_CheckedChanged(object sender, System.EventArgs e)
        {
            if (P1chk.Checked)
            {
                P1txtPsw.UseSystemPasswordChar = false;
                P1txtCon.UseSystemPasswordChar = false;
            }
            else if (!P1chk.Checked)
            {
                P1txtPsw.UseSystemPasswordChar = true;
                P1txtCon.UseSystemPasswordChar = true;
            }
        }

        private void P1btnCon_Click(object sender, System.EventArgs e)
        {
            string cin = txtCIN.Text;
            string psw = P1txtPsw.Text;
            string con = P1txtCon.Text;
            try
            {
                if (string.IsNullOrEmpty(cin) || string.IsNullOrEmpty(psw) || string.IsNullOrEmpty(con))
                    MessageBox.Show("Empty fields..\nCheck again !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                else if (psw != con)
                    MessageBox.Show("Password does not match..\nCheck again !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                else
                {
                    //Call the confirmation method related to the administrator.
                    ADMIN.ChangePsw(cin, psw);
                    //Display notification message.
                    notifyManager.BalloonTipTitle = "Operation successfull";
                    notifyManager.BalloonTipText = "Password changed successfully !";
                    notifyManager.ShowBalloonTip(50);

                    //Refresh the page
                    aDMINBindingSource.DataSource = ADMIN.DisplayAdmins();
                    UpdateP1();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid cin or password !\nPlease check again..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        void UpdateP1()
        {
            P1txtPsw.Clear();
            P1txtCon.Clear();
        }
        void UpdateP2()
        {
            P2txtFullname.ResetText(); P2txtCin.ResetText(); P2txtPhone.ResetText(); P2txtCity.ResetText();
            P2txtEmail.ResetText(); P2txtPsw.ResetText(); P2txtAddress.ResetText();
        }
        void UpdateP3()
        {
            aDMINBindingSource.DataSource = ADMIN.DisplayAdmins();
            P3txtFullname.ResetText(); P3txtCin.ResetText(); P3txtPhone.ResetText(); P3txtCity.ResetText();
            P3txtEmail.ResetText(); P3txtPsw.ResetText(); P3txtAddress.ResetText(); P3txtID.Visible = false;
        }
        void UpdateP4()
        {
            P4txtFullname.Clear(); P4txtCin.Clear(); P4txtPhone.Clear(); P4txtCity.Clear();
            P4txtEmail.Clear(); P4txtPsw.Clear(); P4txtAddress.Clear();
        }
        void UpdateP5()
        {
            uSERBindingSource.DataSource = USER.DisplayUsers();
            P5txtFullname.ResetText(); P5txtCin.ResetText(); P5txtPhone.ResetText(); P5txtCity.ResetText();
            P5txtEmail.ResetText(); P5txtPsw.ResetText(); P5txtAddress.ResetText(); P5txtID.Visible = false;
        }
        private void btnAddAdmin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(P2txtFullname.Text) || string.IsNullOrEmpty(P2txtCin.Text) ||
                string.IsNullOrEmpty(P2txtPhone.Text) || string.IsNullOrEmpty(P2txtCity.Text) ||
                string.IsNullOrEmpty(P2txtEmail.Text) || string.IsNullOrEmpty(P2txtPsw.Text) ||
                string.IsNullOrEmpty(P2txtAddress.Text))
                MessageBox.Show("Empty fields!\nPlease check your inputs..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            else if (string.IsNullOrEmpty(P2txtGenderM.Text) && string.IsNullOrEmpty(P2txtGenderF.Text))
                MessageBox.Show("Empty fields!\nPlease check your inputs..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            else
            {
                //Gender condition.
                string gender = null;
                if (P2txtGenderM.Checked)
                    gender = P2txtGenderM.Text;
                else if (P2txtGenderF.Checked)
                    gender = P2txtGenderF.Text;

                //Calling the methode which insert data into the database.
                ADMIN d = new ADMIN(P2txtFullname.Text, P2txtCin.Text, P2txtPhone.Text, P2txtEmail.Text, P2txtDob.Text, gender, P2txtCity.Text, P2txtAddress.Text, P2txtPsw.Text);
                d.AddAdmin();

                //Display notification message.
                notifyManager.BalloonTipTitle = "Operation successfull";
                notifyManager.BalloonTipText = "Administrator Added Successfully";
                notifyManager.ShowBalloonTip(50);

                //Refresh the page
                aDMINBindingSource.DataSource = ADMIN.DisplayAdmins();
                UpdateP2();
            }
        }

        private void P3txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ADMIN d = ADMIN.SearchAdmin(P3txtSearch.Text, P3txtSearch.Text, P3txtSearch.Text, P3txtSearch.Text, P3txtSearch.Text);
                P3txtFullname.Text = d.FULLNAME; P3txtCin.Text = d.CIN; P3txtPhone.Text = d.PHONE;
                P3txtCity.Text = d.CITY; P3txtEmail.Text = d.EMAIL; P3txtPsw.Text = d.PASSWORD;
                P3txtDob.Text = d.DOB; P3txtGender.Text = d.GENDER; P3txtAddress.Text = d.ADRESS;
                P3txtID.Text = d.ID.ToString(); P3txtID.Visible = true;
            }
            catch (Exception) { UpdateP3(); }
        }

        private void P3Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                P3txtID.Text = P3Dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                P3txtFullname.Text = P3Dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                P3txtCin.Text = P3Dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                P3txtPhone.Text = P3Dgv.Rows[e.RowIndex].Cells[3].Value.ToString();
                P3txtEmail.Text = P3Dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                P3txtDob.Text = P3Dgv.Rows[e.RowIndex].Cells[5].Value.ToString();
                P3txtGender.Text = P3Dgv.Rows[e.RowIndex].Cells[6].Value.ToString();
                P3txtCity.Text = P3Dgv.Rows[e.RowIndex].Cells[7].Value.ToString();
                P3txtAddress.Text = P3Dgv.Rows[e.RowIndex].Cells[8].Value.ToString();
                P3txtPsw.Text = P3Dgv.Rows[e.RowIndex].Cells[9].Value.ToString();
                P3txtID.Visible = true;
            }
            catch (Exception) { UpdateP3(); }
        }

        private void P3btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(P3txtFullname.Text) || string.IsNullOrEmpty(P3txtCin.Text) ||
                string.IsNullOrEmpty(P3txtPhone.Text) || string.IsNullOrEmpty(P3txtCity.Text) ||
                string.IsNullOrEmpty(P3txtEmail.Text) || string.IsNullOrEmpty(P3txtPsw.Text) ||
                string.IsNullOrEmpty(P3txtAddress.Text) || string.IsNullOrEmpty(P3txtGender.Text) ||
                string.IsNullOrEmpty(P3txtDob.Text) || string.IsNullOrEmpty(P3txtID.Text))
                MessageBox.Show("Empty fields!\nPlease check your inputs..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            else
            {
                try
                {
                    ADMIN.UpdateAdmin(Convert.ToInt32(P3txtID.Text), P3txtFullname.Text, P3txtCin.Text, P3txtPhone.Text, P3txtEmail.Text, P3txtDob.Text, P3txtGender.Text, P3txtCity.Text, P3txtAddress.Text, P3txtPsw.Text);
                    //Notification message!
                    notifyManager.BalloonTipTitle = "Operation successfull";
                    notifyManager.BalloonTipText = "Administrator has been updated successfully";
                    notifyManager.ShowBalloonTip(50);

                    //Update content.
                    aDMINBindingSource.DataSource = ADMIN.DisplayAdmins();
                    UpdateP3();
                }
                catch (Exception)
                {
                    MessageBox.Show("Something is Incorrect!\nPlease check your inputs..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void P3btnDelete_Click(object sender, EventArgs e)
        {
            var d = new ADMIN();
            d.ID = Convert.ToInt32(P3txtID.Text);
            d.DeleteAdmin();

            //Notification message!
            notifyManager.BalloonTipTitle = "Operation successfull";
            notifyManager.BalloonTipText = "Administrator has been removed successfully";
            notifyManager.ShowBalloonTip(50);

            //Update content.
            aDMINBindingSource.DataSource = ADMIN.DisplayAdmins();
            UpdateP3();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(P4txtFullname.Text) || string.IsNullOrEmpty(P4txtCin.Text) ||
                string.IsNullOrEmpty(P4txtPhone.Text) || string.IsNullOrEmpty(P4txtCity.Text) ||
                string.IsNullOrEmpty(P4txtEmail.Text) || string.IsNullOrEmpty(P4txtPsw.Text) ||
                string.IsNullOrEmpty(P4txtAddress.Text))
                MessageBox.Show("Empty fields!\nPlease check your inputs..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            else if (string.IsNullOrEmpty(P4txtGenderM.Text) && string.IsNullOrEmpty(P4txtGenderF.Text))
                MessageBox.Show("Empty fields!\nPlease check your inputs..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            else
            {
                try
                {
                    //Gender condition.
                    string gender = null;
                    if (P4txtGenderM.Checked)
                        gender = P4txtGenderM.Text;
                    else if (P4txtGenderF.Checked)
                        gender = P4txtGenderF.Text;

                    //Calling the methode which insert data into the database.
                    var user = new USER(P4txtFullname.Text, P4txtCin.Text, P4txtPhone.Text, P4txtEmail.Text, P4txtDob.Text, gender, P4txtCity.Text, P4txtAddress.Text, P4txtPsw.Text);
                    user.AddUser();

                    //Display notification message.
                    notifyManager.BalloonTipTitle = "Operation successfull";
                    notifyManager.BalloonTipText = "User Added Successfully";
                    notifyManager.ShowBalloonTip(50);

                    //Refresh the page
                    uSERBindingSource.DataSource = USER.DisplayUsers();
                    UpdateP4();
                }
                catch (Exception) { }

            }
        }

        private void P5txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                USER user = USER.SearchUser(P5txtSearch.Text, P5txtSearch.Text, P5txtSearch.Text, P5txtSearch.Text, P5txtSearch.Text);
                P5txtFullname.Text = user.FULLNAME; P5txtCin.Text = user.CIN; P5txtPhone.Text = user.PHONE;
                P5txtCity.Text = user.CITY; P5txtEmail.Text = user.EMAIL; P5txtPsw.Text = user.PASSWORD;
                P5txtDob.Text = user.DOB; P5txtGender.Text = user.GENDER; P5txtAddress.Text = user.ADRESS;
                P5txtID.Text = user.ID.ToString(); P5txtID.Visible = true;
            }
            catch (Exception) { UpdateP5(); }
        }

        private void P5btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(P5txtFullname.Text) || string.IsNullOrEmpty(P5txtCin.Text) ||
                string.IsNullOrEmpty(P5txtPhone.Text) || string.IsNullOrEmpty(P5txtCity.Text) ||
                string.IsNullOrEmpty(P5txtEmail.Text) || string.IsNullOrEmpty(P5txtPsw.Text) ||
                string.IsNullOrEmpty(P5txtAddress.Text) || string.IsNullOrEmpty(P5txtGender.Text) ||
                string.IsNullOrEmpty(P5txtDob.Text) || string.IsNullOrEmpty(P5txtID.Text))
                MessageBox.Show("Empty fields!\nPlease check your inputs..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            else
            {
                try
                {
                    USER.UpdateUser(Convert.ToInt32(P5txtID.Text), P5txtFullname.Text, P5txtCin.Text, P5txtPhone.Text, P5txtEmail.Text, P5txtDob.Text, P5txtGender.Text, P5txtCity.Text, P5txtAddress.Text, P5txtPsw.Text);
                    //Notification message!
                    notifyManager.BalloonTipTitle = "Operation successfull";
                    notifyManager.BalloonTipText = "User has been updated successfully";
                    notifyManager.ShowBalloonTip(50);

                    //Update content.
                    uSERBindingSource.DataSource = USER.DisplayUsers();
                    UpdateP5();
                }
                catch (Exception)
                {
                    MessageBox.Show("Something is Incorrect!\nPlease check your inputs..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void P5btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                USER user = new USER();
                user.ID = Convert.ToInt32(P5txtID.Text);
                user.DeleteUser();

                //Notification message!
                notifyManager.BalloonTipTitle = "Operation successfull";
                notifyManager.BalloonTipText = "User has been removed successfully";
                notifyManager.ShowBalloonTip(50);

                //Update content.
                uSERBindingSource.DataSource = USER.DisplayUsers();
                UpdateP5();
            }
            catch (Exception)
            {
                MessageBox.Show("Something is Incorrect!\nPlease check your inputs..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void P5Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                P5txtID.Text = P5Dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                P5txtFullname.Text = P5Dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                P5txtCin.Text = P5Dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                P5txtPhone.Text = P5Dgv.Rows[e.RowIndex].Cells[3].Value.ToString();
                P5txtEmail.Text = P5Dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                P5txtDob.Text = P5Dgv.Rows[e.RowIndex].Cells[5].Value.ToString();
                P5txtGender.Text = P5Dgv.Rows[e.RowIndex].Cells[6].Value.ToString();
                P5txtCity.Text = P5Dgv.Rows[e.RowIndex].Cells[7].Value.ToString();
                P5txtAddress.Text = P5Dgv.Rows[e.RowIndex].Cells[8].Value.ToString();
                P5txtPsw.Text = P5Dgv.Rows[e.RowIndex].Cells[9].Value.ToString();
                P5txtID.Visible = true;
            }
            catch (Exception) { UpdateP5(); }
        }

        private void P6txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var user = INFOS_USER.SearchUser(P6txtSearch.Text, P6txtSearch.Text);
            }
            catch (Exception) { UpdateP3(); }
        }

        private void btnCP_Click(object sender, EventArgs e)
        {
            MultiPage.SetPage("tabPage1");
        }

        private void btnAA_Click(object sender, EventArgs e)
        {
            MultiPage.SetPage("tabPage2");
        }

        private void btnMA_Click(object sender, EventArgs e)
        {
            MultiPage.SetPage("tabPage3");
        }

        private void btnAU_Click(object sender, EventArgs e)
        {
            MultiPage.SetPage("tabPage4");
        }

        private void btnMU_Click(object sender, EventArgs e)
        {
            MultiPage.SetPage("tabPage5");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            new Login().Show();
            Close();
        }

        private void btnCU_Click(object sender, EventArgs e)
        {
            MultiPage.SetPage("tabPage6");
        }
    }
}
