using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlimeController : EnemyController
{
    [SerializeField] private float move_force_;
    private Transform look_ahead_point_;
    private Transform look_front_point_;
    private bool is_ground_ahead_ = true;

    void Awake()
    {
        DoBaseInit();
        look_ahead_point_ = transform.Find("LookAheadPoint"); 
        look_front_point_ = transform.Find("LookInFrontPoint");
    }

    void FixedUpdate()
    {
        switch (state_) //state machine
        {
            case GlobalEnums.EnemyState.IDLE:
                LookAhead();
                Move();
                animator_.SetBool("IsAttacking", false);
                break;
            case GlobalEnums.EnemyState.MOVE_TO_TARGET:
                LookAhead();
                MoveToTarget();
                animator_.SetBool("IsAttacking", true);
                break;
            case GlobalEnums.EnemyState.ATTACK:
                animator_.SetBool("IsAttacking", true);
                DoAttack();
                break;
            default:
                break;
        }
    }

    protected override void DoAttack()
    {
        
    }

    /// <summary>
    /// Aggro if player detected
    /// </summary>
    public override void DoAggro()
    {
        SetState(GlobalEnums.EnemyState.MOVE_TO_TARGET);
    }

    private void LookAhead()
    {
        var hit = Physics2D.Linecast(transform.position, look_ahead_point_.position, LayerMask.GetMask("Ground"));
        if (hit)
        {
            is_ground_ahead_ = true;
            if (hit.transform.gameObject.CompareTag("Blocker"))
            {
                SetIsFacingLeft(!IsFacingLeft()); //flip
            }
        }
        else
        {
            is_ground_ahead_ = false;
        }
    }

    private void Move()
    {
        if (is_ground_ahead_)
        {
            float dir = IsFacingLeft() ? -1.0f : 1.0f;
            rb_.AddForce(new Vector2(dir, 0) * move_force_);
            rb_.velocity *= 0.9f;
        }
        else
        {
            SetIsFacingLeft(!IsFacingLeft()); //flip
        }
    }

    private void MoveToTarget()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(collision.transform, true);
            scale_x_ = transform.localScale.x;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
            scale_x_ = start_scale_x_;
        }
    }

    /// <summary>
    /// Visual debug
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.Find("LookAheadPoint").position);
    }
}
