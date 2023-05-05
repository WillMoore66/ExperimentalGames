using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class videoPlayer : MonoBehaviour
{
    void Start()
    {
        GetComponent<VideoPlayer>().targetTexture = GetComponent<RawImage>().texture as RenderTexture;
    }
}
