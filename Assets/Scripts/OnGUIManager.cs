using System.Collections;
using System.Collections.Generic;
using System.IO;
using ErfanDeveloper;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnGUIManager : MonoBehaviour
{
    #region Singelton

    public static OnGUIManager instance;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    #endregion

    [SerializeField] private GameObject TreeDCamera, TwoDCamera;
    [SerializeField] private GameObject[] examopels;
    [SerializeField] private GameObject[] buldingPanels;
    [SerializeField] private GameObject[] selectTexurePanel;
    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject editPanel;


    private GameObject currentWall;
    private bool isTreeDCameraActive = true;
    private bool isSelecteTexurePanel;

    public void TakeScreenShot()
    {
        StartCoroutine(ScreenShot());
    }

    private IEnumerator ScreenShot()
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;

        Texture2D screenshotTexure = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexure.ReadPixels(rect, 0, 0);
        screenshotTexure.Apply();

        byte[] byteArray = screenshotTexure.EncodeToPNG();

        File.WriteAllBytes(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Map.png",
            byteArray);
    }

    public void EnabelAndOnEnabelEditMode()
    {
        Animator editPanelAnim = editPanel.GetComponent<Animator>();
        editPanelAnim.SetBool("IsOpened", !editPanelAnim.GetBool("IsOpened"));
    }

    public void ChangeCamera(bool isTreeDCamera)
    {
        if (isTreeDCamera)
        {
            TreeDCamera.SetActive(true);
            TwoDCamera.SetActive(false);
        }
        else
        {
            TwoDCamera.SetActive(true);
            TreeDCamera.SetActive(false);
        }
    }

    public void CreatedWall(GameObject wall)
    {
        Debug.Log("MatrialMode");
        currentWall = wall;
        isSelecteTexurePanel = true;
        SelectTexurePanel();
    }

    private void SelectTexurePanel()
    {
        for (int i = 0; i < selectTexurePanel.Length; i++)
        {
            selectTexurePanel[i].SetActive(true);
        }

        selectTexurePanel[0].GetComponent<Animator>().SetTrigger("BuildingPanel");
        for (int i = 0; i < buldingPanels.Length; i++)
        {
            buldingPanels[i].SetActive(false);
        }
    }

    public void ExitTexurePanel()
    {
        isSelecteTexurePanel = false;
        ChangeBuldingPanel(0);
    }

    public void SelectMetrial(int num)
    {
        if (currentWall == null)
            return;
        currentWall.GetComponent<MeshRenderer>().material = materials[num];
    }

    public void ChangeBuldingPanel(int num)
    {
        if (isSelecteTexurePanel)
            return;
        for (int i = 0; i < buldingPanels.Length; i++)
        {
            if (num == i)
            {
                buldingPanels[i].SetActive(true);
                buldingPanels[i].GetComponent<Animator>().SetTrigger("BuildingPanel");
            }
            else
                buldingPanels[i].SetActive(false);
        }
    }

    public void ChangeScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void ChangeExampel(int index)
    {
        for (int i = 0; i < examopels.Length; i++)
        {
            if (i == index)
                examopels[i].SetActive(true);
            else
                examopels[i].SetActive(false);
        }
    }
}