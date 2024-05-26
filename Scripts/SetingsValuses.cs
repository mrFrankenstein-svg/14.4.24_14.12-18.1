using UnityEngine;

public static class SetingsValuses
{
    [SerializeField] private static float valueOfMinimumCameraZoom = 100f;

    public static float ValueOfMinimumCameraZoom
    {
        get { return valueOfMinimumCameraZoom; }
        set { valueOfMinimumCameraZoom = value; }
    }

    [SerializeField] private static float valueOfMaximumCameraZoom = 200f;

    public static float ValueOfMaximumCameraZoom
    {
        get { return valueOfMaximumCameraZoom; }
        set { valueOfMaximumCameraZoom = value; }
    }

    [SerializeField] private static float moveCameraWhileMovingUp = 0.5f;

    public static float MoveCameraWhileMovingUp
    {
        get { return moveCameraWhileMovingUp; }
        set { moveCameraWhileMovingUp = value; }
    }
    [SerializeField] private static float moveCameraWhileMovingDown=1.2f;

    public static float MoveCameraWhileMovingDown
    {
        get { return moveCameraWhileMovingDown; }
        set { moveCameraWhileMovingDown = value; }
    }


}
