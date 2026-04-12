using UnityEngine;

namespace Platformer
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] float groundDistance = 0.08f;
        [SerializeField] LayerMask groundLayers;

        public bool IsGrounded {  get; private set; }

        private void Update()
        {
            IsGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayers);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundDistance);
        }
    }
}
