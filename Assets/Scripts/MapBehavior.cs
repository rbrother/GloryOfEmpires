using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapBehavior : MonoBehaviour {

    private Sprite[] tiles;

    private GameObject map;

    private const float TILE_RADIUS = 2.17f; // distance from center to any corner

    private readonly float TILE_HEIGHT = 2 * TILE_RADIUS * Mathf.Cos( Mathf.PI / 6 );

    private int AREA_SIZE = 5;

    // Use this for initialization
    void Start( ) {
        this.map = this.gameObject;
        this.tiles = Resources.LoadAll<Sprite>( "Tiles" );
        var preFab = Resources.Load<GameObject>( "TilePrefab" );
        for ( int logicalX = -AREA_SIZE; logicalX <= AREA_SIZE; logicalX++ ) {
            for ( int logicalY = -AREA_SIZE; logicalY <= AREA_SIZE; logicalY++ ) {
                if ( LogicalDistance( logicalX, logicalY ) <= AREA_SIZE ) { 
                    var sprite = tiles[ Random.Range( 0, tiles.Length ) ];
                    var newTile = Instantiate( preFab, TileXYFromLogicalCoords( logicalX, logicalY ), 
                                    Quaternion.identity ) as GameObject;
                    newTile.GetComponent<SpriteRenderer>( ).sprite = sprite;
                }
            }
        }
    }

    // Update is called once per frame
    void Update( ) {

    }

    Vector3 TileXYFromLogicalCoords( int logicalX, int logicalY ) {
        // step in logicalX: ( radius * 1.5, height * 0.5 )
        // step in logicalY: ( 0, height )
        return new Vector3( 
            logicalX * TILE_RADIUS * 1.5f, 
            ( logicalX * 0.5f + logicalY ) * TILE_HEIGHT, 
            0 );
    }

    /// <summary>
    /// Returns number of steps needed to get from (0,0) to logicalX, logicalY
    /// </summary>
    /// <param name="logicalX"></param>
    /// <param name="logicalY"></param>
    /// <returns></returns>
    int LogicalDistance( int logicalX, int logicalY ) {
        var absX = System.Math.Abs( logicalX );
        var absY = System.Math.Abs( logicalY );
        return
            logicalX == 0 ? absY :
            logicalY == 0 ? absX :
            logicalX * logicalY > 0 ? absX + absY : // same sign
            System.Math.Max( absX, absY );
    }
}
