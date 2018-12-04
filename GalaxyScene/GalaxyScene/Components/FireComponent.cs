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
        VertexBuffer treeVertexBuffer;
        private Texture2D textureParticle;
        private Effect effectParticle;

        public FireComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
        }

        public override void Initialize()
        {
            _world = Matrix.Identity;
            _particleSystem = new ParticleSystem(new Vector3(-0.4f, 0.1f, 5), Vector3.UnitZ / 1000, 100, 100);
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
            CreateBillboardVerticesFromList(_particleSystem.Particles.OrderByDescending(p => (p.Position - gameService.Player.PlayerPosition).Length()).Select(p => p.Position).ToList());
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
            effectParticle.Parameters["scale"].SetValue(_particleSystem.Particles[0].Scale);

            Game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            foreach (EffectPass pass in effectParticle.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.SetVertexBuffer(treeVertexBuffer);
                Game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, treeVertexBuffer.VertexCount / 3);
            }
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
        }

        private void CreateBillboardVerticesFromList(List<Vector3> treeList)
        {
            VertexPositionTexture[] billboardVertices = new VertexPositionTexture[treeList.Count * 6];
            int i = 0;
            foreach (Vector3 currentV3 in treeList)
            {
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 1));

                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 1));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 1));
            }

            treeVertexBuffer = new VertexBuffer(Game.GraphicsDevice, typeof(VertexPositionTexture), billboardVertices.Length, BufferUsage.WriteOnly);
            treeVertexBuffer.SetData(billboardVertices);
        }
    }
}
