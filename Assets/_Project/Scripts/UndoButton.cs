using Guymon.DesignPatterns;
using UnityEngine;

public class UndoButton : MonoBehaviour
{
    public void Undo()
    {
        CommandHandler.Undo();
        AudioManager.Instance.Play("Undo");
    }
}
