namespace TGC.Tools.Example
{
    public abstract class TgcExample
    {
        /// <summary>
        ///     Path absoluto del directorio en donde se encuentra el ejemplo
        /// </summary>
        public string ExampleDir { get; private set; }

        /// <summary>
        ///     Categor�a que agrupa a este ejemplo
        /// </summary>
        public abstract string getCategory();

        /// <summary>
        ///     Nombre corto del ejemplo
        /// </summary>
        public abstract string getName();

        /// <summary>
        ///     Descripci�n del ejemplo
        /// </summary>
        public abstract string getDescription();

        /// <summary>
        ///     Se llama cuando el ejemplo es elegido para ejecutar.
        ///     Inicializar todos los recursos y configuraciones que se van a utilizar.
        /// </summary>
        public abstract void init();

        /// <summary>
        ///     Se llama cuando el ejemplo es cerrado.
        ///     Liberar todos los recursos utilizados.
        /// </summary>
        public abstract void close();

        /// <summary>
        ///     Se llama para renderizar cada cuadro del ejemplo.
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el �ltimo frame</param>
        public abstract void render(float elapsedTime);

        internal void setExampleDir(string path)
        {
            ExampleDir = path;
        }
    }
}