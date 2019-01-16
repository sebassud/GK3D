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
        protected Matrix lightViewProjection;

        public BaseGameComponent(Game game) : base(game)
        {
            _effects = new Dictionary<string, Effect>();
            var lightView = Matrix.CreateLookAt(new Vector3(0, 20, 25),
                        new Vector3(0, 0, 0),
                        new Vector3(0, 0, 1));
            lightViewProjection = lightView * gameService.ProjectionLight;
        }

        public override void LoadContent()
        {
            _effects.Add("Shader", Game.Content.Load<Effect>("Shader/Shader"));
            _effects.Add("Shader_Ad", Game.Content.Load<Effect>("Shader/Shader_Ad"));
            _effects.Add("Shader_background", Game.Content.Load<Effect>("Shader/Shader_background"));
            _effects.Add("Shader_Reflect", Game.Content.Load<Effect>("Shader/Shader_Reflect"));
            _effects.Add("Shader_ShadowMap", Game.Content.Load<Effect>("Shader/Shader_ShadowMap"));
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
            effect.Parameters["ShadowMap"]?.SetValue(gameService.ShadowMap);
            effect.Parameters["LightViewProj"]?.SetValue(lightViewProjection);
            effect.Parameters["ShadowMapSize"]?.SetValue(new Vector2(4 * GraphicsDevice.PresentationParameters.BackBufferWidth, 4 * GraphicsDevice.PresentationParameters.BackBufferHeight));
            effect.Parameters["ReflectorsCount"].SetValue(gameService.Reflectors.Where(x => x.Active).Count());
            effect.Parameters["DirectionVectors"].SetValue(gameService.Reflectors.Where(x => x.Active).Select(x => x.Direction).ToArray());
            effect.Parameters["PositionVectors"].SetValue(gameService.Reflectors.Where(x => x.Active).Select(x => x.Position).ToArray());
            effect.Parameters["ColorVectors"].SetValue(gameService.Reflectors.Where(x => x.Active).Select(x => x.Color).ToArray());
            effect.Parameters["DirectionLight"].SetValue(new Vector3(0, 1, 1));

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
                            effect.Parameters["ModelTexture2"].SetValue(basicEffect.Texture);
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

        protected void DrawShadowMapHelper(Model model, Matrix world)
        {
            var shadowMapGenerate = _effects["Shader_ShadowMap"];
            for (int index = 0; index < model.Meshes.Count; index++)
            {
                ModelMesh mesh = model.Meshes[index];
                for (int i = 0; i < mesh.MeshParts.Count; i++)
                {
                    ModelMeshPart meshpart = mesh.MeshParts[i];
                    shadowMapGenerate.Parameters["WorldViewProj"].SetValue(world * lightViewProjection);

                    shadowMapGenerate.CurrentTechnique.Passes[0].Apply();

                    GraphicsDevice.SetVertexBuffer(meshpart.VertexBuffer);
                    GraphicsDevice.Indices = (meshpart.IndexBuffer);
                    int primitiveCount = meshpart.PrimitiveCount;
                    int vertexOffset = meshpart.VertexOffset;
                    int vCount = meshpart.NumVertices;
                    int startIndex = meshpart.StartIndex;

                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, vertexOffset, startIndex,
                        primitiveCount);
                }
            }
        }
    }
}
