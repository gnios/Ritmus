using UnityEngine;

namespace Assets.Frameworks.Gnio2D
{
    public class Explode : MonoBehaviour
    {
        public GameObject Projectile;
        private GameObject _clone;

        private void OnMouseDown()
        {
            _clone = (GameObject)Instantiate(Projectile, gameObject.transform.position, gameObject.transform.rotation);
            _clone.transform.position = gameObject.transform.position;

            // Add force to the cloned object in the object's forward direction
            Debug.Log("Sprite Clicked");
        }

        private void Update()
        {
            if (_clone != null)
                _clone.GetComponent<Rigidbody>().AddForce(_clone.transform.right * 20f);
        }
    }
}