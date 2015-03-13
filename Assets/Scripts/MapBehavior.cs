using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Net.Brotherus;

public class MapBehavior : MonoBehaviour {

    private Sprite[] tiles;

    private GameObject Map { get { return this.gameObject; } }

    public const int GALAXY_SIZE = 4;

    void Start( ) {
        var map = new Map( width: 7, height: 5 ); // Sets singleton
        this.tiles = Resources.LoadAll<Sprite>( "Tiles" );
        var backgroundPreFab = Resources.Load<GameObject>( "BackgroundPrefab" );
        var preFab = Resources.Load<GameObject>( "TilePrefab" );

        foreach ( var loc in map.Locations ) {
            var sprite = tiles[Random.Range( 0, tiles.Length )];
            var backgroundTile = Instantiate( backgroundPreFab, loc.TableXY( 1 ), Quaternion.identity ) as GameObject;
            backgroundTile.transform.SetParent(Map.transform, false); // This makes the object child of the map!
            var textPrefab = Resources.Load<GameObject>( "TextPrefab" );
            var text = Instantiate( textPrefab, new Vector3( 0, 0, -3 ), Quaternion.identity ) as GameObject;
            text.GetComponent<TextMesh>( ).text = loc.LocationName;
            text.transform.SetParent(backgroundTile.transform, false); // Text to be child of tile.
            if ( LogicalDistance( loc.LogicalX, loc.LogicalY ) <= GALAXY_SIZE ) {
                var planetTile = Instantiate( preFab, loc.TableXY(0), Quaternion.identity ) as GameObject;
                planetTile.GetComponent<SpriteRenderer>( ).sprite = sprite;
            }
        }
    }

    void Update( ) {
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
