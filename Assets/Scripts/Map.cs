﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;

namespace Net.Brotherus {

    public struct MapLocation {
        public const float TILE_RADIUS = 2.16f; // distance from center to any corner in unity-units
        public static readonly float TILE_HEIGHT = 2 * TILE_RADIUS * Mathf.Cos( Mathf.PI / 6 );

        public readonly int LogicalX;
        public readonly int LogicalY;
        public readonly string LocationName;

        public float TableX { get { return LogicalToTableX( LogicalX, LogicalY ); } }
        public float TableY { get { return LogicalToTableY( LogicalX, LogicalY ); } }

        public static float LogicalToTableX( int logicalX, int logicalY ) { return logicalX * TILE_RADIUS * 1.5f; }
        public static float LogicalToTableY( int logicalX, int logicalY ) { return ( logicalX * 0.5f + logicalY ) * TILE_HEIGHT; }

        public static bool IsOnTable( int logicalX, int logicalY, int tableWidth, int tableHeight ) {
            float mapX = LogicalToTableX( logicalX, logicalY );
            float mapY = LogicalToTableY( logicalX, logicalY );
            return Mathf.Abs( mapX ) < TILE_RADIUS * 1.5f * tableWidth + 0.01 &&
                                Mathf.Abs( mapY ) < TILE_HEIGHT * tableHeight + 0.01;
        }

        public Vector3 TableXY( float z ) { return new Vector3( TableX, TableY, z ); }

        public MapLocation( int logicalX, int logicalY, int minLogicalX, int minLogicalY ) {
            this.LogicalX = logicalX;
            this.LogicalY = logicalY;
            var valueX = LogicalX - minLogicalX;
            var valueY = LogicalY - minLogicalY + 1;
            this.LocationName = string.Format( "{0}{1}", Letters[valueX], valueY );
        }

        // Lettersd without some problematic cases like I and O
        private static readonly string[] Letters = { "A", "B", "C", "D", "F", "G",
                                         "H", "J", "K", "L", "M", "N", "P",
                                         "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    } // struct

    public struct Map {
        private static Map currentMap;
        private MapLocation[] locations;
        private int width, height;

        public static Map CurrentMap { get { return currentMap; } }

        public MapLocation[] Locations { get { return this.locations; } }

        public Map( int width, int height ) {
            this.width = width;
            this.height = height;
            var areaSize = width + height;
            var rawLocations =
                Enumerable.Range( -areaSize, areaSize * 2 ).SelectMany(
                    x => Enumerable.Range( -areaSize, areaSize * 2 ).Select(
                        y => new { logicalX = x, logicalY = y } ) ).
                            Where( loc => MapLocation.IsOnTable( loc.logicalX, loc.logicalY, width, height ) );
            var minX = rawLocations.Min( loc => loc.logicalX );
            var minY = rawLocations.Min( loc => loc.logicalY );
            this.locations = rawLocations.Select( loc => new MapLocation( loc.logicalX, loc.logicalY, minX, minY ) ).ToArray( );
            currentMap = this;
        }

        /// <summary>
        /// Nearest map location to given point
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="snapSteps">Subdivide the grid to finer steps with 2,3,4,...</param>
        /// <returns></returns>
        public Vector3 NearestLocation( Vector3 pos, int snapSteps = 1 ) {
            Debug.Log( "Finding nearest to: " + pos.ToString( ) + "   locs: " + locations.Length.ToString());
            var areaSize = (this.width + this.height) * snapSteps;
            var snapPositions = Enumerable.Range( -areaSize, areaSize * 2 ).SelectMany(
                x => Enumerable.Range( -areaSize, areaSize * 2 ).Select(
                    y => new Vector3(
                        MapLocation.LogicalToTableX( x, y ) / snapSteps,
                        MapLocation.LogicalToTableY( x, y ) / snapSteps ) ) );
            return snapPositions.MinBy( loc => Vector3.Distance( loc, pos ) );
        }

    } // struct

} // namespace
