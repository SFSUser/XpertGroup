using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace XpertGroup.Models
{
    /// <summary>
    /// Encapsula una linea de conversación del chat y realiza el formateo y operaciones con el
    /// </summary>
    public class ChatLine
    {
        public String Message { get; set; }
        public String User { get; set; }
        public DateTime Date { get; set; }

        /// <summary>
        /// Extrae de una linea de texto la hora, usuario y texto.
        /// </summary>
        /// <param name="line">Linea de texto a procesar</param>
        /// <returns>Si el formateado fue correcto, retorna TRUE</returns>
        public bool ParseChatLine(string line)
        {
            string[] parts = line.Split(':');
            if(parts.Length == 1)
            {
                return false;
            }

            /// Regex para extraer la hora de la linea de texto
            string pattern = @".([0-9]):.([0-9]):.([0-9])";
            Match m = Regex.Match(line, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                this.Date = DateTime.ParseExact(m.Value, "H:m:s", null);

                string noHour = line.Replace(m.Value, "");
                string[] lineParts = noHour.Split(':');

                //Asignar en variables las partes del texto (Usuario y mensaje)
                this.User = lineParts[0].Trim();
                this.Message = lineParts[lineParts.Length - 1].Trim();
                return true;
            }
            //Si la linea no tiene un formato válido, lanza excepción.
            throw new FormatException("El formato de línea no es válido!: " + line);
            //return false;
        }

        /// <summary>
        /// Cuenta el numero de veces que aparece una palabra en el mensaje de chat
        /// </summary>
        /// <param name="word">Palabra en formato string</param>
        /// <returns>int con número de ocurrencias</returns>
        public int CountContainsWord(string word)
        {
            int ocurrences = Regex.Match(this.Message.ToLower(), word.ToLower()).Length;
            return ocurrences;
        }

        /// <summary>
        /// Cuenta el numero de veces que aparece una palabra en el mensaje de chat
        /// </summary>
        /// <param name="word">Palabra en formato de arreglo de string</param>
        /// <returns>int con número de ocurrencias</returns>
        public int CountContainsWord(string[] words)
        {
            int total = 0;
            foreach(string word in words)
            {
                total += this.CountContainsWord(word);
            }
            return total;
        }

        /// <summary>
        /// Formate el mensaje actual de chat a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Date.ToShortTimeString() + " - " + this.User + ": " + this.Message;
        }
    }
}