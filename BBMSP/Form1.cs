using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace BBMSP
{
    public partial class MainForm : Form
    {

        public static string Login_Time, Logout_Time, CIN;
        public static bool access;

        public MainForm()
        {
            InitializeComponent();
        }



        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*////////////////////////////////////////////THIS IS THE NAVIGATING SECTION///////////////////////////////////////////////*/

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            //Indicator movement
            Indicator.Top = ((Control)sender).Top;
            //Dashboard page selection
            MultiPage.SetPage("tabPage1");
            //Update content
            UpdateDashboardContent();
        }

        private void btnAddDonor_Click(object sender, EventArgs e)
        {
            Indicator.Top = ((Control)sender).Top;
            MultiPage.SetPage("tabPage2");
        }

        private void btnDonorsInfos_Click(object sender, EventArgs e)
        {
            Indicator.Top = ((Control)sender).Top;
            MultiPage.SetPage("tabPage3");
            UpdateDIContent();
        }

        private void btnDonation_Click(object sender, EventArgs e)
        {
            Indicator.Top = ((Control)sender).Top;
            MultiPage.SetPage("tabPage4");
            UpdateDOContent();
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            Indicator.Top = ((Control)sender).Top;
            MultiPage.SetPage("tabPage5");
        }

        private void btnPatientsInfos_Click(object sender, EventArgs e)
        {
            Indicator.Top = ((Control)sender).Top;
            MultiPage.SetPage("tabPage6");
            UpdatePIContent();
        }

        private void btnInjection_Click(object sender, EventArgs e)
        {
            Indicator.Top = ((Control)sender).Top;
            MultiPage.SetPage("tabPage7");
            UpdateINContent();
        }

        private void btnBloodStock_Click(object sender, EventArgs e)
        {
            Indicator.Top = ((Control)sender).Top;
            MultiPage.SetPage("tabPage8");
            UpdateBloodStockContent();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            Indicator.Top = ((Control)sender).Top;
            MultiPage.SetPage("tabPage9");
        }



        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*/////////////////////////////////////////THIS IS THE "FORM LOAD-DASHBOARD" SECTION///////////////////////////////////////*/

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Display donors list in the DGV.
            displayDonorsBindingSource.DataSource = DisplayDonors.DonorsList();

            //Display patients list in the DGV.
            displayPatientsBindingSource.DataSource = DisplayPatients.PatientsList();

            //Display the blood infos such as type and quantity in the DGV on the Blood Stock page.
            tBLOODBindingSource.DataSource = TBLOOD.GetBloodType();

            //Display cities on the combobox.
            CitiesList();

            //Update content.
            UpdateDashboardContent();

            //Report if blood quantity should be filled or not.
            //CheckBloodQuantity("A+"); CheckBloodQuantity("A-"); CheckBloodQuantity("O+"); CheckBloodQuantity("O-");
            //CheckBloodQuantity("B+"); CheckBloodQuantity("B-"); CheckBloodQuantity("AB+"); CheckBloodQuantity("AB-");

            //Display today's date and time on the dashboard page.
            timerDate.Start();

            //Insert user / admin's cin on the top of the application.
            txtCIN.Text = CIN;
        }

        private void btnCallAdmin_Click(object sender, EventArgs e)
        {
            //Display admins phone number.
            ADMIN.CallAdmins();
        }



        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*///////////////////////////////////////////THIS IS THE BROWSER'S SECTION/////////////////////////////////////////////////*/

        //Search for Office.
        private async Task Initialize_Office()
        {
            await webViewOffice.EnsureCoreWebView2Async(null);
        }
        public async void InitBrowserOffice()
        {
            await Initialize_Office();
            webViewOffice.CoreWebView2.Navigate("https://www.microsoft.com/fr-ww/microsoft-365/free-office-online-for-the-web");
        }
        private void btnOffice_Click(object sender, EventArgs e)
        {
            MultiWindow.SetPage("tabPage14");
        }

        //Search for Google Maps.
        private async Task Initialize_Maps()
        {
            await webViewMaps.EnsureCoreWebView2Async(null);
        }
        public async void InitBrowserMaps()
        {
            await Initialize_Maps();
            webViewMaps.CoreWebView2.Navigate("https://www.google.com/maps/place/Quartier+Hassan,+Rabat/@33.9660809,-6.8614017,11.89z/data=!4m13!1m7!3m6!1s0xd0b88619651c58d:0xd9d39381c42cffc3!2sMaroc!3b1!8m2!3d31.791702!4d-7.09262!3m4!1s0xda76b8f44375f51:0xb2ca6a567f1d8dc4!8m2!3d34.0222711!4d-6.8270588?hl=fr");
        }
        private void btnMaps_Click(object sender, EventArgs e)
        {
            MultiWindow.SetPage("tabPage12");
        }

        //Search for Whatsapp web.
        private async Task Initialize_Wts()
        {
            await webViewWts.EnsureCoreWebView2Async(null);
        }
        public async void InitBrowserWts()
        {
            await Initialize_Wts();
            webViewWts.CoreWebView2.Navigate("https://web.whatsapp.com/");
        }
        private void btnWts_Click(object sender, EventArgs e)
        {
            MultiWindow.SetPage("tabPage15");
        }

        //Turn on the browser.
        private void btnConnect_Click(object sender, EventArgs e)
        {
            InitBrowserMaps(); InitBrowserOffice(); InitBrowserWts();
            btnMaps.Visible = true; btnOffice.Visible = true; btnWts.Visible = true;
            txtConnect.Text = "Connected"; txtConnect.ForeColor = Color.Green;
            CircleConnect.Visible = true; btnConnect.Visible = false;
        }



        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*/////////////////////////////////////////THIS IS THE DONOR'S SECTION/////////////////////////////////////////////////////*/

        //Save donor infos.
        private void btnConfirmAD_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNameAD.Text) || string.IsNullOrEmpty(txtCinAD.Text) || string.IsNullOrEmpty(txtEmailAD.Text) ||
                string.IsNullOrEmpty(txtPhoneAD.Text) || string.IsNullOrEmpty(txtDobAD.Text) || string.IsNullOrEmpty(txtBloodAD.Text) ||
                string.IsNullOrEmpty(txtCityAD.Text) || string.IsNullOrEmpty(txtAdressAD.Text))
                    MessageBox.Show("Empty fields!\nPlease check your inputs..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                else if (string.IsNullOrEmpty(txtGenderMAD.Text) && string.IsNullOrEmpty(txtGenderFAD.Text))
                    MessageBox.Show("Empty fields!\nPlease check your inputs..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                else
                {
                    //Gender condition.
                    string gender = null;
                    if (txtGenderMAD.Checked)
                        gender = txtGenderMAD.Text;
                    else if (txtGenderFAD.Checked)
                        gender = txtGenderFAD.Text;

                    //Calling the methode which insert data into the database.
                    TDONOR d = new TDONOR(txtNameAD.Text, txtCinAD.Text, txtPhoneAD.Text, txtEmailAD.Text, txtDobAD.Text, gender, Convert.ToInt32(txtBloodAD.SelectedValue), txtCityAD.Text, txtAdressAD.Text, txtAllergiesAD.Text);
                    d.AddDonor();

                    //Display notification message.
                    notifyDonor.BalloonTipTitle = "Operation successfull";
                    notifyDonor.BalloonTipText = "Donor Added Successfully";
                    notifyDonor.ShowBalloonTip(50);

                    //Update content.
                    UpdateADContent(); UpdateDashboardContent(); UpdateDIContent();
                }
            }
            catch (Exception) { TDONOR.Exception(); }
        }

        //Donors list DGV click code which fill the textboxs and the necessary areas.
        private void dgvDI_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblIDDI.Text = dgvDI.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtNameDI.Text = dgvDI.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtCinDI.Text = dgvDI.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtPhoneDI.Text = dgvDI.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtEmailDI.Text = dgvDI.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtDobDI.Text = dgvDI.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtGenderDI.Text = dgvDI.Rows[e.RowIndex].Cells[6].Value.ToString();
                txtBloodDI.Text = dgvDI.Rows[e.RowIndex].Cells[7].Value.ToString();
                txtCityDI.Text = dgvDI.Rows[e.RowIndex].Cells[8].Value.ToString();
                txtAdressDI.Text = dgvDI.Rows[e.RowIndex].Cells[9].Value.ToString();
                txtAllergiesDI.Text = dgvDI.Rows[e.RowIndex].Cells[10].Value.ToString();
                lblIDDI.Visible = true;
            }
            catch (Exception) { }
        }

        //Delete the selected donor.
        private void btnDeleteDI_Click(object sender, EventArgs e)
        {
            if (access)
            {
                try
                {
                    TDONOR d = new TDONOR();
                    d.ID = Convert.ToInt32(lblIDDI.Text);
                    d.DeleteDonor();//<--Methode

                    //Notification message!
                    notifyDonor.BalloonTipTitle = "Operation successfull";
                    notifyDonor.BalloonTipText = "Donor has been removed successfully";
                    notifyDonor.ShowBalloonTip(50);

                    //Update content.
                    UpdateDIContent(); UpdateDashboardContent();
                }
                catch (Exception) { TDONOR.Exception(); }
            }
            else if (!access)
                MessageBox.Show("Only administrators have the authority to do this operation !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

        }

        //Update donor's infos.
        private void btnUpdateDI_Click(object sender, EventArgs e)
        {
            if (access)
            {
                int id, blood; string fullname, cin, phone, email, dob, gender, city, adress, allergies;
                //Check if null values exists.
                TDONOR.CheckNullDI(txtNameDI.Text, txtCinDI.Text, txtEmailDI.Text, txtPhoneDI.Text, txtDobDI.Text, txtBloodDI.Text, txtCityDI.Text, txtAdressDI.Text, txtGenderDI.Text);

                try
                {
                    id = Convert.ToInt32(lblIDDI.Text);
                    blood = Convert.ToInt32(txtBloodDI.SelectedValue);
                    fullname = txtNameDI.Text; cin = txtCinDI.Text; phone = txtPhoneDI.Text; email = txtEmailDI.Text; dob = txtDobDI.Text;
                    gender = txtGenderDI.Text; city = txtCityDI.Text; adress = txtAdressDI.Text; allergies = txtAllergiesDI.Text;
                    TDONOR.UpdateDonor(id, fullname, cin, phone, email, dob, gender, blood, city, adress, allergies);

                    //Notification message!
                    notifyDonor.BalloonTipTitle = "Operation successfull";
                    notifyDonor.BalloonTipText = "Donor has been updated successfully";
                    notifyDonor.ShowBalloonTip(50);

                    //Update content.
                    UpdateDIContent();

                }
                catch (Exception) { TDONOR.Exception(); }
            }
            else if (!access)
                MessageBox.Show("Only administrators have the authority to do this operation !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);


        }

        private void txtSearchDI_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TDONOR d = TDONOR.SearchDonor(txtSearchDI.Text, txtSearchDI.Text, txtSearchDI.Text, txtSearchDI.Text, txtSearchDI.Text);
                lblIDDI.Text = d.ID.ToString(); txtNameDI.Text = d.FULLNAME; txtCinDI.Text = d.CIN; txtPhoneDI.Text = d.PHONE;
                txtEmailDI.Text = d.EMAIL; txtDobDI.Text = d.DOB; txtGenderDI.Text = d.GENDER; txtBloodDI.Text = d.TBLOOD.BLOODTYPE;
                txtCityDI.Text = d.CITY; txtAdressDI.Text = d.ADRESS; txtAllergiesDI.Text = d.ALLERGIES; lblIDDI.Visible = true;
            }
            catch (Exception) { UpdateDIContent(); }
        }

        //Donation button code.
        private void btnDonateDO_Click(object sender, EventArgs e)
        {
            try
            {
                //Inserting donor's data into Donation table.
                string date = TodayDate();
                TDONATION.DonorInfos(Int32.Parse(txtIDDO.Text), txtCinDO.Text, date, Convert.ToInt32(txtQuantityDO.Text));
                //Notification message.
                notifyDonor.BalloonTipTitle = "Operation successfull";
                notifyDonor.BalloonTipText = "Donation Complete";
                notifyDonor.ShowBalloonTip(70);

                //Insert blood quantity in the blood stock.
                TBLOOD.Donation(txtBloodDO.Text, Int32.Parse(txtQuantityDO.Text));

                //Display data in DGV.
                tDONATIONBindingSource.DataSource = TDONATION.DisplayDonorsData(Convert.ToInt32(txtIDDO.Text));

                //Update content.
                UpdateDOContent(); UpdateBloodStockContent(); UpdateDashboardContent();
            }
            catch (Exception)
            {
                MessageBox.Show("Please Check Data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                //Update content.
                UpdateDOContent(); UpdateDashboardContent(); BloodStats();
                tBLOODBindingSource.DataSource = TBLOOD.GetBloodType();
            }
        }

        //The following bloc of code is related to the donor's searchbar in the "DONATION" page.
        private void txtSearchDO_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TDONOR d = TDONOR.SearchDonor(txtSearchDO.Text, txtSearchDO.Text, txtSearchDO.Text, txtSearchDO.Text, txtSearchDO.Text);
                txtIDDO.Text = d.ID.ToString(); txtNameDO.Text = d.FULLNAME; txtCinDO.Text = d.CIN;
                txtBloodDO.Text = d.TBLOOD.BLOODTYPE; txtAllergiesDO.Text = d.ALLERGIES;
                txtIDDO.Visible = true; txtBloodDO.Visible = true;
                tDONATIONBindingSource.DataSource = TDONATION.DisplayDonorsData(Convert.ToInt32(txtIDDO.Text));
            }
            catch (Exception) { UpdateDOContent(); }
        }

        //This button takes you to the Invoice form to print the donor's statement.
        private void btnGenerateDO_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = txtCinDO.Text;
                string path = @"C:\Users\msi\Desktop\Files\";
                string extension = ".docx";
                string fullPath = path + fileName + extension;
                CreateWordDocument(@"D:\Apps\Programming\Visual Studio\Projects\BBMSP\BBMSP\temp.docx", fullPath, Convert.ToInt32(txtIDDO.Text));
            }
            catch (Exception)
            {
                MessageBox.Show("Please Check Data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }





        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*/////////////////////////////////////////THIS IS THE PATIENT'S SECTION///////////////////////////////////////////////////*/

        //Save patient infos.
        private void btnConfirmAP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNameAP.Text) || string.IsNullOrEmpty(txtCinAP.Text) ||
               string.IsNullOrEmpty(txtEmailAP.Text) || string.IsNullOrEmpty(txtDobAP.Text) ||
               string.IsNullOrEmpty(txtBloodAP.Text) || string.IsNullOrEmpty(txtCityAP.Text) ||
               string.IsNullOrEmpty(txtAdressAP.Text) || string.IsNullOrEmpty(txtPhoneAP.Text))
            {
                MessageBox.Show("Empty fields!\nPlease check your inputs..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            else
            {
                try
                {
                    string gender = null;
                    if (txtGenderMAP.Checked)//<--Gender condition.
                    {
                        gender = txtGenderMAP.Text;
                    }
                    else if (txtGenderFAP.Checked)
                    {
                        gender = txtGenderFAP.Text;
                    }

                    //Calling the methode which stock the values in the database.
                    TPATIENT p = new TPATIENT(txtNameAP.Text, txtCinAP.Text, txtPhoneAP.Text, txtEmailAP.Text, txtDobAP.Text, gender, Convert.ToInt32(txtBloodAP.SelectedValue), txtCityAP.Text, txtAdressAP.Text, txtAllergiesAP.Text);
                    p.AddPatient();

                    //Display notification message.
                    notifyPatient.BalloonTipTitle = "Operation successfull";
                    notifyPatient.BalloonTipText = "Patient Added Successfully";
                    notifyPatient.ShowBalloonTip(50);

                    //Update content.
                    UpdateAPContent(); UpdateDashboardContent(); UpdatePIContent();
                }
                catch (Exception)
                {
                    MessageBox.Show("Something is Incorrect!\nPlease check your inputs..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }

        //Patients list DGV click code which fill the textboxs and the necessary areas.
        private void dgvPI_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblIDPI.Text = dgvPI.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtNamePI.Text = dgvPI.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtCinPI.Text = dgvPI.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtPhonePI.Text = dgvPI.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtEmailPI.Text = dgvPI.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtDobPI.Text = dgvPI.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtGenderPI.Text = dgvPI.Rows[e.RowIndex].Cells[6].Value.ToString();
                txtBloodPI.Text = dgvPI.Rows[e.RowIndex].Cells[7].Value.ToString();
                txtCityPI.Text = dgvPI.Rows[e.RowIndex].Cells[8].Value.ToString();
                txtAdressPI.Text = dgvPI.Rows[e.RowIndex].Cells[9].Value.ToString();
                txtAllergiesPI.Text = dgvPI.Rows[e.RowIndex].Cells[10].Value.ToString();
                lblIDPI.Visible = true;
            }
            catch (Exception) { }
        }

        //Delete the selected patient.
        private void btnDeletePI_Click(object sender, EventArgs e)
        {
            if (access)
            {
                try
                {
                    TPATIENT p = new TPATIENT();
                    p.ID = Convert.ToInt32(lblIDPI.Text);
                    p.DeletePatient();

                    //Notification message!
                    notifyPatient.BalloonTipTitle = "Operation successfull";
                    notifyPatient.BalloonTipText = "Patient has been removed successfully";
                    notifyPatient.ShowBalloonTip(50);

                    //Update content.
                    UpdatePIContent(); UpdateDashboardContent();
                }
                catch (Exception ex)
                {
                    if (lblIDPI.Text == string.Empty)
                    {
                        MessageBox.Show("No one found..\nCheck again !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    }
                    else
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            else if (!access)
                MessageBox.Show("Only administrators have the authority to do this operation !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

        }

        //Update the patient's infos.
        private void btnUpdatePI_Click(object sender, EventArgs e)
        {
            if (access)
            {
                int id, blood; string fullname, cin, phone, email, dob, gender, city, adress, allergies;
                if (string.IsNullOrEmpty(txtNamePI.Text) || string.IsNullOrEmpty(txtCinPI.Text) || string.IsNullOrEmpty(txtPhonePI.Text) ||
                   string.IsNullOrEmpty(txtEmailPI.Text) || string.IsNullOrEmpty(txtDobPI.Text) || string.IsNullOrEmpty(txtGenderPI.Text) ||
                   string.IsNullOrEmpty(txtBloodPI.Text) || string.IsNullOrEmpty(txtCityPI.Text) ||
                   string.IsNullOrEmpty(txtAdressPI.Text))
                {
                    MessageBox.Show("Missing fields..\nCheck again !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    try
                    {
                        id = Convert.ToInt32(lblIDPI.Text);
                        blood = Convert.ToInt32(txtBloodPI.SelectedValue);
                        fullname = txtNamePI.Text; cin = txtCinPI.Text; phone = txtPhonePI.Text; email = txtEmailPI.Text; dob = txtDobPI.Text;
                        gender = txtGenderPI.Text; city = txtCityPI.Text; adress = txtAdressPI.Text; allergies = txtAllergiesPI.Text;
                        TPATIENT.UpdatePatient(id, fullname, cin, phone, email, dob, gender, blood, city, adress, allergies);

                        //Notification message!
                        notifyPatient.BalloonTipTitle = "Operation successfull";
                        notifyPatient.BalloonTipText = "Patient has been updated successfully";
                        notifyPatient.ShowBalloonTip(50);

                        //Update content.
                        UpdatePIContent();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }
            }
            else if (!access)
                MessageBox.Show("Only administrators have the authority to do this operation !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);


        }

        //The following bloc of code is related to the patient's searchbar in the "PATIENT'S LIST" page.
        private void txtSearchPI_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TPATIENT p = TPATIENT.SearchPatient(txtSearchPI.Text, txtSearchPI.Text, txtSearchPI.Text, txtSearchPI.Text, txtSearchPI.Text);
                lblIDPI.Text = p.ID.ToString(); txtNamePI.Text = p.FULLNAME; txtCinPI.Text = p.CIN; txtPhonePI.Text = p.PHONE;
                txtEmailPI.Text = p.EMAIL; txtDobPI.Text = p.DOB; txtGenderPI.Text = p.GENDER; txtBloodPI.Text = p.TBLOOD.BLOODTYPE;
                txtCityPI.Text = p.CITY; txtAdressPI.Text = p.ADRESS; txtAllergiesPI.Text = p.ALLERGIES; lblIDPI.Visible = true;
            }
            catch (Exception) { UpdatePIContent(); }
        }

        //Injection button code.
        private void btnInjectIN_Click(object sender, EventArgs e)
        {
            try
            {
                MyLinqDataContext link = new MyLinqDataContext();
                TBLOOD b = (from n in link.TBLOODs where n.BLOODTYPE == comboBloodIN.Text select n).Single();
                if (b.QUANTITY > 0 && Convert.ToInt32(txtQuantityIN.Text) <= b.QUANTITY)
                {
                    b.QUANTITY -= Convert.ToInt32(txtQuantityIN.Text);

                    string date = TodayDate();
                    TINJECTION.InfosPatient(Int32.Parse(txtIDIN.Text), txtCinIN.Text, date, Convert.ToInt32(txtQuantityIN.Text));

                    //Display notification.
                    notifyPatient.BalloonTipTitle = "Operation successfull";
                    notifyPatient.BalloonTipText = "Injection Complete !";
                    notifyPatient.ShowBalloonTip(50);

                    //Show data in DGV.
                    tINJECTIONBindingSource.DataSource = TINJECTION.DisplayPatientsData(Convert.ToInt32(txtIDIN.Text));

                    //Update content.
                    UpdateINContent();
                    UpdateDashboardContent(); BloodStats(); tBLOODBindingSource.DataSource = TBLOOD.GetBloodType();
                }
                else if (b.QUANTITY >= 0 && Convert.ToInt32(txtQuantityIN.Text) > b.QUANTITY)
                {
                    //Display notification.
                    notifyPatient.BalloonTipTitle = "Operation faild";
                    notifyPatient.BalloonTipText = "Quantity Unavailable !";
                    notifyPatient.ShowBalloonTip(50);

                    //Show data in DGV.
                    tINJECTIONBindingSource.DataSource = TINJECTION.DisplayPatientsData(Convert.ToInt32(txtIDIN.Text));

                    //Update content.
                    UpdateINContent();
                }
                else
                {
                    MessageBox.Show("Please Check Data...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                link.SubmitChanges();

            }
            catch (Exception)
            {
                MessageBox.Show("Please Check Data...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        //The following bloc of code is related to the patient's searchbar in the "INJECTION" page.
        private void txtSearchIN_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TPATIENT p = TPATIENT.SearchPatient(txtSearchIN.Text, txtSearchIN.Text, txtSearchIN.Text, txtSearchIN.Text, txtSearchIN.Text);
                txtIDIN.Text = p.ID.ToString(); txtNameIN.Text = p.FULLNAME; txtCinIN.Text = p.CIN;
                txtBloodIN.Text = p.TBLOOD.BLOODTYPE; txtAllergiesIN.Text = p.ALLERGIES;
                txtIDIN.Visible = true; txtBloodIN.Visible = true;
                //Display data in DGV.
                tINJECTIONBindingSource.DataSource = TINJECTION.DisplayPatientsData(Convert.ToInt32(txtIDIN.Text));
                //Blood Condition
                BloodCondition();
            }
            catch (Exception) { UpdateINContent(); }
        }
        void BloodCondition()//<--The "blood condition" method.
        {
            string[] TabApos = { "A+", "A-", "O+", "O-" }; string[] TabAneg = { "A-", "O-" };
            string[] TabBpos = { "B+", "B-", "O+", "O-" }; string[] TabBneg = { "B-", "O-" };
            string[] TabOpos = { "O+", "O-" }; string[] TabOneg = { "O-" };
            string[] TabABpos = { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" }; string[] TabABneg = { "A-", "B-", "O-", "AB-" };
            if (txtBloodIN.Text == "A+") comboBloodIN.DataSource = TabApos; if (txtBloodIN.Text == "A-") comboBloodIN.DataSource = TabAneg;
            if (txtBloodIN.Text == "B+") comboBloodIN.DataSource = TabBpos; if (txtBloodIN.Text == "B-") comboBloodIN.DataSource = TabBneg;
            if (txtBloodIN.Text == "O+") comboBloodIN.DataSource = TabOpos; if (txtBloodIN.Text == "O-") comboBloodIN.DataSource = TabOneg;
            if (txtBloodIN.Text == "AB+") comboBloodIN.DataSource = TabABpos; if (txtBloodIN.Text == "AB-") comboBloodIN.DataSource = TabABneg;
        }

        //This button takes you to the Invoice form to print the patient's statement.
        private void btnGenerateIN_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = txtCinIN.Text;
                string path = @"C:\Users\msi\Desktop\Files\";
                string extension = ".docx";
                string fullPath = path + fileName + extension;
                CreateWordDocument(@"D:\Apps\Programming\Visual Studio\Projects\BBMSP\BBMSP\temp.docx", fullPath, Convert.ToInt32(txtIDIN.Text));
            }
            catch (Exception)
            {
                MessageBox.Show("Please Check Data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }



        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*//////////////////////////////////////////THIS IS THE BLOOD STOCK SECTION////////////////////////////////////////////////*/

        //Insert every blood type percentage into the circle progress bar on the dashboard.
        void BloodStats()
        {
            try
            {
                TBLOOD t = new TBLOOD();
                float max = t.BloodId(1) + t.BloodId(2) + t.BloodId(3) + t.BloodId(4) + t.BloodId(5) + t.BloodId(6) + t.BloodId(7) + t.BloodId(8);

                Apos.MaxValue = Convert.ToInt32(max); Apos.Value = t.BloodId(1); lblApos.Text = string.Format("{0:n}", (t.BloodId(1) / max * 100));
                Aneg.MaxValue = Convert.ToInt32(max); Aneg.Value = t.BloodId(2); lblAneg.Text = string.Format("{0:n}", (t.BloodId(2) / max * 100));
                Bpos.MaxValue = Convert.ToInt32(max); Bpos.Value = t.BloodId(3); lblBpos.Text = string.Format("{0:n}", (t.BloodId(3) / max * 100));
                Bneg.MaxValue = Convert.ToInt32(max); Bneg.Value = t.BloodId(4); lblBneg.Text = string.Format("{0:n}", (t.BloodId(4) / max * 100));
                Opos.MaxValue = Convert.ToInt32(max); Opos.Value = t.BloodId(5); lblOpos.Text = string.Format("{0:n}", (t.BloodId(5) / max * 100));
                Oneg.MaxValue = Convert.ToInt32(max); Oneg.Value = t.BloodId(6); lblOneg.Text = string.Format("{0:n}", (t.BloodId(6) / max * 100));
                ABpos.MaxValue = Convert.ToInt32(max); ABpos.Value = t.BloodId(7); lblABpos.Text = string.Format("{0:n}", (t.BloodId(7) / max * 100));
                ABneg.MaxValue = Convert.ToInt32(max); ABneg.Value = t.BloodId(8); lblABneg.Text = string.Format("{0:n}", (t.BloodId(8) / max * 100));
            }
            catch (Exception)
            {
                lblApos.Text = "0,00"; lblAneg.Text = "0,00"; lblBpos.Text = "0,00"; lblBneg.Text = "0,00";
                lblOpos.Text = "0,00"; lblOneg.Text = "0,00"; lblABpos.Text = "0,00"; lblABneg.Text = "0,00";
            }
        }

        //Filter blood in the DGV in "Blood Stock" page.
        private void comboBBS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBBS.SelectedIndex == 0)
                tBLOODBindingSource.DataSource = TBLOOD.Filter_By_Blood_ID();
            else if (comboBBS.SelectedIndex == 1)
                tBLOODBindingSource.DataSource = TBLOOD.Filter_By_Quantity_ASC();
            else if (comboBBS.SelectedIndex == 2)
                tBLOODBindingSource.DataSource = TBLOOD.Filter_By_Quantity_DESC();
        }

        //Filter donor in the DGV in "Blood Stock" page.
        private void comboDBS_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboDBS.SelectedIndex)
            {
                case 0: dgvFilterD.DataSource = TBLOOD.Filter_Donor_By_Blood_Type(1); break;
                case 1: dgvFilterD.DataSource = TBLOOD.Filter_Donor_By_Blood_Type(2); break;
                case 2: dgvFilterD.DataSource = TBLOOD.Filter_Donor_By_Blood_Type(3); break;
                case 3: dgvFilterD.DataSource = TBLOOD.Filter_Donor_By_Blood_Type(4); break;
                case 4: dgvFilterD.DataSource = TBLOOD.Filter_Donor_By_Blood_Type(5); break;
                case 5: dgvFilterD.DataSource = TBLOOD.Filter_Donor_By_Blood_Type(6); break;
                case 6: dgvFilterD.DataSource = TBLOOD.Filter_Donor_By_Blood_Type(7); break;
                case 7: dgvFilterD.DataSource = TBLOOD.Filter_Donor_By_Blood_Type(8); break;
            }
        }

        //Filter patient in the DGV in "Blood Stock" page.
        private void comboPBS_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboPBS.SelectedIndex)
            {
                case 0: dgvFilterP.DataSource = TBLOOD.Filter_Patient_By_Blood_Type(1); break;
                case 1: dgvFilterP.DataSource = TBLOOD.Filter_Patient_By_Blood_Type(2); break;
                case 2: dgvFilterP.DataSource = TBLOOD.Filter_Patient_By_Blood_Type(3); break;
                case 3: dgvFilterP.DataSource = TBLOOD.Filter_Patient_By_Blood_Type(4); break;
                case 4: dgvFilterP.DataSource = TBLOOD.Filter_Patient_By_Blood_Type(5); break;
                case 5: dgvFilterP.DataSource = TBLOOD.Filter_Patient_By_Blood_Type(6); break;
                case 6: dgvFilterP.DataSource = TBLOOD.Filter_Patient_By_Blood_Type(7); break;
                case 7: dgvFilterP.DataSource = TBLOOD.Filter_Patient_By_Blood_Type(8); break;
            }
        }

        //Add quantity to blood stock from foreign source.
        private void btnFillBS_Click(object sender, EventArgs e)
        {
            try
            {
                //Insert blood quantity in the blood stock.
                TBLOOD.Donation(comboFillBS.Text, Int32.Parse(txtQuantityBS.Text));

                //Display notification.
                notifyBlood.BalloonTipTitle = "Operation successfull";
                notifyBlood.BalloonTipText = "Blood stock has been filled !";
                notifyBlood.ShowBalloonTip(50);

                //Reset text.
                txtQuantityBS.Clear(); txtQuantityBS.Focus();

                //Display the blood infos such as type and quantity in the DGV on the Blood Stock page.
                tBLOODBindingSource.DataSource = TBLOOD.GetBloodType();

                //Display the percentage of the blood quantity on the Blood Stock page.
                BloodStats();

                //Update content.
                UpdateBloodStockContent(); UpdateDashboardContent(); tBLOODBindingSource.DataSource = TBLOOD.GetBloodType();
            }
            catch (Exception)
            {
                MessageBox.Show("Please Check Data...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }



        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*///////////////////////////////////////////THIS IS THE METHODS SECTION///////////////////////////////////////////////////*/

        //These methods relate to some updates in the application after any opperation.
        void UpdateDashboardContent()
        {
            //Display total numbers of donors and patients in the label.
            int num = TDONOR.DonorsNumber(); int num2 = TPATIENT.PatientsNumber();
            txtTotalDI.Text = num.ToString(); txtTotalPI.Text = num2.ToString();
            txtTotalD.Text = num.ToString(); txtTotalP.Text = num2.ToString();

            //Update circle progress bar content.
            BloodStats();

            //Display the blood infos such as type and quantity in the DGV on the Blood Stock page.
            tBLOODBindingSource.DataSource = TBLOOD.GetBloodType();

            //Display total number of males and females of donors and patients.
            txtNumMDO.Text = TBLOOD.Donors_Gender_Number("male").ToString();
            txtNumFDO.Text = TBLOOD.Donors_Gender_Number("female").ToString();
            txtNumMPA.Text = TBLOOD.Patients_Gender_Number("male").ToString();
            txtNumFPA.Text = TBLOOD.Patients_Gender_Number("female").ToString();
        }

        /*//////////////////////////////////////////////////////////////*/
        void UpdateADContent()
        {
            //Reset all texts.
            txtNameAD.ResetText(); txtCinAD.ResetText(); txtPhoneAD.ResetText(); txtEmailAD.ResetText(); txtCityAD.ResetText();
            txtAdressAD.ResetText(); txtAllergiesAD.ResetText(); txtNameAD.Focus();
        }

        /*//////////////////////////////////////////////////////////////*/
        void UpdateDIContent()
        {
            //Update "DONOR'S LIST" DGV, donors number and reset texts.
            displayDonorsBindingSource.DataSource = DisplayDonors.DonorsList();
            int num = TDONOR.DonorsNumber();
            txtTotalDI.Text = num.ToString();
            lblIDDI.Visible = false; txtNameDI.ResetText(); txtCinDI.ResetText(); txtPhoneDI.ResetText(); txtEmailDI.ResetText();
            txtDobDI.ResetText(); txtCityDI.ResetText(); txtGenderDI.ResetText(); txtAdressDI.ResetText(); txtAllergiesDI.ResetText();
        }

        /*//////////////////////////////////////////////////////////////*/
        void UpdateDOContent()
        {
            //Reset all texts.
            txtIDDO.ResetText(); txtNameDO.ResetText(); txtCinDO.ResetText(); txtBloodDO.ResetText(); txtAllergiesDO.ResetText();
            txtQuantityDO.Clear();
        }

        /*//////////////////////////////////////////////////////////////*/
        void UpdateAPContent()
        {
            //Reset all texts.
            txtNameAP.ResetText(); txtCinAP.ResetText(); txtPhoneAP.ResetText(); txtEmailAP.ResetText(); txtCityAP.ResetText();
            txtAdressAP.ResetText(); txtAllergiesAP.ResetText(); txtNameAP.Focus();
        }

        /*//////////////////////////////////////////////////////////////*/
        void UpdatePIContent()
        {
            //Update "PATIENT'S LIST" DGV, patients number and reset texts.
            displayPatientsBindingSource.DataSource = DisplayPatients.PatientsList();
            int num = TPATIENT.PatientsNumber();
            txtTotalPI.Text = num.ToString();
            lblIDPI.Visible = false; txtNamePI.ResetText(); txtCinPI.ResetText(); txtPhonePI.ResetText(); txtEmailPI.ResetText();
            txtDobPI.ResetText(); txtCityPI.ResetText(); txtGenderPI.ResetText(); txtAdressPI.ResetText(); txtAllergiesPI.ResetText();
        }

        /*//////////////////////////////////////////////////////////////*/
        void UpdateINContent()
        {
            //Reset all texts.
            txtIDIN.ResetText(); txtNameIN.ResetText(); txtCinIN.ResetText(); txtBloodIN.ResetText(); txtAllergiesIN.ResetText();
            txtQuantityIN.Clear();
        }

        /*//////////////////////////////////////////////////////////////*/
        void UpdateBloodStockContent()
        {
            //Display donors list in the DGV.
            displayDonorsBindingSource.DataSource = DisplayDonors.DonorsList();

            //Display patients list in the DGV.
            displayPatientsBindingSource.DataSource = DisplayPatients.PatientsList();

            //Display the blood infos such as type and quantity in the DGV on the Blood Stock page.
            tBLOODBindingSource.DataSource = TBLOOD.GetBloodType();
        }

        /*//////////////////////////////////////////////////////////////*/
        string TodayDate()
        {
            //The following code fit to stock today's date in a variable.
            var dt1 = DateTime.Now;
            var dt2 = DateTime.Now;
            string date = dt1.ToString("dddd") + " " + dt2.ToString("d");
            return date;
        }

        /*//////////////////////////////////////////////////////////////*/
        void CheckBloodQuantity(string blood)//<--this is the method of reporting blood quantity we're working with.
        {
            MyLinqDataContext link = new MyLinqDataContext();
            TBLOOD check = (from n in link.TBLOODs where n.BLOODTYPE == blood select n).Single();
            if (check.QUANTITY < 500)
            {
                //Display notification message.
                notifyBlood.BalloonTipTitle = "Quantity is very low";
                notifyBlood.BalloonTipText = $"Volume of {blood} is low, it must be filled !";
                notifyBlood.ShowBalloonTip(50);
            }
        }

        /*//////////////////////////////////////////////////////////////*/
        private void timerDate_Tick(object sender, EventArgs e)//<--this is the timer we're working with.
        {
            txtTimeDSH.Text = DateTime.Now.ToLongTimeString();
            txtDateDSH.Text = DateTime.Now.ToLongDateString();
        }

        /*//////////////////////////////////////////////////////////////*/

        /*//////////////////////////////////////////////////////////////*/
        void CitiesList()
        {
            AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
            string[] city = CITY.GetCity().ToArray();
            auto.AddRange(city);
            txtCityAD.AutoCompleteCustomSource = auto;
            txtCityAP.AutoCompleteCustomSource = auto;
            txtCityDI.AutoCompleteCustomSource = auto;
            txtCityPI.AutoCompleteCustomSource = auto;
        }
        /*//////////////////////////////////////////////////////////////*/
        void checkAllergies()//<--Check the text allergies enabled.
        {
            if (checckAllergies.Checked)
            {
                txtAllergiesAD.Enabled = true;
            }
            else if (!checckAllergies.Checked)
            {
                txtAllergiesAD.ResetText();
                txtAllergiesAD.Enabled = false;
            }
        }
        private void checckAllergies_CheckedChanged(object sender, EventArgs e)
        {
            checkAllergies();
        }
        void checkAllergies2()//<--Check the text allergies enabled.
        {
            if (checkAllergiesPa.Checked)
            {
                txtAllergiesAP.Enabled = true;
            }
            else if (!checkAllergiesPa.Checked)
            {
                txtAllergiesAP.ResetText();
                txtAllergiesAP.Enabled = false;
            }
        }
        private void checkAllergiesPa_CheckedChanged(object sender, EventArgs e)
        {
            checkAllergies2();
        }


        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        /*//////////////////////////////////////////////THIS IS THE WORD SECTION///////////////////////////////////////////////////*/


        //Find and Replace Method
        private void FindAndReplace(Word.Application wordApp, object ToFindText, object replaceWithText)
        {
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundLike = false;
            object nmatchAllforms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;

            wordApp.Selection.Find.Execute(ref ToFindText,
                ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundLike,
                ref nmatchAllforms, ref forward,
                ref wrap, ref format, ref replaceWithText,
                ref replace, ref matchKashida,
                ref matchDiactitics, ref matchAlefHamza,
                ref matchControl);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (!access)
            {
                var date = DateTime.Now;
                Logout_Time = date.ToString();

                var link = new MyLinqDataContext();
                var query = (from n in link.USERs where n.CIN == txtCIN.Text select n).Single();

                var infos = new INFOS_USER(Convert.ToInt32(query.ID), query.CIN, Login_Time, Logout_Time);
                infos.AddInfos();
                new Login().Show();
                Close();

            }
            else
            {
                new Login().Show();
                Close();
            }

        }

        //Creeate the Doc Method
        private void CreateWordDocument(object filename, object SaveAs, int id)
        {
            Word.Application wordApp = new Word.Application();
            object missing = Missing.Value;
            Word.Document myWordDoc = null;

            if (File.Exists((string)filename))
            {
                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                myWordDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly,
                                        ref missing, ref missing, ref missing,
                                        ref missing, ref missing, ref missing,
                                        ref missing, ref missing, ref missing,
                                        ref missing, ref missing, ref missing, ref missing);
                myWordDoc.Activate();
                /*-----------------------------------------------*/
                MyLinqDataContext link = new MyLinqDataContext();
                TDONOR d = (from n in link.TDONORs where n.ID == id select n).Single();
                /*-----------------------------------------------*/
                //find and replace
                this.FindAndReplace(wordApp, "<NAME>", d.FULLNAME);
                this.FindAndReplace(wordApp, "<CIN>", d.CIN);
                this.FindAndReplace(wordApp, "<PHONE>", d.PHONE);
                this.FindAndReplace(wordApp, "<EMAIL>", d.CIN);
                this.FindAndReplace(wordApp, "<ADDRESS>", d.ADRESS);
                this.FindAndReplace(wordApp, "<ALLERGIES>", d.ALLERGIES);
                this.FindAndReplace(wordApp, "<date>", DateTime.Now.ToShortDateString());
            }
            else
            {
                MessageBox.Show("File not Found!");
            }

            //Save as
            myWordDoc.SaveAs2(ref SaveAs, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing);

            myWordDoc.Close();
            wordApp.Quit();
            MessageBox.Show("File Created!");
        }
    }
}
