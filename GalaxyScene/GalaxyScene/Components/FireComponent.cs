using GalaxyScene.Render;
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
    public class FireComponent : BaseGameComponent
    {
        private Matrix _world;
        private ParticleSystem _particleSystem;
        ParticleRender _particleRender;
        private Texture2D textureParticle;
        private Effect effectParticle;

        public FireComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
        }

        public override void Initialize()
        {
            _world = Matrix.Identity;
            Vector3 pos = new Vector3(-0.4f, 0.1f, 5);
            _particleSystem = new ParticleSystem(pos, Vector3.Normalize(pos), 1000, 500, 0.02f);
            _particleRender = new ParticleRender();
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            effectParticle = Game.Content.Load<Effect>("Shader/Shader_Particle");
            textureParticle = Game.Content.Load<Texture2D>("Fire/fire");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _particleSystem.Update();
            _particleRender.Clear();
            CreateBillboardVerticesFromList(_particleSystem.Particles.OrderByDescending(p => (p.Position - gameService.Player.PlayerPosition).Length()).ToList());
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            DrawBillboards();
        }

        private void DrawBillboards()
        {
            effectParticle.CurrentTechnique = effectParticle.Techniques["CylBillboard"];
            effectParticle.Parameters["xWorld"].SetValue(Matrix.Identity);
            effectParticle.Parameters["xView"].SetValue(gameService.View);
            effectParticle.Parameters["xProjection"].SetValue(gameService.Projection);
            effectParticle.Parameters["xCamPos"].SetValue(gameService.Player.PlayerPosition);
            effectParticle.Parameters["xAllowedRotDir"].SetValue(gameService.Player.UpDirection);
            effectParticle.Parameters["xBillboardTexture"].SetValue(textureParticle);
            effectParticle.Parameters["scale"].SetValue(_particleSystem.Scale);

            Game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            foreach (EffectPass pass in effectParticle.CurrentTechnique.Passes)
            {
                pass.Apply();
                var buffer = _particleRender.GetBuffer(GraphicsDevice);
                Game.GraphicsDevice.SetVertexBuffer(buffer);
                Game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, buffer.VertexCount / 3);

            }
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
        }

        private void CreateBillboardVerticesFromList(List<Particle> particles)
        {
            foreach (Particle particle in particles)
            {
                _particleRender.AddParticle(particle);
            }
        }
    }
}
