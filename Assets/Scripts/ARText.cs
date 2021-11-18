using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARText : MonoBehaviour
{
    [SerializeField] private ARRaycastManager arRay;    

    [SerializeField] private GameObject obj;

    private Vector3 screenCenter => Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

    private void Update()
    {

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRay.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            Pose hitPos = hits[0].pose;

            obj.SetActive(true);
            obj.transform.SetPositionAndRotation(hitPos.position, hitPos.rotation);
        }
        else
        {
            obj.SetActive(false);
        }
    }

}
