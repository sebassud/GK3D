using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyScene.Components
{
    class ReflectComponent : BaseGameComponent
    {
        private Matrix _world;
        private Model _model;
        private TextureCube _textureReflect;

        public ReflectComponent(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            _world = Matrix.CreateScale(0.005f) *
                Matrix.CreateTranslation(new Vector3(1, 0, 7)) * Matrix.CreateScale(gameService.Scale);
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _model = Game.Content.Load<Model>("Reflect/reflect");
            _textureReflect = Game.Content.Load<TextureCube>("Reflect/Sunset");
            LoadMesh(_model, "Shader_Reflect");
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
                    effect.Parameters["ModelTexture"].SetValue(_textureReflect);
                    effect.Parameters["WorldInverseTranspose"].SetValue(
                                    Matrix.Transpose(Matrix.Invert(_world * mesh.ParentBone.Transform)));
                    effect.CurrentTechnique = effect.Techniques["Reflection"];
                    part.Effect = effect;
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
