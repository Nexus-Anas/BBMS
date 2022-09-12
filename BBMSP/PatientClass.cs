using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBMSP
{
    internal class PatientClass
    {
    }

    partial class TPATIENT
    {
        //////////////////////////////Registration Form/////////////////////////////////

        //This is a constructor which gives columns in the table the values of the parameters bellow.
        public TPATIENT(string fullname, string cin, string phone, string email, string dob, string gender, int id_blood, string city, string adress, string allergies)
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
        public bool AddPatient()
        {
            MyLinqDataContext linq = new MyLinqDataContext();
            TPATIENT p = new TPATIENT()
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
            linq.TPATIENTs.InsertOnSubmit(p);
            linq.SubmitChanges();
            return true;
        }


        //Display total number of patients in the label.

        public static int PatientsNumber()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            int num = (from n in link.TPATIENTs select n).Count();
            return num;
        }


        //Update Patient.

        public static void UpdatePatient(int id, string fullname, string cin, string phone, string email, string dob, string gender, int id_blood, string city, string adress, string allergies)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            TPATIENT p = (from n in link.TPATIENTs where n.ID == id select n).Single();
            p.FULLNAME = fullname;
            p.CIN = cin;
            p.PHONE = phone;
            p.EMAIL = email;
            p.DOB = dob;
            p.GENDER = gender;
            p.ID_BLOOD = id_blood;
            p.CITY = city;
            p.ADRESS = adress;
            p.ALLERGIES = allergies;
            link.SubmitChanges();
        }




        //Delete patient.
        public void DeletePatient()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            TPATIENT p = (from n in link.TPATIENTs where n.ID == this.ID select n).Single();
            link.TPATIENTs.DeleteOnSubmit(p);
            link.SubmitChanges();
        }


        //Search for patient.
        public static TPATIENT SearchPatient(string id, string name, string cin, string phone, string email)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            TPATIENT p = (from n in link.TPATIENTs where n.ID.ToString() == id || n.FULLNAME == name || n.CIN == cin || n.PHONE == phone || n.EMAIL == email select n).Single();
            return p;
        }
    }


    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    class DisplayPatients : DisplayDonors, IAccessors  /*Class created to custom a list of data from different tables*/
    {

        //Generate the constructor.
        public DisplayPatients(int id, string fullname, string cin, string phone, string email, string dob, string gender, string blood, string city, string adress, string allergies)
        : base(id, fullname, cin, phone, email, dob, gender, blood, city, adress, allergies)
        {

        }

        //Input data into list.
        public static List<DisplayPatients> PatientsList()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            var Patients = (from n in link.TPATIENTs select new { n.ID, n.FULLNAME, n.CIN, n.PHONE, n.EMAIL, n.DOB, n.GENDER, n.TBLOOD.BLOODTYPE, n.CITY, n.ADRESS, n.ALLERGIES });
            List<DisplayPatients> ls = new List<DisplayPatients>();
            foreach (var c in Patients)
            {
                DisplayPatients s = new DisplayPatients(c.ID, c.FULLNAME, c.CIN, c.PHONE, c.EMAIL, c.DOB, c.GENDER, c.BLOODTYPE, c.CITY, c.ADRESS, c.ALLERGIES);
                ls.Add(s);
            }
            return ls;
        }


    }

}
