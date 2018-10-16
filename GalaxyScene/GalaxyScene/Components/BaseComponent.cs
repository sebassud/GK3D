using GalaxyScene.Services;
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
        protected IGameService gameService;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="game">Obiekt gry</param>
        public BaseComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
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
