using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Tools.Model;
using TGC.Tools.Utils.Shaders;
using TGC.Tools.Utils.TgcSceneLoader;

namespace TGC.Tools.Utils.TgcGeometry
{
    /// <summary>
    ///     Representa un volumen de Elipsoide con un centro y un radio distinto para cada uno de los tres ejes
    /// </summary>
    public class TgcElipsoid : IRenderObject
    {
        /// <summary>
        ///     Cantidad de tramos que tendr� el mesh del Elipsoid a dibujar
        /// </summary>
        public const int ELIPSOID_MESH_RESOLUTION = 10;

        private bool dirtyValues;

        protected Effect effect;

        protected string technique;
        private CustomVertex.PositionColored[] vertices;

        /// <summary>
        ///     Crear Elipsoid vacia
        /// </summary>
        public TgcElipsoid()
        {
            RenderColor = Color.Yellow.ToArgb();
            dirtyValues = true;
            AlphaBlendEnable = false;
        }

        /// <summary>
        ///     Crear Elipsoid con centro y radio
        /// </summary>
        /// <param name="center">Centro</param>
        /// <param name="radius">Radios para los 3 ejes</param>
        public TgcElipsoid(Vector3 center, Vector3 radius)
            : this()
        {
            setValues(center, radius);
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
        ///     Centro del Elipsoid
        /// </summary>
        public Vector3 Center { get; private set; }

        /// <summary>
        ///     Radios del Elipsoid para cada uno de los 3 ejes
        /// </summary>
        public Vector3 Radius { get; private set; }

        /// <summary>
        ///     Color de renderizado del Elipsoid.
        /// </summary>
        public int RenderColor { get; private set; }

        public Vector3 Position
        {
            get { return Center; }
        }

        /// <summary>
        ///     Habilita el renderizado con AlphaBlending para los modelos
        ///     con textura o colores por v�rtice de canal Alpha.
        ///     Por default est� deshabilitado.
        /// </summary>
        public bool AlphaBlendEnable { get; set; }

        /// <summary>
        ///     Renderizar el Elipsoid
        /// </summary>
        public void render()
        {
            var d3dDevice = GuiController.Instance.D3dDevice;
            var texturesManager = GuiController.Instance.TexturesManager;

            texturesManager.clear(0);
            texturesManager.clear(1);

            //Cargar shader si es la primera vez
            if (effect == null)
            {
                effect = GuiController.Instance.Shaders.VariosShader;
                technique = TgcShaders.T_POSITION_COLORED;
            }

            //Actualizar vertices del Elipsoid solo si hubo una modificaci�n
            if (dirtyValues)
            {
                updateValues();
                dirtyValues = false;
            }

            GuiController.Instance.Shaders.setShaderMatrixIdentity(effect);
            d3dDevice.VertexDeclaration = GuiController.Instance.Shaders.VdecPositionColored;
            effect.Technique = technique;

            //Render con shader
            effect.Begin(0);
            effect.BeginPass(0);
            d3dDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices.Length / 2, vertices);
            effect.EndPass();
            effect.End();
        }

        /// <summary>
        ///     Libera los recursos del objeto
        /// </summary>
        public void dispose()
        {
            vertices = null;
        }

        /// <summary>
        ///     Configurar valores del Elipsoid
        /// </summary>
        /// <param name="center">Centro</param>
        /// <param name="radius">Radios para los 3 ejes</param>
        public void setValues(Vector3 center, Vector3 radius)
        {
            Center = center;
            Radius = radius;

            dirtyValues = true;
        }

        /// <summary>
        ///     Configurar un nuevo centro del Elipsoid
        /// </summary>
        /// <param name="center">Nuevo centro</param>
        public void setCenter(Vector3 center)
        {
            setValues(center, Radius);
        }

        /// <summary>
        ///     Desplazar el centro respecto de su posici�n actual
        /// </summary>
        /// <param name="movement">Movimiento relativo a realizar</param>
        public void moveCenter(Vector3 movement)
        {
            setValues(Center + movement, Radius);
        }

        /// <summary>
        ///     Configurar el color de renderizado del Elipsoid
        ///     Ejemplo: Color.Yellow.ToArgb();
        /// </summary>
        public void setRenderColor(Color color)
        {
            RenderColor = color.ToArgb();
            dirtyValues = true;
        }

        /// <summary>
        ///     Construye el mesh del Elipsoid
        /// </summary>
        private void updateValues()
        {
            if (vertices == null)
            {
                var verticesCount = (ELIPSOID_MESH_RESOLUTION * 2 + 2) * 3;
                vertices = new CustomVertex.PositionColored[verticesCount];
            }

            var index = 0;

            var step = FastMath.TWO_PI / ELIPSOID_MESH_RESOLUTION;
            // Plano XY
            for (var a = 0f; a <= FastMath.TWO_PI; a += step)
            {
                vertices[index++] =
                    new CustomVertex.PositionColored(
                        new Vector3(FastMath.Cos(a) * Radius.X, FastMath.Sin(a) * Radius.Y, 0f) + Center, RenderColor);
                vertices[index++] =
                    new CustomVertex.PositionColored(
                        new Vector3(FastMath.Cos(a + step) * Radius.X, FastMath.Sin(a + step) * Radius.Y, 0f) + Center,
                        RenderColor);
            }

            // Plano XZ
            for (var a = 0f; a <= FastMath.TWO_PI; a += step)
            {
                vertices[index++] =
                    new CustomVertex.PositionColored(
                        new Vector3(FastMath.Cos(a) * Radius.X, 0f, FastMath.Sin(a) * Radius.Z) + Center, RenderColor);
                vertices[index++] =
                    new CustomVertex.PositionColored(
                        new Vector3(FastMath.Cos(a + step) * Radius.X, 0f, FastMath.Sin(a + step) * Radius.Z) + Center,
                        RenderColor);
            }

            // Plano YZ
            for (var a = 0f; a <= FastMath.TWO_PI; a += step)
            {
                vertices[index++] =
                    new CustomVertex.PositionColored(
                        new Vector3(0f, FastMath.Cos(a) * Radius.Y, FastMath.Sin(a) * Radius.Z) + Center, RenderColor);
                vertices[index++] =
                    new CustomVertex.PositionColored(
                        new Vector3(0f, FastMath.Cos(a + step) * Radius.Y, FastMath.Sin(a + step) * Radius.Z) + Center,
                        RenderColor);
            }
        }

        public override string ToString()
        {
            return "Center " + TgcParserUtils.printVector3(Center) + ", Radius " + TgcParserUtils.printVector3(Radius);
        }

        /// <summary>
        ///     Devuelve el radio mas grande del Elipsoid
        /// </summary>
        /// <returns>Mayor radio</returns>
        public float getMaxRadius()
        {
            return Radius.X > Radius.Y ? Radius.X : (Radius.Y > Radius.Z ? Radius.Y : Radius.Z);
        }

        /// <summary>
        ///     Convertir a struct
        /// </summary>
        public ElipsoidStruct toStruct()
        {
            var eStruct = new ElipsoidStruct();
            eStruct.center = Center;
            eStruct.radius = Radius;
            return eStruct;
        }

        /// <summary>
        ///     Elipsoid en un struct liviano
        /// </summary>
        public struct ElipsoidStruct
        {
            public Vector3 center;
            public Vector3 radius;

            /// <summary>
            ///     Convertir a clase
            /// </summary>
            public TgcElipsoid toClass()
            {
                return new TgcElipsoid(center, radius);
            }
        }
    }
}