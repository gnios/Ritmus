using System.Collections.Generic;
using Assets.Frameworks.Gnio2D.Cam;
using Assets.Frameworks.Gnio2D.Grid;
using Assets.Src.Models;
using GameDevWare.Serialization;
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
        private Dictionary<TileTypeEnum, GameObject> _gameObjectsMap;
        private Grid _grid;

        private void Awake()
        {
            _gameObjectsMap = new Dictionary<TileTypeEnum, GameObject>
            {
                {TileTypeEnum.Empty, EmptyBlock},
                {TileTypeEnum.Bomb, BombBlock}
            };
            _level = new Level
            {
                TileMap = new[] {
                    new Tile { Row = 1, Col = 1, TileType = TileTypeEnum.Bomb },
                    new Tile { Row = 4, Col = 2, TileType = TileTypeEnum.Bomb },
                    new Tile { Row = 3, Col = 6, TileType = TileTypeEnum.Bomb },
                    new Tile { Row = 1, Col = 6, TileType = TileTypeEnum.Bomb },
                }
            };
        }

        private void Start()
        {
            _grid = gameObject.AddComponent<Grid>();
            _grid.Setup(Camera, SizeTile);
            foreach (var elementMap in _level.TileMap)
                _grid.AddInGrid(_gameObjectsMap[elementMap.TileType], elementMap.Col, elementMap.Row);

            _grid.FillEmptySpaces(_gameObjectsMap[TileTypeEnum.Empty]);
            var json = Json.SerializeToString(_level, SerializationOptions.SuppressTypeInformation);

            //write string to file
            System.IO.File.WriteAllText(Application.dataPath + @"\Resources\level.txt", json);
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}