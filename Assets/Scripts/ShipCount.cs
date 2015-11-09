using UnityEngine;
using System.Collections;

public class ShipCount : MonoBehaviour {

    private int _count = 1;
    private GameObject _countText = null;

    public int Count {
        get { return _count; }
        set {
            var originalCount = _count;
            _count = value;
            Debug.Log( this.gameObject.name + " count = " + _count.ToString( ) );
            if ( originalCount == 1 && _count == 2 ) {
                var textPrefab = Resources.Load<GameObject>( "TextPrefab" );
                _countText = Instantiate( textPrefab, new Vector3( 0.5f, 0, -5 ), Quaternion.identity ) as GameObject;
                _countText.GetComponent<TextMesh>( ).fontSize = 100;
                _countText.GetComponent<TextMesh>( ).fontStyle = FontStyle.Bold;
                _countText.transform.SetParent( this.transform, false );
                UpdateText( );
            } else if ( originalCount == 2 && _count == 1 ) {
                Destroy( _countText );
                _countText = null;
            } else {
                UpdateText( );
            }
        }
    }

    private void UpdateText( ) {
        _countText.GetComponent<TextMesh>( ).text = _count.ToString( );
    }

    public void IncreaseShipCount( ) { Count++; }

    public void DecreaseShipCount( ) { if ( Count > 1 ) Count--; }
    
    void Start( ) {
	
	}

    void Update () {
	
	}
}
