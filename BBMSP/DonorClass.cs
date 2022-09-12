using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BBMSP
{
    internal class DonorClass
    {
    }
    partial class TDONOR
    {
        //////////////////////////////Registration Form/////////////////////////////////

        //This is a constructor which gives columns in the table the values of the parameters bellow.
        public TDONOR(string fullname, string cin, string phone, string email, string dob, string gender, int id_blood, string city, string adress, string allergies)
        {
            _FULLNAME = fullname;
            _CIN = cin;
            _PHONE = phone;
            _EMAIL = email;
            _DOB = dob;
            _GENDER = gender;
            _ID_BLOOD = id_blood;
            _CITY = city;
            _ADRESS = adress;
            _ALLERGIES = allergies;
        }
        public bool AddDonor()
        {
            MyLinqDataContext linq = new MyLinqDataContext();
            TDONOR d = new TDONOR()
            {
                FULLNAME = _FULLNAME,
                CIN = _CIN,
                PHONE = _PHONE,
                EMAIL = _EMAIL,
                DOB = _DOB,
                GENDER = _GENDER,
                ID_BLOOD = _ID_BLOOD,
                CITY = _CITY,
                ADRESS = _ADRESS,
                ALLERGIES = _ALLERGIES
            };
            linq.TDONORs.InsertOnSubmit(d);
            linq.SubmitChanges();
            return true;
        }
        //Check nulls in DI.
        public static void CheckNullDI(string name, string cin, string email, string phone, string dob, string blood, string city, string adress, string gender)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(cin) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(dob) || string.IsNullOrEmpty(blood) ||
                string.IsNullOrEmpty(city) || string.IsNullOrEmpty(adress) || string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Missing fields..\nCheck again !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Display total number of donors in the label.

        public static int DonorsNumber()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            int num = (from n in link.TDONORs select n).Count();
            return num;
        }


        //Update Donor.

        public static void UpdateDonor(int id, string fullname, string cin, string phone, string email, string dob, string gender, int id_blood, string city, string adress, string allergies)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            TDONOR d = (from n in link.TDONORs where n.ID == id select n).Single();
            d.FULLNAME = fullname;
            d.CIN = cin;
            d.PHONE = phone;
            d.EMAIL = email;
            d.DOB = dob;
            d.GENDER = gender;
            d.ID_BLOOD = id_blood;
            d.CITY = city;
            d.ADRESS = adress;
            d.ALLERGIES = allergies;
            link.SubmitChanges();
        }




        //Delete donor.
        public void DeleteDonor()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            TDONOR d = (from n in link.TDONORs where n.ID == this.ID select n).Single();
            link.TDONORs.DeleteOnSubmit(d);
            link.SubmitChanges();
        }


        //Search donor.
        public static TDONOR SearchDonor(string id, string name, string cin, string phone, string email)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            TDONOR d = (from n in link.TDONORs where n.ID.ToString() == id || n.FULLNAME == name || n.CIN == cin || n.PHONE == phone || n.EMAIL == email select n).Single();
            return d;
        }

        //Exception method.
        public static void Exception()
        {
            MessageBox.Show("Something is Incorrect!\nPlease check your inputs..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

    }


    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    interface IAccessors
    {
        int Id { get; set; }
        string Fullname { get; set; }
        string Cin { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        string Dob { get; set; }
        string Gender { get; set; }
        string Blood { get; set; }
        string City { get; set; }
        string Adress { get; set; }
        string Allergies { get; set; }
    }

    class DisplayDonors : IAccessors  /*Class created to custom a list of data from different tables*/
    {
        //Declare needed variables.
        private int id; private string fullname, cin, phone, email, dob, gender, blood, city, adress, allergies;

        //Generate getters and setters.
        public int Id { get => id; set => id = value; }
        public string Fullname { get => fullname; set => fullname = value; }
        public string Cin { get => cin; set => cin = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public string Dob { get => dob; set => dob = value; }
        public string Gender { get => gender; set => gender = value; }
        public string Blood { get => blood; set => blood = value; }
        public string City { get => city; set => city = value; }
        public string Adress { get => adress; set => adress = value; }
        public string Allergies { get => allergies; set => allergies = value; }

        //Generate the constructor.
        public DisplayDonors(int id, string fullname, string cin, string phone, string email, string dob, string gender, string blood, string city, string adress, string allergies)
        {
            Id = id;
            Fullname = fullname;
            Cin = cin;
            Phone = phone;
            Email = email;
            Dob = dob;
            Gender = gender;
            Blood = blood;
            City = city;
            Adress = adress;
            Allergies = allergies;
        }

        //Threw data into list.
        public static List<DisplayDonors> DonorsList()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            var Donors = (from n in link.TDONORs select new { n.ID, n.FULLNAME, n.CIN, n.PHONE, n.EMAIL, n.DOB, n.GENDER, n.TBLOOD.BLOODTYPE, n.CITY, n.ADRESS, n.ALLERGIES });
            List<DisplayDonors> ls = new List<DisplayDonors>();
            foreach (var c in Donors)
            {
                DisplayDonors s = new DisplayDonors(c.ID, c.FULLNAME, c.CIN, c.PHONE, c.EMAIL, c.DOB, c.GENDER, c.BLOODTYPE, c.CITY, c.ADRESS, c.ALLERGIES);
                ls.Add(s);
            }
            return ls;
        }


    }
}
