using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLifeGPU : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture renderTexture;

    private int groupCount;
    private int threadCount=32;
    [SerializeField]
    private int resolution = 8192;
    // Start is called before the first frame update
    void Start()
    {
        renderTexture = new RenderTexture(resolution, resolution, 0);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        groupCount = Mathf.CeilToInt(renderTexture.width / threadCount);

        InitSimulation(); 
    }

    private void InitSimulation()
    {
        computeShader.SetTexture(0, "Result", renderTexture);
        
        computeShader.Dispatch(0, groupCount, groupCount, 1);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        computeShader.SetTexture(1, "Result", renderTexture);

        computeShader.Dispatch(1, groupCount, groupCount, 1);

        Graphics.Blit(renderTexture, destination);
    }
}
