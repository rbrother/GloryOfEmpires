using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    private static GameObject _hexLinesPrefab = null;
    private GameObject selectionHex = null;
        
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

    void Update( ) {
        if ( GetComponent<Dragging>( ).Selected && selectionHex == null ) {
            selectionHex = Instantiate<GameObject>( HexLinesPrefab );
            selectionHex.transform.Translate( 0, 0, -1 );
            selectionHex.transform.SetParent( this.transform, false );
        } else if ( !GetComponent<Dragging>( ).Selected && selectionHex != null ) {
            Destroy( selectionHex );
            selectionHex = null;
        }
    }

}
