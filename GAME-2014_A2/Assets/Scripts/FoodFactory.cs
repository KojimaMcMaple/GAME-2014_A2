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
public class FoodFactory : MonoBehaviour
{
    [SerializeField] private GameObject low_food_obj_;
    [SerializeField] private GameObject default_food_obj_;
    [SerializeField] private GameObject high_food_obj_;
    [SerializeField] private GameObject beyond_food_obj_;
    [SerializeField] private List<Sprite> low_food_sprites_ = new List<Sprite>();
    [SerializeField] private List<Sprite> default_food_sprites_ = new List<Sprite>();
    [SerializeField] private List<Sprite> high_food_sprites_ = new List<Sprite>();
    [SerializeField] private List<Sprite> beyond_food_sprites_ = new List<Sprite>();

    /// <summary>
    /// Instantiates an object and returns a reference to it
    /// </summary>
    /// <returns></returns>
    public GameObject CreateObj(GlobalEnums.FoodType type = GlobalEnums.FoodType.DEFAULT)
    {
        GameObject temp = null;
        switch (type)
        {
            case GlobalEnums.FoodType.DEFAULT:
                temp = Instantiate(default_food_obj_, this.transform);
                break;
            case GlobalEnums.FoodType.LOW:
                temp = Instantiate(low_food_obj_, this.transform);
                break;
            case GlobalEnums.FoodType.HIGH:
                temp = Instantiate(high_food_obj_, this.transform);
                break;
            case GlobalEnums.FoodType.BEYOND:
                temp = Instantiate(beyond_food_obj_, this.transform);
                break;
            default:
                break;
        }
        temp.SetActive(false);
        return temp;
    }

    /// <summary>
    /// Sets a random sprite for food item
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Sprite GetRandSprite(GlobalEnums.FoodType type = GlobalEnums.FoodType.DEFAULT)
    {
        Sprite temp = null;
        switch (type)
        {
            case GlobalEnums.FoodType.DEFAULT:
                temp = default_food_sprites_[Random.Range(0, default_food_sprites_.Count)];
                break;
            case GlobalEnums.FoodType.LOW:
                temp = low_food_sprites_[Random.Range(0, low_food_sprites_.Count)];
                break;
            case GlobalEnums.FoodType.HIGH:
                temp = high_food_sprites_[Random.Range(0, high_food_sprites_.Count)];
                break;
            case GlobalEnums.FoodType.BEYOND:
                temp = beyond_food_sprites_[Random.Range(0, beyond_food_sprites_.Count)];
                break;
            default:
                break;
        }
        return temp;
    }
}
