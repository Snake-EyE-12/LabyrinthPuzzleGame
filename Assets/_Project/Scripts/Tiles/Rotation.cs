using UnityEngine;

public class Rotation
{
    private int currentRotation = 0;

    public static Rotation Copy(Rotation rotation)
    {
        return new Rotation
        {
            currentRotation = rotation.currentRotation
        };
    }
    public int GetRotationValue()
    {
        return currentRotation;
    }
    public void Rotate(RotationDirection direction, int rotationAmount)
    {
        if (direction == RotationDirection.Clockwise) currentRotation += rotationAmount;
        else currentRotation -= rotationAmount;
        ClampRotation();
    }

    public Vector2Int RotateDirectionAccoundingly(Vector2Int direction)
    {
        switch (currentRotation)
        {
            case 0:
                return direction;
            case 1:
                return new Vector2Int(-direction.y, direction.x);
            case 2:
                return new Vector2Int(-direction.x, -direction.y);
            case 3:
                return new Vector2Int(direction.y, -direction.x);
            default:
                return direction;
        }
        
    }

    private void ClampRotation()
    {
        currentRotation = GameUtils.ModPositive(currentRotation, 4);
    }

    public void SetStringRotation(string rotation)
    {
        switch (rotation)
        {
            case "90":
                currentRotation = 1;
                break;
            case "180":
                currentRotation = 2;
                break;
            case "270":
                currentRotation = 3;
                break;
            case "Random":
                currentRotation = Random.Range(0, 4);
                break;
            default:
                currentRotation = 0;
                break;
        }
        
    }
}
public enum RotationDirection
{
    Clockwise,
    Counterclockwise
}