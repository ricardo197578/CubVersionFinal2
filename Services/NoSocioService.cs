using System;
using System.Collections.Generic;
using ClubMinimal.Interfaces;
using ClubMinimal.Models;
using ClubMinimal.Repositories;

namespace ClubMinimal.Services

{
    public class NoSocioService : INoSocioService
    {
        private readonly INoSocioRepository _repository;

        public NoSocioService(INoSocioRepository repository)
        {
            _repository = repository;
        }

        public void RegistrarNoSocio(string nombre, string apellido, string dni)
        {
            var noSocio = new NoSocio { Nombre = nombre, Apellido = apellido, Dni = dni};
            _repository.Agregar(noSocio);
        }

        public List<NoSocio> ObtenerNoSocios()
        {
            return _repository.ObtenerTodos();
        }

        
        public NoSocio BuscarPorDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede estar vac�o");

            return _repository.BuscarPorDni(dni); 
        }
    }
}