using System;
using ClubMinimal.Models;

namespace ClubMinimal.Interfaces
{
    public interface ICuotaService
    {
        Socio BuscarSocio(string dni);
        void ProcesarPago(int socioId, decimal monto, MetodoPago metodo);
        decimal ObtenerValorCuota();
        Socio BuscarSocioPorId(int socioId);
    }
}
