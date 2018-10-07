using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyScene.Components
{
    /// <summary>
    /// Klasa bazowa dla komponentów
    /// </summary>
    public class BaseComponent : DrawableGameComponent
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="game">Obiekt gry</param>
        public BaseComponent(Game game) : base(game)
        {
        }
        /// <summary>
        /// Metoda ładująca komponent
        /// </summary>
        public new virtual void LoadContent()
        {
            base.LoadContent();
        }
        /// <summary>
        /// Metoda zwalniająca komponent
        /// </summary>
        public new virtual void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}
