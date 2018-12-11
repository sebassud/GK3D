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
        private Dictionary<string, Effect> _effects;

        public BaseGameComponent(Game game) : base(game)
        {
            _effects = new Dictionary<string, Effect>();
        }

        public override void LoadContent()
        {
            _effects.Add("Shader", Game.Content.Load<Effect>("Shader/Shader"));
            _effects.Add("Shader_Ad", Game.Content.Load<Effect>("Shader/Shader_Ad"));
            _effects.Add("Shader_background", Game.Content.Load<Effect>("Shader/Shader_background"));
            _effects.Add("Shader_Reflect", Game.Content.Load<Effect>("Shader/Shader_Reflect"));
            base.LoadContent();
        }

        protected Effect GetEffect(Effect effect)
        {
            return FillParameter(effect);
        }

        protected Effect GetEffect(string nameShader = "Shader")
        {
            var effect = _effects[nameShader].Clone();

            return FillParameter(effect);
        }

        private Effect FillParameter(Effect effect)
        {
            effect.Parameters["View"].SetValue(gameService.View);
            effect.Parameters["Projection"].SetValue(gameService.Projection);
            effect.Parameters["CameraPosition"].SetValue(gameService.Player.PlayerPosition);
            effect.Parameters["ReflectorsCount"].SetValue(gameService.Reflectors.Where(x => x.Active).Count());
            effect.Parameters["DirectionVectors"].SetValue(gameService.Reflectors.Where(x => x.Active).Select(x => x.Direction).ToArray());
            effect.Parameters["PositionVectors"].SetValue(gameService.Reflectors.Where(x => x.Active).Select(x => x.Position).ToArray());
            effect.Parameters["ColorVectors"].SetValue(gameService.Reflectors.Where(x => x.Active).Select(x => x.Color).ToArray());
            effect.Parameters["DirectionLight"].SetValue(new Vector3(0, 0, 1));

            return effect;
        }

        /// <summary>
        /// Ładowanie informacji z domyślnego efektu
        /// </summary>
        /// <param name="model">Model, dla którego ładowany jest efekt</param>
        protected void LoadMesh(Model model, string nameShader = "Shader")
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    var basicEffect = part.Effect as BasicEffect;
                    if (basicEffect != null)
                    {
                        var effect = _effects[nameShader].Clone();

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
