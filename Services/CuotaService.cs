using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ClubMinimal.Interfaces;
using ClubMinimal.Models;

namespace ClubMinimal.Services
{
   
        
         public class CuotaService : ICuotaService
    {
        private readonly ICuotaRepository _cuotaRepository;
        private readonly ISocioRepository _socioRepository;
        private readonly decimal _valorCuota = 5000.00m;

        public CuotaService(ICuotaRepository cuotaRepository, ISocioRepository socioRepository)
        {
            if (cuotaRepository == null)
                throw new ArgumentNullException("cuotaRepository");
            if (socioRepository == null)
                throw new ArgumentNullException("socioRepository");

            _cuotaRepository = cuotaRepository;
            _socioRepository = socioRepository;
        }

        public Socio BuscarSocio(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede estar vacío");

            return _cuotaRepository.BuscarSocioActivoPorDni(dni.Trim());
        }

        public void ProcesarPago(int socioId, decimal monto, MetodoPago metodo)
        {                   
            if (monto != _valorCuota)
                throw new ArgumentException(string.Format("El monto debe ser exactamente {0:C}", _valorCuota));

            var nuevaFechaVencimiento = CalcularNuevoVencimiento(socioId);

            using (var transaction = new TransactionScope())
            {
                try
                {
                    _cuotaRepository.RegistrarPagoCuota(
                        socioId: socioId,
                        monto: monto,
                        fechaPago: DateTime.Now,
                        fechaVencimiento: nuevaFechaVencimiento,
                        metodo: metodo);

                    _cuotaRepository.ActualizarFechaVencimiento(socioId, nuevaFechaVencimiento);
                    _cuotaRepository.ActivarSocio(socioId);

                    transaction.Complete();
                }
                catch
                {
                    transaction.Dispose();
                    throw;
                }
            }
        }    
    
        public decimal ObtenerValorCuota()
        {
            return _valorCuota;
        }

        public IEnumerable<Cuota> ObtenerCuotasPorVencer(DateTime fechaLimite)
        {
            return _cuotaRepository.ObtenerCuotasPorVencer(fechaLimite)
                .OrderBy(c => c.FechaVencimiento)
                .ToList();
        }

        public IEnumerable<Cuota> ObtenerCuotasPorSocio(int socioId)
        {
            return _cuotaRepository.ObtenerCuotasPorSocio(socioId)
                .OrderByDescending(c => c.FechaVencimiento)
                .ToList();
        }

        private DateTime CalcularNuevoVencimiento(int socioId)
        {
            var socio = _socioRepository.ObtenerPorId(socioId);
            if (socio == null) throw new KeyNotFoundException("Socio no encontrado");

            switch (socio.Tipo)
            {
                case TipoSocio.Premium:
                    return DateTime.Now.AddMonths(2);
                case TipoSocio.Familiar:
                    return DateTime.Now.AddMonths(3);
                default: // Standard
                    return DateTime.Now.AddMonths(1);
            }
        }
	
	// En CuotaService.cs
        public Socio BuscarSocioPorId(int socioId)
      {
             return _socioRepository.ObtenerPorId(socioId);
       }
    }
}