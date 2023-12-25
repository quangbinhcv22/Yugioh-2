using UnityEngine;

namespace Testing
{
    public class Rotator : MonoBehaviour
    {
        public Vector3 speed;

        private void Update()
        {
            transform.eulerAngles += speed * Time.deltaTime;
        }
    }
}