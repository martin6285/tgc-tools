using TgcViewer.Utils.TgcGeometry;

namespace TgcViewer.Utils.TgcKeyFrameLoader
{
    /// <summary>
    /// Animación de una malla animada por KeyFrames
    /// </summary>
    public class TgcKeyFrameAnimation
    {
        public TgcKeyFrameAnimation(TgcKeyFrameAnimationData data, TgcBoundingBox boundingBox)
        {
            this.data = data;
            this.boundingBox = boundingBox;
        }

        private TgcBoundingBox boundingBox;

        /// <summary>
        /// BoundingBox de la animación
        /// </summary>
        public TgcBoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        private TgcKeyFrameAnimationData data;

        /// <summary>
        /// Datos de vértices de la animación
        /// </summary>
        public TgcKeyFrameAnimationData Data
        {
            get { return data; }
        }
    }
}