using UnityEngine;

public class CameraPanning : MonoBehaviour {

	private float targetCameraSize;
	private Vector3 lastPosition;

	private const float MAX_CAMERA_SIZE = 30.0f;
	private const float MIN_CAMERA_SIZE = 2.0f;

	void Start () {
		targetCameraSize = this.GetComponent<Camera>().orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
		// Zooming with  mouse wheel
		float mouseWheel = Input.GetAxis ("Mouse ScrollWheel"); // Movement since last frame
		if (mouseWheel != 0) this.targetCameraSize *= (1.0f - mouseWheel * 1.0f);
		if (this.targetCameraSize > MAX_CAMERA_SIZE) this.targetCameraSize = MAX_CAMERA_SIZE;
		if (this.targetCameraSize < MIN_CAMERA_SIZE) this.targetCameraSize = MIN_CAMERA_SIZE;
		var deltaSize = (targetCameraSize - GetComponent<Camera>().orthographicSize);
		GetComponent<Camera>().orthographicSize += deltaSize * Time.deltaTime * 5.0f;
		// Panning with mouse button
		if (Input.GetMouseButtonDown (1)) {
			lastPosition = Input.mousePosition;
		}
		if (Input.GetMouseButton (1)) {
			var deltaPos = ( lastPosition - Input.mousePosition );
			var deltaCamera = deltaPos * GetComponent<Camera>().orthographicSize * 2.0f / Screen.height;
			transform.Translate( deltaCamera.x, deltaCamera.y, 0 );
			lastPosition = Input.mousePosition;
		}
	}
}
