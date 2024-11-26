using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMenuEscape : MonoBehaviour
{
    [SerializeField] private SettingsMenu menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           menu.Open();
            
        }    
    }
}
