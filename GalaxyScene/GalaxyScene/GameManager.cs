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
        private RenderTarget2D shadowMapRenderTarget;
        private IGameService gameService;
        private SpriteBatch spriteBatch;

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
                //new AdComponent(_game),
                //new ReflectComponent(_game),
                //new FireComponent(_game),
                //new MeteorComponent(_game),
                //new MenuComponent(_game)
            };
            foreach (var component in _components)
            {
                component.Initialize();
            }
            GraphicsDevice = _game.GraphicsDevice;
            _renderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, true, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8);
            gameService.TextureAd = _renderTarget;
            shadowMapRenderTarget = new RenderTarget2D(GraphicsDevice, 4 * GraphicsDevice.PresentationParameters.BackBufferWidth, 4 * GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Single, DepthFormat.Depth24Stencil8);
            gameService.ShadowMap = shadowMapRenderTarget;
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
            //GraphicsDevice.SetRenderTarget(_renderTarget);
            //foreach (var component in _components)
            //{
            //    if (!(component is AdComponent))
            //        component.Draw(gameTime);
            //}

            //GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.SetRenderTarget(shadowMapRenderTarget);
            //GraphicsDevice.Clear(Color.White);
            foreach (var component in _components)
            {
                component.DrawShadowMap();
            }

            GraphicsDevice.SetRenderTarget(null);
            //spriteBatch.Begin(0, BlendState.Opaque, SamplerState.AnisotropicClamp);
            //spriteBatch.Draw(shadowMapRenderTarget, new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            //spriteBatch.End();
            foreach (var component in _components)
            {
                component.Draw(gameTime);
            }
        }
    }
}
