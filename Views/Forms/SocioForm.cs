using System;
using System.Windows.Forms;
using ClubMinimal.Services;
using ClubMinimal.Repositories;
using ClubMinimal.Models;
using System.Drawing; 

namespace ClubMinimal.Views.Forms
{
    public class SocioForm : Form
    {
        private readonly SocioService _socioService;
        private readonly TextBox txtNombre;
        private readonly TextBox txtApellido;
        private readonly TextBox txtDni;
        private readonly DateTimePicker dtpFechaInscripcion;
        private readonly DateTimePicker dtpFechaVencimiento;
        private readonly CheckBox chkEstadoActivo;
        private readonly ComboBox cmbTipoSocio;
        private readonly ListBox listBox;

        public SocioForm()
        {
            // Configuración inicial
            this.Text = "Gestión de Socios";
            this.Width = 550;
            this.Height = 550;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Inicialización de dependencias
            var dbHelper = new DatabaseHelper();
            var socioRepo = new SocioRepository(dbHelper);
            _socioService = new SocioService(socioRepo);

            // Inicializar campos readonly 
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtDni = new TextBox();
            dtpFechaInscripcion = new DateTimePicker();
            dtpFechaVencimiento = new DateTimePicker();
            chkEstadoActivo = new CheckBox();
            cmbTipoSocio = new ComboBox();
            listBox = new ListBox();

            // Crear controles
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Configurar los controles ya inicializados
            int topPosition = 20;
            int labelWidth = 150;
            int controlLeft = labelWidth + 30;
            int verticalSpacing = 35;
            int controlWidth = 250;

            // Configuración de controles
            txtNombre.Left = controlLeft;
            txtNombre.Top = topPosition;
            txtNombre.Width = controlWidth;

            txtApellido.Left = controlLeft;
            txtApellido.Top = topPosition + verticalSpacing;
            txtApellido.Width = controlWidth;

            txtDni.Left = controlLeft;
            txtDni.Top = topPosition + verticalSpacing * 2;
            txtDni.Width = controlWidth;

            dtpFechaInscripcion.Left = controlLeft;
            dtpFechaInscripcion.Top = topPosition + verticalSpacing * 3;
            dtpFechaInscripcion.Width = controlWidth;
            dtpFechaInscripcion.Format = DateTimePickerFormat.Short;

            dtpFechaVencimiento.Left = controlLeft;
            dtpFechaVencimiento.Top = topPosition + verticalSpacing * 4;
            dtpFechaVencimiento.Width = controlWidth;
            dtpFechaVencimiento.Format = DateTimePickerFormat.Short;

            chkEstadoActivo.Left = controlLeft;
            chkEstadoActivo.Top = topPosition + verticalSpacing * 5;
            chkEstadoActivo.Text = "Activo";
            chkEstadoActivo.Checked = true;
            chkEstadoActivo.Width = controlWidth;

            cmbTipoSocio.Left = controlLeft;
            cmbTipoSocio.Top = topPosition + verticalSpacing * 6;
            cmbTipoSocio.Width = controlWidth;
            cmbTipoSocio.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoSocio.DataSource = Enum.GetValues(typeof(TipoSocio));

            listBox.Left = 20;
            listBox.Top = topPosition + verticalSpacing * 8;
            listBox.Width = this.Width - 40;
            listBox.Height = 180;

            // Etiquetas 
            var lblNombre = new Label
            {
                Text = "Nombre:",
                Left = 20,
                Top = topPosition,
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblApellido = new Label
            {
                Text = "Apellido:",
                Left = 20,
                Top = topPosition + verticalSpacing,
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblDni = new Label
            {
                Text = "DNI:",
                Left = 20,
                Top = topPosition + verticalSpacing * 2,
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblFechaInscripcion = new Label
            {
                Text = "Fecha Inscripción:",
                Left = 20,
                Top = topPosition + verticalSpacing * 3,
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblFechaVencimiento = new Label
            {
                Text = "Vencimiento Primera Cuota:",
                Left = 20,
                Top = topPosition + verticalSpacing * 4,
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblTipoSocio = new Label
            {
                Text = "Tipo de Socio:",
                Left = 20,
                Top = topPosition + verticalSpacing * 6,
                Width = labelWidth,
                TextAlign = ContentAlignment.MiddleRight
            };

            // Botones
            var btnGuardar = new Button
            {
                Text = "Guardar Socio",
                Left = (this.Width / 2) - 140,
                Top = topPosition + verticalSpacing * 7,
                Width = 120
            };

            var btnListar = new Button
            {
                Text = "Ver Socios",
                Left = (this.Width / 2) + 20,
                Top = topPosition + verticalSpacing * 7,
                Width = 120
            };

            // Eventos
            btnGuardar.Click += btnGuardar_Click;
            btnListar.Click += btnListar_Click;

            // Agregar controles al formulario 
            this.Controls.AddRange(new Control[] {
                lblNombre, txtNombre,
                lblApellido, txtApellido,
                lblDni, txtDni,
                lblFechaInscripcion, dtpFechaInscripcion,
                lblFechaVencimiento, dtpFechaVencimiento,
                chkEstadoActivo,
                lblTipoSocio, cmbTipoSocio,
                btnGuardar, btnListar,
                listBox
            });
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNombre.Text) &&
                !string.IsNullOrWhiteSpace(txtApellido.Text) &&
                !string.IsNullOrWhiteSpace(txtDni.Text))
            {
                // Validar que el DNI no exista usando el nuevo método
                if (_socioService.ExisteDni(txtDni.Text))
                {
                    MessageBox.Show("El DNI ingresado ya está registrado. Por favor ingrese un DNI diferente.");
                    txtDni.Focus();
                    return;
                }

                var nuevoSocio = new Socio
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Dni = txtDni.Text,
                    FechaInscripcion = dtpFechaInscripcion.Value,
                    FechaVencimientoCuota = dtpFechaVencimiento.Value,
                    EstadoActivo = chkEstadoActivo.Checked,
                    Tipo = (TipoSocio)cmbTipoSocio.SelectedValue
                };

                _socioService.RegistrarSocio(nuevoSocio);
                MessageBox.Show("Socio registrado correctamente!");
                LimpiarFormulario();
            }
            else
            {
                MessageBox.Show("Por favor complete todos los campos obligatorios (Nombre, Apellido y DNI)");
            }
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            var socios = _socioService.ObtenerSocios();
            foreach (var socio in socios)
            {
                listBox.Items.Add(string.Format("{0}: {1}, {2} - DNI: {3} - Tipo: {4} - Vencimiento: {5} - Estado: {6}",
                    socio.Id,
                    socio.Apellido,
                    socio.Nombre,
                    socio.Dni,
                    socio.Tipo,
                    socio.FechaVencimientoCuota.ToShortDateString(),
                    socio.EstadoActivo ? "Activo" : "Inactivo"));
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtDni.Clear();
            dtpFechaInscripcion.Value = DateTime.Today;
            dtpFechaVencimiento.Value = DateTime.Today.AddMonths(1);
            chkEstadoActivo.Checked = true;
            cmbTipoSocio.SelectedIndex = 0;
        }
    }
}
