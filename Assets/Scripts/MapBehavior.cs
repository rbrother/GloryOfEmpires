using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class MapConstants {
    public const float TILE_RADIUS = 2.16f; // distance from center to any corner in unity-units
    public static readonly float TILE_HEIGHT = 2 * TILE_RADIUS * Mathf.Cos( Mathf.PI / 6 );
    public const int GALAXY_SIZE = 4;
}

public struct MapLocation {
    public int LogicalX;
    public int LogicalY;

    public float TableX { get { return LogicalX * MapConstants.TILE_RADIUS * 1.5f; } }
    public float TableY { get { return ( LogicalX * 0.5f + LogicalY ) * MapConstants.TILE_HEIGHT; } }

    public Vector3 TableXY { get { return new Vector3( TableX, TableY, 0 ); } }

    public bool IsOnTable(int tableWidth, int tableHeight) {
        return Mathf.Abs( TableXY.x ) < MapConstants.TILE_RADIUS * 1.5f * tableWidth + 0.01 &&
                            Mathf.Abs( TableXY.y ) < MapConstants.TILE_HEIGHT * tableHeight + 0.01;
    }

    public string LocationName( int minLogicalX, int minLogicalY ) {
        var valueX = LogicalX - minLogicalX;
        var valueY = LogicalY - minLogicalY + 1;
        return string.Format( "{0}{1}", Letters[valueX], valueY );
    }

    // Lettersd without some problematic cases like I and O
    private static readonly string[] Letters = { "A", "B", "C", "D", "F", "G",
                                         "H", "J", "K", "L", "M", "N", "P",
                                         "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
}

public class MapBehavior : MonoBehaviour {

    private Sprite[] tiles;

    private GameObject Map { get { return this.gameObject; } }

    public const int TABLE_WIDTH = 7;
    public const int TABLE_HEIGHT = 5;

    void Start( ) {
        this.tiles = Resources.LoadAll<Sprite>( "Tiles" );
        var preFab = Resources.Load<GameObject>( "TilePrefab" );

        var locs = TableLocations( TABLE_WIDTH, TABLE_HEIGHT );
        var minLogicalX = locs.Min( loc => loc.LogicalX );
        var minLogicalY = locs.Min( loc => loc.LogicalY );
        foreach ( var loc in locs ) {
            var sprite = tiles[Random.Range( 0, tiles.Length )];
            var newTile = Instantiate( preFab, loc.TableXY, Quaternion.identity ) as GameObject;
            newTile.transform.parent = Map.transform; // This makes the object child of the map!
            if ( LogicalDistance( loc.LogicalX, loc.LogicalY ) <= MapConstants.GALAXY_SIZE ) {
                newTile.GetComponent<SpriteRenderer>( ).sprite = sprite;
            }
            var textPrefab = Resources.Load<GameObject>( "TextPrefab" );
            var text = Instantiate( textPrefab, new Vector3( loc.TableX, loc.TableY, -0.1f ), Quaternion.identity ) as GameObject;
            text.GetComponent<TextMesh>( ).text = loc.LocationName( minLogicalX, minLogicalY );
            // text.transform.parent = newTile.transform; // we would like to specify parent and position relative to parent, but trouble working
        }

    }

    private IEnumerable<MapLocation> TableLocations( int tableWidth, int tableHeight ) {
        var areaSize = tableWidth + tableHeight;
        return
            Enumerable.Range( -areaSize, areaSize * 2 ).SelectMany(
                x => Enumerable.Range( -areaSize, areaSize * 2 ).Select(
                    y => new MapLocation { LogicalX = x, LogicalY = y } ) ).
                        Where( loc => loc.IsOnTable( tableWidth, tableHeight ) );
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
