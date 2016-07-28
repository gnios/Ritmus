using Assets.Frameworks.Gnio2D.Cam;
using UnityEngine;

namespace Assets.Frameworks.Gnio2D.Grid
{
    public class Grid
    {
        private readonly float _sizeTile;
        public float AdjustX;
        public float AdjustY;

        private readonly CameraResolution _camera;

        public Grid(int sizeTile, CameraResolution camera)
        {
            _sizeTile = sizeTile;
            _camera = camera;
        }

        public Vector3 GetPositionInGrid(int col, int row)
        {
            return new Vector3(CalculatePosition(col, _camera.Width) + AdjustX, CalculatePosition(row, _camera.Height) + AdjustY);
        }

        private int Collumns()
        {
            return Mathf.RoundToInt(_camera.Width / this._sizeTile);
        }

        private int Rows()
        {
            return Mathf.RoundToInt(_camera.Height / this._sizeTile);
        }

        private float CalculatePosition(int interator, float cameraSize)
        {
            var adjustRatio = (_sizeTile / 2) / _camera.UnitsPerPixel;
            var position = cameraSize / (2 * _camera.UnitsPerPixel);
            position *= -1;
            return (position + adjustRatio + (_sizeTile * interator) / _camera.UnitsPerPixel);
        }
    }
}