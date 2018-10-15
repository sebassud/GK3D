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
    public class SatelliteComponent : BaseComponent
    {
        private IGameService gameService;
        private Matrix _world1;
        private Matrix _world2;
        private Model _model;

        public SatelliteComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
        }

        public override void Initialize()
        {
            _world1 = Matrix.CreateRotationX(MathHelper.PiOver4) * Matrix.CreateRotationZ(-(MathHelper.PiOver4 + MathHelper.Pi)) * Matrix.CreateScale(0.002f) *
                Matrix.CreateTranslation(new Vector3(4, 4, 5)) * Matrix.CreateScale(gameService.Scale);
            _world2 = Matrix.CreateRotationX(MathHelper.PiOver4 + MathHelper.Pi) * Matrix.CreateRotationZ(-(MathHelper.PiOver4 + MathHelper.Pi)) * Matrix.CreateScale(0.002f) *
                Matrix.CreateTranslation(new Vector3(-4, -4, -5)) * Matrix.CreateScale(gameService.Scale);
            base.Initialize();
        }

        public override void LoadContent()
        {
            _model = Game.Content.Load<Model>("Satellite/Satellite");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _world1 *= Matrix.CreateRotationZ(0.001f);
            _world2 *= Matrix.CreateRotationZ(0.001f);
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
                    be.World = modelTransforms[mesh.ParentBone.Index] * _world1;
                    be.View = gameService.View;
                    be.Projection = gameService.Projection;
                }
                mesh.Draw();
            }

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.World = modelTransforms[mesh.ParentBone.Index] * _world2;
                    be.View = gameService.View;
                    be.Projection = gameService.Projection;
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
