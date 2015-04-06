using UnityEngine;
using System.Collections;
using Net.Brotherus;
using Photon;

public class TileScript : Photon.MonoBehaviour {

    private bool _selected = false;
    private Vector3 lastPosition;
    private Vector3 mouseDownPosition;
    private static GameObject _hexLinesPrefab = null;
    private GameObject selectionHex;

    public string _spriteName = "";
        
	// Use this for initialization
	void Start () {
	}

    private static GameObject HexLinesPrefab {
        get {
            if ( _hexLinesPrefab == null ) {
                _hexLinesPrefab = Resources.Load<GameObject>( "HexLines" );
            }
            return _hexLinesPrefab;
        }
    }

    public string SpriteName {
        get { return _spriteName;  }
        set {
            if (_spriteName != value) {
                _spriteName = value;
                GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Tiles/" + _spriteName);
            }
        }
    }
	
	void Update () {
        if (CameraPanning.IsDraggingPiece && this.Selected) {
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
            CameraPanning.IsDraggingPiece = true;
        }
    }

    public void OnMouseUp( ) {
        Debug.Log( "OnMouseUp" );
        if (MouseCloseToDownPos) {
            Debug.Log( "Selected" );
            this.Selected = !this.Selected;
        } else if ( CameraPanning.IsDraggingPiece ) {
            this.Selected = false;
            // TODO: Try to refactor away dependency of Map.CurrentMap
            var snappedLocation = Map.CurrentMap.NearestLocation( this.gameObject.transform.position );
            this.gameObject.transform.position = snappedLocation.TableXY( 0 );
        }
        CameraPanning.IsDraggingPiece = false;
    }

    public bool Selected {
        get { return this._selected; }
        set {
            if ( value != _selected ) { 
                _selected = value;
                if ( _selected ) {
                    selectionHex = Instantiate<GameObject>( HexLinesPrefab );
                    selectionHex.transform.Translate( 0, 0, -1 );
                    selectionHex.transform.SetParent( this.transform, false );
                    var photonView = GetComponent<PhotonView>();
                    if (!photonView.isMine) photonView.RequestOwnership();
                } else {
                    Destroy( selectionHex );
                }
                GetComponent<SpriteRenderer>( ).color = _selected ? new Color(1, 1, 1, 0.75f) : Color.white;
                this.gameObject.transform.Translate( 0, 0, _selected ? -1 : 1 ); // Move sepected to front
            }
        }
    }

    public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info ) {
        if (stream.isWriting) {
            stream.SendNext(SpriteName);
        } else {
            this.SpriteName = (string)stream.ReceiveNext();
        }
    }

}
