using Capstone.DataLoad;
using TMPro;
using UnityEngine;

public class TeamOptionDisplay : Display<CharacterLayout>
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private CharacterColorDisplay prefab;

    public override void Render()
    {
        text.text = item.Name;
        foreach (var character in item.Characters)
        {
            CharacterColorDisplay colorDisplay = Instantiate(prefab, transform);
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