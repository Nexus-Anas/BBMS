using System;
using System.IO;
using System.Windows.Forms;

namespace BBMSP
{
    public partial class Login : Form
    {
        /*This field is used to display the user's name in the notification*/
        public static string USER_FULLNAME, ADMIN_FULLNAME, CODE_SERIAL, CODE_CIN;


        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            radioUser.Checked = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void radioAdmin_CheckedChanged(object sender, EventArgs e)
        {
            if (radioAdmin.Checked)
                multiPage.SetPage("tabPage2");
        }

        private void radioUser_CheckedChanged(object sender, EventArgs e)
        {
            if (radioUser.Checked)
                multiPage.SetPage("tabPage1");
        }

        private void btnRecover_Click(object sender, EventArgs e)
        {
            multiPage.SetPage("tabPage3");
            MessageBox.Show("Check your folder ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            //Generate the recovery file
            SERIAL_CODE.PrintRecoveryCode(1);
            SERIAL_CODE.PrintRecoveryCin(2);

            StreamWriter sw = new StreamWriter("Recovery.txt", false);
            sw.Write($"Default Cin : {CODE_CIN}\nRecovery code : {CODE_SERIAL}");
            sw.Close();
        }

        private void btnManager_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtAdminname.Text) || string.IsNullOrEmpty(txtAdminPsw.Text))
                {
                    MessageBox.Show("Empty field..\nCheck again !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {
                    //Call the login method related to the administrator.
                    ADMIN admin = new ADMIN();
                    admin.GetAdmin(txtAdminname.Text, txtAdminname.Text, txtAdminPsw.Text);

                    //Display notification message.
                    notifyLogin.BalloonTipTitle = "Operation successfull";
                    notifyLogin.BalloonTipText = ($"Welcome {ADMIN_FULLNAME}");
                    notifyLogin.ShowBalloonTip(50);

                    //Move to the Manager form.
                    new Manager().Show();
                    Hide();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect Cin, Email or Password!\nPlease check your inputs..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRecoveryCin.Text) || string.IsNullOrEmpty(txtRecoveryCode.Text))
                {
                    MessageBox.Show("Empty field..\nCheck again !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }


                if ((txtRecoveryCin.Text != CODE_CIN) || (txtRecoveryCode.Text != CODE_SERIAL))
                    MessageBox.Show("Incorrect Cin or recovry code!\nPlease check your inputs..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                else
                {
                    //Display notification message.
                    notifyLogin.BalloonTipTitle = "Operation successfull";
                    notifyLogin.BalloonTipText = "Welcome";
                    notifyLogin.ShowBalloonTip(80);
                    new Manager().Show();
                    Hide();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something is Incorrect !\nPlease check your inputs..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnConfirmAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtAdminname.Text) || string.IsNullOrEmpty(txtAdminPsw.Text))
                {
                    MessageBox.Show("Empty field..\nCheck again !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {
                    //Call the login method related to the administrator.
                    ADMIN admin = new ADMIN();
                    admin.GetAdmin(txtAdminname.Text, txtAdminname.Text, txtAdminPsw.Text);

                    //Display notification message.
                    notifyLogin.BalloonTipTitle = "Operation successfull";
                    notifyLogin.BalloonTipText = ($"Welcome {ADMIN_FULLNAME}");
                    notifyLogin.ShowBalloonTip(50);

                    //Move to the Main form.
                    new MainForm().Show();
                    Hide();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect Cin, Email or Password!\nPlease check your inputs..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnConfirmUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtUserPsw.Text))
                {
                    MessageBox.Show("Empty fields..\nCheck again !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {
                    //User login.
                    var user = new USER();
                    user.GetUser(txtUsername.Text, txtUsername.Text, txtUserPsw.Text);

                    //Display notification message.
                    notifyLogin.BalloonTipTitle = "Operation successfull";
                    notifyLogin.BalloonTipText = ($"Welcome {USER_FULLNAME}");
                    notifyLogin.ShowBalloonTip(50);

                    //Move to the Main form.
                    new MainForm().Show();
                    Hide();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect Cin, Email or Password!\nPlease check your inputs..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
    }
}
