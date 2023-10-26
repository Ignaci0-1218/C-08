using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Formulario_5___P.A___IJCA
{
    public partial class Form1 : Form
    {
        string conexionSQL = "Server=localhost;Port=3306;Database=formulario;Uid=root;Pwd=Paquetax0";
        public Form1()
        {

            InitializeComponent();

            textBox1.TextChanged += ValidarNombre;
            textBox2.TextChanged += ValidarApellidos;
            textBox4.TextChanged += ValidarTelefono;
            textBox3.TextChanged += ValidarEstatura;
            textBox5.TextChanged += ValidarEdad;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombres = textBox1.Text;
            string apellidos = textBox2.Text;
            string telefono = textBox4.Text;
            string estatura = textBox3.Text;
            string edad = textBox5.Text;

            string genero = "";
            if (radioButton1.Checked)
            {
                genero = "Hombre";
            }
            else if (radioButton2.Checked)
            {
                genero = "Mujer";
            }

            if (esenterovalido(edad) && esdecimalvalido(estatura) && esenterode10digitos(telefono) && estextovalido(nombres) && estextovalido(apellidos))
            {
                // Crear una cadena con los datos
                string datos = $"Nombres: {nombres}\r\nApellidos: {apellidos}\r\nTeléfono: {telefono}\r\nEstatura: {estatura}\r\nEdad: {edad}\r\nGénero: {genero}";

                string rutaArchivo = ("C:/Users/ignac/Documents/Documentos de Programación/Visual/Practica 1/Formulario 5 - P.A - IJCA/datos.txt");
                bool archivoExiste = File.Exists(rutaArchivo);
                Console.WriteLine(archivoExiste);

                // Verificar si el archivo ya existe
                using (StreamWriter writer = new StreamWriter(rutaArchivo, true))
                    if (archivoExiste)
                    {
                        writer.WriteLine();
                        // Programación de fucionalidad de insert SQL
                        insertarRegistro(nombres, apellidos, int.Parse(edad), decimal.Parse(estatura), telefono, genero);
                        MessageBox.Show("Datos ingresados correctamente");
                    }
                    else
                    {
                        writer.WriteLine(datos);
                        // Programación de fucionalidad de insert SQL
                        insertarRegistro(nombres, apellidos, int.Parse(edad), decimal.Parse(estatura), telefono, genero);
                        MessageBox.Show("Datos ingresados correctamente");
                    }

                //Mostrar un mensaje con los datos capturados
                MessageBox.Show($"Datos guardados con éxito\n\n{datos}", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Por favor, ingrese datos válidos en los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private bool esenterovalido(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }

        private bool esdecimalvalido(string valor)
        {
            decimal resultado;
            return decimal.TryParse(valor, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out resultado);
        }


        private bool esenterode10digitos(string valor)
        {
            long resultado;
            return long.TryParse(valor, out resultado);
        }

        private bool estextovalido(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-Z\s]+$");
        }
        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!esenterovalido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese una edad válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }
        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!esdecimalvalido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese una estatura válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }
        private void ValidarTelefono(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string input = textBox.Text;
            if (input.Length < 10)
            {
                if (!esenterode10digitos(input))
                {
                    MessageBox.Show("Por favor, ingrese una estatura válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox.Clear();
                }
            } else if (!esenterode10digitos(input))
            {
                MessageBox.Show("Por favor, ingrese una estatura válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }

        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!estextovalido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese un nombre válido (solo letras y espacios)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }
        private void ValidarApellidos(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!estextovalido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese apellidos válidos (solo letras y espacios)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox3.Clear();
            textBox5.Clear();
        }
        private void insertarRegistro(string nombre, string apellidos, int edad, decimal estatura, string telefono, string genero)
        {
            using (MySqlConnection connection = new MySqlConnection(conexionSQL))
            {
                connection.Open();
                string insertQuery = "INSERT INTO informacion (Nombre, Apellidos, Telefono, Estatura, Edad, Genero) " + "VALUES (@Nombre, @Apellidos, @Telefono, @Estatura, @Edad, @Genero)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Apellidos", apellidos);
                    command.Parameters.AddWithValue("@Telefono", telefono);
                    command.Parameters.AddWithValue("@Estatura", estatura);
                    command.Parameters.AddWithValue("@Edad", edad);
                    command.Parameters.AddWithValue("@Genero", genero);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
