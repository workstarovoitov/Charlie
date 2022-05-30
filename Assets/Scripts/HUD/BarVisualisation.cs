using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarVisualisation : MonoBehaviour
{
    [SerializeField] private Sprite frameMin;
    [SerializeField] private Sprite frameMid;
    [SerializeField] private Sprite frameMax;
    [SerializeField] private Sprite frameMidMax;

    [SerializeField] private Vector3 fadedColor;
    [SerializeField] private float speed;

    private int frameSize;
    private List<GameObject> frameList;
    public List<GameObject> FrameList
    {
        get => frameList;
    }

    private Vector3 color;

    private void Awake()
    {
        frameList = new List<GameObject>();
    }

    public void CreateFrame()
    {
        // Create Game Object
        GameObject frameGameObject = new GameObject("Frame", typeof(Image));

        // Set as child of this transform
        frameGameObject.transform.SetParent(transform);
        frameGameObject.transform.localPosition = Vector3.zero;
        frameGameObject.transform.localScale = Vector3.one;

        // Locate and Size heart
        frameGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32);

        // Set frame sprite
        Image heartImageUI = frameGameObject.GetComponent<Image>();
        heartImageUI.preserveAspect = true;

        frameList.Add(frameGameObject);
    }

    public void RemoveFrame(int frameNum)
    {
        Destroy(frameList[frameNum]);
        frameList.RemoveAt(frameNum);
    }
    public void RemoveAllFrames()
    {
        for (int i = frameList.Count; i > 0; i--)
        {
            Destroy(frameList[i-1]);
            frameList.RemoveAt(i-1);
        }
    }
    public void SetFrameType(GameObject g, int fragments)
    {
        switch (fragments)
        {
            case 0: g.GetComponent<Image>().sprite = frameMin; break;
            case 1: g.GetComponent<Image>().sprite = frameMid; break;
            case 2: g.GetComponent<Image>().sprite = frameMax; break;
            case 3: g.GetComponent<Image>().sprite = frameMidMax; break;
        }
    }

    public void RunFrameBlink()
    {
        StopAllCoroutines();
        color.x = 255;
        color.y = 255;
        color.z = 255;

        foreach (GameObject frame in frameList)
        {
            frame.GetComponent<Image>().color = new Color(color.x, color.y, color.z, 1f);
        }
        StartCoroutine(SetColor());
    }

    IEnumerator SetColor()
    {
        color.x = 255;
        color.y = 255;
        color.z = 255;
        while (Mathf.Abs(color.x - fadedColor.x) > 1  && Mathf.Abs(color.y - fadedColor.y) > 1 && Mathf.Abs(color.z - fadedColor.z) > 1)
        {
            color.x = Mathf.Lerp(color.x, fadedColor.x, Time.deltaTime * speed);
            color.y = Mathf.Lerp(color.y, fadedColor.y, Time.deltaTime * speed);
            color.z = Mathf.Lerp(color.z, fadedColor.z, Time.deltaTime * speed);
            color.x = Mathf.Clamp(color.x, 0, 255);
            color.y = Mathf.Clamp(color.y, 0, 255);
            color.z = Mathf.Clamp(color.z, 0, 255);

            foreach(GameObject frame in frameList)
            {
                frame.GetComponent<Image>().color = new Color(color.x/255, color.y/255, color.z/255, 1f);
            }
            yield return new WaitForFixedUpdate();
        }
        
        while (Mathf.Abs(color.x - 255) > 1 && Mathf.Abs(color.y - 255) > 1 && Mathf.Abs(color.z - 255) > 1)
        {
            color.x = Mathf.Lerp(color.x, 255, Time.deltaTime * speed);
            color.y = Mathf.Lerp(color.y, 255, Time.deltaTime * speed);
            color.z = Mathf.Lerp(color.z, 255, Time.deltaTime * speed);
            color.x = Mathf.Clamp(color.x, 0, 255);
            color.y = Mathf.Clamp(color.y, 0, 255);
            color.z = Mathf.Clamp(color.z, 0, 255);

            foreach(GameObject frame in frameList)
            {
                frame.GetComponent<Image>().color = new Color(color.x/255, color.y/255, color.z/255, 1f);
            }

            yield return new WaitForFixedUpdate();
        }
    }

}
