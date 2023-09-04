using System;
using UnityEngine;

namespace ErfanDeveloper
{
    public class CameraController : MonoBehaviour
    {
        [Header("Rotate Setting")]
        [SerializeField] private Transform center;
        [SerializeField] private float yawSpeed = 100f;
        private float currentInput;

        [Space(5f)]
        
        [Header("Zoom Setting")]
        [SerializeField] private float zoomSpeed = 4f;
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 15f;
        private Camera mainCamera;
        private float currentZoom = 10f;

        private void Start()
        {
            currentZoom = 25;
            mainCamera = GetComponent<Camera>();
        }

        private void Update()
        {
            currentInput = -Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            mainCamera.fieldOfView = currentZoom;
        }

        private void LateUpdate()
        {
            transform.RotateAround(center.position,Vector3.up ,currentInput);
        }
    }
}