namespace TGC.Tools.Utils.TgcSkeletalAnimation
{
    /// <summary>
    ///     Informaci�n de animiaci�n de un esqueleto
    /// </summary>
    public class TgcSkeletalAnimationData
    {
        //Info general de la animacion
        public int bonesCount;

        //Frames para cada Bone
        public TgcSkeletalAnimationBoneData[] bonesFrames;

        public int endFrame;
        public int frameRate;

        public int framesCount;
        public string name;

        public float[] pMax;

        //BoundingBox para esta animaci�n
        public float[] pMin;

        public int startFrame;
    }
}