using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyScene.Model;
using GalaxyScene.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GalaxyScene.Components
{
    public class PlayerComponent : BaseComponent
    {
        private IGameService gameService;

        private float speed = 0.1f;

        public PlayerComponent(Game game) : base(game)
        {
            gameService = game.Services.GetService<IGameService>();
        }

        public override void Initialize()
        {
            gameService.Player.PlayerPosition = new Vector3(7, 0, 0);
            gameService.Player.Direction = new Vector3(-1, 0, 0);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            Player player = gameService.Player;
            Vector3 position = player.PlayerPosition;
            Vector3 upVector = player.Direction;
            Vector3 leftVector = Vector3.Transform(upVector, Matrix.CreateRotationZ((float)Math.PI / 2));
            if (keyboard.IsKeyDown(Keys.W) )
            {
                position += upVector * speed;
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                position += leftVector * speed;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                position -= leftVector * speed;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                position -= upVector * speed;
            }

            player.PlayerPosition = position;

            base.Update(gameTime);
        }
    }
}
