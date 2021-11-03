using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: EnemyController.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Defines behavior for the enemy
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
public class EnemyController : MonoBehaviour, IDamageable<int>
{
    [Header("Stats")]
    [SerializeField] private int hp_ = 100;
    [SerializeField] private int score_ = 50;

    [Header("Movement")]
    private Vector3 start_pos_;
    [SerializeField] private float vertical_range_ = 0.47f;
    
    [Header("Bullets")]
    private Transform bullet_spawn_pos_;
    //public GameObject bulletPrefab;
    [SerializeField] private float speed_ = 0.75f;
    [SerializeField] private float firerate_ = 0.47f;
    private float shoot_countdown_ = 0.0f;

    private Transform fov_;
    private bool is_facing_left_ = true;
    private GlobalEnums.EnemyState state_ = GlobalEnums.EnemyState.IDLE;
    private Animator animator_;

    private BulletManager bullet_manager_;
    private ExplosionManager explode_manager_;
    private FoodManager food_manager_;
    private GameManager game_manager_;

    private VfxSpriteFlash flash_vfx_;

    [SerializeField] private AudioClip shoot_sfx_;
    [SerializeField] private AudioClip damaged_sfx_;
    private AudioSource audio_source_;

    void Awake()
    {
        start_pos_ = transform.position;
        is_facing_left_ = transform.localScale.x > 0 ? false : true;
        shoot_countdown_ = firerate_;
        animator_ = GetComponent<Animator>();
        fov_ = transform.Find("FieldOfVision");
        bullet_spawn_pos_ = transform.Find("BulletSpawnPosition");
        bullet_manager_ =   GameObject.FindObjectOfType<BulletManager>();
        explode_manager_ =   GameObject.FindObjectOfType<ExplosionManager>();
        food_manager_ =     GameObject.FindObjectOfType<FoodManager>();
        game_manager_ =     GameObject.FindObjectOfType<GameManager>();
        flash_vfx_ = GetComponent<VfxSpriteFlash>();
        audio_source_ = GetComponent<AudioSource>();

        Init(); //IDamageable method
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x, Mathf.PingPong(Time.time * speed_, vertical_range_) + start_pos_.y); //bops up and down
        float scale_x = is_facing_left_ ? -1.0f : 1.0f;
        transform.localScale = new Vector3(scale_x, transform.localScale.y, transform.localScale.z); //sets which way the enemy faces

        switch (state_) //state machine
        {
            case GlobalEnums.EnemyState.IDLE:
                animator_.SetBool("IsAttacking", false);
                break;
            case GlobalEnums.EnemyState.ATTACK:
                animator_.SetBool("IsAttacking", true);
                DoShoot();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Shoots a bullet, pooled from queue
    /// </summary>
    private void DoShoot()
    {
        shoot_countdown_ -= Time.deltaTime;
        if (shoot_countdown_ <= 0)
        {
            GlobalEnums.BulletDir dir = is_facing_left_ ? GlobalEnums.BulletDir.LEFT : GlobalEnums.BulletDir.RIGHT;
            bullet_manager_.GetBullet(bullet_spawn_pos_.position, GlobalEnums.ObjType.ENEMY, dir);
            shoot_countdown_ = firerate_;
            audio_source_.PlayOneShot(shoot_sfx_);
        }
    }

    /// <summary>
    /// Mutator for private variable
    /// </summary>
    public void SetState(GlobalEnums.EnemyState value)
    {
        state_ = value;
    }

    /// <summary>
    /// Mutator for private variable
    /// </summary>
    public void SetIsFacingLeft(bool value)
    {
        is_facing_left_ = value;
    }

    /// <summary>
    /// IDamageable methods
    /// </summary>
    public void Init() //Link hp to class hp
    {
        health = hp_;
        obj_type = GlobalEnums.ObjType.ENEMY;
    }
    public int health { get; set; } //Health points
    public GlobalEnums.ObjType obj_type { get; set; } //Type of gameobject
    public void ApplyDamage(int damage_value) //Deals damage to object
    {
        health -= damage_value;
        flash_vfx_.DoFlash();
        audio_source_.PlayOneShot(damaged_sfx_);
        if (health <= 0)
        {
            explode_manager_.GetObj(this.transform.position, obj_type);
            food_manager_.GetObj(this.transform.position, (GlobalEnums.FoodType)Random.Range(0, (int)GlobalEnums.FoodType.TYPE_COUNT));
            game_manager_.IncrementScore(score_);
            this.gameObject.SetActive(false);
        }
    }
    public void HealDamage(int heal_value) { } //Adds health to object

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
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y - vertical_range_, transform.position.z), new Vector3(0.2f, 0.05f, 1));
    }
}
