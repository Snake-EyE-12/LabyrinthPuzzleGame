using Capstone.DataLoad;
using TMPro;
using UnityEngine;

public class TeamOptionDisplay : Display<CharacterLayout>
{
    [SerializeField] private TMP_Text teamNameTextbox;
    [SerializeField] private Transform holderParent;
    [SerializeField] private CharacterColorDisplay prefab;

    public override void Render()
    {
        teamNameTextbox.text = item.Name;
        foreach (var character in item.Characters)
        {
            CharacterColorDisplay colorDisplay = Instantiate(prefab, holderParent);
            teamNameTextbox.text = item.Name;
            colorDisplay.Set(new CharacterColorData(DataHolder.characterColorEquivalenceTable.GetColor(character),
                character));

        }
    }

    private TeamBuilderHandler tbh;
    public void SetHandler(TeamBuilderHandler handler)
    {
        tbh = handler;
    }
    public void OnClick()
    {
        tbh.PickFull(item.Characters);
    }

}