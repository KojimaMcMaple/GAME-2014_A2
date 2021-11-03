using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: ExplosionManager.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Manages the queue
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
[System.Serializable]
public class ExplosionManager : MonoBehaviour
{
    private Queue<GameObject> player_pool_;
    private Queue<GameObject> enemy_pool_;
    private int player_pool_num_ = 0;
    private int enemy_pool_num_ = 0;

    private ExplosionFactory factory_;

    private GameManager game_manager_;

    private void Awake()
    {
        player_pool_ = new Queue<GameObject>();
        enemy_pool_ = new Queue<GameObject>();
        factory_ = GetComponent<ExplosionFactory>();

        game_manager_ = FindObjectOfType<GameManager>(); //show game over after player explosion is returned
    }

    /// <summary>
    /// Uses the factory to spawn one object, add it to the queue, and increase the pool size 
    /// </summary>
    private void AddObj(GlobalEnums.ObjType type = GlobalEnums.ObjType.ENEMY)
    {
        var temp = factory_.CreateObj(type);

        switch (type)
        {
            case GlobalEnums.ObjType.PLAYER:
                player_pool_.Enqueue(temp);
                player_pool_num_++;
                break;
            case GlobalEnums.ObjType.ENEMY:
                enemy_pool_.Enqueue(temp);
                enemy_pool_num_++;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Removes an object from the pool and returns a reference to it
    /// </summary>
    /// <param name="position"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetObj(Vector2 position, GlobalEnums.ObjType type = GlobalEnums.ObjType.ENEMY)
    {
        GameObject temp = null;
        switch (type)
        {
            case GlobalEnums.ObjType.PLAYER:
                if (player_pool_.Count < 1) //add one bullet if pool empty
                {
                    AddObj(GlobalEnums.ObjType.PLAYER);
                }
                temp = player_pool_.Dequeue();
                break;
            case GlobalEnums.ObjType.ENEMY:
                if (enemy_pool_.Count < 1) //add one bullet if pool empty
                {
                    AddObj(GlobalEnums.ObjType.ENEMY);
                }
                temp = enemy_pool_.Dequeue();
                break;
            default:
                break;
        }
        temp.transform.position = position;
        temp.SetActive(true);
        temp.GetComponent<ExplosionController>().DoExplode();
        return temp;
    }

    /// <summary>
    /// Returns an object back into the pool
    /// </summary>
    /// <param name="returned_obj"></param>
    public void ReturnObj(GameObject returned_obj, GlobalEnums.ObjType type = GlobalEnums.ObjType.ENEMY)
    {
        returned_obj.SetActive(false);

        switch (type)
        {
            case GlobalEnums.ObjType.PLAYER:
                player_pool_.Enqueue(returned_obj);
                game_manager_.DoShowOverlayPanel();
                break;
            case GlobalEnums.ObjType.ENEMY:
                enemy_pool_.Enqueue(returned_obj);
                break;
            default:
                break;
        }
    }
}
