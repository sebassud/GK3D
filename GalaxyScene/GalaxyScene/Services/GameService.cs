﻿using GalaxyScene.GameModels;
using GalaxyScene.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        /// <summary>
        /// Reflektory
        /// </summary>
        public List<Reflector> Reflectors { get; set; }

        public GraphicsDeviceManager Graphics { get; }

        public Texture2D TextureAd { get; set; }

        public Texture2D ShadowMap { get; set; }

        public Matrix ProjectionLight
        {
            get
            {
                return projection.MatrixOrthographic;
            }
        }

        #endregion

        public GameService(GraphicsDeviceManager graphics)
        {
            projection = new Projection();
            Player = new Player();
            Scale = 1;
            Reflectors = new List<Reflector>();
            Graphics = graphics;
        }
    }
}
