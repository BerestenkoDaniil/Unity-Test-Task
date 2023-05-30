using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLaunch : MonoBehaviour
{
    private bool camAvaible;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    // Start is called before the first frame update
    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] device = WebCamTexture.devices;

        if (device.Length == 0)
        {
            Debug.Log("No Camera");
            camAvaible = false;
            return;
        }

        for (int i = 0; i < device.Length; i++)
        {
            if (!device[i].isFrontFacing)
            {
                backCam = new WebCamTexture(device[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            Debug.Log("Unable to find Camera");
            return;
        }
        backCam.Play();
        background.texture = backCam;

        camAvaible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!camAvaible)
            return;

        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
}
