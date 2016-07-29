using Assets.Frameworks.Gnio2D.Cam;
using UnityEngine;

namespace Assets.Frameworks.Gnio2D.Grid
{
    public class Grid : MonoBehaviour
    {
        public float AdjustX;
        public float AdjustY;

        public int SizeTile;
        public CameraResolution Camera;
        public GameObject[,] GridObjects = null;

        private int? _collumns;
        private int? _rows;

        public void Setup(CameraResolution camera, int sizeTile)
        {
            SizeTile = sizeTile;
            Camera = camera;
            camera.SizeTile = SizeTile;
            GridObjects = new GameObject[Collumns.GetValueOrDefault(), Rows.GetValueOrDefault()];
        }

        public int? Collumns
        {
            get { return _collumns ?? (_collumns = Mathf.RoundToInt(Camera.Width / this.SizeTile)); }
        }

        public int? Rows
        {
            get { return _rows ?? (_rows = Mathf.RoundToInt(Camera.Height / this.SizeTile)); }
        }

        public void AddInGrid(GameObject element, int col, int row)
        {
            var instantiateGameObject = (GameObject)Instantiate(element);
            instantiateGameObject.transform.position = GetPositionInGrid(col, row);
            GridObjects[col, row] = instantiateGameObject;
        }

        public void FillEmptySpaces(GameObject element)
        {
            for (var col = 0; col < GridObjects.GetLength(0); col++)
            {
                for (var row = 0; row < GridObjects.GetLength(1); row++)
                {
                    if (GridObjects[col, row] != null)
                        continue;

                    var instantiateGameObject = (GameObject)Instantiate(element);
                    instantiateGameObject.transform.position = GetPositionInGrid(col, row);
                    GridObjects[col, row] = instantiateGameObject;
                }
            }
        }

        private float CalculatePosition(int interator, float cameraSize)
        {
            var adjustRatio = (SizeTile / 2) / Camera.UnitsPerPixel;
            var position = cameraSize / (2 * Camera.UnitsPerPixel);
            position *= -1;
            return (position + adjustRatio + (SizeTile * interator) / Camera.UnitsPerPixel);
        }

        private Vector3 GetPositionInGrid(int col, int row)
        {
            return new Vector3(CalculatePosition(col, Camera.Width) + AdjustX, CalculatePosition(row, Camera.Height) + AdjustY);
        }
    }
}