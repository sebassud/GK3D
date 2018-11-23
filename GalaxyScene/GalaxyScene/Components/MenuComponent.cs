using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyScene.Components
{
    class MenuComponent : BaseComponent
    {
        GraphicsDeviceManager graphics;
        public MenuComponent(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            graphics = gameService.Graphics;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.GetPressedKeys().ToList().Exists(k => k == Keys.M))
            {
                graphics.PreferMultiSampling = !graphics.PreferMultiSampling;
                Game.IsMouseVisible = !Game.IsMouseVisible;
                graphics.ApplyChanges();
            }

            base.Update(gameTime);
        }
    }
}
