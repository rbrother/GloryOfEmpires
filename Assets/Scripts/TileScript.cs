using UnityEngine;
using System.Collections;
using Net.Brotherus;

public class TileScript : MonoBehaviour {

    private bool _selected = false;
    private bool dragging = false;
    private Vector3 lastPosition;
    private Vector3 mouseDownPosition;
        
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (dragging) {
            var deltaPos = ( lastPosition - Input.mousePosition );
            var deltaCamera = deltaPos * Camera.main.orthographicSize * 2.0f / Screen.height;
            this.gameObject.transform.Translate( -deltaCamera.x, -deltaCamera.y, 0 );
            lastPosition = Input.mousePosition;
        } 
    }

    private bool MouseCloseToDownPos {
        get {
            var deltaPos = ( mouseDownPosition - Input.mousePosition );
            return deltaPos.magnitude < 5.0;
        }
    }

    public void OnMouseDown( ) {
        Debug.Log( "OnMouseDown" );
        mouseDownPosition = Input.mousePosition;
        lastPosition = mouseDownPosition;
        if (Selected) {
            this.dragging = true;
        }
    }

    public void OnMouseUp( ) {
        Debug.Log( "OnMouseUp" );
        if (MouseCloseToDownPos) {
            Debug.Log( "Selected" );
            this.Selected = !this.Selected;
        } else if ( dragging ) {
            this.Selected = false;
            var snappedLocation = Map.CurrentMap.NearestLocation( this.gameObject.transform.position );
            this.gameObject.transform.position = snappedLocation.TableXY( 0 );
        }
        this.dragging = false;
    }

    public bool Selected {
        get { return this._selected; }
        set {
            if ( value != _selected ) { 
                _selected = value;
                GetComponent<SpriteRenderer>( ).color = _selected ? Color.red : Color.white;
                this.gameObject.transform.Translate( 0, 0, _selected ? -1 : 1 ); // Move sepected to front
            }
        }
    }

}
