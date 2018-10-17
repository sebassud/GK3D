using GalaxyScene.GameModels;
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
        private float _speed = 0.001f; 
        private Matrix _world1;
        private Matrix _world2;
        private Model _model;

        private Reflector reflector1;
        private Reflector reflector2;

        public SatelliteComponent(Game game) : base(game)
        {
            reflector1 = new Reflector();
            reflector2 = new Reflector();
        }

        public override void Initialize()
        {
            _world1 = Matrix.CreateRotationX(MathHelper.PiOver4) * Matrix.CreateRotationZ(-(MathHelper.PiOver4 + MathHelper.Pi)) * Matrix.CreateScale(0.002f) *
                Matrix.CreateTranslation(new Vector3(4, 4, 5)) * Matrix.CreateScale(gameService.Scale);
            _world2 = Matrix.CreateRotationX(MathHelper.PiOver4 + MathHelper.Pi) * Matrix.CreateRotationZ(-(MathHelper.PiOver4 + MathHelper.Pi)) * Matrix.CreateScale(0.002f) *
                Matrix.CreateTranslation(new Vector3(-4, -4, -5)) * Matrix.CreateScale(gameService.Scale);

            reflector1.Position = Vector3.Transform(new Vector3(4, 4, 5), Matrix.CreateScale(gameService.Scale));
            reflector1.Direction = Vector3.Transform(new Vector3(0, 0, -1), Matrix.CreateRotationX(MathHelper.PiOver4) * Matrix.CreateRotationZ(-(MathHelper.PiOver4 + MathHelper.Pi)));
            reflector1.Color = new Vector4(1, 0, 0, 0);
            gameService.Reflectors.Add(reflector1);

            reflector2.Position = Vector3.Transform(new Vector3(-4, -4, -5), Matrix.CreateScale(gameService.Scale));
            reflector2.Direction = Vector3.Transform(new Vector3(0, 0, -1), Matrix.CreateRotationX(MathHelper.PiOver4 + MathHelper.Pi) * Matrix.CreateRotationZ(-(MathHelper.PiOver4 + MathHelper.Pi)));
            reflector2.Color = new Vector4(0, 0, 1, 0);
            gameService.Reflectors.Add(reflector2);

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
            _world1 *= Matrix.CreateRotationZ(_speed);
            _world2 *= Matrix.CreateRotationZ(_speed);
            reflector1.Position = Vector3.Transform(reflector1.Position, Matrix.CreateRotationZ(_speed));
            reflector1.Direction = Vector3.Transform(reflector1.Direction, Matrix.CreateRotationZ(_speed));
            reflector2.Position = Vector3.Transform(reflector2.Position, Matrix.CreateRotationZ(_speed));
            reflector2.Direction = Vector3.Transform(reflector2.Direction, Matrix.CreateRotationZ(_speed));
            reflector1.Color = new Vector4(reflector1.Color.X, (reflector1.Color.Y + 0.001f) % 1, reflector1.Color.Z, reflector1.Color.W);
            reflector2.Color = new Vector4(reflector2.Color.X, (reflector2.Color.Y + 0.001f) % 1, reflector2.Color.Z, reflector2.Color.W);
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
