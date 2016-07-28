using Assets.Frameworks.Gnio2D.Cam;
using UnityEngine;

namespace Assets.Frameworks.Gnio2D
{
    [ExecuteInEditMode]
    public class Initializer : MonoBehaviour
    {
        private void Awake()
        {
            if (Camera.main.gameObject.GetComponent<CameraResolution>() == null)
                Camera.main.gameObject.AddComponent<CameraResolution>();

            ConfigResolutionCamera();
        }

        public void ConfigResolutionCamera()
        {
            var configResolution = Camera.main.GetComponent<CameraResolution>();
            configResolution.Width = 1024f;
            configResolution.Height = 768f;
            configResolution.UnitsPerPixel = 100;
        }
    }
}