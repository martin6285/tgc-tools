using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Tools.Utils.Shaders;
using TGC.Tools.Utils.TgcSceneLoader;

namespace TGC.Tools.Utils.TgcGeometry
{
    /// <summary>
    ///     Herramienta para crear un Triangulo 3D.
    ///     No est� pensado para rasterizar gran cantidad de triangulos, sino mas
    ///     para una herramienta de debug.
    /// </summary>
    public class TgcTriangle : IRenderObject
    {
        private Color color;

        protected Effect effect;

        protected string technique;
        private readonly VertexBuffer vertexBuffer;

        public TgcTriangle()
        {
            var d3dDevice = GuiController.Instance.D3dDevice;

            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), 3, d3dDevice,
                Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);

            A = Vector3.Empty;
            B = Vector3.Empty;
            C = Vector3.Empty;
            Enabled = true;
            color = Color.Blue;
            AlphaBlendEnable = false;

            //Shader
            effect = GuiController.Instance.Shaders.VariosShader;
            technique = TgcShaders.T_POSITION_COLORED;
        }

        /// <summary>
        ///     Primer v�rtice del tri�ngulo
        /// </summary>
        public Vector3 A { get; set; }

        /// <summary>
        ///     Segundo v�rtice del tri�ngulo
        /// </summary>
        public Vector3 B { get; set; }

        /// <summary>
        ///     Tercer v�rtice del tri�ngulo
        /// </summary>
        public Vector3 C { get; set; }

        /// <summary>
        ///     Color del plano
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        ///     Indica si el plano habilitado para ser renderizado
        /// </summary>
        public bool Enabled { get; set; }

        public Vector3 Position
        {
            //Habria que devolver el centro pero es costoso calcularlo cada vez
            get { return A; }
        }

        /// <summary>
        ///     Shader del mesh
        /// </summary>
        public Effect Effect
        {
            get { return effect; }
            set { effect = value; }
        }

        /// <summary>
        ///     Technique que se va a utilizar en el effect.
        ///     Cada vez que se llama a render() se carga este Technique (pisando lo que el shader ya tenia seteado)
        /// </summary>
        public string Technique
        {
            get { return technique; }
            set { technique = value; }
        }

        /// <summary>
        ///     Habilita el renderizado con AlphaBlending para los modelos
        ///     con textura o colores por v�rtice de canal Alpha.
        ///     Por default est� deshabilitado.
        /// </summary>
        public bool AlphaBlendEnable { get; set; }

        /// <summary>
        ///     Renderizar el Tri�ngulo
        /// </summary>
        public void render()
        {
            if (!Enabled)
                return;

            var d3dDevice = GuiController.Instance.D3dDevice;
            var texturesManager = GuiController.Instance.TexturesManager;

            texturesManager.clear(0);
            texturesManager.clear(1);

            GuiController.Instance.Shaders.setShaderMatrixIdentity(effect);
            d3dDevice.VertexDeclaration = GuiController.Instance.Shaders.VdecPositionColored;
            effect.Technique = technique;
            d3dDevice.SetStreamSource(0, vertexBuffer, 0);

            //Render con shader
            effect.Begin(0);
            effect.BeginPass(0);
            d3dDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            effect.EndPass();
            effect.End();
        }

        /// <summary>
        ///     Liberar recursos de la flecha
        /// </summary>
        public void dispose()
        {
            if (vertexBuffer != null && !vertexBuffer.Disposed)
            {
                vertexBuffer.Dispose();
            }
        }

        /// <summary>
        ///     Actualizar par�metros del tri�ngulo en base a los valores configurados
        /// </summary>
        public void updateValues()
        {
            var vertices = new CustomVertex.PositionColored[3];

            //Crear tri�ngulo
            var ci = color.ToArgb();
            vertices[0] = new CustomVertex.PositionColored(A, ci);
            vertices[1] = new CustomVertex.PositionColored(B, ci);
            vertices[2] = new CustomVertex.PositionColored(C, ci);

            //Cargar vertexBuffer
            vertexBuffer.SetData(vertices, 0, LockFlags.None);
        }

        /// <summary>
        ///     Calcular normal del Tri�ngulo
        /// </summary>
        /// <returns>Normal (esta normalizada)</returns>
        public Vector3 computeNormal()
        {
            var n = Vector3.Cross(B - A, C - A);
            n.Normalize();
            return n;
        }

        /// <summary>
        ///     Calcular centro del Tri�ngulo
        /// </summary>
        /// <returns>Centro</returns>
        public Vector3 computeCenter()
        {
            return Vector3.Scale(A + B + C, 1 / 3f);
        }

        /// <summary>
        ///     Crea una flecha a modo debug para mostrar la normal de la cara del triangulo
        /// </summary>
        /// <returns>TgcArrow que representa la face-normal</returns>
        public TgcArrow createNormalArrow()
        {
            return TgcArrow.fromDirection(computeCenter(), Vector3.Scale(computeNormal(), 10f));
        }

        /// <summary>
        ///     Convierte el Tri�ngulo en un TgcMesh
        /// </summary>
        /// <param name="meshName">Nombre de la malla que se va a crear</param>
        public TgcMesh toMesh(string meshName)
        {
            var d3dDevice = GuiController.Instance.D3dDevice;

            //Crear Mesh con solo color
            var d3dMesh = new Mesh(1, 3, MeshFlags.Managed, TgcSceneLoader.TgcSceneLoader.VertexColorVertexElements,
                d3dDevice);

            //Calcular normal: left-handed
            var normal = computeNormal();
            var ci = color.ToArgb();

            //Cargar VertexBuffer
            using (var vb = d3dMesh.VertexBuffer)
            {
                var data = vb.Lock(0, 0, LockFlags.None);
                TgcSceneLoader.TgcSceneLoader.VertexColorVertex v;

                //a
                v = new TgcSceneLoader.TgcSceneLoader.VertexColorVertex();
                v.Position = A;
                v.Normal = normal;
                v.Color = ci;
                data.Write(v);

                //b
                v = new TgcSceneLoader.TgcSceneLoader.VertexColorVertex();
                v.Position = B;
                v.Normal = normal;
                v.Color = ci;
                data.Write(v);

                //c
                v = new TgcSceneLoader.TgcSceneLoader.VertexColorVertex();
                v.Position = C;
                v.Normal = normal;
                v.Color = ci;
                data.Write(v);

                vb.Unlock();
            }

            //Cargar IndexBuffer en forma plana
            using (var ib = d3dMesh.IndexBuffer)
            {
                var indices = new short[3];
                for (var j = 0; j < indices.Length; j++)
                {
                    indices[j] = (short)j;
                }
                ib.SetData(indices, 0, LockFlags.None);
            }

            //Malla de TGC
            var tgcMesh = new TgcMesh(d3dMesh, meshName, TgcMesh.MeshRenderType.VERTEX_COLOR);
            tgcMesh.Materials = new[] { TgcD3dDevice.DEFAULT_MATERIAL };
            tgcMesh.createBoundingBox();
            tgcMesh.Enabled = true;
            return tgcMesh;
        }
    }
}