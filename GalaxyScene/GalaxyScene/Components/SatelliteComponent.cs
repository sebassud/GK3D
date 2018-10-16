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
    public class SatelliteComponent : BaseGameComponent
    {
        private Matrix _world1;
        private Matrix _world2;
        private Model _model;

        public SatelliteComponent(Game game) : base(game)
        {
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
            base.LoadContent();
            _model = Game.Content.Load<Model>("Satellite/Satellite");
            LoadMesh(_model);
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
                foreach (var part in mesh.MeshParts)
                {
                    var effect = GetEffect(part.Effect);
                    effect.Parameters["World"].SetValue(modelTransforms[mesh.ParentBone.Index] * _world1);
                    part.Effect = effect;
                }

                mesh.Draw();
            }

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    var effect = GetEffect(part.Effect);
                    effect.Parameters["World"].SetValue(modelTransforms[mesh.ParentBone.Index] * _world2);
                    part.Effect = effect;
                }

                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
