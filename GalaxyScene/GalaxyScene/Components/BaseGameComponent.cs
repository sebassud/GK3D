using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalaxyScene.Components
{
    public class BaseGameComponent : BaseComponent
    {
        private Effect _effect;

        public BaseGameComponent(Game game) : base(game)
        {
        }

        public override void LoadContent()
        {
            _effect = Game.Content.Load<Effect>("Shader/Shader");
            base.LoadContent();
        }

        protected Effect GetEffect(Effect effect)
        {
            return FillParameter(effect);
        }

        protected Effect GetEffect()
        {
            var effect = _effect.Clone();

            return FillParameter(effect);
        }

        private Effect FillParameter(Effect effect)
        {
            effect.Parameters["View"].SetValue(gameService.View);
            effect.Parameters["Projection"].SetValue(gameService.Projection);
            effect.Parameters["CameraPosition"].SetValue(gameService.Player.PlayerPosition);
            effect.Parameters["ReflectorsCount"].SetValue(gameService.Reflectors.Count);
            effect.Parameters["DirectionVectors"].SetValue(gameService.Reflectors.Select(x => x.Direction).ToArray());
            effect.Parameters["PositionVectors"].SetValue(gameService.Reflectors.Select(x => x.Position).ToArray());
            effect.Parameters["ColorVectors"].SetValue(gameService.Reflectors.Select(x => x.Color).ToArray());

            return effect;
        }

        /// <summary>
        /// Ładowanie informacji z domyślnego efektu
        /// </summary>
        /// <param name="model">Model, dla którego ładowany jest efekt</param>
        protected void LoadMesh(Model model)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    var basicEffect = part.Effect as BasicEffect;
                    if (basicEffect != null)
                    {
                        var effect = _effect.Clone();

                        if (basicEffect.Texture != null)
                        {
                            effect.CurrentTechnique = effect.Techniques["Textured"];
                            effect.Parameters["ModelTexture"].SetValue(basicEffect.Texture);
                        }
                        else
                        {
                            effect.CurrentTechnique = effect.Techniques["Colored"];
                            effect.Parameters["MaterialColor"].SetValue(
                                new Vector4(basicEffect.DiffuseColor, 1f));
                        }
                        part.Effect = effect;

                    }
                }
            }

        }
    }
}
