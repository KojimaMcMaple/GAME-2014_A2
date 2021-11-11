using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private GlobalEnums.MovingPlatformDir dir_ = GlobalEnums.MovingPlatformDir.HORIZONTAL;
    [SerializeField] private float speed_ = 2.0f;
    [SerializeField] private float distance_ = 1.0f;
    [SerializeField] private bool is_looping_ = true;
    private Vector2 start_pos_;
    private bool can_move_ = true;

    void Awake()
    {
        start_pos_ = transform.position;
    }

    void Update()
    {
        if (can_move_)
        {
            Move();
        }
    }

    private void Move()
    {
        float pingpong_value = can_move_ ? Mathf.PingPong(Time.time * speed_, distance_): distance_;
        if (!is_looping_ && (pingpong_value >= distance_ - 0.05f))
        {
            can_move_ = false;
        }
        switch (dir_)
        {
            case GlobalEnums.MovingPlatformDir.HORIZONTAL:
                transform.position = new Vector2(start_pos_.x + pingpong_value, transform.position.y);
                break;
            case GlobalEnums.MovingPlatformDir.VERTICAL:
                transform.position = new Vector2(transform.position.x, start_pos_.y + pingpong_value);
                break;
            case GlobalEnums.MovingPlatformDir.DIAGONAL_UP:
                transform.position = new Vector2(start_pos_.x + pingpong_value, start_pos_.y + pingpong_value);
                break;
            case GlobalEnums.MovingPlatformDir.DIAGONAL_DOWN:
                transform.position = new Vector2(start_pos_.x + pingpong_value, start_pos_.y - pingpong_value);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Visual debug
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Vector3 col_extents = transform.GetComponent<BoxCollider2D>().bounds.extents;
        switch (dir_)
        {
            case GlobalEnums.MovingPlatformDir.HORIZONTAL:
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(new Vector3(transform.position.x + distance_, transform.position.y, transform.position.z), new Vector3(0.05f, col_extents.y, 1));
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(new Vector3(transform.position.x + distance_ + (col_extents.x), transform.position.y, transform.position.z), new Vector3(0.05f, col_extents.y, 1));
                Gizmos.DrawWireCube(new Vector3(transform.position.x + distance_ - (col_extents.x), transform.position.y, transform.position.z), new Vector3(0.05f, col_extents.y, 1));
                break;
            case GlobalEnums.MovingPlatformDir.VERTICAL:
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + distance_, transform.position.z), new Vector3(col_extents.x, 0.05f, 1));
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + distance_ + (col_extents.y), transform.position.z), new Vector3(col_extents.x, 0.05f, 1));
                Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + distance_ - (col_extents.y), transform.position.z), new Vector3(col_extents.x, 0.05f, 1));
                break;
            case GlobalEnums.MovingPlatformDir.DIAGONAL_UP:
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(new Vector3(transform.position.x + distance_, transform.position.y + distance_, transform.position.z), new Vector3(col_extents.x*2, col_extents.y*2, 1));
                break;
            case GlobalEnums.MovingPlatformDir.DIAGONAL_DOWN:
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(new Vector3(transform.position.x + distance_, transform.position.y - distance_, transform.position.z), new Vector3(col_extents.x*2, col_extents.y*2, 1));
                break;
            default:
                break;
        }
    }
}

// PHYSICS TEST
//public class MovingPlatformController : MonoBehaviour
//{
//    [SerializeField] private Vector2 dir_ = Vector2.right;
//    [SerializeField] private float move_force_ = 10.0f;
//    [SerializeField] private float distance_ = 2.0f;
//    [SerializeField] private bool is_looping_ = true;
//    private float dir_x_;
//    private float dir_y_;
//    private Vector2 start_pos_;
//    private Vector2 end_pos_;
//    private bool can_move_ = true;
//    private bool is_on_start_dir_ = true;

//    private Rigidbody2D rb_;

//    void Awake()
//    {
//        rb_ = GetComponent<Rigidbody2D>();
//        dir_x_ = dir_.x;
//        dir_y_ = dir_.y;
//        start_pos_ = transform.position;
//        dir_ = dir_.normalized;
//        end_pos_ = start_pos_ + dir_ * distance_;
//    }

//    void FixedUpdate()
//    {
//        if (can_move_)
//        {
//            Move();
//        }
//    }

//    private void Move()
//    {
//        if (dir_x_ > 0)
//        {
//            if (transform.position.x >= end_pos_.x)
//            {
//                is_on_start_dir_ = false;
//            }
//            if (transform.position.x <= start_pos_.x)
//            {
//                is_on_start_dir_ = true;
//            }
//        }
//        else if (dir_x_ < 0)
//        {
//            if (transform.position.x <= end_pos_.x)
//            {
//                is_on_start_dir_ = false;
//            }
//            if (transform.position.x >= start_pos_.x)
//            {
//                is_on_start_dir_ = true;
//            }
//        }
//        if (dir_y_ > 0)
//        {
//            if (transform.position.y >= end_pos_.y)
//            {
//                is_on_start_dir_ = false;
//            }
//            if (transform.position.y <= start_pos_.y)
//            {
//                is_on_start_dir_ = true;
//            }
//        }
//        else if (dir_y_ < 0)
//        {
//            if (transform.position.y <= end_pos_.y)
//            {
//                is_on_start_dir_ = false;
//            }
//            if (transform.position.y >= start_pos_.y)
//            {
//                is_on_start_dir_ = true;
//            }
//        }

//        if (is_on_start_dir_)
//        {
//            dir_ = new Vector2(dir_x_, dir_y_);
//        }
//        else
//        {
//            dir_ = new Vector2(-dir_x_, -dir_y_);
//            if (!is_looping_)
//            {
//                can_move_ = false;
//                rb_.velocity = Vector2.zero;
//            }
//        }

//        if (can_move_)
//        {
//            rb_.AddForce(dir_ * move_force_);
//        }
//    }

//    /// <summary>
//    /// Visual debug
//    /// </summary>
//    void OnDrawGizmosSelected()
//    {
//        Vector2 dir = dir_.normalized;
//        Vector2 end_pos = new Vector2(transform.position.x, transform.position.y) + dir * distance_;
//        Vector3 col_extents = transform.GetComponent<BoxCollider2D>().bounds.extents;
//        Gizmos.color = Color.magenta;
//        Gizmos.DrawWireCube(end_pos, new Vector3(col_extents.x * 2, col_extents.y * 2, 1));
//    }
//}
