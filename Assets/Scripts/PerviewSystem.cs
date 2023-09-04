using UnityEngine;

namespace ErfanDeveloper
{
    public class PerviewSystem : MonoBehaviour
    {
        #region Singelton

        public static PerviewSystem instance;

        private void Awake()
        {
            if (instance != null)
                return;
            instance = this;
        }

        #endregion

        [HideInInspector] public int currentDirectionRotationNum;

        [Header("Object Refrense")] [SerializeField]
        private DataBaseObjectsSO dataBase;

        [SerializeField] private Transform parentObjectToInstance;
        [SerializeField] private float prviewYOffSet = 0.06f;
        [SerializeField] private GameObject cellIndictor;
        [SerializeField] private Material perviewMatrialsPrfab;

        private Material perviewMatrialInstance;
        public GameObject prviewObject;
        private ObjectData prviewObjectObjectData;
        private Renderer cellIndicatorRenderer;


        private void Start()
        {
            perviewMatrialInstance = new Material(perviewMatrialsPrfab);
            cellIndictor.gameObject.SetActive(false);
            cellIndicatorRenderer = cellIndictor.GetComponentInChildren<Renderer>();
        }

        private void Update()
        {
            if (prviewObject == null || prviewObjectObjectData == null)
            {
                currentDirectionRotationNum = 0;
                return;
            }

            cellIndictor.SetActive(prviewObjectObjectData == null);
            if (Input.GetKeyDown(KeyCode.E) && prviewObjectObjectData.objectId != 0)
            {
                //Wall ObjectId
                RotateObject();
            }
        }

        private void RotateObject()
        {
            ChangePrviewObjectRotate();
        }

        private void ChangePrviewObjectRotate()
        {
            currentDirectionRotationNum++;

            if (currentDirectionRotationNum > prviewObjectObjectData.objectPositionInDifrentRoattion.Length - 1)
                currentDirectionRotationNum = 0;
            prviewObject.transform.GetChild(0).transform.Rotate(Vector3.up, 90);
            
            prviewObject.transform.GetChild(0).localPosition =
                new Vector3(prviewObjectObjectData.objectPositionInDifrentRoattion[currentDirectionRotationNum].x,
                    prviewObject.transform.GetChild(0).transform.localPosition.y,
                    prviewObjectObjectData.objectPositionInDifrentRoattion[currentDirectionRotationNum].y);
            ChangeSettingForPrviewObjectRotated(currentDirectionRotationNum);
        }

        private void ChangeSettingForPrviewObjectRotated(int currentRotate)
        {
            dataBase.objectsData[prviewObjectObjectData.objectId].Size =
                prviewObjectObjectData.objectGridSizeInDifrentRotation[currentRotate];
        }

        public void StartShowingPlacementPerview(GameObject prfab, Vector2 size)
        {
            prviewObject = Instantiate(prfab, parentObjectToInstance);
            prviewObjectObjectData = prviewObject.GetComponent<ObjectData>();
            if (prviewObjectObjectData != null)
                ChangeSettingForPrviewObjectRotated(0);


            if (prviewObjectObjectData != null && GameManager.instance.lastObject != null)
            {
                prviewObject.transform.GetChild(0).localEulerAngles =
                    GameManager.instance.lastObject.transform.GetChild(0).localEulerAngles;
                prviewObject.transform.GetChild(0).localPosition =
                    GameManager.instance.lastObject.transform.GetChild(0).localPosition;
            }

            PreparePreview();
            PrepareCursor(size);
            cellIndictor.SetActive(true);
        }

        private void PrepareCursor(Vector2 size)
        {
            if (size.x > 0 || size.y > 0)
            {
                //Edited
                cellIndictor.transform.localScale = new Vector3(size.x / 10, 1, size.y / 10);
                cellIndicatorRenderer.material.mainTextureScale = size * 10;
            }
        }

        private void PreparePreview()
        {
            Renderer[] renderers = prviewObject.transform.GetChild(0).GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = perviewMatrialInstance;
                }

                renderer.materials = materials;
            }
        }

        public void StopShowingPreview()
        {
            cellIndictor.SetActive(false);
            if (prviewObject != null)
                Destroy(prviewObject);
        }

        public void UpdatePosition(Vector3 position, bool validity)
        {
            if (prviewObject != null)
            {
                MovePreview(position);
                ApplayFeedbackToPreview(validity);
            }

            MoveCursor(position);
            ApplayFeedbackToCursor(validity);
        }

        private void ApplayFeedbackToPreview(bool validity)
        {
            Color c = validity ? Color.white : Color.red;

            perviewMatrialInstance.color = c;
        }

        private void ApplayFeedbackToCursor(bool validity)
        {
            Color c = validity ? Color.white : Color.red;
            cellIndicatorRenderer.material.color = c;
            c.a = 0.5f;
            perviewMatrialInstance.color = c;
        }

        private void MoveCursor(Vector3 position)
        {
            cellIndictor.transform.position = position;
        }

        private void MovePreview(Vector3 position)
        {
            prviewObject.transform.position = new Vector3(position.x, position.y + prviewYOffSet, position.z);
        }

        internal void StartShowingPlacementRemovePerview()
        {
            cellIndictor.SetActive(true);
            PrepareCursor(Vector2Int.one);
            ApplayFeedbackToCursor(false);
        }
    }
}