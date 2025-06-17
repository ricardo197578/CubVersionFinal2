using System;
using System.Windows.Forms;
using ClubMinimal.Services;
using ClubMinimal.Repositories;
using ClubMinimal.Models;
using ClubMinimal.Interfaces;
using ClubMinimal.Views.Forms;

namespace ClubMinimal.Views.Forms
{
    public class MenuPrincipalForm : Form
    {
        private readonly ISocioService _socioService;
        private readonly ICarnetService _carnetService;
        private readonly IPagoService _pagoService;
        private readonly SocioRepository _socioRepository;
        private readonly INoSocioService _noSocioService;
        private readonly IActividadService _actividadService;
        private readonly ActividadRepository _actividadRepository;
        private readonly ICuotaService _cuotaService;
        private readonly ICuotaRepository _cuotaRepository;


        public MenuPrincipalForm(
            ISocioService socioService,
            ICarnetService carnetService,
            IPagoService pagoService,
            SocioRepository socioRepository,
            INoSocioService noSocioService,
            IActividadService actividadService,
            ActividadRepository actividadRepository,
            ICuotaService cuotaService,      
            ICuotaRepository cuotaRepository) 
        {
            _socioService = socioService;
            _carnetService = carnetService;
            _pagoService = pagoService;
            _socioRepository = socioRepository;
            _noSocioService = noSocioService;
            _actividadService = actividadService;
            _actividadRepository = actividadRepository;
            _cuotaService = cuotaService;
            _cuotaRepository = cuotaRepository;

            InitializeUI();
        }

        private void InitializeUI()
        {
            // Configuración del formulario
            this.Text = "Menú Principal - Club Minimal";
            this.Width = 350;
            this.Height = 420; 
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Configuración de controles 
            var btnSocios = CreateButton("Gestión de Socios", 30);
            var btnCarnetSocio = CreateButton("Gestión de Carnet", 70);
            var btnPagoCuota = CreateButton("Pago de Cuota Social", 110);
            var btnNoSocios = CreateButton("Gestión de No Socios", 150);
            var btnGestionActividades = CreateButton("Gestión de Actividades", 190);          
            var btnPagoActividades = CreateButton("Pago de Actividades", 230);
            //var btnBuscarPorDni = CreateButton("Buscar Socio por DNI", 270);
            var btnVencimientosDiarios = CreateButton("Vencimientos Diarios", 310);
           

            var btnSalir = CreateButton("Salir", 340); 

            // Event handlers
            btnSocios.Click += (s, e) => new SocioForm().ShowDialog();
            btnNoSocios.Click += (s, e) => new NoSocioForm().ShowDialog();
            btnCarnetSocio.Click += (s, e) => new frmGestionCarnet(_socioService, _carnetService).ShowDialog();
            //btnBuscarPorDni.Click += (s, e) => new frmBuscarSocioPorDni(_socioRepository).ShowDialog();
            btnPagoActividades.Click += (s, e) => new PagoActividadForm(
                                        _noSocioService,
                                        _actividadService,
                                        _pagoService).ShowDialog();
            btnGestionActividades.Click += (s, e) => new frmActividad(_actividadRepository).ShowDialog();
            btnPagoCuota.Click += (s, e) => new PagoCuotaForm(_cuotaService, _cuotaRepository).ShowDialog();
            
		btnVencimientosDiarios.Click += (s, e) => new VencimientosDiariosForm(_cuotaService).ShowDialog();
	    btnSalir.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                btnSocios,
                btnNoSocios,
                btnCarnetSocio,
                //btnBuscarPorDni,
                btnPagoActividades,
                btnGestionActividades, 
                btnPagoCuota,
                btnVencimientosDiarios,
                btnSalir
            });
        }

        private Button CreateButton(string text, int top)
        {
            return new Button
            {
                Text = text,
                Left = 75,
                Top = top,
                Width = 200,
                Height = 30
            };
        }
    }
}
