using UnityEngine;
using UnityEngine.UI;

public class CameraPanning : MonoBehaviour {

    private static bool _enablePanning = true;

	private float targetCameraSize;
	private Vector3 lastPosition;

    public GameObject goSlider;
    private Slider uiSlider;

	private const float MAX_CAMERA_SIZE = 30.0f;
	private const float MIN_CAMERA_SIZE = 2.0f;

    public static bool EnablePanning {
        get { return _enablePanning; }
        set { _enablePanning = value; }
    }

    public float CameraSize {
        get {
            return ( MAX_CAMERA_SIZE - this.targetCameraSize ) / ( MAX_CAMERA_SIZE - MIN_CAMERA_SIZE );  
        }
        set { 
            this.targetCameraSize = MAX_CAMERA_SIZE - ( MAX_CAMERA_SIZE - MIN_CAMERA_SIZE) * value; 
        }
    }

	void Start () {
		targetCameraSize = this.GetComponent<Camera>().orthographicSize;
        uiSlider = goSlider.GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		// Zooming with  mouse wheel
		float mouseWheel = Input.GetAxis ("Mouse ScrollWheel"); // Movement since last frame
		if (mouseWheel != 0) this.targetCameraSize *= (1.0f - mouseWheel * 1.0f);
		if (this.targetCameraSize > MAX_CAMERA_SIZE) this.targetCameraSize = MAX_CAMERA_SIZE;
		if (this.targetCameraSize < MIN_CAMERA_SIZE) this.targetCameraSize = MIN_CAMERA_SIZE;

        uiSlider.value = this.CameraSize; // update ui-slider based on target-camera-size

		var deltaSize = (targetCameraSize - GetComponent<Camera>().orthographicSize);
		GetComponent<Camera>().orthographicSize += deltaSize * Time.deltaTime * 5.0f;
		// Panning with mouse button
		if (Input.GetMouseButtonDown (0) && EnablePanning) {
			lastPosition = Input.mousePosition;
		}
		if (Input.GetMouseButton (0) && EnablePanning) {
			var deltaPos = ( lastPosition - Input.mousePosition );
			var deltaCamera = deltaPos * GetComponent<Camera>().orthographicSize * 2.0f / Screen.height;
			transform.Translate( deltaCamera.x, deltaCamera.y, 0 );
			lastPosition = Input.mousePosition;
		}
	}
}
