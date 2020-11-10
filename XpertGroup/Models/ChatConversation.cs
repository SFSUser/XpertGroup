using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XpertGroup.Models
{
    /// <summary>
    /// Encapsula el nombre de la conversación y los mensajes en un solo objeto
    /// y realiza operaciones con el.
    /// </summary>
    public class ChatConversation
    {
        public String Nombre { get; set; }
        public List<ChatLine> ChatHistory;

        public ChatConversation()
        {
            this.ChatHistory = new List<ChatLine>();
        }

        /// <summary>
        /// Propiedad que representa el total de puntos calculados según los datos de conversación
        /// </summary>
        public Int32 Points {
            get {
                return this.CalcPoints();
            }
        }

        /// <summary>
        /// Propiedad que representa el puntaje (de 0 a 5) segun los puntos calculados
        /// </summary>
        public Int32 Score
        {
            get
            {
                return this.CalcScore();
            }
        }

        /// <summary>
        /// Propiedad que representa el total de segundos que duró la conversación
        /// </summary>
        public double Duration
        {
            get
            {
                return this.GetDuration();
            }
        }

        /// <summary>
        /// Retorna el total de segundos que duró la conversación
        /// </summary>
        /// <returns>Número de segundos</returns>
        private double GetDuration()
        {
            if(this.ChatHistory.Count < 1)
            {
                return 0;
            }

            DateTime a = this.ChatHistory.ElementAt(0).Date;
            DateTime b = this.ChatHistory.ElementAt(this.ChatHistory.Count - 1).Date;
            return b.Subtract(a).TotalSeconds;
        }

        /// <summary>
        /// Calcula el puntake actual según las condiciones establecidas
        /// </summary>
        /// <returns>Puntaje en total</returns>
        private int CalcScore()
        {
            int score = 0;
            int points = this.CalcPoints();

            if (points <= 0)
            {
                score = 0;
            }
            else if (points <= 25)
            {
                score = 1;
            }
            else if (points <= 50)
            {
                score = 2;
            }
            else if (points <= 75)
            {
                score = 3;
            }
            else if (points <= 90)
            {
                score = 4;
            }
            else if (points > 90)
            {
                score = 5;
            }
            return score;
        }

        /// <summary>
        /// Retorna la cantidad de puntos calculados segun las condiciones establecidas
        /// </summary>
        /// <returns>Total de puntos</returns>
        private int CalcPoints()
        {
            int points = 0;
            if (ChatHistory.Count <= 5)
            {
                points += 20;
            }
            else
            {
                points += 10;
            }
            if (ChatHistory.Count == 1)
            {
                points = - 100;
            }

            if (this.CountWordInHistory("URGENTE") <= 2)
            {
                points -= 5;
            }
            else
            {
                points -= 10;
            }

            if (this.GetDuration() >= 60)
            {
                points += 25;
            }
            else
            {
                points += 50;
            }

            //Lista de palabras establecidas
            string[] listaPalabras = "Gracias,Buena Atención,Muchas Gracias".Split(',');
            if(this.CountWordInHistory("EXCELENTE SERVICIO") > 0)
            {
                points += 100;
                return points;
            }
            else if(this.CountWordInHistory(listaPalabras) > 0)
            {
                points += 10;
            }
            return points;
        }

        /// <summary>
        /// Cuenta la cantidad de ocurrencias encontradas en el historial de conversación actual.
        /// </summary>
        /// <param name="word">Palabra a buscar</param>
        /// <returns>Cantidad de ocurrencias</returns>
        public int CountWordInHistory(string word)
        {
            int ocurrences = 0;
            foreach (ChatLine cl in this.ChatHistory)
            {
                ocurrences += cl.CountContainsWord(word);
            }
            return ocurrences;
        }


        /// <summary>
        /// Cuenta la cantidad de ocurrencias encontradas en el historial de conversación actual.
        /// </summary>
        /// <param name="word">Palabras a buscar (arreglo de string)</param>
        /// <returns>Cantidad de ocurrencias</returns>
        public int CountWordInHistory(string[] words)
        {
            int ocurrences = 0;
            foreach (ChatLine cl in this.ChatHistory)
            {
                ocurrences += cl.CountContainsWord(words);
            }
            return ocurrences;
        }

        /// <summary>
        /// Convierte el objeto y historial actual a formato JSON
        /// </summary>
        /// <returns>JSON string</returns>
        public string getJson()
        {
            string json = "";
            json = JsonConvert.SerializeObject(this);
            return json;
        }

        /// <summary>
        /// Agrega una objeto ChatLine al historial de chat
        /// </summary>
        /// <param name="cl">Instancia de ChatLine</param>
        /// <returns>ChatLine</returns>
        public ChatLine AddChatLine(ChatLine cl)
        {
            this.ChatHistory.Add(cl);
            return cl;
        }

        /// <summary>
        /// Formatea y agrega una linea de conversación al historial actual
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public ChatLine AddChatLine(string line)
        {
            ChatLine chatLine = new ChatLine();
            chatLine.ParseChatLine(line);
            this.ChatHistory.Add(chatLine);
            return chatLine;
        }

        /// <summary>
        /// Convierte a formato string el objeto actual y conversaciones.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string history = this.Nombre;
            foreach(ChatLine c in this.ChatHistory)
            {
                history += "\n" + c;
            }
            return history;
        }
    }
}