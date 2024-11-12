using Guymon.DesignPatterns;
using UnityEngine;

public class UndoButton : MonoBehaviour
{
    public void Undo()
    {
        if(CommandHandler.Count() <= 0) return;
        CommandHandler.Undo();
        AudioManager.Instance.Play("Undo");
        EventHandler.Invoke("Ability/DestroyPanel", null);
    }
}
