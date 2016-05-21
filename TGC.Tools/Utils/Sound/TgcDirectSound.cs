using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using TGC.Tools.Utils.TgcSceneLoader;

namespace TGC.Tools.Utils.Sound
{
    /// <summary>
    ///     Herramienta para manipular el Device de DirectSound
    /// </summary>
    public class TgcDirectSound
    {
        private readonly Buffer primaryBuffer;

        public TgcDirectSound()
        {
            //Crear device de DirectSound
            DsDevice = new Device();
            DsDevice.SetCooperativeLevel(GuiController.Instance.MainForm, CooperativeLevel.Normal);

            //Crear Listener3D
            var primaryBufferDesc = new BufferDescription();
            primaryBufferDesc.Control3D = true;
            primaryBufferDesc.PrimaryBuffer = true;
            primaryBuffer = new Buffer(primaryBufferDesc, DsDevice);
            Listener3d = new Listener3D(primaryBuffer);
            Listener3d.Position = new Vector3(0f, 0f, 0f);
            Listener3d.Orientation = new Listener3DOrientation(new Vector3(1, 0, 0), new Vector3(0, 1, 0));
        }

        /// <summary>
        ///     Device de DirectSound
        /// </summary>
        public Device DsDevice { get; }

        /// <summary>
        ///     Representa el objeto central del universo 3D que escucha todos los dem�s sonidos.
        ///     En base a su posici�n var�a la captaci�n de todos los demas sonidos 3D.
        /// </summary>
        public Listener3D Listener3d { get; }

        /// <summary>
        ///     Objeto al cual el Listener3D va a seguir para variar su posici�n en cada cuadro.
        ///     Solo puede haber un objeto que est� siendo seguido por el Listener3D a la vez.
        ///     En caso de haber configurado un objeto a seguir, el Listener3D actualiza su posici�n en forma
        ///     autom�tica en cada cuadro.
        /// </summary>
        public ITransformObject ListenerTracking { get; set; }

        /// <summary>
        ///     Actualiza la posici�n del Listener3D en base al ListenerTracking
        /// </summary>
        internal void updateListener3d()
        {
            if (ListenerTracking != null)
            {
                Listener3d.Position = ListenerTracking.Position;
            }
        }
    }
}