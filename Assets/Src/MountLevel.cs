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

        private Level _level;
        private Dictionary<ElementTypeEnum, GameObject> _gameObjectsMap;
        private Grid _grid;

        private void Awake()
        {
            _grid = new Grid(128, Camera);
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
            GameObject element;
            foreach (var elementMap in _level.Elements)
            {
                element = (GameObject)Instantiate(_gameObjectsMap[elementMap.ElementType]);
                element.transform.position = _grid.GetPositionInGrid(elementMap.Col, elementMap.Row);
            }
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}