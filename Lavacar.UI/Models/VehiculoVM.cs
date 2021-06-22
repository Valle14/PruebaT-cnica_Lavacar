using Lavacar.Entidades;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lavacar.Presentacion.Models
{
    public class VehiculoVM
    {
        public int IdVehiculo { get; set; }

        [Required(ErrorMessage = "El campo placa es obligatorio.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "El campo dueño es obligatorio."), Display(Name = "Dueño")]
        public string Dueno { get; set; }

        [Required(ErrorMessage = "El campo marca es obligatorio.")]
        public string Marca { get; set; }

        [Display(Name = "Servicios a utilizar")]
        public int[] ListaServicios { get; set; }

    }

}
