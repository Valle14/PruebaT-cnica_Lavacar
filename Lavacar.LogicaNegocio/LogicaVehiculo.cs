using Lavacar.AccesoDatos.Interfaces;
using Lavacar.Entidades;
using Lavacar.Entidades.Enum;
using Lavacar.Entidades.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lavacar.LogicaNegocio
{
    public class LogicaVehiculo
    {
        private readonly IRepositorioVehiculo _repositorioVehiculo;

        public LogicaVehiculo(IRepositorioVehiculo repositorioVehiculo)
        {
            _repositorioVehiculo = repositorioVehiculo;
        }

        #region CRUD Vehiculos

        public async Task<List<Vehiculo>> ListarTodosLosVehiculos()
        {
            var listaDeVehiculos = await _repositorioVehiculo.ListarVehiculos();

            return listaDeVehiculos;
        }

        public async Task<Respuesta> AgregarVehiculo(Vehiculo vehiculo)
        {
            Respuesta respuesta;

            bool yaExiste = await VerificarExistenciaDeVehiculo(vehiculo);

            if (!yaExiste)
            {
                var vehiculoAgregado = await _repositorioVehiculo.AgregarVehiculo(vehiculo);

                if (vehiculo.ListaServicios != null)
                {
                    vehiculo.IdVehiculo = vehiculoAgregado.IdVehiculo;
                    await AgregarServiciosDeVehiculo(vehiculo);
                }

                respuesta = new Respuesta { Ok = true, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.Agregado, "El vehiculo") };
            }
            else
            {
                respuesta = new Respuesta { Ok = false, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.ErrorYaExiste, "El vehiculo") };
            }

            return respuesta;
        }

        public async Task<Respuesta> EditarVehiculo(Vehiculo vehiculoEditado)
        {
            Respuesta respuesta;

            var yaEditado = await VerificarEdicionDeVehiculo(vehiculoEditado);

            var serviciosEditados = await ComprobarEdicionDeServiciosDeVehiculo(vehiculoEditado);

            if (yaEditado || serviciosEditados)
            {
                bool yaExiste = await VerificarExistenciaDeVehiculo(vehiculoEditado);

                if (!yaExiste)
                {
                    if (yaEditado)
                    {
                        var vehiculoExistente = await ObtenerVehiculoPorId(vehiculoEditado.IdVehiculo);

                        vehiculoExistente.Placa = vehiculoEditado.Placa;
                        vehiculoExistente.Dueno = vehiculoEditado.Dueno;
                        vehiculoExistente.Marca = vehiculoEditado.Marca;

                        await _repositorioVehiculo.EditarVehiculo(vehiculoExistente);
                    }

                    if (serviciosEditados)
                    {
                        await EditarServiciosDeVehiculo(vehiculoEditado);
                    }

                    respuesta = new Respuesta { Ok = true, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.Editado, "El vehiculo") };

                }
                else
                {
                    respuesta = new Respuesta { Ok = false, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.ErrorYaExiste, "El vehiculo") };
                }
            }
            else
            {
                respuesta = new Respuesta { Ok = false, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.ErrorEditar, "") };
            }

            return respuesta;

        }

        public async Task<Respuesta> EliminarVehiculo(int id)
        {
            var poseeServicios = await VerificarExistenciaDeServicios(id);

            if (poseeServicios)
            {
                await _repositorioVehiculo.EliminarServiciosDeVehiculo(id);
            }

            await _repositorioVehiculo.EliminarVehiculo(id);

            return _ = new Respuesta { Ok = true, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.Eliminado, "El vehiculo") };
        }

        #endregion

        #region Obteniendo 
        public async Task<Vehiculo> ObtenerVehiculoPorId(int id)
        {
            return await _repositorioVehiculo.ObtenerVehiculoPorId(id);
        }

        public async Task<int[]> ObtenerIdServiciosDeVehiculo(int id)
        {
            var listaServiciosDeVehiculo = await ObtenerServiciosDeVehiculo(id);

            List<int> ListaIdServicios = new List<int>();

            foreach (VehiculoServicio vehiculoServicio in listaServiciosDeVehiculo)
            {
                ListaIdServicios.Add(vehiculoServicio.IdServicio);
            }

            return ListaIdServicios.ToArray();
        }

        public async Task<List<VehiculoServicio>> ObtenerServiciosDeVehiculo(int id)
        {
            return await _repositorioVehiculo.ObtenerServiciosDeVehiculo(id);
        }

        #endregion

        #region Servicios de Vehículos 
        public async Task AgregarServiciosDeVehiculo(Vehiculo vehiculo)
        {
            var listaDeServiciosDeVehiculo = new List<VehiculoServicio>();

            foreach (int idServicio in vehiculo.ListaServicios)
            {
                var vehiculoServicio = new VehiculoServicio { IdVehiculo = vehiculo.IdVehiculo, IdServicio = idServicio };
                listaDeServiciosDeVehiculo.Add(vehiculoServicio);
            }

            await _repositorioVehiculo.AgregarServiciosDeVehiculo(listaDeServiciosDeVehiculo);
        }

        public async Task EditarServiciosDeVehiculo(Vehiculo vehiculo)
        {
            await _repositorioVehiculo.EliminarServiciosDeVehiculo(vehiculo.IdVehiculo);

            if (vehiculo.ListaServicios != null)
                await AgregarServiciosDeVehiculo(vehiculo);
        }


        public async Task<List<Vehiculo>> ListarVehiculosPorServicio(int id)
        {
            return await _repositorioVehiculo.ListarVehiculosPorServicio(id);
        }

        #endregion

        #region Verificaciones 

        public async Task<bool> VerificarExistenciaDeVehiculo(Vehiculo vehiculo)
        {
            var vehiculoExistente = await _repositorioVehiculo.VerificarExistenciaDeVehiculo(vehiculo);

            bool respuesta = vehiculoExistente != null;

            return respuesta;
        }

        public async Task<bool> VerificarEdicionDeVehiculo(Vehiculo vehiculoEditado)
        {
            var vehiculoExistente = await _repositorioVehiculo.ObtenerVehiculoPorId(vehiculoEditado.IdVehiculo);

            bool respuesta;

            if (vehiculoExistente.Dueno != vehiculoEditado.Dueno
                || vehiculoExistente.Placa != vehiculoEditado.Placa
                || vehiculoExistente.Marca != vehiculoEditado.Marca)
            {

                respuesta = true;

            }
            else
            {

                respuesta = false;
            }

            return respuesta;
        }

        public async Task<bool> VerificarExistenciaDeServicios(int id)
        {
            var vehiculo = await _repositorioVehiculo.ObtenerVehiculoConServiciosPorId(id);

            var existenciaDeServicios = vehiculo.VehiculoServicios.Count != 0;

            return existenciaDeServicios;
        }

        public async Task<bool> ComprobarEdicionDeServiciosDeVehiculo(Vehiculo vehiculo)
        {
            var listaServiciosDeVehiculo = await ObtenerServiciosDeVehiculo(vehiculo.IdVehiculo);

            var respuesta = false;

            if (vehiculo.ListaServicios != null && vehiculo.ListaServicios.Length == listaServiciosDeVehiculo.Count)
            {
                foreach (int idServicio in vehiculo.ListaServicios)
                {
                    if (!listaServiciosDeVehiculo.Exists(servicio => servicio.IdServicio == idServicio))
                    {
                        respuesta = true;
                        return respuesta;
                    }
                }
            }
            else
            {
                respuesta = true;
                return respuesta;
            }

            return respuesta;
        }




        #endregion

    }
}
