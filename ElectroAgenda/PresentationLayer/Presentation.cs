using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EntityLayer;
using BusinessLayer;

namespace PresentationLayer
{
    public partial class Presentation : Form
    {
        public Presentation()
        {
            InitializeComponent();
        }

        /**
         * Registra a un nuevo usuario.
         */
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            isSignUpBtn = !isSignUpBtn;
            var txt = linkLabel1.Text;
            panelLbl.Text = txt;
            loginBtn.Text = txt;
            if (isSignUpBtn)
                linkLabel1.Text = "Iniciar Sesion";
            else
                linkLabel1.Text = "Registrarse";      
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
           attemptingUser = new E_User(usernameField.Text, passwordField.Text);

           if (isSignUpBtn) 
               signUpHander();
           else
               loginHandler();
        }

        private void loginHandler()
        {
            bool isValid = userLogic.validateLogin(attemptingUser);

            if (isValid)
                goToAddRecordPage();
            else
                MessageBox.Show("Usuario o contraseña incorrecto");
        }

        private void signUpHander()
        {
            bool res = userLogic.registerUser(attemptingUser);
            if (res)
                goToAddRecordPage();
            else
                MessageBox.Show("El usario no esta disponible. Por favor, eligir otro.");
        }

        private void goToAddRecordPage()
        {
            tabControl1.TabPages.Remove(loginPage);
            tabControl1.ItemSize = new Size(100, 20);
            tabControl1.Appearance = TabAppearance.Normal;
        }

        private void insertRecord(object sender, EventArgs e)
        {
            fillRecord();
            if (recordLogic.insertRecord(recordToAdd))
                MessageBox.Show("El record ha sido insertado.");
            else
                MessageBox.Show("No se ha podido insertar el record.");
        }

        private void fillRecord()
        {
            recordToAdd = new E_Record();
            recordToAdd.Name = nameField.Text;
            recordToAdd.Surname = surnameField.Text;
            recordToAdd.Birthday = birthdayField.Text;
            recordToAdd.Address = addressField.Text;
            recordToAdd.Genre = genre();
            recordToAdd.CivilState = civilState();
            recordToAdd.Phone = phoneField.Text;
            recordToAdd.Telephone = telephoneField.Text;
            recordToAdd.Email = emailField.Text;
        }

        private string genre(bool forEdit = false)
        {
            if (forEdit)
            {
                if (maleRBEdit.Checked)
                    return maleRB.Text;
                if (femaleRBEdit.Checked)
                    return femaleRB.Text;
            }
            else
            {
                if (maleRB.Checked)
                    return maleRB.Text;
                if (femaleRB.Checked)
                    return femaleRB.Text;
            }
            return null;
        }

        private string civilState(bool forEdit = false)
        {
            Control.ControlCollection controls = null;
            if (forEdit)
                controls = flowLayoutPanel4.Controls;
            else
                controls = flowLayoutPanel2.Controls;

            foreach (var control in controls)
            {
                RadioButton radio = control as RadioButton;

                if (radio != null && radio.Checked)
                    return radio.Text;
            }
            return null;
        }

        private void searchRecord(object sender, EventArgs e)
        {
            var toFind = new E_Record();
            toFind.Email = searchField.Text;
            fetchedRecord = recordLogic.findRecord(toFind);
            if (fetchedRecord != null)
                fillModifyPanel(fetchedRecord);
            else
            {
                MessageBox.Show("No se ha podido encontrar ningun record asociado a ese correo.");
                modifyPanel.Visible = false;
            }
        }

        private void fillModifyPanel(E_Record record)
        {
            nameEdit.Text = record.Name;
            surnameEdit.Text = record.Surname;
            addressEdit.Text = record.Address;
            setGenre(record.Genre);
            setCivilState(record.CivilState);
            phoneEdit.Text = record.Phone;
            telephoneEdit.Text = record.Telephone;
            emailEdit.Text = record.Email;
            birthdayEdit.Text = record.Birthday;

            modifyPanel.Visible = true;
        }

        private void setGenre(string genre)
        {
            if (genre == "Masculino")
                maleRBEdit.Checked = true;
            else
                femaleRBEdit.Checked = true;
        }

        private void setCivilState(string civil_state)
        {
            foreach (var control in flowLayoutPanel4.Controls)
            {
                RadioButton radio = control as RadioButton;

                if (radio != null && radio.Text == civil_state)
                    radio.Checked = true;
            }
        }

        private void deleteRecord(object sender, EventArgs e)
        {
            bool res = recordLogic.deleteRecord(fetchedRecord);
            if (res)
            {
                MessageBox.Show("El record ha sido eliminado.");
                modifyPanel.Visible = false;
            }
            else
                MessageBox.Show("El record no pudo ser eliminado.");
        }

        private void updateRecord(object sender, EventArgs e)
        {
            bool res = recordLogic.updateRecord(fetchFromModifyPanel());
            if (res)
                MessageBox.Show("El record ha sido actualizado.");
            else
                MessageBox.Show("No se ha podido actualizar el record.");
        }

        private E_Record fetchFromModifyPanel()
        {
            var record = new E_Record();
            record.Name = nameEdit.Text;
            record.Surname = surnameEdit.Text;
            record.Address = addressEdit.Text;
            record.Genre =  genre(true);
            record.CivilState = civilState(true);
            record.Phone = phoneEdit.Text;
            record.Telephone = telephoneEdit.Text;
            record.Email = emailEdit.Text;
            record.Birthday = birthdayEdit.Text;
            return record;
        }

        private E_User attemptingUser;

        private E_Record recordToAdd;
        private E_Record fetchedRecord;

        private bool isSignUpBtn = false;

        private B_User userLogic = new B_User();
        private B_Record recordLogic = new B_Record();
    }
}
