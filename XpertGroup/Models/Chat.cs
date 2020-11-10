using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace XpertGroup.Models
{
    /// <summary>
    /// Esta clase administra todas las conversaciones y realiza los calculos finales
    /// de puntajes 
    /// </summary>
    public class Chat
    {
        private List<ChatConversation> Chats;
        private ChatConversation LastConveration = null;

        public Chat()
        {
            this.Chats = new List<ChatConversation>();
        }

        /// <summary>
        /// Obtiene en formato JSON los valores extraidos en objetos 
        /// </summary>
        /// <returns>JSON string</returns>
        public string getJson()
        {
            string json = "";
            json = JsonConvert.SerializeObject(this.Chats);
            return json;
        }

        /// <summary>
        /// Carga los datos de conversaciones desde un archivo de texto
        /// </summary>
        /// <param name="filePath">Ruta al archivo</param>
        public void ReadFromFile(string filePath)
        {
            string line = "";
            StreamReader file = new StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                this.ParseLine(line);
            }
            file.Close();
        }

        /// <summary>
        /// Procesa una linea del archivo, formateando la fecha, el usuario y el texto, 
        /// posteriormente lo guarda en una instancia de ChatLine
        /// </summary>
        /// <param name="line">Linea de texto a procesar</param>
        protected void ParseLine(string line)
        {
            line = line.Trim();
            if(line.Length == 0)
            {
                return;
            }

            string[] parts = line.Split(':');

            /// Si la primera linea describe el nombre de la conversacion, prepara una uneva instancia de
            /// ChatConversation
            if (parts.Length <= 1)
            {
                this.LastConveration = new ChatConversation();
                this.Chats.Add(this.LastConveration);
                this.LastConveration.Nombre = line;
                return;
            }

            if(this.LastConveration == null)
            {
                throw new FormatException("El archivo no se inicializó con un encabezado de conversación valido!");
            }
            /// Si la linea describe el tiempo, usuario y conversación, crea una instancia de ChatLine
            this.LastConveration.AddChatLine(line);
        }

        /// <summary>
        /// Convierte el objeto actual en formato String para pruebas...
        /// </summary>
        /// <returns>Objeto formateado</returns>
        public override string ToString()
        {
            string chats = "";
            foreach(ChatConversation cc in this.Chats)
            {
                chats += cc;
            }
            return chats;
        }
    }
}