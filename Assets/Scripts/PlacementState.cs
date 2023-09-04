using UnityEngine;

namespace ErfanDeveloper
{
    public class PlacementState : IBuildingState
    {
        private int selectedObjectIndex = -1;
        private int ID;
        private Grid grid;
        private PerviewSystem previewSystem;
        private PlacementSystem placementSystem;
        private DataBaseObjectsSO database;
        private GridData floorData;
        private GridData furnitureData;
        private ObjectPlacer objectPlacer;
        private SoundFeedBack soundFeedBack;

        public PlacementState(int ID,
            Grid grid,
            PerviewSystem previewSystem,
            DataBaseObjectsSO database,
            GridData floorData,
            GridData furnitureData,
            ObjectPlacer objectPlacer,
            SoundFeedBack soundFeedBack,
            PlacementSystem placementSystem)
        {
            this.ID = ID;
            this.grid = grid;
            this.previewSystem = previewSystem;
            this.database = database;
            this.floorData = floorData;
            this.furnitureData = furnitureData;
            this.objectPlacer = objectPlacer;
            this.soundFeedBack = soundFeedBack;
            this.placementSystem = placementSystem;
            selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
            if (selectedObjectIndex > -1)
            {
                previewSystem.StartShowingPlacementPerview(database.objectsData[selectedObjectIndex].Perfab,
                    database.objectsData[selectedObjectIndex].Size);
            }
            else
                throw new System.Exception($"No objuect with ID{ID}");
        }

        public void EndState()
        {
            previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            if (!placementValidity)
            {
                soundFeedBack.PlaySound(SoundType.WrongPlacement);
                return;
            }

            int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Perfab,
                grid.CellToWorld(gridPosition));
            int objectID = database.objectsData[selectedObjectIndex].ID;
            GridData selectData = objectID == 0 ? floorData : furnitureData;


            selectData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size,
                database.objectsData[selectedObjectIndex].ID,
                index);


            if (objectID >= 4)
            {
                if (GameManager.instance.isEditMod)
                {
                    placementSystem.StopPlacement();
                    GameManager.instance.SetBool(false);
                    //Out The Edit Movement Mode
                }
            }

            soundFeedBack.PlaySound(SoundType.CreateSmallObject);
            if (objectID >= 2 && objectID < 4)
                placementSystem.StopPlacement();


            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
        }

        private bool CheckPlacementValidity(Vector3Int gridPosition, int objectIndex)
        {
            GridData selectData = database.objectsData[objectIndex].ID == 0 ? floorData : furnitureData;
            bool hasObjectCreatedHere =
                selectData.CanPlaceObjectAt(gridPosition, database.objectsData[objectIndex].Size);

            return hasObjectCreatedHere;
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
        }
    }
}