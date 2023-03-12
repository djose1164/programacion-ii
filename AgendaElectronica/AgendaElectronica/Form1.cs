using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Data.SqlClient;
namespace AgendaElectronica
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool isFieldEmpty(TextBox field)
        {
            return field.Text.Trim().Length == 0;
        }

        /**
         * Confirma que los campos que son obligatorios sean llenados.
         */ 
        private bool validate_necessary_fields()
        {
            if (isFieldEmpty(nameField) && isFieldEmpty(surnameField) && isFieldEmpty(emailField))
                return true;
            return false;
        }

        private string get_genre(bool forEdit = false)
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

        private string get_civil_state(bool forEdit = false)
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

        private void button1_Click(object sender, EventArgs e)
        {
            var conn = get_conn();

            string query = "INSERT INTO agenda VALUES('" + nameField.Text + "','" + surnameField.Text + "','" 
                + birthdayField.Text +"','" + addressField.Text +"','" + get_genre() +"','" + get_civil_state()
                +"','" + phoneField.Text +"','" + telephoneField.Text +"','" + emailField.Text +  "')";

            try
            {
                conn.Open();
                var stmt = new SqlCommand(query, conn);
                stmt.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Agenda agregada con exito!");
            }
            catch (SqlException se)
            {
                MessageBox.Show("No se pudo realizar la operacion: " + se.Message);
            }

            MessageBox.Show(string.Format("{0} {1} {2}", get_civil_state(), get_genre(), nameField.Text));
        }

        private SqlConnection get_conn()
        {
            string uri = "Data Source=DANIEL-PC; Initial Catalog=agenda;Integrated Security=True";
            var conn = new SqlConnection(uri);
            return conn;
        }

        private void set_genre(string genre)
        {
            if (genre == "Masculino")
                maleRBEdit.Checked = true;
            else
                femaleRBEdit.Checked = true;
        }

        private void set_civil_state(string civil_state)
        {
            foreach (var control in flowLayoutPanel4.Controls)
            {
                RadioButton radio = control as RadioButton;

                if (radio != null && radio.Text == civil_state)
                    radio.Checked = true;
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            if (searchField.Text.Length < 1)
                return;
            modifyPanel.Visible = false;

            var conn = get_conn();
            string query = "SELECT * FROM agenda WHERE correo_electronico = '" + searchField.Text + "'";
            try
            {
                conn.Open();
                var stmt = new SqlCommand(query, conn);
                var fetched = stmt.ExecuteReader();

                if (fetched.Read())
                {
                    nameEdit.Text = fetched["nombre"].ToString();
                    surnameEdit.Text = fetched["apellido"].ToString();
                    birthdayEdit.Text = fetched.GetDateTime(2).ToString("dd/MM/yyyy");
                    addressEdit.Text = fetched["direccion"].ToString();
                    set_genre(fetched["genero"].ToString());
                    set_civil_state(fetched["estado_civil"].ToString());
                    phoneEdit.Text = fetched["movil"].ToString();
                    telephoneEdit.Text = fetched["telefono"].ToString();
                    emailEdit.Text = thisemail = fetched["correo_electronico"].ToString();

                    modifyPanel.Visible = true;
                }
                else
                    MessageBox.Show("El registro solicitado no pudo ser encontrado");

                conn.Close();
                
            }
            catch (SqlException se)
            {
                MessageBox.Show("No se pudo procesar la accion: " + se.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var conn = get_conn();
            var query = "DELETE FROM agenda WHERE correo_electronico = '" + emailEdit.Text + "'";
            var stmt = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                stmt.ExecuteNonQuery();
                conn.Close();

                modifyPanel.Visible = false;
                MessageBox.Show("Registro eliminado exitosamente!");
            }
            catch (SqlException se)
            {
                MessageBox.Show("Error al eliminar registro: " + se.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var conn = get_conn();
            var query = "UPDATE agenda SET nombre='" + nameField.Text + "', apellido='" + surnameEdit.Text + "', fecha_nacimiento='"
                + birthdayEdit.Text + "', direccion='" + addressEdit.Text + "', movil='" + phoneEdit.Text + "', telefono='"
                + telephoneEdit.Text + "', genero='" + get_genre(true) + "', estado_civil='" + get_civil_state(true) + "', correo_electronico='"
                + emailEdit.Text + "' WHERE correo_electronico='" + thisemail + "'";
            var stmt = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                stmt.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Registro actualizado con exito!");
            }
            catch (SqlException se)
            {
                MessageBox.Show("Error al actualizar registo: " + se.Message);
            }
        }

        private string thisemail;
    }
}
