using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Net.Brotherus;

public class MapBehavior : MonoBehaviour {

    private Sprite[] tiles;

    private GameObject Map { get { return this.gameObject; } }

    public const int GALAXY_SIZE = 4;

    private string[] setupTileNames = new string[] { "Red", "Yellow", 
        "LightBlue", "MediumBlue", "DarkBlue" };

    void Start( ) {
        var map = new Map( width: 4, height: 4 ); // Sets singleton
        var backgroundPreFab = Resources.Load<GameObject>( "BackgroundPrefab" );
        var preFab = Resources.Load<GameObject>( "TilePrefab" );

        foreach ( var loc in map.Locations ) {
            var backgroundTile = Instantiate( backgroundPreFab, loc.TableXY( 1 ), Quaternion.identity ) as GameObject;
            backgroundTile.transform.SetParent(Map.transform, false); // This makes the object child of the map!
            var textPrefab = Resources.Load<GameObject>( "TextPrefab" );
            var text = Instantiate( textPrefab, new Vector3( 0, 0, -3 ), Quaternion.identity ) as GameObject;
            text.GetComponent<TextMesh>( ).text = loc.LocationName;
            text.transform.SetParent(backgroundTile.transform, false); // Text to be child of tile.
            var dist = LogicalDistance( loc.LogicalX, loc.LogicalY );
            if ( dist <= GALAXY_SIZE ) {
                backgroundTile.GetComponent<SpriteRenderer>( ).sprite =
                    Resources.Load<Sprite>( "SetupTiles/Tile-Setup-" + setupTileNames[dist] );
            }
        }

        // Make "palette" of tiles to the left-hand side, draggable
        var x = 4;
        foreach ( var tileset in new string[] { "1planet", "2planet", "Special" } ) {
            x++;
            this.tiles = Resources.LoadAll<Sprite>( "Tiles/" + tileset );
            for ( int i = 0; i < tiles.Length; ++i ) {
                var sprite = tiles[i];
                var planetTile = Instantiate( preFab, new Vector3( -x * MapLocation.TILE_RADIUS * 2, (i-7) * MapLocation.TILE_HEIGHT, 0 ), Quaternion.identity ) as GameObject;
                planetTile.GetComponent<SpriteRenderer>( ).sprite = sprite;
            }
        }

        PhotonNetwork.ConnectUsingSettings( "v0.1" );
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
