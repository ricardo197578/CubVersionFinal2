using System;

namespace ClubMinimal.Models
{
    public class Carnet
    {
        public Carnet()
        {
            AptoFisico = false;
            FechaEmision = DateTime.Now;
            FechaVencimiento = DateTime.Now.AddYears(1); // Vencimiento por defecto: 1 a�o
        }

        public int Id { get; set; }
        public int NroCarnet { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool AptoFisico { get; set; }
        public int SocioId { get; set; } // Clave for�nea

        
    }
}