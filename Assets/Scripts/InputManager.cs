using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ErfanDeveloper
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private LayerMask placementLayer;
        
        private Camera mainCamera;
        private Vector3 lastPosition;

        public event Action OnClicked , OnExit;
        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
                OnClicked?.Invoke();
            if(Input.GetKeyDown(KeyCode.Escape))
                OnExit?.Invoke();
        }
        public bool IsPointerOverUI
            => EventSystem.current.IsPointerOverGameObject();
        public Vector3 GetSelectMapPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCamera.nearClipPlane;
            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,100,placementLayer))
            {
                lastPosition = hit.point;
            }
            return lastPosition;
        }
    }
}
