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
    public class BackgroundComponent : BaseGameComponent
    {
        private Matrix _world;
        private Model _model;
        private TextureCube _textureCube;
        private Matrix _rotation;

        public BackgroundComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
        }

        public override void Initialize()
        {
            _world = Matrix.CreateTranslation(new Vector3(0, 0, 0)) * Matrix.CreateScale(gameService.Scale * 1000);
            _rotation = Matrix.CreateRotationX(-MathHelper.PiOver2);
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _model = Game.Content.Load<Model>("Background/cube");
            _textureCube = Game.Content.Load<TextureCube>("Background/Sunset");
            LoadMesh(_model, "Shader_background");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            Matrix[] modelTransforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    var effect = GetEffect(part.Effect);
                    effect.Parameters["World"].SetValue(modelTransforms[mesh.ParentBone.Index] * _world * Matrix.CreateTranslation(gameService.Player.PlayerPosition));
                    part.Effect.Parameters["ModelTexture"].SetValue(_textureCube);
                    part.Effect.Parameters["Rotation"].SetValue(_rotation);
                    part.Effect = effect;
                }

                mesh.Draw();
            }
            Game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            base.Draw(gameTime);
        }
    }
}
