using System;
using System.Collections.Generic;
using ClubMinimal.Models;

namespace ClubMinimal.Interfaces
{
    //Es una interface, por lo que solo declara métodos sin implementarlos
    public interface ISocioService
    {
        void RegistrarSocio(string nombre, string apellido, string dni);
        List<Socio> ObtenerSocios();
        Socio GetSocio(int id);
        Socio GetSocio(string dni);
              
        //EXPLICACION DE AGREGADO DE METODOS
        bool ExisteDni(string dni); // 1RO AGREGO EL METODO PARA VALIDAR PRIMERO 

    }
}

/*
// Interfaces/ISocioService.cs
using ClubMinimal.Models;
using System.Collections.Generic;

namespace ClubMinimal.Interfaces
{
    public interface ISocioService
    {
        void RegistrarSocio(string nombre, string apellido, string dni, TipoSocio tipo);
        List<Socio> ObtenerSocios();
        Socio GetSocio(int id);
        Socio GetSocio(string dni);
        bool ExisteDni(string dni);
        Socio ObtenerSocioPorId(int id);
        void ActualizarVencimientoCuota(int socioId, DateTime nuevaFechaVencimiento);
    }
}
*/