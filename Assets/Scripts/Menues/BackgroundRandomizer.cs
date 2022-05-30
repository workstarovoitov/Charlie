using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundRandomizer : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];
    }

}
