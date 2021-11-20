using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRobotController : EnemyController
{
    // ROBOT
    [SerializeField] private float vertical_range_ = 0.47f;
    private Transform bullet_spawn_pos_;
    //public GameObject bulletPrefab;

    void Awake()
    {
        DoBaseInit();
        bullet_spawn_pos_ = transform.Find("BulletSpawnPosition");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, Mathf.PingPong(Time.time * speed_, vertical_range_) + start_pos_.y); //bops up and down
        DoBaseUpdate();
    }

    /// <summary>
    /// Shoots a bullet, pooled from queue
    /// </summary>
    protected override void DoAttack()
    {
        shoot_countdown_ -= Time.deltaTime;
        if (shoot_countdown_ <= 0)
        {
            GlobalEnums.BulletDir dir = is_facing_left_ ? GlobalEnums.BulletDir.LEFT : GlobalEnums.BulletDir.RIGHT;
            bullet_manager_.GetBullet(bullet_spawn_pos_.position, GlobalEnums.ObjType.ENEMY, dir);
            shoot_countdown_ = firerate_;
            audio_source_.PlayOneShot(attack_sfx_);
        }
    }

    /// <summary>
    /// Aggro if player detected
    /// </summary>
    public override void DoAggro()
    {
        SetState(GlobalEnums.EnemyState.ATTACK);
    }

    /// <summary>
    /// Visual debug
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.Find("BulletSpawnPosition").position, 0.05f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + vertical_range_, transform.position.z), new Vector3(0.2f, 0.05f, 1));
    }
}
