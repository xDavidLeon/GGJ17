using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class SmoothFollow : MonoBehaviour
	{

		// The target we are following
		[SerializeField]
		private Transform target;
        private Vector3 targetPos = Vector3.zero;

		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;

        // Input
        public float rotSpeed = 1.0f;
        public float maxHeight = 100.0f;
        public float minHeight = 4.0f;
        public float rotSpeedHeight = 1.0f;

        private float rotationAngle = 0.0f;
        private float rotHeight = 10.0f;

        // Repositioning
        public float maxTargetOffset = 100.0f;
        public float targetOffset = 0.0f;

        // Use this for initialization
        void Start() { }

		// Update is called once per frame
		void LateUpdate()
		{
			// Early out if we don't have a target
			if (!target)
				return;

            targetPos = target.position;
            rotationAngle += Input.GetAxis("Mouse X")  * rotSpeed;
            rotationAngle = Mathf.Clamp(rotationAngle , - 359.0f, 359.0f);
            rotHeight += Input.GetAxis("Mouse Y")  * rotSpeedHeight;

            // Calculate the current rotation angles
            var wantedRotationAngle = rotationAngle;
			var wantedHeight = target.position.y + rotHeight;
            wantedHeight = Mathf.Clamp(wantedHeight, minHeight, maxHeight);
			var currentRotationAngle = transform.eulerAngles.y;
			var currentHeight = transform.position.y;

			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			transform.position = target.position;
			transform.position -= currentRotation * Vector3.forward * distance;

			// Set the height of the camera
			transform.position = new Vector3(transform.position.x ,currentHeight , transform.position.z);

            targetOffset = Mathf.Lerp(0, maxTargetOffset, (rotHeight - minHeight) / (maxHeight - minHeight));
            Vector3 forward = target.transform.forward;
            Quaternion newRotation = Quaternion.FromToRotation(target.transform.forward, new Vector3(0, rotationAngle, 0));
            targetPos.y += targetOffset;
			// Always look at the target
			transform.LookAt(targetPos);
		}
	}
}