using System;
using System.Collections.Generic;
using System.Linq;
using amulware.Graphics;
using amulware.Graphics.ShaderManagement;
using Bearded.Utilities.Input;
using Bearded.Utilities.Math;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GenericGamedev.LocalisedCrepuscularRays
{
    sealed class GameWindow : amulware.Graphics.Program
    {
        private ShaderManager shaderMan;
        private int glWidth;
        private int glHeight;

        private List<Layer> layers;
        private Texture renderTexture;
        private RenderTarget renderTarget;
        private PostProcessSurface copyToScreen;
        private VertexSurface<CrepuscularRayVertex> raySurface;

        public GameWindow()
            : base(1280, 720, GraphicsMode.Default,
            "GameDev<T> Localised Crepuscular Rays",
            GameWindowFlags.Default, DisplayDevice.Default,
            3, 0, GraphicsContextFlags.ForwardCompatible)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            this.shaderMan = new ShaderManager();

            var shaderLoader = ShaderFileLoader.CreateDefault("shaders");

            // load all shaders
            var shaders = shaderLoader.Load("").ToList();
            this.shaderMan.Add(shaders);


            var layerShader = this.shaderMan.MakeShaderProgram()
                .WithVertexShader("copy").WithFragmentShader("copy")
                .As("layer");

            var copyShader = this.shaderMan.MakeShaderProgram()
                .WithVertexShader("post").WithFragmentShader("copy")
                .As("copypost");

            var wispShader = this.shaderMan.MakeShaderProgram()
                .WithVertexShader("wisp").WithFragmentShader("wisp")
                .As("wisp");

            var rayShader = this.shaderMan.MakeShaderProgram()
                .WithVertexShader("crepuscularrays")
                .WithGeometryShader("crepuscularrays")
                .WithFragmentShader("crepuscularrays")
                .As("crepuscularrays");

            this.renderTexture = new Texture(1, 1);
            this.renderTexture.SetParameters(TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.ClampToBorder, TextureWrapMode.ClampToBorder);
            
            this.renderTarget = new RenderTarget(this.renderTexture);
            var renderTextureUniform = new TextureUniform("diffuseTexture", this.renderTexture);

            this.copyToScreen = new PostProcessSurface();
            this.copyToScreen.AddSettings(renderTextureUniform, new ColorUniform("color", Color.White));
            copyShader.UseOnSurface(this.copyToScreen);

            this.raySurface = new VertexSurface<CrepuscularRayVertex>(PrimitiveType.Points);
            this.raySurface.AddSettings(renderTextureUniform);
            rayShader.UseOnSurface(this.raySurface);

            var rayGeo = new CrepuscularRayGeometry(this.raySurface);


            const int layerCount = 4;
            const float maxBrightness = 0.03f;
            const float brightnessStep = maxBrightness / layerCount;


            this.layers = Enumerable.Range(0, layerCount).Reverse()
                .Select(i => new Layer(layerShader, wispShader, rayGeo, "layer" + i, GameMath.Sqrt(brightnessStep * i), i * i * 5))
                .ToList();


            InputManager.Initialize(this.Mouse);

        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
            if (this.Focused)
            {
                InputManager.Update();

            }

            float t = e.ElapsedTimeInSf;

            if (InputManager.IsKeyPressed(Key.Number2))
                t *= 0.1f;

            foreach (var layer in this.layers)
            {
                //layer.Update(t);
            }
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            // resize viewport if needed
            if (this.Height != this.glHeight || this.Width != this.glWidth)
            {
                this.glHeight = this.Height;
                this.glWidth = this.Width;
                GL.Viewport(0, 0, this.glWidth, this.glHeight);

                this.renderTexture.Resize(this.glWidth, this.glHeight);
            }

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, this.renderTarget);

            GL.CullFace(CullFaceMode.FrontAndBack);
            GL.ClearColor(Color.GrayScale(200));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SurfaceBlendSetting.PremultipliedAlpha.Set(null);

            foreach (var layer in layers)
            {
                layer.Draw();
            }

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            this.copyToScreen.Render();

            if (InputManager.IsKeyPressed(Key.Number1))
            {
                this.raySurface.Clear();
            }
            else
            {
                this.raySurface.Render();   
            }

            this.SwapBuffers();
        }
    }
}
