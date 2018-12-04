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

        public BackgroundComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
        }

        public override void Initialize()
        {
            _world = Matrix.CreateScale(0.01f) *
                Matrix.CreateTranslation(new Vector3(0, 0, 0)) * Matrix.CreateScale(gameService.Scale * 10);
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _model = Game.Content.Load<Model>("Background/skybox_galaxy");
            LoadMesh(_model, "Shader_background");
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
                    effect.Parameters["World"].SetValue(modelTransforms[mesh.ParentBone.Index] * _world * Matrix.CreateTranslation(gameService.Player.PlayerPosition));
                    part.Effect = effect;
                }

                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
