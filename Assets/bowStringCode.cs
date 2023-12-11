using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class bowStringCode : MonoBehaviour
{
    public static event Action<float> PullActionReleased;

    public Transform start, end;
    public GameObject midPoint;

    public float pullAmount = 0f;

    private LineRenderer _lineRenderer;
    private IXRSelectInteractor _interactor = null;


    private void Awake()
    {
        _lineRenderer= GetComponent<LineRenderer>();
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        _interactor = args.interactorObject;
    }

    public void Release()
    {
        PullActionReleased?.Invoke(pullAmount);
        _interactor = null;
        pullAmount = 0f;
        //midPoint.transform.localPosition = new Vector3(midPoint.transform.localPosition.x, midPoint.transform.localPosition.y, -0.2f);
        UpdateString();
    }

    public void Update()
    {
        if(_interactor != null)
        {
            Vector3 pullPosition = _interactor.transform.position;
            Vector3 pullDirection = pullPosition - start.position;
            Vector3 targetDirection = end.position - start.position;
            float maxLength = targetDirection.magnitude;

            targetDirection.Normalize();
            float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
            pullAmount = Mathf.Clamp(pullValue, 0f, 1);
            UpdateString();
        }

    }

    public void UpdateString()
    {
        Vector3 linePostion = Vector3.forward * Mathf.Lerp(start.transform.localPosition.z, end.transform.localPosition.z, pullAmount);
        midPoint.transform.localPosition = new Vector3(midPoint.transform.localPosition.x, midPoint.transform.localPosition.y, linePostion.z - 0.2f);
        _lineRenderer.SetPosition(1, linePostion);
    }
}
