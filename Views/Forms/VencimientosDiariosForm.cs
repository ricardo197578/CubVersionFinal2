using System;
using System.Windows.Forms;
using ClubMinimal.Services;
using ClubMinimal.Interfaces;
using System.Collections.Generic;
using ClubMinimal.Models;
using System.Drawing;

namespace ClubMinimal.Views.Forms
{
    public class VencimientosDiariosForm : Form
    {
        private readonly ICuotaService _cuotaService;
        private DataGridView _dgvVencimientos;
        private Button _btnCerrar;

        public VencimientosDiariosForm(ICuotaService cuotaService)
        {
            if (cuotaService == null)
                throw new ArgumentNullException("cuotaService");
            
            _cuotaService = cuotaService;
            InitializeComponents();
            CargarVencimientosDiarios();
        }

        private void InitializeComponents()
        {
            this.Text = "Vencimientos Diarios";
            this.Width = 800;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // DataGridView
            _dgvVencimientos = new DataGridView
            {
                Left = 20,
                Top = 20,
                Width = 740,
                Height = 400,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Configurar columnas
            ConfigurarColumnas();

            // Botón Cerrar
            _btnCerrar = new Button 
            { 
                Text = "Cerrar", 
                Left = 660, 
                Top = 430, 
                Width = 100 
            };
            _btnCerrar.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                _dgvVencimientos,
                _btnCerrar
            });
        }

        private void ConfigurarColumnas()
        {
            _dgvVencimientos.Columns.Clear();
            
            _dgvVencimientos.Columns.Add("SocioId", "ID Socio");
            _dgvVencimientos.Columns.Add("Nombre", "Nombre");
            _dgvVencimientos.Columns.Add("Dni", "DNI");
            _dgvVencimientos.Columns.Add("Vencimiento", "Fecha Vencimiento");
            _dgvVencimientos.Columns.Add("Monto", "Monto");
            _dgvVencimientos.Columns.Add("Estado", "Estado");

            // Formato de columnas
            _dgvVencimientos.Columns["Vencimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";
            _dgvVencimientos.Columns["Monto"].DefaultCellStyle.Format = "C2";
            _dgvVencimientos.Columns["Monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void CargarVencimientosDiarios()
        {
            try
            {
                DateTime fechaHoy = DateTime.Today;
                var cuotasPorVencer = _cuotaService.ObtenerCuotasPorVencer(fechaHoy);

                _dgvVencimientos.Rows.Clear();

                foreach (var cuota in cuotasPorVencer)
                {
                    var socio = _cuotaService.BuscarSocioPorId(cuota.SocioId);
                    
                    string nombreCompleto = "Socio no encontrado";
                    string dni = "N/A";
                    
                    if (socio != null)
                    {
                        nombreCompleto = string.Format("{0}, {1}", socio.Apellido, socio.Nombre);
                        dni = socio.Dni;
                    }

                    string estado = cuota.Pagada ? "Pagada" : "Pendiente";

                    int rowIndex = _dgvVencimientos.Rows.Add(
                        cuota.SocioId,
                        nombreCompleto,
                        dni,
                        cuota.FechaVencimiento,
                        cuota.Monto,
                        estado
                    );

                    // Resaltar cuotas pendientes
                    if (!cuota.Pagada)
                    {
                        _dgvVencimientos.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                        _dgvVencimientos.Rows[rowIndex].DefaultCellStyle.Font = new Font(_dgvVencimientos.Font, FontStyle.Bold);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al cargar vencimientos: {0}", ex.Message), 
                              "Error", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Error);
            }
        }
    }
}