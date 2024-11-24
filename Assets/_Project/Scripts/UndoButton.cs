using Guymon.DesignPatterns;
using UnityEngine;

public class UndoButton : MonoBehaviour
{
    public void Undo()
    {
        DeselectCharacter();
        DeselectCard();
        if(CommandHandler.Count() <= 0) return;
        CommandHandler.Undo();
        AudioManager.Instance.Play("Undo");
        EventHandler.Invoke("Ability/DestroyPanel", null);
    }
    private void DeselectCharacter()
    {
        if (GameManager.Instance.AbilityUser != null)
        {
            GameManager.Instance.AbilityUser.Active(false);
            GameManager.Instance.AbilityUser = null;
        }
    }

    private void DeselectCard()
    {
        GameManager.Instance.SetCardToPlace(null);
    }
}
