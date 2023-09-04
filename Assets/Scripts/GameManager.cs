using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singelton

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    #endregion

    public bool isEditMod;
    public GameObject lastObject;

    public void SetBool(bool editMode)
    {
        isEditMod = editMode;
    }
    public void SetValue(GameObject newObejct)
    {
        lastObject = newObejct;
    }
}