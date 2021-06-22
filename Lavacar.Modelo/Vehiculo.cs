using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



#nullable disable

namespace Lavacar.Entidades
{
    [Table("Vehiculo")]
    public partial class Vehiculo
    {
        public Vehiculo()
        {
            VehiculoServicios = new HashSet<VehiculoServicio>();
        }

        [Key]
        [Column("ID_Vehiculo")]
        public int IdVehiculo { get; set; }

        [Required]
        [StringLength(30)]
        public string Placa { get; set; }

        [Required, Display(Name = "Dueño")]
        public string Dueno { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Marca { get; set; }

        [NotMapped]
        public int[] ListaServicios { get; set; }

        [InverseProperty(nameof(VehiculoServicio.IdVehiculoNavigation))]
        public virtual ICollection<VehiculoServicio> VehiculoServicios { get; set; }
    }
}
