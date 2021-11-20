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
    // BASE STATS
    [SerializeField] protected int hp_ = 100;
    [SerializeField] protected int score_ = 50;
    [SerializeField] protected float speed_ = 0.75f;
    [SerializeField] protected float firerate_ = 0.47f;
    protected float shoot_countdown_ = 0.0f;
    protected Vector3 start_pos_;

    // UNITY COMPONENTS
    protected Animator animator_;
    protected Rigidbody2D rb_;

    // LOGIC
    protected Transform fov_; //not used
    protected bool is_facing_left_ = true;
    protected float start_scale_x_;
    protected float scale_x_;
    protected GlobalEnums.EnemyState state_ = GlobalEnums.EnemyState.IDLE;

    // MANAGERS
    protected BulletManager bullet_manager_;
    protected ExplosionManager explode_manager_;
    protected FoodManager food_manager_;
    protected GameManager game_manager_;

    // VFX
    protected VfxSpriteFlash flash_vfx_;

    // SFX
    [SerializeField] protected AudioClip attack_sfx_;
    [SerializeField] protected AudioClip damaged_sfx_;
    protected AudioSource audio_source_;

    protected void DoBaseInit()
    {
        start_pos_ = transform.position;
        is_facing_left_ = transform.localScale.x > 0 ? false : true;
        start_scale_x_ = Mathf.Abs(transform.localScale.x);
        scale_x_ = start_scale_x_;
        animator_ = GetComponent<Animator>();
        rb_ = GetComponent<Rigidbody2D>();
        explode_manager_ =   GameObject.FindObjectOfType<ExplosionManager>();
        food_manager_ =     GameObject.FindObjectOfType<FoodManager>();
        game_manager_ =     GameObject.FindObjectOfType<GameManager>();
        flash_vfx_ = GetComponent<VfxSpriteFlash>();
        audio_source_ = GetComponent<AudioSource>();
        fov_ = transform.Find("FieldOfVision");

        bullet_manager_ = GameObject.FindObjectOfType<BulletManager>();
        shoot_countdown_ = firerate_;

        Init(); //IDamageable method
    }

    protected void DoBaseUpdate()
    {
        switch (state_) //state machine
        {
            case GlobalEnums.EnemyState.IDLE:
                animator_.SetBool("IsAttacking", false);
                break;
            case GlobalEnums.EnemyState.ATTACK:
                animator_.SetBool("IsAttacking", true);
                DoAttack();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Attack behaviour
    /// </summary>
    protected virtual void DoAttack()
    {
        
    }

    /// <summary>
    /// Aggro behaviour
    /// </summary>
    public virtual void DoAggro()
    {

    }

    /// <summary>
    /// Mutator for private variable
    /// </summary>
    public void SetState(GlobalEnums.EnemyState value)
    {
        state_ = value;
    }

    /// <summary>
    /// Accessor for private variable
    /// </summary>
    public bool IsFacingLeft()
    {
        return is_facing_left_;
    }

    /// <summary>
    /// Mutator for private variable
    /// </summary>
    public void SetIsFacingLeft(bool value)
    {
        is_facing_left_ = value;
        transform.localScale = new Vector3(is_facing_left_ ? -scale_x_ : scale_x_, transform.localScale.y, transform.localScale.z); //sets which way the enemy faces
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
            food_manager_.GetObj(this.transform.position, (GlobalEnums.FoodType)Random.Range(0, (int)GlobalEnums.FoodType.NUM_OF_TYPE));
            game_manager_.IncrementScore(score_);
            this.gameObject.SetActive(false);
        }
    }
    public void HealDamage(int heal_value) { } //Adds health to object
}
