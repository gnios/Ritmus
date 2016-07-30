using System;
using System.Collections.Generic;
using Assets.Frameworks.Gnio2D.Projectiles;
using UnityEngine;

namespace Assets.Src.Behaviours
{
    public class BombTile : MonoBehaviour
    {
        public Projectile Projectile;
        public float Speed = 20f;

        private Dictionary<Direction, Func<GameObject, Vector2>> _relativeForces;

        private void Awake()
        {
            _relativeForces = new Dictionary<Direction, Func<GameObject, Vector2>>
            {
                {Direction.Right, go => go.transform.right * Speed },
                {Direction.Up,go => (go.transform.up * -1) * Speed },
                {Direction.Left, go => (go.transform.right * -1) * Speed },
                {Direction.Down,go => (go.transform.up) * Speed},
            };
        }

        private void Explode()
        {
            var projectiles = new List<GameObject>
            {
                CreateObjectWith(Direction.Right),
                CreateObjectWith(Direction.Up),
                CreateObjectWith(Direction.Left),
                CreateObjectWith(Direction.Down)
            };

            IgnoreCollisions(projectiles);
        }

        private void OnMouseDown()
        {
            Explode();
        }

        private GameObject CreateObjectWith(Direction direction)
        {
            var angle = 90 * (int)direction;
            var projectile = (Projectile)Instantiate(Projectile, gameObject.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            projectile.RelativeForce = _relativeForces[direction](projectile.gameObject);
            return projectile.gameObject;
        }

        private void IgnoreCollisions(IList<GameObject> listGameObjects)
        {
            for (int i = 0; i < listGameObjects.Count; i++)
            {
                Physics2D.IgnoreCollision(listGameObjects[i].transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());

                for (int j = 0; j < listGameObjects.Count; j++)
                    if (i != j)
                        Physics2D.IgnoreCollision(listGameObjects[i].transform.GetComponent<Collider2D>(), listGameObjects[j].transform.GetComponent<Collider2D>());
            }
        }

        public enum Direction
        {
            Right,
            Up,
            Left,
            Down
        }
    }
}