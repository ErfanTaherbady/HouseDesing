using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ErfanDeveloper
{
    public class ObjectPlacer : MonoBehaviour
    {
        public static ObjectPlacer instance;
        private void Awake()
        {
            if(instance != null)
                return;
            instance = this;
        }

        [SerializeField] private EditManager editPositionWallObject;
        [SerializeField] private SoundFeedBack soundFeedBack;

        [Space(5f)] [Header("Object Setting")] [SerializeField]
        private Transform parentObjects;

        [HideInInspector] public List<GameObject> placedGameObjects = new();

        public int PlaceObject(GameObject prfab, Vector3 position)
        {
            GameObject newObject = Instantiate(prfab, parentObjects);

            PerviewSystem.instance.currentDirectionRotationNum = 0;

            ObjectData data = newObject.GetComponent<ObjectData>();

            if (data == null)
            {
                newObject.GetComponent<ShowSizeObject>().DisabelOurEnavelUi();
            }

            if (data != null)
            {
                //OnGUIManager.instance.CreatedWall(newObject); Show Panel Select Matrial After Craete Wall 

                if (GameManager.instance.lastObject != null)
                {
                    newObject.transform.GetChild(0).GetComponent<MeshRenderer>().material =
                        GameManager.instance.lastObject.transform.GetChild(0).GetComponent<MeshRenderer>().material;
                    Destroy(GameManager.instance.lastObject.gameObject);
                }


                if (editPositionWallObject.isEditingOneOfWall)
                {
                    editPositionWallObject.isEditingOneOfWall = false;
                }
            }

            newObject.transform.GetChild(0).localPosition =
                PerviewSystem.instance.prviewObject.transform.GetChild(0).localPosition;

            newObject.transform.GetChild(0).localEulerAngles =
                PerviewSystem.instance.prviewObject.transform.GetChild(0).localEulerAngles;

            newObject.transform.position = position;
            placedGameObjects.Add(newObject);

            return placedGameObjects.Count - 1;
        }

        public void RemoveObjectAt(int gameObjectIndex)
        {
            if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
                return;
            soundFeedBack.PlaySound(SoundType.RemoveObject);
            Destroy(placedGameObjects[gameObjectIndex]);
            placedGameObjects[gameObjectIndex] = null;
        }
    }
}