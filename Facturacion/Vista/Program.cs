using System;
using System.Windows.Forms;

namespace Vista
{
    internal static class Program
    {

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTI4MzQyM0AzMjMwMmUzNDJlMzBQUHR2OGVaUEhlN1NKNlp3ZS8zMytmaS9oNW1EcTk5K09iblVuaUxHM0ljPQ==;Mgo+DSMBaFt/QHRqVVhjVFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jSn5bd0BjW35Yd3FXQQ==;MTI4MzQyNUAzMjMwMmUzNDJlMzBtVXYxdEV2TWVTSGxCRGpvL3NidE5kS3VyNWZVS3F6QW11R2dJZjREYm9JPQ==");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
