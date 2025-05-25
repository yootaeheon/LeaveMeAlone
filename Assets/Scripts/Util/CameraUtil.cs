using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class CameraUtil
{
    /// <summary>
    /// 카메라 어두워지는 FadeOut 기능
    /// 카메라 어두워졌다 밝아지는 FadeIn 기능
    /// </summary>
    public static void CameraFadeIn()
    {
        Camera cam = Camera.main;

        cam.GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
