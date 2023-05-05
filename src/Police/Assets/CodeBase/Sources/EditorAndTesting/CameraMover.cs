using UnityEngine;

namespace Testing
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private float _shiftModifier = 10;
        [SerializeField] private float _regularSpeed = 6;
        
        private void Update()
        {
            var direction = Vector3.forward * (Time.deltaTime * _regularSpeed);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                direction *= _shiftModifier;
            }
            
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += -direction;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += direction;
            }
        }
    }
}