using UnityEngine;
using System.Collections;
using Net.Brotherus;

public class Dragging : Photon.MonoBehaviour {

    public static GameObject SelectedShip = null;

    private bool _selected = false;
    private Vector3 lastPosition;
    private Vector3 mouseDownPosition;
    private float _originalZ;

    public int SnapSteps { get; set; }

    public Dragging( ) {
        SnapSteps = 1;
    }

	void Start () {	
	}

    private bool MouseCloseToDownPos {
        get {
            var deltaPos = ( mouseDownPosition - Input.mousePosition );
            return deltaPos.magnitude < 5.0;
        }
    }
	
	void Update () {
        if ( !CameraPanning.EnablePanning && this.Selected ) {
            var deltaPos = ( lastPosition - Input.mousePosition );
            var deltaCamera = deltaPos * Camera.main.orthographicSize * 2.0f / Screen.height;
            this.gameObject.transform.Translate( -deltaCamera.x, -deltaCamera.y, 0 );
            lastPosition = Input.mousePosition;
        } 	
	}

    public void OnMouseDown( ) {
        mouseDownPosition = Input.mousePosition;
        lastPosition = mouseDownPosition;
        if ( Selected ) CameraPanning.EnablePanning = false;
    }

    public void OnMouseUp( ) {
        if ( MouseCloseToDownPos ) {
            this.Selected = !this.Selected;
        } else if ( !CameraPanning.EnablePanning ) {
            // TODO: Try to refactor away dependency of Map.CurrentMap
            var snappedLocation = Map.CurrentMap.NearestLocation( this.transform.position, SnapSteps );
            this.transform.position = new Vector3( snappedLocation.x, snappedLocation.y, _originalZ );
        }
        CameraPanning.EnablePanning = true;
    }

    public bool Selected {
        get { return this._selected; }
        set {
            if ( value != _selected ) {
                if ( value && SelectedShip != null ) {
                    // Allow only one ship to be selected at a time
                    // If selecting ship and other ship is selected, deselect the previous one
                    SelectedShip.GetComponent<Dragging>( ).Selected = false;
                }
                _selected = value;
                var photonView = GetComponent<PhotonView>( );
                if ( !photonView.isMine ) photonView.RequestOwnership( );
                GetComponent<SpriteRenderer>( ).color = _selected ? new Color( 1, 1, 1, 0.75f ) : Color.white;
                if ( _selected ) {
                    _originalZ = this.transform.position.z;
                }
                this.transform.Translate( 0, 0, _selected ? -1 : 1 ); // Move selected to front
                SelectedShip = _selected ? this.gameObject : null;
            }
        }
    }

}
