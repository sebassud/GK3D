using GalaxyScene.Components;
using GalaxyScene.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private RenderTarget2D _renderTarget;
        private GraphicsDevice GraphicsDevice;
        private IGameService gameService;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="game">Obiekt gry</param>
        public GameManager(Game game, GraphicsDeviceManager graphicsDeviceManager)
        {
            _game = game;
            gameService = new GameService(graphicsDeviceManager);
            _game.Services.AddService(typeof(IGameService), gameService);
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
                new BackgroundComponent(_game),
                new AdComponent(_game),
                new ReflectComponent(_game),
                new FireComponent(_game),
                new MeteorComponent(_game),
                new MenuComponent(_game)};
            foreach (var component in _components)
            {
                component.Initialize();
            }
            GraphicsDevice = _game.GraphicsDevice;
            _renderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, true, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8);
            gameService.TextureAd = _renderTarget;
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
            GraphicsDevice.SetRenderTarget(_renderTarget);
            foreach (var component in _components)
            {
                if (!(component is AdComponent))
                    component.Draw(gameTime);
            }

            GraphicsDevice.SetRenderTarget(null);
            foreach (var component in _components)
            {
                component.Draw(gameTime);
            }
        }
    }
}
