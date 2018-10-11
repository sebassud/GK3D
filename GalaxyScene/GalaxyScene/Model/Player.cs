using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyScene.Model
{
    public class Player
    {
        /// <summary>
        /// Pozycja gracza
        /// </summary>
        public Vector3 PlayerPosition { get; set; }


        /// <summary>
        /// Kierunek patrzenia
        /// </summary>
        public Vector3 Direction { get; set; }
    }
}
