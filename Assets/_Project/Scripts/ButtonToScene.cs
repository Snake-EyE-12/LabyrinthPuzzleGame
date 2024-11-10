using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void ChangeScene()
    {
        SceneChanger.LoadScene(sceneName);
    }
}
