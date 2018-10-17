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
    public class SpaceshipComponent : BaseGameComponent
    {
        private Matrix _world;
        private Model _model;

        public SpaceshipComponent(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            _world = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateRotationY(0.15f) * Matrix.CreateScale(0.000025f) *
                Matrix.CreateTranslation(new Vector3(1, 0, 5)) * Matrix.CreateScale(gameService.Scale);
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _model = Game.Content.Load<Model>("Spaceship/VSDI");

            LoadMesh(_model);
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
                foreach (var part in mesh.MeshParts)
                {
                    var effect = GetEffect(part.Effect);
                    effect.Parameters["World"].SetValue(modelTransforms[mesh.ParentBone.Index] * _world);
                    effect.Parameters["SpecularIntensity"].SetValue(0.5f);
                    part.Effect = effect;
                }

                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
