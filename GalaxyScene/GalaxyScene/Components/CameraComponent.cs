using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyScene.Services;
using Microsoft.Xna.Framework;

namespace GalaxyScene.Components
{
    public class CameraComponent : BaseComponent
    {
        private IGameService gameService;

        private Matrix _view;
        public CameraComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
        }

        public override void Initialize()
        {
            _view = Matrix.CreateLookAt(new Vector3(7, 0, 2), new Vector3(-7, 0, -2), Vector3.UnitZ);
            gameService.View = _view;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
