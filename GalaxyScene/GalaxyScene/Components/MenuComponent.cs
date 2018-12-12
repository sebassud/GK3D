using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private SpriteFont font;
        SpriteBatch spriteBatch;
        private bool MagFilter;
        private bool MipFilter;
        private float MipMapLevelOfDetailBias;
        private SamplerState _samplerState;

        private bool pressingM;
        private bool pressingF3;
        private bool pressingF4;
        private bool pressingF5;
        private bool pressingF6;

        public MenuComponent(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            graphics = gameService.Graphics;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();
            MagFilter = true;
            MipFilter = true;
            MipMapLevelOfDetailBias = 1;
            pressingM = false;
            pressingF3 = false;
            pressingF4 = false;
            pressingF5 = false;
            pressingF6 = false;
            SetFilters();

            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Game.Content.Load<SpriteFont>("Menu/Font");
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.M) && !pressingM)
            {
                graphics.PreferMultiSampling = !graphics.PreferMultiSampling;
                graphics.ApplyChanges();
                pressingM = true;
            }
            if (keyboard.IsKeyUp(Keys.M))
            {
                pressingM = false;
            }
            if (keyboard.IsKeyDown(Keys.F3) && !pressingF3)
            {
                MagFilter = !MagFilter;
                pressingF3 = true;
            }
            if (keyboard.IsKeyUp(Keys.F3))
            {
                pressingF3 = false;
            }
            if (keyboard.IsKeyDown(Keys.F4) && !pressingF4)
            {
                MipFilter = !MipFilter;
                pressingF4 = true;
            }
            if (keyboard.IsKeyUp(Keys.F4))
            {
                pressingF4 = false;
            }
            if (keyboard.IsKeyDown(Keys.F5) && !pressingF5)
            {
                if(MipMapLevelOfDetailBias>-15) MipMapLevelOfDetailBias--;
                pressingF5 = true;
            }
            if (keyboard.IsKeyUp(Keys.F5))
            {
                pressingF5 = false;
            }
            if (keyboard.IsKeyDown(Keys.F6) && !pressingF6)
            {
                if (MipMapLevelOfDetailBias < 15) MipMapLevelOfDetailBias++;
                pressingF6 = true;
            }
            if (keyboard.IsKeyUp(Keys.F6))
            {
                pressingF6 = false;
            }

            SetFilters();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var magFilter = MagFilter ? "True" : "False";
            var preferMultisampling = graphics.PreferMultiSampling ? "True" : "False";
            var mipFilter = MipFilter ? "True" : "False";

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, _samplerState, DepthStencilState.Default);
            spriteBatch.DrawString(font, $"MagFilter:{magFilter}", new Vector2(50, 50), Color.WhiteSmoke);
            spriteBatch.DrawString(font, $"MipFilter:{mipFilter}", new Vector2(50, 70), Color.WhiteSmoke);
            spriteBatch.DrawString(font, $"MipMapLevelOfDetailsBias :{MipMapLevelOfDetailBias}", new Vector2(50, 90), Color.WhiteSmoke);
            spriteBatch.DrawString(font, $"MultiSampling :{preferMultisampling}", new Vector2(50, 110), Color.WhiteSmoke);
            spriteBatch.End();
            GraphicsDevice.BlendState = BlendState.Opaque;
        }

        private void SetFilters()
        {
            TextureFilter filter;
            if (MagFilter)
            {
                if(MipFilter)
                {
                    filter = TextureFilter.Linear;
                }
                else
                {
                    filter = TextureFilter.LinearMipPoint;
                }
            }
            else
            {
                if (MipFilter)
                {
                    filter = TextureFilter.MinLinearMagPointMipLinear;
                }
                else
                {
                    filter = TextureFilter.MinLinearMagPointMipPoint;
                }
            }

            _samplerState = new SamplerState();
            _samplerState.Filter = filter;
            _samplerState.FilterMode = TextureFilterMode.Default;
            _samplerState.MipMapLevelOfDetailBias = MipMapLevelOfDetailBias;
            GraphicsDevice.SamplerStates[0] = _samplerState;
        }
    }
}
