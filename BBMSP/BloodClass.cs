using System.Collections.Generic;
using System.Linq;

namespace BBMSP
{
    internal class BloodClass
    {
    }

    partial class TBLOOD
    {
        //Insert Blood Type into a DGV or combobox.
        public static List<TBLOOD> GetBloodType()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<TBLOOD> ls = (from n in link.TBLOODs select n).ToList<TBLOOD>();
            return ls;
        }

        //Donate Blood.
        public static void Donation(string blood, int q)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            TBLOOD b = (from n in link.TBLOODs where n.BLOODTYPE == blood select n).Single();
            b.QUANTITY += q;
            link.SubmitChanges();
        }



        //Inject Blood.

        //I wrote the code in the button due to some miscalculations.




        //The matter of the following code is to collect the quantity of each blood type.
        public int BloodId(int id)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            var v = (from n in link.TBLOODs where n.ID == id select n).Single();
            int quantity = (int)v.QUANTITY;
            return quantity;
        }


        //Filter the DGVs in blood stock section by using procedures from database.

        //Filter blood by quantity ascendant.
        public static List<FILTER_Q_ASCResult> Filter_By_Quantity_ASC()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<FILTER_Q_ASCResult> ls = link.FILTER_Q_ASC().ToList();
            return ls;
        }
        //Filter blood by quantity descendant.
        public static List<FILTER_Q_DESCResult> Filter_By_Quantity_DESC()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<FILTER_Q_DESCResult> ls = link.FILTER_Q_DESC().ToList();
            return ls;
        }
        //Filter blood id.
        public static List<FILTER_Q_IDResult> Filter_By_Blood_ID()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<FILTER_Q_IDResult> ls = link.FILTER_Q_ID().ToList();
            return ls;
        }



        //Filter Donor by blood type.
        public static List<FILTER_D_BResult> Filter_Donor_By_Blood_Type(int id)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<FILTER_D_BResult> ls = link.FILTER_D_B(id).ToList();
            return ls;
        }
        //Filter patient by blood type.
        public static List<FILTER_P_BResult> Filter_Patient_By_Blood_Type(int id)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<FILTER_P_BResult> ls = link.FILTER_P_B(id).ToList();
            return ls;
        }

        //total number of males and females of donors and patients.
        public static int Donors_Gender_Number(string gender)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            int num = (from n in link.TDONORs where n.GENDER == gender select n.GENDER).Count();
            return num;
        }
        public static int Patients_Gender_Number(string gender)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            int num = (from n in link.TPATIENTs where n.GENDER == gender select n.GENDER).Count();
            return num;
        }

        //Display every donor's phone number related to a specific blood type.
        public static List<FILTER_PHONEResult> Donors_Phone(int num)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<FILTER_PHONEResult> ls = link.FILTER_PHONE(num).ToList<FILTER_PHONEResult>();
            return ls;
        }


        //ez
        public static TBLOOD ez(string lol)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            TBLOOD b = (from n in link.TBLOODs where n.BLOODTYPE == lol select n).Single();
            return b;
        }
    }


    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    partial class TDONATION
    {
        //Insert Operation date and infos into donation table.
        public static void DonorInfos(int id_donor, string cin_donor, string op, int q)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            TDONATION d = new TDONATION();
            d.ID_DONOR = id_donor;
            d.CIN_DONOR = cin_donor;
            d.OPERATION_DATE = op;
            d.QUANTITY_DONATED = q;
            link.TDONATIONs.InsertOnSubmit(d);
            link.SubmitChanges();
        }

        //Insert table's Data into DGV.
        public static List<TDONATION> DisplayDonorsData(int id)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<TDONATION> ls = (from n in link.TDONATIONs where n.ID_DONOR == id select n).ToList<TDONATION>();
            return ls;
        }
    }


    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    partial class TINJECTION
    {
        //Insert Operation date and infos into donation table.

        public static void InfosPatient(int id_donor, string cin_donor, string op, int q)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            TINJECTION i = new TINJECTION();
            i.ID_PATIENT = id_donor;
            i.CIN_PATIENT = cin_donor;
            i.OPERATION_DATE = op;
            i.QUANTITY_INJECTED = q;
            link.TINJECTIONs.InsertOnSubmit(i);
            link.SubmitChanges();
        }

        //Insert table Data into DGV.
        public static List<TINJECTION> DisplayPatientsData(int id)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<TINJECTION> ls = (from n in link.TINJECTIONs where n.ID_PATIENT == id select n).ToList<TINJECTION>();
            return ls;
        }
    }


    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/


    partial class CITY
    {
        //Insert Cities into a combobox.
        public static List<string> GetCity()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<string> ls = (from n in link.CITies select n.CITY_NAME).ToList();
            return ls;
        }
    }
}
