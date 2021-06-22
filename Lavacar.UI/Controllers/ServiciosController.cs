using Lavacar.Entidades;
using Lavacar.Entidades.Enum;
using Lavacar.Entidades.Helpers;
using Lavacar.LogicaNegocio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavacar.Presentacion.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly LogicaServicio _logicaServicio;
        private readonly LogicaVehiculo _logicaVehiculo;

        public ServiciosController(LogicaServicio logicaServicio, LogicaVehiculo logicaVehiculo)
        {
            _logicaServicio = logicaServicio;
            _logicaVehiculo = logicaVehiculo;
        }

        public async Task<IActionResult> ListaDeServicios(string id)
        {
            var listaDeServicios = await _logicaServicio.ListarTodosLosServicios();

            ViewBag.Mensaje = id;

            return View(listaDeServicios);         
        }

        public async Task<JsonResult> ListarServicios()
        {
            var listaDeServicios = await _logicaServicio.ListarTodosLosServicios();

            return Json(listaDeServicios);
        }

        public IActionResult AgregarServicio()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarServicio(Servicio servicio)
        {
            Respuesta respuesta;

            try
            {
                if (ModelState.IsValid)
                {
                    respuesta = await _logicaServicio.AgregarServicio(servicio);

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

        public async Task<IActionResult> EditarServicio(int id)
        {
            var servicio = await _logicaServicio.ObtenerServicioPorId(id);

            return View(servicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditarServicio(Servicio servicio)
        {
            Respuesta respuesta;

            try
            {
                if (ModelState.IsValid)
                {
                    respuesta = await _logicaServicio.EditarServicio(servicio);

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
        public async Task<JsonResult> EliminarServicio(int id)
        {
            Respuesta respuesta;

            try
            {
                respuesta = await _logicaServicio.EliminarServicio(id);

                return Json(respuesta);

            }
            catch
            {

                respuesta = new Respuesta { Ok = false, Mensaje = HelperMensaje.GenerarMensaje(TipoMensaje.ErrorSolicitud, "") };

                return Json(respuesta);
            }

        }

        public async Task<IActionResult> VehiculosPorServicio(int id, string descripcion)
        {
            var listaDeServicios = await _logicaVehiculo.ListarVehiculosPorServicio(id);

            ViewBag.Servicio = descripcion;

            return View(listaDeServicios);
        }

       
    }

}
