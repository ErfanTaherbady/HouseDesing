using UnityEngine;

namespace ErfanDeveloper
{
    public class RemovingState : IBuildingState
    {
        private int gameObjectIndex = -1;
        private Grid grid;
        private PerviewSystem previewSystem;
        private GridData floorData;
        private GridData furnitureData;
        private ObjectPlacer objectPlacer;

        public RemovingState(Grid grid, PerviewSystem previewSystem, GridData floorData,GridData furnitureData, ObjectPlacer objectPlacer)
        {
            this.grid = grid;
            this.previewSystem = previewSystem;
            this.floorData = floorData;
            this.furnitureData = furnitureData;
            this.objectPlacer = objectPlacer;
            
            previewSystem.StartShowingPlacementRemovePerview();
        }

        public void EndState()
        {
            previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            GridData selectedData = null;
            if (furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
            {
                selectedData = furnitureData;
            }
            else if (floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
            {
                selectedData = floorData;
            }

            if (selectedData == null)
            {
                //sound 
            }
            else
            {
                gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
                if(gameObjectIndex == -1)
                    return;
                selectedData.RemoveObjectAt(gridPosition);
                objectPlacer.RemoveObjectAt(gameObjectIndex);
            }

            Vector3 cellPosition = grid.CellToWorld(gridPosition);
            previewSystem.UpdatePosition(cellPosition,CheckIfSeletionIsValid(gridPosition));
        }

        private bool CheckIfSeletionIsValid(Vector3Int gridPosition)
        {
            return !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
                     (floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one)));
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool validity = CheckIfSeletionIsValid(gridPosition);
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition),validity);
        }
    }
}