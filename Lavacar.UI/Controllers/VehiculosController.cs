using Lavacar.Entidades;
using Lavacar.Entidades.Enum;
using Lavacar.Entidades.Helpers;
using Lavacar.LogicaNegocio;
using Lavacar.Presentacion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavacar.Presentacion.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly LogicaVehiculo _logicaVehiculo;

        public VehiculosController(LogicaVehiculo logicaVehiculo)
        {
            _logicaVehiculo = logicaVehiculo;
        }
        public async Task<IActionResult> ListaDeVehiculos(string id)
        {
            var listaDeVehiculos = await _logicaVehiculo.ListarTodosLosVehiculos();

            ViewBag.Mensaje = id;

            return View(listaDeVehiculos);
        }

        public IActionResult AgregarVehiculo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarVehiculo(VehiculoVM vehiculo)
        {
            Respuesta respuesta;

            try
            {
                if (ModelState.IsValid)
                {

                    var nuevoVehiculo = new Vehiculo
                    {
                        IdVehiculo = 0,
                        Dueno = vehiculo.Dueno,
                        Marca = vehiculo.Marca,
                        Placa = vehiculo.Placa,
                        ListaServicios = vehiculo.ListaServicios
                    };

                    respuesta = await _logicaVehiculo.AgregarVehiculo(nuevoVehiculo);

                    return Json(respuesta);
                }
                else
                {
                    respuesta = new Respuesta { Ok = false, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.Incorrecto, "") };

                    return Json(respuesta);
                }

            }
            catch
            {
                respuesta = new Respuesta { Ok = false, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.ErrorSolicitud, "") };

                return Json(respuesta);
            }
        }

        public async Task<IActionResult> EditarVehiculo(int id)
        {
            var vehiculo = await _logicaVehiculo.ObtenerVehiculoPorId(id);

            var vehiculoVM = new VehiculoVM
            {
                IdVehiculo = vehiculo.IdVehiculo,
                Dueno = vehiculo.Dueno,
                Placa = vehiculo.Placa,
                Marca = vehiculo.Marca
            };

            return View(vehiculoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditarVehiculo(VehiculoVM vehiculo)
        {
            Respuesta respuesta;

            try
            {
                if (ModelState.IsValid)
                {
                    var vehiculoEditado = new Vehiculo
                    {
                        IdVehiculo = vehiculo.IdVehiculo,
                        Dueno = vehiculo.Dueno,
                        Marca = vehiculo.Marca,
                        Placa = vehiculo.Placa,
                        ListaServicios = vehiculo.ListaServicios
                    };

                    respuesta = await _logicaVehiculo.EditarVehiculo(vehiculoEditado);

                    return Json(respuesta);

                }
                else
                {

                    respuesta = new Respuesta { Ok = false, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.Incorrecto, "") };

                    return Json(respuesta);
                }

            }
            catch (Exception e)
            {
                respuesta = new Respuesta { Ok = false, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.ErrorSolicitud, e.Message) };

                return Json(respuesta);
            }

        }

        [HttpPost]
        public async Task<JsonResult> EliminarVehiculo(int id)
        {
            Respuesta respuesta;

            try
            {
                respuesta = await _logicaVehiculo.EliminarVehiculo(id);

                return Json(respuesta);

            }
            catch
            {

                respuesta = new Respuesta { Ok = false, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.ErrorSolicitud, "") };

                return Json(respuesta);
            }

        }

        public async Task<JsonResult> ObtenerIdServiciosDeVehiculo(int id)
        {
            var ListaIdServicios = await _logicaVehiculo.ObtenerIdServiciosDeVehiculo(id);

            return Json(ListaIdServicios);
        }


    }
}
