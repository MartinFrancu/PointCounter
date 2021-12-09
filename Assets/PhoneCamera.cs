using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhoneCamera : MonoBehaviour
{
    const int bufferSize = 20;
    private int currentTimeIndex = 0;
    private Texture2D[] pictureBuffer;
    private int replayCurrentIndex = 0;
    private int markedTimeIndex = 0;
    private Texture2D[] savedPictures;

    public int slowDown = 4;
    public int replaySlowCounter = 0;
    
    public bool replay = false;
    

    public bool inLiveMode = true;

    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    private void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;
        
        if(devices.Length == 0)
        {
            Debug.Log("No camera detexted");
            camAvailable = false;
            return;
        }

        for(int i = 0; i < devices.Length; i++)
        {
            if(!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if(backCam == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }
        camAvailable = true;
        inLiveMode = true;
        SetCamAsBackground();
        InitializeBuffers();
    }

    private void InitializeBuffers() {
        pictureBuffer = new Texture2D[bufferSize];
        for(int index = 0; index < pictureBuffer.Length; index++) {
            pictureBuffer[index] = new Texture2D(backCam.width, backCam.height);
        }

        savedPictures = new Texture2D[bufferSize];
        for(int index = 0; index < savedPictures.Length; index++) {
            savedPictures[index] = new Texture2D(backCam.width, backCam.height);
        }
    }
    
    private void SetCamAsBackground() {
        if(camAvailable) {
            backCam.Play();
            background.texture = backCam;
        } else {
            Debug.Log("SetCamAsBackground: cam not available");
        }
    }

    private void Update()
    {
        if(!camAvailable)
        {
            return;
        }
        
        if(inLiveMode) {
            float ratio = (float)backCam.width / (float)backCam.height;
            fit.aspectRatio = ratio;

            float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
            background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
            int orient = -backCam.videoRotationAngle;

            background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

            currentTimeIndex = WrappedIndexIncrement(currentTimeIndex);

            if (pictureBuffer[currentTimeIndex].width != backCam.width || pictureBuffer[currentTimeIndex].height != backCam.height) {
                pictureBuffer[currentTimeIndex] = new Texture2D(backCam.width, backCam.height);
            }
            pictureBuffer[currentTimeIndex].SetPixels(backCam.GetPixels());
            pictureBuffer[currentTimeIndex].Apply();
        } else {
            if (replay) {
                if(replaySlowCounter == 0)
                {
                    replayCurrentIndex = WrappedIndexIncrement(replayCurrentIndex);
                    background.texture = savedPictures[replayCurrentIndex];
                }

                replaySlowCounter++;
                if(replaySlowCounter >= slowDown) {
                    replaySlowCounter = 0;
                }
            } else {
                // dont step
            }
        }
    }

    public void OnMark()
    {
        Texture2D[] help = savedPictures;
        savedPictures = pictureBuffer;
        markedTimeIndex = currentTimeIndex;

        // reinitialize buffer to new array
        pictureBuffer = help;
        currentTimeIndex = 0;
    }

    private int WrappedIndexIncrement(int index) {
        return index < bufferSize - 1 ? index + 1 : 0;
    }

    public void OnReviewToggle() {
        inLiveMode = !inLiveMode;
        if(inLiveMode) {
            background.texture = backCam;
        } else {
            replayCurrentIndex = WrappedIndexIncrement(markedTimeIndex);
            background.texture = savedPictures[replayCurrentIndex];   
        }
    }

    public void TogglePlayPause() {
        replay = !replay;
    }
}
