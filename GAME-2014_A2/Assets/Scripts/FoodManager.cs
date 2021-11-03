using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: FoodManager.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Manages the queue
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
[System.Serializable]
public class FoodManager : MonoBehaviour
{
    private Queue<GameObject> pool_;
    private Queue<GameObject> low_pool_;
    private Queue<GameObject> high_pool_;
    private Queue<GameObject> beyond_pool_;
    private int pool_num_ = 0;
    private int low_pool_num_ = 0;
    private int high_pool_num_ = 0;
    private int beyond_pool_num_ = 0;

    private FoodFactory factory_;

    [SerializeField] private AudioClip eat_sfx_;
    [SerializeField] private float low_pitch_ = .75f;
    [SerializeField] private float default_pitch_ = 1.0f;
    [SerializeField] private float high_pitch_ = 1.5f;
    [SerializeField] private float beyond_pitch_ = 2.0f;
    private AudioSource audio_source_;

    private void Awake()
    {
        pool_ = new Queue<GameObject>();
        low_pool_ = new Queue<GameObject>();
        high_pool_ = new Queue<GameObject>();
        beyond_pool_ = new Queue<GameObject>();
        factory_ = GetComponent<FoodFactory>();
        audio_source_ = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Uses the factory to spawn one object, add it to the queue, and increase the pool size 
    /// </summary>
    private void AddObj(GlobalEnums.FoodType type = GlobalEnums.FoodType.DEFAULT)
    {
        var temp = factory_.CreateObj(type);
        switch (type)
        {
            case GlobalEnums.FoodType.DEFAULT:
                pool_.Enqueue(temp);
                pool_num_++;
                break;
            case GlobalEnums.FoodType.LOW:
                low_pool_.Enqueue(temp);
                low_pool_num_++;
                break;
            case GlobalEnums.FoodType.HIGH:
                high_pool_.Enqueue(temp);
                high_pool_num_++;
                break;
            case GlobalEnums.FoodType.BEYOND:
                beyond_pool_.Enqueue(temp);
                beyond_pool_num_++;
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
    public GameObject GetObj(Vector2 position, GlobalEnums.FoodType type = GlobalEnums.FoodType.DEFAULT)
    {
        GameObject temp = null;
        switch (type)
        {
            case GlobalEnums.FoodType.DEFAULT:
                if (pool_.Count < 1) //add one obj if pool empty
                {
                    AddObj(type);
                }
                temp = pool_.Dequeue();
                break;
            case GlobalEnums.FoodType.LOW:
                if (low_pool_.Count < 1) //add one obj if pool empty
                {
                    AddObj(type);
                }
                temp = low_pool_.Dequeue();
                break;
            case GlobalEnums.FoodType.HIGH:
                if (high_pool_.Count < 1) //add one obj if pool empty
                {
                    AddObj(type);
                }
                temp = high_pool_.Dequeue();
                break;
            case GlobalEnums.FoodType.BEYOND:
                if (beyond_pool_.Count < 1) //add one obj if pool empty
                {
                    AddObj(type);
                }
                temp = beyond_pool_.Dequeue();
                break;
            default:
                break;
        }
        temp.transform.position = position;
        temp.GetComponent<FoodController>().SetSpawnPos(position);
        temp.GetComponent<SpriteRenderer>().sprite = factory_.GetRandSprite(type);
        temp.SetActive(true);
        return temp;
    }

    /// <summary>
    /// Returns an object back into the pool
    /// </summary>
    /// <param name="returned_obj"></param>
    public void ReturnObj(GameObject returned_obj, GlobalEnums.FoodType type = GlobalEnums.FoodType.DEFAULT)
    {
        returned_obj.SetActive(false);

        switch (type)
        {
            case GlobalEnums.FoodType.DEFAULT:
                pool_.Enqueue(returned_obj);
                audio_source_.pitch = default_pitch_;
                audio_source_.PlayOneShot(eat_sfx_);
                break;
            case GlobalEnums.FoodType.LOW:
                low_pool_.Enqueue(returned_obj);
                audio_source_.pitch = low_pitch_;
                audio_source_.PlayOneShot(eat_sfx_);
                break;
            case GlobalEnums.FoodType.HIGH:
                high_pool_.Enqueue(returned_obj);
                audio_source_.pitch = high_pitch_;
                audio_source_.PlayOneShot(eat_sfx_);
                break;
            case GlobalEnums.FoodType.BEYOND:
                beyond_pool_.Enqueue(returned_obj);
                audio_source_.pitch = beyond_pitch_;
                audio_source_.PlayOneShot(eat_sfx_);
                break;
            default:
                break;
        }
    }
}
