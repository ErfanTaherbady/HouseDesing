using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowSizeObject : MonoBehaviour
{
    [SerializeField] private float x_Size, z_Size;
    [SerializeField] private TextMeshPro x_Axis_Text, z_Axis_Text;
    private void Start()
    {
        UpdateUi();
    }
    public void UpdateUi()
    {
        x_Axis_Text.text = x_Size.ToString();
        z_Axis_Text.text = z_Size.ToString();
      
    }
    public void DisabelOurEnavelUi()
    {
        bool isActive = x_Axis_Text.gameObject.activeSelf;
        x_Axis_Text.gameObject.SetActive(!isActive);
        z_Axis_Text.gameObject.SetActive(!isActive);
    }
}
