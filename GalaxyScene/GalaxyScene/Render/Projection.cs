using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyScene.Render
{
    /// <summary>
    /// Klasa dostarczająca macierz projekcji
    /// </summary>
    public class Projection
    {
        /// <summary>
        /// Długość ekranu
        /// </summary>
        public int PreferredBackBufferWidth { get; private set; }
        /// <summary>
        /// Wysokość ekranu
        /// </summary>
        public int PreferredBackBufferHeight { get; private set; }
        /// <summary>
        /// Macierz projekcji
        /// </summary>
        public Matrix MatrixProjection { get; private set; }

        public Projection(int preferredBackBufferWidth = 1920, int preferredBackBufferHeight = 1080)
        {
            Update(preferredBackBufferWidth, preferredBackBufferHeight);
        }
        /// <summary>
        /// Aktualizuje projekcję
        /// </summary>
        /// <param name="preferredBackBufferWidth">Długość ekranu</param>
        /// <param name="preferredBackBufferHeight">Wysokość ekranu</param>
        public void Update(int preferredBackBufferWidth, int preferredBackBufferHeight)
        {
            PreferredBackBufferHeight = preferredBackBufferHeight;
            PreferredBackBufferWidth = preferredBackBufferWidth;
            MatrixProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), preferredBackBufferWidth / (float)preferredBackBufferHeight, 0.01f, 2000);
        }
    }
}
