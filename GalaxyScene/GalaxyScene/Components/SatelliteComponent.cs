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
        private Matrix _world;
        private Microsoft.Xna.Framework.Graphics.Model _model;
        private Texture2D _texture;

        public SatelliteComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
        }

        public override void Initialize()
        {
            _world = Matrix.CreateScale(0.01f * gameService.Scale) * Matrix.CreateTranslation(new Vector3(0, 0, 0));
            base.Initialize();
        }

        public override void LoadContent()
        {
            _model = Game.Content.Load<Microsoft.Xna.Framework.Graphics.Model>("Satellite/Satellite");
            _texture = Game.Content.Load<Texture2D>("Satellite/10477_Satellite_v1_Diffuse");
            base.LoadContent();
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
                    be.LightingEnabled = true;
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
