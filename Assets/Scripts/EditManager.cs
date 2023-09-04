using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ErfanDeveloper
{
    public class EditManager : MonoBehaviour
    {
        [HideInInspector] public bool isEditingOneOfWall;

        [SerializeField] private bool isEditMovementMode;
        [SerializeField] private Camera camera;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private Grid grid;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(GetMouseRay(), out hit, 100, wallLayer) && !isEditingOneOfWall)
                {
                    
                    int objectId = hit.transform.parent.GetComponent<ObjectData>().objectId;
                    Debug.Log("Clicked");
                    if (isEditMovementMode)
                    {
                        Debug.Log("Clicked1");
                        Edit(hit.transform.parent.transform,objectId);
                        Debug.Log("Clicked2");
                    }
                    else if (!isEditMovementMode && objectId != 5)
                       OnGUIManager.instance.CreatedWall(hit.transform.gameObject);
                }
            }
        }

        private void Edit(Transform objectTransform, int objectId)
        {
            Debug.Log("Clicked3");
            if (GameManager.instance.lastObject != null)
                return;
            Debug.Log("Clicked4");
            GameManager.instance.SetBool(true);
            isEditingOneOfWall = true;
            GameManager.instance.SetValue(objectTransform.gameObject);
            Vector3Int gridPosition = grid.WorldToCell(objectTransform.position);
            PlacementSystem.instance.fournitureData.RemoveObjectAt(gridPosition);
            PlacementSystem.instance.StartPlacement(objectId);
            objectTransform.gameObject.SetActive(false);
            Debug.Log("Clicked5");
        }

        private Ray GetMouseRay()
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePos);
            return ray;
        }
    }
}