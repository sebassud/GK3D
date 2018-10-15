using GalaxyScene.Components;
using GalaxyScene.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyScene
{
    public class GameManager
    {
        private Game _game;
        private List<BaseComponent> _components;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="game">Obiekt gry</param>
        public GameManager(Game game)
        {
            _game = game;
            _game.Services.AddService(typeof(IGameService), new GameService());
        }
        /// <summary>
        /// Pobiera komponenty
        /// </summary>
        public void Initialize()
        {
            _components = new List<BaseComponent>() {
                new CameraComponent(_game),
                new PlayerComponent(_game),
                new PlanetoidComponent(_game),
                new StationComponent(_game),
                new SatelliteComponent(_game),
                new SpaceshipComponent(_game),
                new ComDishComponent(_game),
                new BackgroundComponent(_game)};
            foreach (var component in _components)
            {
                component.Initialize();
            }
        }
        /// <summary>
        /// Ładuje komponenty
        /// </summary>
        public void LoadContent()
        {
            foreach (var component in _components)
            {
                component.LoadContent();
            }
        }
        /// <summary>
        /// Wyłącza komponenty
        /// </summary>
        public void UnloadContent()
        {
            foreach (var component in _components)
            {
                component.UnloadContent();
            }
            _components.Clear();
        }
        /// <summary>
        /// Aktualizuje komponenty
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }
        /// <summary>
        /// Rysuje komponenty
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Draw(gameTime);
            }
        }
    }
}
