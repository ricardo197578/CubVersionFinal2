using System;
using System.Linq;
using ClubMinimal.Models;
using System.Collections.Generic; 

namespace ClubMinimal.Interfaces
{
    public interface ICuotaRepository
    {
        Socio BuscarSocioActivoPorDni(string dni);
        void RegistrarPagoCuota(int socioId, decimal monto, DateTime fechaPago, DateTime fechaVencimiento, MetodoPago metodo);
        DateTime ObtenerFechaVencimientoActual(int socioId);
        void ActualizarFechaVencimiento(int socioId, DateTime nuevaFecha);
        void ActivarSocio(int socioId);

	// Nuevos métodos requeridos
        IEnumerable<Cuota> ObtenerCuotasPorVencer(DateTime fechaLimite);
        IEnumerable<Cuota> ObtenerCuotasPorSocio(int socioId);
        decimal ObtenerValorActualCuota();
    }
}