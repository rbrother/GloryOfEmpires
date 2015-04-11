using UnityEngine;
using System.Collections;

public class SyncSprite : Photon.MonoBehaviour {

    private string _spriteName = "";

    public string SpriteName {
        get { return _spriteName; }
        set {
            if ( _spriteName != value ) {
                _spriteName = value;
                GetComponent<SpriteRenderer>( ).sprite =
                    Resources.Load<Sprite>( _spriteName );
            }
        }
    }

    public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info ) {
        if ( stream.isWriting ) {
            stream.SendNext( SpriteName );
        } else {
            this.SpriteName = (string)stream.ReceiveNext( );
        }
    }

	void Start () {
	}
	
	void Update () {
	}
}
