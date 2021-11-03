using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: BulletController.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Responsible for moving the individual bullets
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
public class BulletController : MonoBehaviour
{
    [SerializeField] private GlobalEnums.ObjType type_ = GlobalEnums.ObjType.ENEMY; //compares with hit obj, determines which pool to return to
    [SerializeField] private float speed_;
    [SerializeField] private float travel_distance_ = 10f;
    [SerializeField] private int damage_ = 10;
    private Vector3 spawn_pos_;
    private GlobalEnums.BulletDir dir_ = GlobalEnums.BulletDir.DEFAULT;
    private BulletManager bullet_manager_;

    private void Awake()
    {
        bullet_manager_ = GameObject.FindObjectOfType<BulletManager>();
    }

    private void FixedUpdate()
    {
        Move();
        CheckBounds();
    }

    /// <summary>
    /// Moves the bullet from left to right
    /// </summary>
    private void Move()
    {
        switch (dir_)
        {
            case GlobalEnums.BulletDir.LEFT:
                transform.position -= new Vector3(speed_, 0f);
                break;
            case GlobalEnums.BulletDir.RIGHT:
                transform.position += new Vector3(speed_, 0f);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// When bullet is fully off-screen, move bullet back to pool
    /// </summary>
    private void CheckBounds()
    {
        if (transform.position.x > spawn_pos_.x + travel_distance_ || transform.position.x < spawn_pos_.x - travel_distance_)
        {
            bullet_manager_.ReturnBullet(this.gameObject, type_);
        }
    }

    /// <summary>
    /// Mutator for private variable
    /// </summary>
    /// <param name="value"></param>
    public void SetSpawnPos(Vector3 value)
    {
        spawn_pos_ = value;
    }

    /// <summary>
    /// Mutator for private variable
    /// </summary>
    /// <param name="value"></param>
    public void SetDir(GlobalEnums.BulletDir value)
    {
        dir_ = value;
    }

    /// <summary>
    /// When bullet collides with something, move bullet back to pool
    /// </summary>
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "FieldOfVision")
        {
            return;
        }
        IDamageable<int> other_interface = other.gameObject.GetComponent<IDamageable<int>>();
        if (other_interface != null)
        {
            if (other_interface.obj_type != type_)
            {
                other_interface.ApplyDamage(damage_);
            }
        }
        bullet_manager_.ReturnBullet(this.gameObject, type_);
    }
}
