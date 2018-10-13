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

        private float rotationSpeed = 0.01f;

        private int mousePositionX;
        private int mousePositionY;

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
            mousePositionX = 500;
            mousePositionY = 500;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
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
                position += frontVector * speed * gameService.Scale;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                position -= frontVector * speed * gameService.Scale;
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                position += leftVector * speed * gameService.Scale;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                position -= leftVector * speed * gameService.Scale;
            }
            if (keyboard.IsKeyDown(Keys.NumPad8))
            {
                position += upVector * speed * gameService.Scale;
            }
            if (keyboard.IsKeyDown(Keys.NumPad2))
            {
                position -= upVector * speed * gameService.Scale;
            }
            if (keyboard.IsKeyDown(Keys.NumPad4))
            {
                gameService.Player.LeftDirection = Vector3.Transform(gameService.Player.LeftDirection, Matrix.CreateFromAxisAngle(gameService.Player.Direction, -rotationSpeed));
                gameService.Player.LeftDirection.Normalize();
                gameService.Player.UpDirection = Vector3.Transform(gameService.Player.UpDirection, Matrix.CreateFromAxisAngle(gameService.Player.Direction,  -rotationSpeed));
                gameService.Player.UpDirection.Normalize();
            }
            if (keyboard.IsKeyDown(Keys.NumPad6))
            {
                gameService.Player.LeftDirection = Vector3.Transform(gameService.Player.LeftDirection, Matrix.CreateFromAxisAngle(gameService.Player.Direction, rotationSpeed));
                gameService.Player.LeftDirection.Normalize();
                gameService.Player.UpDirection = Vector3.Transform(gameService.Player.UpDirection, Matrix.CreateFromAxisAngle(gameService.Player.Direction, rotationSpeed));
                gameService.Player.UpDirection.Normalize();
            }
            if (mouse.X != mousePositionX)
            {
                gameService.Player.LeftDirection = Vector3.Transform(gameService.Player.LeftDirection, Matrix.CreateFromAxisAngle(gameService.Player.UpDirection, (mousePositionX - mouse.X) * rotationSpeed * 0.02f));
                gameService.Player.LeftDirection.Normalize();
                gameService.Player.Direction = Vector3.Transform(gameService.Player.Direction, Matrix.CreateFromAxisAngle(gameService.Player.UpDirection, (mousePositionX - mouse.X) * rotationSpeed * 0.02f));
                gameService.Player.Direction.Normalize();
                Mouse.SetPosition(mousePositionX, mousePositionY);
            }
            if (mouse.Y != mousePositionY)
            {
                gameService.Player.UpDirection = Vector3.Transform(gameService.Player.UpDirection, Matrix.CreateFromAxisAngle(gameService.Player.LeftDirection, (mouse.Y - mousePositionY) * rotationSpeed * 0.02f));
                gameService.Player.UpDirection.Normalize();
                gameService.Player.Direction = Vector3.Transform(gameService.Player.Direction, Matrix.CreateFromAxisAngle(gameService.Player.LeftDirection, (mouse.Y - mousePositionY) * rotationSpeed * 0.02f));
                gameService.Player.Direction.Normalize();
                Mouse.SetPosition(mousePositionX, mousePositionY);
            }

            player.PlayerPosition = position;

            base.Update(gameTime);
        }
    }
}
