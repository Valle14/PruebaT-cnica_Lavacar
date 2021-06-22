using Lavacar.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavacar.AccesoDatos.Interfaces
{
    public interface IRepositorioServicio
    {
        Task<List<Servicio>> ListarServicios();

        Task AgregarServicio(Servicio servicio);

        Task EditarServicio(Servicio servicio);

        Task EliminarServicio(int id);

        Task EliminarVehiculosDeServicio(int id);

        Task<Servicio> ObtenerServicioPorId(int id);

        Task<Servicio> ObtenerServicioConVehiculosPorId(int id);

        Task<Servicio> VerificarExistenciaDeServicio(Servicio servicio);

    }
}
