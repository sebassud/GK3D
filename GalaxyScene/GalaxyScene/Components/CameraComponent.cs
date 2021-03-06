﻿using System;
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
        private Matrix _view;
        public CameraComponent(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            _view = Matrix.CreateLookAt(gameService.Player.PlayerPosition, gameService.Player.PlayerPosition + gameService.Player.Direction, gameService.Player.UpDirection);
            gameService.View = _view;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _view = Matrix.CreateLookAt(gameService.Player.PlayerPosition, gameService.Player.PlayerPosition + gameService.Player.Direction, gameService.Player.UpDirection);
            gameService.View = _view;

            base.Update(gameTime);
        }
    }
}
