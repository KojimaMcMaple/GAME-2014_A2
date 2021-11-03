/// <summary>
///  The Source file name: GlobalEnums.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Global game manager script
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
public static class GlobalEnums
{
    public enum BulletDir
    {
        DEFAULT,
        LEFT,
        RIGHT,
        UP,
        DOWN
    };

    public enum ObjType
    {
        DEFAULT,
        PLAYER,
        ENEMY,
        BOSS,
        TYPE_COUNT
    };

    public enum FoodType
    {
        DEFAULT,
        LOW,
        HIGH,
        BEYOND,
        TYPE_COUNT
    };

    public enum EnemyState
    {
        IDLE,
        ATTACK
    };
}
