using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

internal static class YieldInstructionCache
{
    public static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x == y;
        }
        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());
    public static WaitForSeconds waitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!timeInterval.TryGetValue(seconds, out wfs)) timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
}

public class UIController : MonoBehaviour
{
    private ARRaycastManager arRay;

    private TouchScreenKeyboard keyboard;    
    [SerializeField] private Transform arCamera;    

    private TextMesh inputText;    
    [SerializeField] private GameObject textObj;

    public bool onCreateText { get; private set; }
    public bool onTyping { get; private set; }

    private Vector3 screenCenter => Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

    [SerializeField] private Text console;
    private bool clear;


    #region Button Function
    public void OnKeyboard()
    {
        if (onCreateText) onCreateText = false;
        else onCreateText = true;
    }  

    #endregion    

    public void CreateTextMode()
    {
        if (Input.touchCount > 0)
        {
            Vector2 touchPos = Input.GetTouch(0).position;            
            if (touchPos.y > 320)
            {
                RaycastHit hit;

                if (Physics.Raycast(Vector3.zero, arCamera.forward ,out hit))
                {
                    inputText = Instantiate(textObj, hit.point, arCamera.rotation).transform.GetChild(0).GetComponent<TextMesh>();

                    onCreateText = false;
                    SendConsole("RayCast Hit", 3);
                    keyboard = TouchScreenKeyboard.Open("");
                    onTyping = true;
                }
                else
                {
                    SendConsole("RayCast Can't Hit", 3);
                }
            }
        }        
    }

    public void CreateTextMdoe2()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        
        if (Input.touchCount > 0)
        {
            arRay.Raycast(screenCenter, hits, TrackableType.Planes);

            if (hits.Count > 0)
            {
                Pose hitPos = hits[0].pose;

                inputText = Instantiate(textObj, hitPos.position, arCamera.rotation).transform.GetChild(0).GetComponent<TextMesh>();

                onCreateText = false;
                onTyping = true;

                keyboard = TouchScreenKeyboard.Open("");
                SendConsole("RayCast Hit", 1);
            }
        }
    }

    public void UpdateText()
    {
        if (keyboard != null)
        {
            if (keyboard.active)
            {
                inputText.text = keyboard.text;
            }
        }
    }

    #region Console

    public void SendConsole(string text, float time)
    {        
        console.text += text + "\n";
        if (!clear) Invoke("ClearConsole", time);
        clear = true;        
    }

    public void ClearConsole()
    {
        console.text = "";
        clear = false;
    }

    #endregion
}
