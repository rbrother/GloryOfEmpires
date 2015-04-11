using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Net.Brotherus;

public class MapBehavior : MonoBehaviour {

    private GameObject Map { get { return this.gameObject; } }

    public const int GALAXY_SIZE = 4;

    private string[] setupTileNames = new string[] { "Red", "Yellow", 
        "LightBlue", "MediumBlue", "DarkBlue" };

    void Start( ) {
        var map = new Map( width: 5, height: 4 ); // Sets singleton
        var backgroundPreFab = Resources.Load<GameObject>( "BackgroundPrefab" );

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

        PhotonNetwork.ConnectUsingSettings( "v0.1" );
    }

    void Update( ) {
    }

    void OnJoinedLobby() {
        Debug.Log("Photon OnJoinedLobby");
        var options = new RoomOptions();
        PhotonNetwork.JoinOrCreateRoom("Sandbox", options, TypedLobby.Default);
    }

    void OnCreatedRoom() {
        Debug.Log("OnCreatedRoom");
        CreateTilePalette( );
        CreateShipPalette( );
    }

    private static void CreateTilePalette( ) {
        // Make "palette" of tiles to the left-hand side, draggable.
        // These are Photon multiplayer-objects
        var allTiles = new[] { "1planet", "2planet", "Special" }.
            SelectMany( set => Resources.LoadAll<Sprite>( "Tiles/" + set ).
                Select( sprite => new { Set = set, Sprite = sprite } ) );
        var x = -MapLocation.TILE_RADIUS * 10.0f;
        var startY = -MapLocation.TILE_RADIUS * 8.0f;
        var y = startY;
        foreach ( var tile in allTiles ) {
            var planetTile = PhotonNetwork.Instantiate( "TilePrefab",
                new Vector3( x, y, 0 ), Quaternion.identity, group: 0 ) as GameObject;
            planetTile.GetComponent<SyncSprite>( ).SpriteName = "Tiles/" + tile.Set + "/" + tile.Sprite.name;
            y += MapLocation.TILE_RADIUS * 2;
            if ( y > MapLocation.TILE_RADIUS * 8.0f ) {
                y = startY;
                x -= MapLocation.TILE_RADIUS * 2;
            }
        }
    }

    private static void CreateShipPalette( ) {
        var shipColors = new[] { "Green", "Iron", "Orange", "Pink", "Red", "SkyBlue" };
        var plasticsPieces = new[] { 
            new { Name = "GF", Count = 12 },
            new { Name = "MU", Count = 4 },
            new { Name = "Fighter", Count = 10 },
            new { Name = "Destroyer", Count = 8 },
            new { Name = "Cruiser", Count = 8 },
            new { Name = "Carrier", Count = 4 },
            new { Name = "Dreadnaught", Count = 5 },
            new { Name = "Warsun", Count = 2 },
            new { Name = "Flagship", Count = 1 },
            new { Name = "SpaceDock", Count = 3 }
        };
        var startX = MapLocation.TILE_RADIUS * 9.0f;
        var y = -10.0f;
        foreach ( var color in shipColors ) {
            var x = startX;
            foreach ( var unit in plasticsPieces ) {
                for ( int n = 1; n <= unit.Count; n++ ) {
                    var ship = PhotonNetwork.Instantiate( "ShipPrefab",
                        new Vector3( x, y, -3 ),
                        Quaternion.identity, group: 0 ) as GameObject;
                    ship.GetComponent<SyncSprite>( ).SpriteName = 
                        string.Format("Ships/{0}/Unit-{0}-{1}", color, unit.Name);
                    x += 0.3f;
                }
                x += 1.0f;
                if ( unit.Name == "Destroyer" ) {
                    x = startX;
                    y += 1.5f;
                }
            }
            y += 2.5f;
        }
    }

    void OnJoinedRoom()  {
        Debug.Log("Joined Room: " + PhotonNetwork.room.ToStringFull());
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
