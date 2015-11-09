using UnityEngine;
using System.Collections;

public class ShipCountManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddSelectedShipCount( ) {
        if ( Dragging.SelectedShip != null ) { 
            Dragging.SelectedShip.GetComponent<ShipCount>().IncreaseShipCount();
        }
    }

    public void ReduceSelectedShipCount( ) {
        if ( Dragging.SelectedShip != null ) {
            Dragging.SelectedShip.GetComponent<ShipCount>( ).DecreaseShipCount( );
        }
    }

}
