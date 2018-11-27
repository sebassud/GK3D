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
        private bool MagFilter;
        private bool MipFilter;
        private float MipMapLevelOfDetailBias;

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
            SetFilters();

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
            SetFilters();

            base.Update(gameTime);
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

            var samplerState = new SamplerState();
            samplerState.Filter = filter;
            samplerState.FilterMode = TextureFilterMode.Default;
            samplerState.MipMapLevelOfDetailBias = MipMapLevelOfDetailBias;
            GraphicsDevice.SamplerStates[0] = samplerState;
        }
    }
}
