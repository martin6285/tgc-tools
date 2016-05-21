namespace TGC.Tools.Utils.TgcSkeletalAnimation
{
    /// <summary>
    ///     Informaci�n de animaci�n de un hueso para una animaci�n particular
    /// </summary>
    public class TgcSkeletalAnimationBoneData
    {
        public int id;

        public TgcSkeletalAnimationBoneFrameData[] keyFrames;
        public int keyFramesCount;
    }
}