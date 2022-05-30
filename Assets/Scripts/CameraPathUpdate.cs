using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;


public class CameraPathUpdate : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private CinemachineTrackedDolly track;
    private float position = 0.5f;
    private float positionTarget = 1;
    [SerializeField] private float speed = 0.001f;
    [SerializeField] private float minPos = 0.2f;
    [SerializeField] private float MaxPos = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        vcam = camera.GetComponentInChildren<CinemachineVirtualCamera>();
        track = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    // Update is called once per frame
    void Update()
    {
        if (position >= MaxPos)
        {
            positionTarget = 0;
        }

        if (position <= minPos)
        {
            positionTarget = 1;
        }

        position = Mathf.Lerp(position, positionTarget, speed);
        track.m_PathPosition = position;
    }
}
