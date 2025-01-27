﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

[System.Serializable]
public class MarkerPrefabs
{
    public string marker;
    public GameObject targetPrefab;
}

public class ARPerImage_v2 : MonoBehaviour
{
    /* Insepctor array */
    public MarkerPrefabs[] markerPrefabCombos;
    ARTrackedImageManager m_TrackedImageManager;

    void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.updated)
        {
            /* If an image is properly tracked */
            if (trackedImage.trackingState == TrackingState.Tracking)
            {

                /* Loop through image/prefab-combo array */
                for (int i = 0; i < markerPrefabCombos.Length; i++)
                {
                    /* If trackedImage matches an image in the array */
                    if (markerPrefabCombos[i].marker == trackedImage.referenceImage.name)
                    {

                        /* Set the corresponding prefab to active at the center of the tracked image */
                        markerPrefabCombos[i].targetPrefab.SetActive(true);
                        markerPrefabCombos[i].targetPrefab.transform.position = trackedImage.transform.position;
                    }
                }
                /* If not properly tracked */
            }
            else
            {

                /* Deactivate all prefabs */
                for (int i = 0; i < markerPrefabCombos.Length; i++)
                {
                    markerPrefabCombos[i].targetPrefab.SetActive(false);
                }
            }
        }
    }

}

