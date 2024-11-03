
using UnityEngine;

public class AttackBoxHitPointDisplay : Display<HitPointData>
{
    [SerializeField] private SpriteRenderer box;
    [SerializeField] private SpriteRenderer end;
    [SerializeField] private SpriteRenderer separator;
    [SerializeField] private Color emptyColor;
    public override void Render()
    {
        switch (item.state)
        {
            case HitPointState.Full:
                box.enabled = true;
                box.color = GetFullColor();
                end.enabled = false;
                break;
            case HitPointState.End:
                box.enabled = false;
                end.enabled = true;
                end.color = emptyColor;
                break;
            case HitPointState.Empty:
                box.enabled = true;
                box.color = emptyColor;
                end.enabled = false;
                separator.enabled = false;
                break;
            default:
                box.enabled = false;
                end.enabled = false;
                separator.enabled = false;
                break;
        }

        separator.enabled = item.isSeparator;
    }

    private Color GetFullColor()
    {
        return new Color(1f, 0f, 0f, 150f / 255f);
    }
}

