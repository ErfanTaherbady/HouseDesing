using System;
using System.Collections.Generic;
using UnityEngine;

namespace ErfanDeveloper
{
    public class PlacementSystem : MonoBehaviour
    {
        #region Singelton

        public static PlacementSystem instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion
        [Header("Code Setting")] [SerializeField]
        private DataBaseObjectsSO dataBase;
        [SerializeField] private ObjectPlacer objectPlacer;
        [SerializeField] private PerviewSystem preview;
        [SerializeField] private SoundFeedBack soundFeedBack;
        [SerializeField] private InputManager inputManager;

        [Space(5f)] [Header("Grid Setting")]
        public Grid grid;
        [SerializeField] private GameObject gridVisualtion;
        
        
        public GridData floorData ,fournitureData;
        private IBuildingState buildingState;
        private Vector3Int lastDetectedPosition = Vector3Int.zero;

        private void Start()
        {
            StopPlacement();
            gridVisualtion.SetActive(false);
            floorData = new();
            fournitureData = new();
        }

        public void StartPlacement(int ID)
        {
            StopPlacement();
            gridVisualtion.SetActive(true);
            buildingState = new PlacementState(ID, grid, preview, dataBase, floorData,fournitureData, objectPlacer,soundFeedBack,this);
            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }

        public void StartRemoving()
        {
            StopPlacement();
            gridVisualtion.SetActive(true);
            buildingState = new RemovingState(grid, preview, floorData, fournitureData, objectPlacer);
            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }
        public void StopPlacement()
        {
            if(buildingState == null)
                return;
            
            gridVisualtion.SetActive(false);
            buildingState.EndState();
            inputManager.OnClicked -= PlaceStructure;
            inputManager.OnExit -= StopPlacement;
            lastDetectedPosition = Vector3Int.zero;
            buildingState = null;
        }

        private void PlaceStructure()
        {
            if (inputManager.IsPointerOverUI)
            {
                return;
            }
            Vector3 mousePosition = inputManager.GetSelectMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            buildingState.OnAction(gridPosition);
        }

        private void Update()
        {
            if (buildingState == null)
                return;
            Vector3 mousePosition = inputManager.GetSelectMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            if (lastDetectedPosition != gridPosition)
            {
                buildingState.UpdateState(gridPosition);
                lastDetectedPosition = gridPosition;
            }
        }
    }
}