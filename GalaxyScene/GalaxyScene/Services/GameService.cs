using GalaxyScene.GameModels;
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
        /// Gracz
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Skala gry
        /// </summary>
        public float Scale { get; set; }
        #endregion

        public GameService()
        {
            projection = new Projection();
            Player = new Player();
            Scale = 1;
        }
    }
}
