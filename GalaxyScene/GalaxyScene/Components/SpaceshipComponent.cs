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
    public class SpaceshipComponent : BaseComponent
    {
        private IGameService gameService;
        private Matrix _world;
        private Model _model;

        public SpaceshipComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
        }

        public override void Initialize()
        {
            _world = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateRotationY(0.15f) * Matrix.CreateScale(0.000025f) *
                Matrix.CreateTranslation(new Vector3(1, 0, 5)) * Matrix.CreateScale(gameService.Scale);
            base.Initialize();
        }

        public override void LoadContent()
        {
            _model = Game.Content.Load<Model>("Spaceship/VSDI");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix[] modelTransforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.World = modelTransforms[mesh.ParentBone.Index] * _world;
                    be.View = gameService.View;
                    be.Projection = gameService.Projection;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
