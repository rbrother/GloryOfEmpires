using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    private static GameObject _hexLinesPrefab = null;
    private GameObject selectionHex;
        
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

    /*
                    selectionHex = Instantiate<GameObject>( HexLinesPrefab );
                    selectionHex.transform.Translate( 0, 0, -1 );
                    selectionHex.transform.SetParent( this.transform, false );
     * 
     *                     Destroy( selectionHex );

    */

    void Update () {
    }

}
