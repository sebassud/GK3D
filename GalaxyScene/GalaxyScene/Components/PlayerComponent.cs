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
            gameService.Player.LeftDirection = new Vector3(0, -1, 0);
            gameService.Player.UpDirection = new Vector3(0, 0, 1);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            Player player = gameService.Player;
            Vector3 position = player.PlayerPosition;
            Vector3 frontVector = new Vector3(player.Direction.X, player.Direction.Y, player.Direction.Z);
            frontVector.Normalize();

            Vector3 leftVector = new Vector3(gameService.Player.LeftDirection.X, gameService.Player.LeftDirection.Y, gameService.Player.LeftDirection.Z);
            leftVector.Normalize();

            Vector3 upVector = new Vector3(gameService.Player.UpDirection.X, gameService.Player.UpDirection.Y, gameService.Player.UpDirection.Z);
            upVector.Normalize();

            if (keyboard.IsKeyDown(Keys.W) )
            {
                position += frontVector * speed;
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
                position -= frontVector * speed;
            }
            if (keyboard.IsKeyDown(Keys.NumPad8))
            {
                position += upVector * speed;
            }
            if (keyboard.IsKeyDown(Keys.NumPad2))
            {
                position -= upVector * speed;
            }

            player.PlayerPosition = position;

            base.Update(gameTime);
        }
    }
}
