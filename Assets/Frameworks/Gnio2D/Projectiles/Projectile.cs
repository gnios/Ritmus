using UnityEngine;

namespace Assets.Frameworks.Gnio2D.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public Vector2 RelativeForce;

        private void Update()
        {
            GetComponent<Rigidbody2D>().AddRelativeForce(RelativeForce);
        }
    }
}