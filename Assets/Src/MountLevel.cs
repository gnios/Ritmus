using System.Collections.Generic;
using Assets.Frameworks.Gnio2D.Cam;
using Assets.Frameworks.Gnio2D.Grid;
using Assets.Src.Models;
using UnityEngine;

namespace Assets.Src
{
    public class MountLevel : MonoBehaviour
    {
        public GameObject EmptyBlock;
        public GameObject BombBlock;
        public CameraResolution Camera;
        public int SizeTile = 128;
        private Level _level;
        private Dictionary<ElementTypeEnum, GameObject> _gameObjectsMap;
        private Grid _grid;

        private void Awake()
        {
            _gameObjectsMap = new Dictionary<ElementTypeEnum, GameObject>
            {
                {ElementTypeEnum.Empty, EmptyBlock},
                {ElementTypeEnum.Bomb, BombBlock}
            };
            _level = new Level
            {
                Elements = new List<ElementMap> {
                    new ElementMap { Row = 1, Col = 1, ElementType = ElementTypeEnum.Bomb },
                    new ElementMap { Row = 4, Col = 2, ElementType = ElementTypeEnum.Bomb },
                    new ElementMap { Row = 3, Col = 6, ElementType = ElementTypeEnum.Bomb },
                }
            };
        }

        private void Start()
        {
            _grid = gameObject.AddComponent<Grid>();
            _grid.Setup(Camera, SizeTile);
            foreach (var elementMap in _level.Elements)
                _grid.AddInGrid(_gameObjectsMap[elementMap.ElementType], elementMap.Col, elementMap.Row);

            _grid.FillEmptySpaces(_gameObjectsMap[ElementTypeEnum.Empty]);
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}