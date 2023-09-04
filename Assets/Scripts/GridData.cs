using System;
using System.Collections.Generic;
using UnityEngine;

namespace ErfanDeveloper
{
    public class GridData
    {
        private Dictionary<Vector3, PlacementData> placedObjects = new();

        public void AddObjectAt(Vector3 gridPosition, Vector2 objectSize, int ID, int placedObjectIndex)
        {
            List<Vector3> positionToOccupy = CalculatePosition(gridPosition, objectSize);
            PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
            foreach (var pos in positionToOccupy)
            {
                if (placedObjects.ContainsKey(pos))
                    throw new Exception($"Dictionary already contains this cell position {pos}");
                placedObjects[pos] = data;
            }
        }

        private List<Vector3> CalculatePosition(Vector3 gridPosition, Vector2 objectSize)
        {
            List<Vector3> returnValo = new();
            for (int x = 0; x < objectSize.x; x++)
            {
                for (int y = 0; y < objectSize.y; y++)
                {
                    returnValo.Add(gridPosition + new Vector3(x, 0, y));
                }
            }

            return returnValo;
        }

        public bool CanPlaceObjectAt(Vector3 gridPosition, Vector2 objectSize)
        {
            List<Vector3> positionToOccupy = CalculatePosition(gridPosition, objectSize);
            foreach (var pos in positionToOccupy)
            {
                if (placedObjects.ContainsKey(pos))
                    return false;
            }
            return true;
        }

        public int GetRepresentationIndex(Vector3Int gridPosition)
        {
            if (placedObjects.ContainsKey(gridPosition) == false)
                return -1;
            return placedObjects[gridPosition].PlacedObjectIndex;
        }

        public void RemoveObjectAt(Vector3Int gridPosition)
        {
            foreach (var pos in placedObjects[gridPosition].occupiedPosition)
            {
                placedObjects.Remove(pos);
            }
        }
    }

    [Serializable]
    public class PlacementData
    {
        public List<Vector3> occupiedPosition;
        public int ID { get; private set; }
        public int PlacedObjectIndex { get; private set; }

        public PlacementData(List<Vector3> occupiedPosition, int ID, int PlacedObjectIndex)
        {
            this.occupiedPosition = occupiedPosition;
            this.ID = ID;
            this.PlacedObjectIndex = PlacedObjectIndex;
        }
    }
}