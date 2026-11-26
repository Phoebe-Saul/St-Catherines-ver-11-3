using UnityEngine;

public static class DoorSpawnContext
{
    public static string NextSpawnId;

    // Position to return to after exiting a house
    public static Vector3 ReturnPosition;

    // Scene the player entered the house from
    public static string ReturnSceneName;

    // Whether a return position has been saved
    public static bool HasReturnPosition = false;
}
