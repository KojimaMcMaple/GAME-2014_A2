using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: FoodController.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Responsible for the individual object spawn from the pool & factory
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
public class FoodController : MonoBehaviour
{
    [SerializeField] private GlobalEnums.FoodType type_ = GlobalEnums.FoodType.DEFAULT;
    [SerializeField] private int heal_value_ = 20;
    [SerializeField] private float speed_ = 0.75f;
    private Vector3 spawn_pos_;
    [SerializeField] private float vertical_range_ = 0.47f;
    private FoodManager manager_;

    private void Awake()
    {
        spawn_pos_ = transform.position;
        manager_ = FindObjectOfType<FoodManager>();
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x, Mathf.PingPong(Time.time * speed_, vertical_range_) + spawn_pos_.y);
    }

    /// <summary>
    /// Mutator for private variable
    /// </summary>
    public void SetSpawnPos(Vector3 value)
    {
        spawn_pos_ = value;
    }

    /// <summary>
    /// When object collides with something, move it back to pool
    /// </summary>
    public void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable<int> other_interface = other.gameObject.GetComponent<IDamageable<int>>();
        if (other_interface != null)
        {
            if (other_interface.obj_type == GlobalEnums.ObjType.PLAYER)
            {
                other_interface.HealDamage(heal_value_);
                manager_.ReturnObj(this.gameObject, type_);
            }
        }
    }
}
