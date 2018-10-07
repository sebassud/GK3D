using GalaxyScene.Render;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyScene.Services
{
    class GameService : IGameService
    {
        #region Zmienne prywatne
        private Projection projection;
        #endregion

        #region Zmienne publiczne
        /// <summary>
        /// Macierz projekcji
        /// </summary>
        public Matrix Projection => projection.MatrixProjection;

        /// <summary>
        /// Macierz widoku
        /// </summary>
        public Matrix View { get; set; }

        /// <summary>
        /// Pozycja gracza
        /// </summary>
        public Vector3 PlayerPosition { get; set; }

        /// <summary>
        /// Skala gry
        /// </summary>
        public float Scale { get; set; }
        #endregion

        public GameService()
        {
            projection = new Projection();
            PlayerPosition = new Vector3(0, 0, 0);
            Scale = 1;
        }
    }
}
