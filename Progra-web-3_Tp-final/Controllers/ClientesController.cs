﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Progra_web_3_Tp_final.Models;
using Progra_web_3_Tp_final.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progra_web_3_Tp_final.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        _20211CTPContext context;
        private IClientesServicio _clienteServicio;
        private readonly TokenServicio _tokenServicio;
        private readonly IConfiguration _configuration;
        private readonly NavegarServicio _navegarServicio;

        public ClientesController(IConfiguration config)
        {
            context = new _20211CTPContext();
            _clienteServicio = new ClientesServicio(context);
            _tokenServicio = new TokenServicio();
            _configuration = config;
            _navegarServicio = new NavegarServicio();
        }
        
        [AllowAnonymous]
        public IActionResult Index()
        {

            var secretKey = _configuration.GetValue<string>("SecretKey");
            string returnView = _navegarServicio.ValidarNavegacion(HttpContext.Session.GetString("Token"), HttpContext.Session.GetString("EsAdmin"), _configuration, 'Y', "Clientes");

            if (returnView == "Home")
            {
                HttpContext.Session.SetString("VistaAnteriorSinLogin", "/Usuarios");
                return Redirect("/Home");
            }

            return View(context.Clientes.ToList());
        }

        public ActionResult NuevoCliente()
        {
            return View();
        }

        public ActionResult EditarCliente(int id)
        {
            return View(_clienteServicio.ObtenerPorId(id));
        }

        public StatusCodeResult Alta(Cliente cliente)
        {
            _clienteServicio.Alta(cliente);
            return Ok();
        }

        public StatusCodeResult Eliminar(int id) {
            Cliente cliente = _clienteServicio.ObtenerPorId(id);
            _clienteServicio.Eliminar(cliente);

            return Ok();
        }

        public Boolean ExisteNumero(int numero)
        {
            return context.Clientes.Where(c => c.Numero == numero).Count() > 0;
        }
    }
}
