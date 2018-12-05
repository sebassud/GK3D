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
    public class MeteorComponent : BaseGameComponent
    {
        private Matrix _world;
        List<Vector3> positions;
        VertexBuffer meteorVertexBuffer;
        private Texture2D textureBillboard;
        private Effect effectBillbord;
        private float scale;
        public MeteorComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
        }

        public override void Initialize()
        {
            var _rand = new Random();
            _world = Matrix.Identity;
            positions = new List<Vector3>();
            var center = new Vector3(0, 8, 6);
            for(int i=0; i < 50; i++)
            {
                var x = center.X + 2 * ((float)_rand.NextDouble() - 0.5f);
                var y = center.Y + 2 * ((float)_rand.NextDouble() - 0.5f);
                var z = center.Z + 2 * ((float)_rand.NextDouble() - 0.5f);
                positions.Add(new Vector3(x, y, z));
            }
            
            scale = 0.08f * gameService.Scale;

            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            effectBillbord = Game.Content.Load<Effect>("Shader/Shader_Billboard");
            textureBillboard = Game.Content.Load<Texture2D>("Meteor/meteor");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            positions = positions.OrderByDescending(p => (p - gameService.Player.PlayerPosition).Length()).ToList();
            CreateBillboardVerticesFromList(positions);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            DrawBillboards();
        }

        private void DrawBillboards()
        {
            effectBillbord.CurrentTechnique = effectBillbord.Techniques["CylBillboard"];
            effectBillbord.Parameters["xWorld"].SetValue(Matrix.Identity);
            effectBillbord.Parameters["xView"].SetValue(gameService.View);
            effectBillbord.Parameters["xProjection"].SetValue(gameService.Projection);
            effectBillbord.Parameters["xCamPos"].SetValue(gameService.Player.PlayerPosition);
            effectBillbord.Parameters["xAllowedRotDir"].SetValue(gameService.Player.UpDirection);
            effectBillbord.Parameters["xBillboardTexture"].SetValue(textureBillboard);
            effectBillbord.Parameters["scale"].SetValue(scale);

            Game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            foreach (EffectPass pass in effectBillbord.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.SetVertexBuffer(meteorVertexBuffer);
                Game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, meteorVertexBuffer.VertexCount / 3);
            }
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
        }

        private void CreateBillboardVerticesFromList(List<Vector3> meteorList)
        {
            VertexPositionTexture[] billboardVertices = new VertexPositionTexture[meteorList.Count * 6];
            int i = 0;
            foreach (Vector3 currentV3 in meteorList)
            {
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 1));

                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 1));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 1));
            }

            meteorVertexBuffer = new VertexBuffer(Game.GraphicsDevice, typeof(VertexPositionTexture), billboardVertices.Length, BufferUsage.WriteOnly);
            meteorVertexBuffer.SetData(billboardVertices);
        }
    }
}
