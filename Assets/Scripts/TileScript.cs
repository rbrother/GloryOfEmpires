using UnityEngine;
using System.Collections;
using Net.Brotherus;

public class TileScript : MonoBehaviour {

    private bool dragging;
    private Vector3 lastPosition;
        
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!dragging) return;
        var deltaPos = ( lastPosition - Input.mousePosition );
        var deltaCamera = deltaPos * Camera.main.orthographicSize * 2.0f / Screen.height;
        this.gameObject.transform.Translate( -deltaCamera.x, -deltaCamera.y, 0 );
        lastPosition = Input.mousePosition;
    }

    public void OnMouseDown( ) {
        this.dragging = true;
        this.gameObject.transform.Translate( 0, 0, -1 );
        lastPosition = Input.mousePosition;
    }

    public void OnMouseUp( ) {
        if ( dragging ) { 
            this.dragging = false;
            // Todo: do dependency inversion to avoid referring here Map.CurrentMap
            var snappedLocation = Map.CurrentMap.NearestLocation( this.gameObject.transform.position );
            this.gameObject.transform.position = snappedLocation.TableXY( 0 );
        }
    }

}
