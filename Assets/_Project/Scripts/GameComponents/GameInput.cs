using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameInput
{
    public GameInput(InputSelector s)
    {
        selector = s;
    }

    protected InputSelector selector;
    public abstract void Update();
    
}
public class CardGameInput : GameInput
{
    public CardGameInput(InputSelector s) : base(s)
    {
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            selector.Activate(null);
        }
    }
}

public class TileGameInput : GameInput
{
    public TileGameInput(InputSelector s) : base(s)
    {
    }
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.RotateSelectedTile(RotationDirection.Clockwise, 1);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.RotateSelectedTile(RotationDirection.Counterclockwise, 1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            GameManager.Instance.DirectionToSlide = CardinalDirection.North;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.Instance.DirectionToSlide = CardinalDirection.South;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameManager.Instance.DirectionToSlide = CardinalDirection.East;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.DirectionToSlide = CardinalDirection.West;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            selector.Activate(null);
        }
    }
    
}

public class CharacterMovementGameInput : GameInput
{
    public CharacterMovementGameInput(InputSelector s) : base(s)
    {
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            selector.Activate(new DirectionalSelectableActivatorData(Vector2Int.up));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            selector.Activate(new DirectionalSelectableActivatorData(Vector2Int.down));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            selector.Activate(new DirectionalSelectableActivatorData(Vector2Int.right));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            selector.Activate(new DirectionalSelectableActivatorData(Vector2Int.left));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            selector.Activate(new ConfirmSelectableActivatorData());
        }
    }
}
public class AbilityGameInput : GameInput
{
    public AbilityGameInput(InputSelector s) : base(s)
    {
        
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) selector.Activate(new ConfirmSelectableActivatorData());
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameManager.Instance.AbilityUser.GetMap().Move(GameManager.Instance.AbilityUser as GridPositionable, Vector2Int.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.Instance.AbilityUser.GetMap().Move(GameManager.Instance.AbilityUser as GridPositionable, Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameManager.Instance.AbilityUser.GetMap().Move(GameManager.Instance.AbilityUser as GridPositionable, Vector2Int.right);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.AbilityUser.GetMap().Move(GameManager.Instance.AbilityUser as GridPositionable, Vector2Int.left);
        }
    }
}
public class EnemyGameInput : GameInput
{
    public EnemyGameInput(InputSelector s) : base(s)
    {
        
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) selector.Activate(new ConfirmSelectableActivatorData());
    }
}