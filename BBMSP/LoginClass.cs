using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BBMSP
{
    internal class LoginClass
    {
    }
    partial class ADMIN
    {
        //////////////////////////////Registration Form/////////////////////////////////

        //This is a constructor which gives columns in the table the values of the parameters bellow.
        public ADMIN(string fullname, string cin, string phone, string email, string dob, string gender, string city, string address, string password)
        {
            _FULLNAME = fullname;
            _CIN = cin;
            _PHONE = phone;
            _EMAIL = email;
            _DOB = dob;
            _GENDER = gender;
            _CITY = city;
            _ADRESS = address;
            _PASSWORD = password;
        }
        public bool AddAdmin()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            ADMIN admin = new ADMIN();
            admin.FULLNAME = _FULLNAME;
            admin.CIN = _CIN;
            admin.PHONE = _PHONE;
            admin.EMAIL = _EMAIL;
            admin.DOB = _DOB;
            admin.GENDER = _GENDER;
            admin.CITY = _CITY;
            admin.ADRESS = _ADRESS;
            admin.PASSWORD = _PASSWORD;
            link.ADMINs.InsertOnSubmit(admin);
            link.SubmitChanges();
            return true;
        }


        //////////////////////////////Login Form/////////////////////////////////
        public ADMIN GetAdmin(string email, string cin, string psw)
        {
            //In admin login, I tell the programme which row should be selected to get his infos.

            MyLinqDataContext link = new MyLinqDataContext();

            ADMIN admin = (from n in link.ADMINs where (n.EMAIL == email || n.CIN == cin) && n.PASSWORD == psw select n).Single();
            Login.ADMIN_FULLNAME = admin.FULLNAME;
            MainForm.access = true;
            Manager.CIN = admin.CIN;
            MainForm.CIN = admin.CIN;
            return admin;
        }

        //////////////////////////////Display Section/////////////////////////////////

        public static List<ADMIN> DisplayAdmins()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<ADMIN> admin = (from n in link.ADMINs select n).ToList();
            return admin;
        }


        //////////////////////////////Cin Confirmation/////////////////////////////////
        public static bool AdminCin(string cin)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            var admin = (from n in link.ADMINs where n.CIN == cin select n).Single();
            return true;
        }

        //////////////////////////////Search Section///////////////////////////////////
        public static ADMIN SearchAdmin(string id, string name, string cin, string email, string phone)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            ADMIN admin = (from n in link.ADMINs where n.ID.ToString() == id || n.FULLNAME == name || n.CIN == cin || n.PHONE == phone || n.EMAIL == email select n).Single();
            return admin;
        }


        //////////////////////////////Change password/////////////////////////////////
        public static void ChangePsw(string cin, string Newpsw)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            var admin = link.ADMINs.FirstOrDefault(x => x.CIN.Equals(cin));
            admin.PASSWORD = Newpsw;
            link.SubmitChanges();
        }


        //Update admin.
        public static void UpdateAdmin(int id, string name, string cin, string phone, string email, string dob, string gender, string city, string address, string password)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            ADMIN admin = (from n in link.ADMINs where n.ID == id select n).Single();
            admin.FULLNAME = name;
            admin.CIN = cin;
            admin.PHONE = phone;
            admin.EMAIL = email;
            admin.DOB = dob;
            admin.GENDER = gender;
            admin.CITY = city;
            admin.ADRESS = address;
            admin.PASSWORD = password;
            link.SubmitChanges();
        }


        //Delete admin.
        public void DeleteAdmin()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            ADMIN admin = (from n in link.ADMINs where n.ID == this.ID select n).Single();
            link.ADMINs.DeleteOnSubmit(admin);
            link.SubmitChanges();
        }

        //////////////////////////////Call administrator///////////////////////////////
        public static void CallAdmins()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            //Display admins phone number.
            var phone = (from n in link.ADMINs select n.PHONE).ToList();
            int i = 1;
            foreach (var item in phone)
            {
                MessageBox.Show($"Admin {i} : {item}", "Contact", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                i++;
            }
        }
    }


    /*/////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////*/

    partial class USER
    {
        //////////////////////////////Registration Form/////////////////////////////////

        //This is a constructor which gives columns in the table the values of the parameters bellow.
        public USER(string fullname, string cin, string phone, string email, string dob, string gender, string city, string address, string password)
        {
            _FULLNAME = fullname;
            _CIN = cin;
            _PHONE = phone;
            _EMAIL = email;
            _DOB = dob;
            _GENDER = gender;
            _CITY = city;
            _ADRESS = address;
            _PASSWORD = password;
        }
        public bool AddUser()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            USER user = new USER();
            user.FULLNAME = _FULLNAME;
            user.CIN = _CIN;
            user.PHONE = _PHONE;
            user.EMAIL = _EMAIL;
            user.DOB = _DOB;
            user.GENDER = _GENDER;
            user.CITY = _CITY;
            user.ADRESS = _ADRESS;
            user.PASSWORD = _PASSWORD;
            link.USERs.InsertOnSubmit(user);
            link.SubmitChanges();
            return true;
        }

        //////////////////////////////Login Form/////////////////////////////////

        public USER GetUser(string email, string cin, string psw)
        {
            //In user login, I tell the programme which row should be selected to get his infos.

            MyLinqDataContext link = new MyLinqDataContext();
            USER user = (from n in link.USERs where (n.EMAIL == email || n.CIN == cin) && n.PASSWORD == psw select n).Single();
            Login.USER_FULLNAME = user.FULLNAME;

            //Insert login time in a variable.
            var date = DateTime.Now;
            MainForm.Login_Time = date.ToString();
            MainForm.access = false;
            MainForm.CIN = user.CIN;
            return user;
        }
        //////////////////////////////Display Section/////////////////////////////////

        public static List<USER> DisplayUsers()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<USER> user = (from n in link.USERs select n).ToList();
            return user;
        }

        //////////////////////////////Search Section/////////////////////////////////

        public static USER SearchUser(string id, string name, string cin, string email, string phone)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            USER user = (from n in link.USERs where n.ID.ToString() == id || n.FULLNAME == name || n.CIN == cin || n.PHONE == phone || n.EMAIL == email select n).Single();
            return user;
        }





        //Update User.

        public static void UpdateUser(int id, string name, string cin, string phone, string email, string dob, string gender, string city, string address, string password)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            USER user = (from n in link.USERs where n.ID == id select n).Single();
            user.FULLNAME = name;
            user.CIN = cin;
            user.PHONE = phone;
            user.EMAIL = email;
            user.DOB = dob;
            user.GENDER = gender;
            user.CITY = city;
            user.ADRESS = address;
            user.PASSWORD = password;
            link.SubmitChanges();
        }

        //Delete User.
        public void DeleteUser()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            var user = (from n in link.USERs where n.ID == this.ID select n).Single();
            link.USERs.DeleteOnSubmit(user);
            link.SubmitChanges();
        }
    }


    /*/////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////*/


    partial class SERIAL_CODE
    {
        public bool RecoveryCode(string code)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            SERIAL_CODE recover = (from n in link.SERIAL_CODEs where n.SERIAL_NUMBER == code select n).Single();
            return true;
        }
        public static SERIAL_CODE PrintRecoveryCode(int id)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            var recover = (from n in link.SERIAL_CODEs where n.ID == id select n).Single();
            Login.CODE_SERIAL = recover.SERIAL_NUMBER;
            return recover;
        }
        public static SERIAL_CODE PrintRecoveryCin(int id)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            var recover = (from n in link.SERIAL_CODEs where n.ID == id select n).Single();
            Login.CODE_CIN = recover.SERIAL_NUMBER;
            return recover;
        }
    }


    /*/////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*/////////////////////////////////////////////////////////////////////////////////////////////////*/


    partial class INFOS_USER
    {
        //////////////////////////////Display Section/////////////////////////////////

        public static List<INFOS_USER> DisplayUsers()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            List<INFOS_USER> user = (from n in link.INFOS_USERs select n).ToList();
            return user;
        }


        //////////////////////////////Search Section/////////////////////////////////

        public static INFOS_USER SearchUser(string id, string cin)
        {
            MyLinqDataContext link = new MyLinqDataContext();
            INFOS_USER user = (from n in link.INFOS_USERs where n.ID_USER.ToString() == id || n.CIN_USER == cin select n).Single();
            return user;
        }

        //////////////////////////////Insertion Form/////////////////////////////////

        //This is a constructor which gives columns in the table the values of the parameters bellow.
        public INFOS_USER(int id_user, string cin_user, string login, string logout)
        {
            _ID_USER = id_user;
            _CIN_USER = cin_user;
            _LOGIN_TIME = login;
            _LOGOUT_TIME = logout;
        }
        public bool AddInfos()
        {
            MyLinqDataContext link = new MyLinqDataContext();
            var infos = new INFOS_USER();
            infos.ID_USER = _ID_USER;
            infos.CIN_USER = _CIN_USER;
            infos.LOGIN_TIME = _LOGIN_TIME;
            infos.LOGOUT_TIME = _LOGOUT_TIME;
            link.INFOS_USERs.InsertOnSubmit(infos);
            link.SubmitChanges();
            return true;
        }
    }
}
