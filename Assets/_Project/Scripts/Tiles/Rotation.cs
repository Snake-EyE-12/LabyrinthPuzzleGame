using UnityEngine;

public class Rotation
{
    private int currentRotation = 0;

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