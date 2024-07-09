using UnityEngine;

namespace PSG.IsleOfColors.UI
{
	public class CameraControl : MonoBehaviour
	{
		[SerializeField] Transform player1Transform;
        [SerializeField] Transform player2Transform;

		private Transform target;

        private void Start()
        {
            target = player1Transform;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target.position + Vector3.forward * transform.position.z, 1.5f * Time.deltaTime);
        }

        public void MoveToPlayer1()
		{
            target = player1Transform;

        }

        public void MoveToPlayer2()
        {
            target = player2Transform;
        }
    }
}
