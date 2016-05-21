using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace TGC.Tools.Utils.Input
{
    /// <summary>
    ///     Interfaz general para una c�mara del Framework
    /// </summary>
    public interface TgcCamera
    {
        /// <summary>
        ///     Activar o desactivar la camara
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        ///     Posici�n de la c�mara
        /// </summary>
        Vector3 getPosition();

        /// <summary>
        ///     Posici�n del punto al que mira la c�mara
        /// </summary>
        Vector3 getLookAt();

        /// <summary>
        ///     Actualizar el estado interno de la c�mara en cada frame
        /// </summary>
        void updateCamera();

        /// <summary>
        ///     Actualizar la matriz View en base a los valores de la c�mara
        /// </summary>
        void updateViewMatrix(Device d3dDevice);
    }
}