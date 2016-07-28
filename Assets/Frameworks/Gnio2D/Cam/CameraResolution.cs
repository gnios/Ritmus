using UnityEngine;

namespace Assets.Frameworks.Gnio2D.Cam
{
    using Camera = UnityEngine.Camera;

    public class CameraResolution : MonoBehaviour
    {
        public float Width = 640;
        public float Height = 480;
        public float UnitsPerPixel = 100;
        public int SizeTile = 32;

        private void Start()
        {
            this.ConfigurationCamera();
        }

        private void ConfigurationCamera()
        {
            var camera = this.GetComponent<Camera>();
            camera.orthographicSize = this.CalculateOrthograficSize();
            Camera.main.aspect = Width / Height;
        }

        public float CalculateOrthograficSize()
        {
            var orthograficSize = (float)Height / this.UnitsPerPixel;
            orthograficSize = orthograficSize / 2;
            return orthograficSize;
        }
    }
}