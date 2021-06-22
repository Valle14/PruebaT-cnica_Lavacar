// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// #region Variables Globales

let Accion;
const IdForm = "#idForm";
const IdFormTitulo = "#FormTitulo";
const IdModal = "#modalForm"
const AccionForm = { Agregar: "Agregar", Editar: "Editar" };
const UrlObtenerServicios = "/Servicios/ListarServicios";

// #endregion


// #region Metodos para mensajes

//constante con tiempo de espera en los mensajes con  timer
const timer = 1500;

//funcion que muestra error de conexion en un mensaje.
function ErrorDeConexion() {
    swal({
        title: "Error, no se pudo hacer conexión con el servidor",
        text: "Verifique su conexión a la red",
        icon: "error",
        buttons: {
            confirm: {
                text: "Confirmar",
            },
        },
    });
}//fin ErrorDeConexion

//funcion que muestra mensajes al usuario este debe confirmar el mensaje
function MostrarMensajeDeConfirmarcion(titulo, mensaje, tipo) {
    swal({
        title: titulo,
        content: {
            element: 'p',
            attributes: {
                innerHTML: "<h4>" + mensaje + "</h4>",
            },
        },
        icon: tipo,
        buttons: {
            confirm: {
                text: "Confirmar",
            },
        },
        closeOnClickOutside: false,
        closeOnEsc: false
    });
}

//funcion que muestra un mensaje con un gif de cargando en las peticiones
function Cargando() {
    swal({
        title: "Cargando...",
        text: "<h2>Espere por favor</h2>",
        icon: "/Img/cargando.gif",
        content: {
            element: 'p',
            attributes: {
               // innerHTML: '<h2><strong>Cargando...</strong><br></h2>' +
               //     '<h4 class="mb-4">Espere por favor</h4><i class="fas fa-spinner fa-spin fa-3x"></i> ',
            },
        },
        button: false,
        closeOnClickOutside: false,
        closeOnEsc: false
    });
}

//Configuración de mensajes de notificación de acciones
toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": true,
    "progressBar": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "swing",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

// #endregion


// #region FuncionGet y Validaciones de Formularios


//Funcion que permite hacer solicitud de datos al servidor por medio de ajax
function Get(url) {
    return new Promise(function (resolve, reject) {
        $.get(url).done(function (data) {
            resolve(data)
        }).fail(function (err) {
            reject(err)
        });
    });
}

function Validar(idForm) {
    $.validator.unobtrusive.parse($(idForm));
    $(".text-danger").show(); //Muestra los mensajes de error en caso de que existan 
}

function EsFormularioValido(idForm) {
    return $(idForm).valid() && $(idForm).validate().pendingRequest === 0;
}

// #endregion


// #region Funciones de solicitud(Msj confirmacion) y SolicitudPOST

//Muestra el mensaje de confirmación y en el caso de confirmar llama a SolicitudPOST
function Solicitud(url, urlRedirect) {

    if (EsFormularioValido(IdForm)) {

        swal({
            title: "¿Está seguro?",
            icon: "warning",
            closeOnClickOutside: false,
            closeOnEsc: false,
            buttons: {
                cancel: {
                    text: "Cancelar",
                    visible: true,
                },
                confirm: {
                    text: "Confirmar",
                },
            },
            dangerMode: true,
        }).then((result) => {
            if (result) {
                SolicitudPOST(url, urlRedirect);
            }
        });

    } else {
        Validar(IdForm);
    }
}


//Realiza la solicitud Post (de Agregar, Editar o Eliminar), muestra el mensaje y esconde la ventana modal.
function SolicitudPOST(url, urlRedirect) {

    let data = $(IdForm).serialize();

    //Cargando();
    $.post(url, data).done(function (respuesta) {
        if (respuesta.ok) {

            swal.close();
            window.location.href = urlRedirect + respuesta.mensaje;

        } else {

            MostrarMensajeDeConfirmarcion("Atención", respuesta.mensaje, "warning");
        }
    }).fail(function () {
        ErrorDeConexion();
    });

}


function Eliminar(datos) {

    swal({
        title: "¿Desea eliminar " + datos.PronomMin + " " + datos.Sujeto + " " + datos.Nombre + "?",
        icon: "warning",
        closeOnClickOutside: false,
        closeOnEsc: false,
        buttons: {
            cancel: {
                text: "Cancelar",
                visible: true,
            },
            confirm: {
                text: "Confirmar",
            },
        },
    }).then((willDelete) => {
        if (!willDelete) {
            swal.close();
        } else {
            swal({
                title: "¿Está seguro?",
                content: {
                    element: 'p',
                    attributes: {
                        innerHTML: "<h4>" + datos.PronomMay + " " + datos.Sujeto + " " + "<strong class='text-danger'>" + datos.Nombre + "</strong>" + " será eliminado permanentemente.</h4>",
                    },
                },
                icon: "warning",
                closeOnClickOutside: false,
                closeOnEsc: false,
                buttons: {
                    cancel: {
                        text: "Cancelar",
                        visible: true,
                    },
                    confirm: {
                        text: "Confirmar",
                    },
                },
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {

                        let data = { id: datos.Id };

                        //Cargando();
                        $.post(datos.URL, data).done(function (respuesta) {
                            if (respuesta.ok) {
                                                                
                                swal.close();
                                window.location.href = datos.urlRedirect + respuesta.mensaje;

                            } else {

                                MostrarMensajeDeConfirmarcion("Atención", respuesta.mensaje, "warning");
                            }
                        }).fail(function () {
                            ErrorDeConexion();
                        });

                    }
                });
        }
    });

}



// #endregion


// #region select


//Inicialización de los select 
$("#Select").selectpicker({
    selectAllText: "Seleccionar todos",
    deselectAllText: "Quitar todos",
    actionsBox: true,
    header: "Seleccione una o varios servicios",
    noneSelectedText: "Ninguno seleccionado",
    title: "Seleccione",
    selectedTextFormat: "count",
    size: 4,
    style: "form-control",
    styleBase: "form-control",
    countSelectedText: "{0} servicios seleccionados"
});

// #endregion


Get(UrlObtenerServicios).then(function (listaDeServicios) {

    GestionarSeleccionDeServicios(listaDeServicios);

}).catch(() => ErrorDeConexion());

function GestionarSeleccionDeServicios(listaServicios) {

    let opciones = "";

    $.each(listaServicios, function (i) {
        opciones += '<option value=' + listaServicios[i].idServicio + '>' + listaServicios[i].descripcion + '</option>';
    });

    $("#Select").html(opciones);

    $("#Select").selectpicker("refresh");

}