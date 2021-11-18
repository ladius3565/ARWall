using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    
    private UIController ui;

    private void Awake()
    {
        ui = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();        

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (ui == null) return;

        if (ui.onCreateText)
        {
            //ui.CreateTextMode();
            ui.CreateTextMdoe2();
        }
        if (ui.onTyping) ui.UpdateText();        
    }

}
