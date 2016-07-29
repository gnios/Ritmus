using UnityEngine;

namespace Assets.Frameworks.Gnio2D
{
    public class Explode : MonoBehaviour
    {
        public GameObject Projectile;

        private GameObject projectileRight;
        private GameObject projectileLeft;
        private GameObject projectileUp;
        private GameObject projectileDown;

        private void OnMouseDown()
        {
            projectileRight = (GameObject)Instantiate(Projectile, gameObject.transform.position, Quaternion.AngleAxis(0, Vector3.forward));
            projectileLeft = (GameObject)Instantiate(Projectile, gameObject.transform.position, Quaternion.AngleAxis(180, Vector3.forward));
            projectileUp = (GameObject)Instantiate(Projectile, gameObject.transform.position, Quaternion.AngleAxis(90, Vector3.forward));
            projectileDown = (GameObject)Instantiate(Projectile, gameObject.transform.position, Quaternion.AngleAxis(270f, Vector3.forward));

            Debug.Log("Sprite Clicked");
        }

        private void Update()
        {
            if (projectileRight == null)
                return;
            projectileRight.GetComponent<Rigidbody>().AddRelativeForce(projectileRight.transform.right * 20f);
            projectileLeft.GetComponent<Rigidbody>().AddRelativeForce((projectileLeft.transform.right * -1) * 20f);
            projectileUp.GetComponent<Rigidbody>().AddRelativeForce((projectileUp.transform.up * -1) * 20f);
            projectileDown.GetComponent<Rigidbody>().AddRelativeForce((projectileDown.transform.up) * 20f);
        }
    }
}