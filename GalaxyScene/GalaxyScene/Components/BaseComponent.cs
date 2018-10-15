using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        protected Effect effect;

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
            effect = Game.Content.Load<Effect>("Shader/Shader");
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
