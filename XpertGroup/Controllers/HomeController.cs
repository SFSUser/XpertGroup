using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XpertGroup.Models;

namespace XpertGroup.Controllers
{
    /// <summary>
    /// Clase principal para el controlador.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Muestra la pagina principal con el formulario
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Servicio que recibirá el archivo y nos retornará la información procesada con los puntajes
        /// </summary>
        /// <returns>JSON con la información procesada</returns>
        public ActionResult Lista()
        {
            HttpPostedFileBase file = null;
            String file_path = "";
            String file_name = "";

            /// Mensajes predeterminados
            int result = 0;
            string message = "No se cargó el archivo.";
            Chat chatManager = new Chat();

            /// Verificar si se enviaron archivos
            if (Request.Files.Count > 0)
            {
                file = Request.Files[0];
            }

            /// Si se encontro el archivo, se procesa...
            if (file != null)
            {
                file_name = file.FileName;
                file_path = Path.Combine(Server.MapPath("~/App_Data/"), file_name);
                file.SaveAs(file_path);
                try
                {
                    chatManager.ReadFromFile(file_path);
                }
                catch (FormatException ex)
                {
                    message = ex.Message;
                }
                result = 1;
                message = chatManager.getJson();
                return this.Content(message, "application/json");
            }
           return Json(new { Result = result, Message = message });
        }
    }
}