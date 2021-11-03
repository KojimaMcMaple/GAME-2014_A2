using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: FoodFactory.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Manages the type of object to spawn
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
[System.Serializable]
public class ExplosionFactory : MonoBehaviour
{
    [SerializeField] private GameObject player_obj_;
    [SerializeField] private GameObject enemy_obj_;

    /// <summary>
    /// Instantiates an object and returns a reference to it
    /// </summary>
    /// <returns></returns>
    public GameObject CreateObj(GlobalEnums.ObjType type = GlobalEnums.ObjType.PLAYER)
    {
        GameObject temp = null;
        switch (type)
        {
            case GlobalEnums.ObjType.PLAYER:
                temp = Instantiate(player_obj_, this.transform);
                break;
            case GlobalEnums.ObjType.ENEMY:
                temp = Instantiate(enemy_obj_, this.transform);
                break;
            default:
                break;
        }
        temp.SetActive(false);
        return temp;
    }
}
