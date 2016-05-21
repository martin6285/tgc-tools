using TGC.Tools.Utils.TgcGeometry;

namespace TGC.Tools.Utils.PortalRendering
{
    /// <summary>
    ///     Portal de PortalRendering que comunica dos celdas
    /// </summary>
    public class TgcPortalRenderingPortal
    {
        public TgcPortalRenderingPortal(string name, TgcBoundingBox boundingBox)
        {
            Name = name;
            BoundingBox = boundingBox;
        }

        /// <summary>
        ///     Nombre del portal
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     BoundingBox del Portal
        /// </summary>
        public TgcBoundingBox BoundingBox { get; }

        /// <summary>
        ///     Indica si la celda ya fue visitada por el algoritmo de visibilidad
        /// </summary>
        public bool Visited { get; set; }

        public override string ToString()
        {
            return "Portal: " + Name;
        }
    }
}