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
    public class BonusComponent : BaseComponent
    {
        private IGameService gameService;
        private Matrix _world;
        private VertexPositionColor[] _vertices;
        private BasicEffect _basicEffect;

        public BonusComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
        }

        public override void Initialize()
        {
            InitializePoint();
            _basicEffect = new BasicEffect(Game.GraphicsDevice);
            _world = Matrix.CreateScale(gameService.Scale) * Matrix.CreateTranslation(new Vector3(0, 0, 0));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _world *= Matrix.CreateTranslation(-_world.Translation) * Matrix.CreateRotationZ(MathHelper.ToRadians(1)) * Matrix.CreateTranslation(_world.Translation);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            _basicEffect.World = _world;
            _basicEffect.View = gameService.View;
            _basicEffect.Projection = gameService.Projection;
            _basicEffect.VertexColorEnabled = true;
            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, 8);
            }

            base.Draw(gameTime);
        }

        private void InitializePoint()
        {
            Vector3[] points = new Vector3[6]
            {
                new Vector3(0,0,2),
                new Vector3(1,0,0),
                new Vector3(0,1,0),
                new Vector3(-1,0,0),
                new Vector3(0,-1,0),
                new Vector3(0,0,-2),
            };

            _vertices = new VertexPositionColor[24];
            _vertices[0] = new VertexPositionColor(points[1], Color.LawnGreen);
            _vertices[1] = new VertexPositionColor(points[4], Color.Green);
            _vertices[2] = new VertexPositionColor(points[0], Color.DarkGreen);
            _vertices[3] = new VertexPositionColor(points[1], Color.LawnGreen);
            _vertices[4] = new VertexPositionColor(points[2], Color.Green);
            _vertices[5] = new VertexPositionColor(points[5], Color.DarkGreen);
            _vertices[6] = new VertexPositionColor(points[3], Color.LawnGreen);
            _vertices[7] = new VertexPositionColor(points[2], Color.Green);
            _vertices[8] = new VertexPositionColor(points[0], Color.DarkGreen);
            _vertices[9] = new VertexPositionColor(points[3], Color.LawnGreen);
            _vertices[10] = new VertexPositionColor(points[4], Color.Green);
            _vertices[11] = new VertexPositionColor(points[5], Color.DarkGreen);
            _vertices[12] = new VertexPositionColor(points[2], Color.Green);
            _vertices[13] = new VertexPositionColor(points[1], Color.LawnGreen);
            _vertices[14] = new VertexPositionColor(points[0], Color.DarkGreen);
            _vertices[15] = new VertexPositionColor(points[2], Color.Green);
            _vertices[16] = new VertexPositionColor(points[3], Color.LawnGreen);
            _vertices[17] = new VertexPositionColor(points[5], Color.DarkGreen);
            _vertices[18] = new VertexPositionColor(points[4], Color.Green);
            _vertices[19] = new VertexPositionColor(points[3], Color.LawnGreen);
            _vertices[20] = new VertexPositionColor(points[0], Color.DarkGreen);
            _vertices[21] = new VertexPositionColor(points[4], Color.Green);
            _vertices[22] = new VertexPositionColor(points[1], Color.LawnGreen);
            _vertices[23] = new VertexPositionColor(points[5], Color.DarkGreen);
        }
    }
}
