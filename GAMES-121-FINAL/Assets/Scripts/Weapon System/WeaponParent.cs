using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public abstract class WeaponParent : MonoBehaviour
{
    #region General Variables
    [Header("General Variabes")]
    [SerializeField] protected string m_weaponName;
    public string weaponName
    {
        get { return m_weaponName; }
        set { m_weaponName = value; }
    }

    Card m_card;
    public Card card { get { return m_card; } set { m_card = value; } }
    #endregion

    #region Bullet Settings
    [Header("Bullet Settings")]
    [SerializeField] protected int m_bulletCount;
    public int bulletCount
    {
        get { return m_bulletCount; }
        set { m_bulletCount = value; }
    }
    [SerializeField] protected GameObject m_bullet;
    [SerializeField] protected Transform m_bulletPoint;
    #endregion

    #region Mouse Aimming Variables
    private Camera m_mainCam;
    private Vector3 m_mousePos;
    public Vector2 aimDir {get; private set;}
    #endregion

    #region Animation
    [SerializeField] protected SpriteRenderer m_weaponSprite;
    protected Animator m_animator;
    #endregion

    protected void Awake()
    {
        //Can't attach weapon to player directly because that will mess up aimming direction
        #region Set Parent Constraint
        ConstraintSource _player = new ConstraintSource();
        _player.sourceTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _player.weight = 1;
        GetComponent<ParentConstraint>().AddSource(_player);
        #endregion
    }

    protected void Start()
    {
        #region Fetch Components
        m_mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        m_animator = GetComponent<Animator>();
        #endregion

        #region Set Values
        aimDir = Vector2.zero;
        #endregion
    }

    protected virtual void Update()
    {
        #region Aimming
        //Mouse
        m_mousePos = m_mainCam.ScreenToWorldPoint(Input.mousePosition);
        aimDir = m_mousePos - transform.position;

        //Joystick
        float _joystickX = Input.GetAxis("Horizontal Right Stick");
        float _joystickY = Input.GetAxis("Vertical Right Stick");
        if (_joystickX != 0 || _joystickY != 0) aimDir = new Vector2(_joystickX, _joystickY);

        //Rotate
        m_weaponSprite.flipY = (aimDir.x < 0);
        float _rotation = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        m_weaponSprite.transform.rotation = Quaternion.Euler(0, 0, _rotation);
        #endregion

        #region Fire Input
        if (Input.GetButtonDown("Fire") && m_bulletCount > 0)
        {
            m_bulletCount--;
            if (m_card != null) m_card.SetAmmoCount(m_bulletCount);
            Fire();
        }
        #endregion
    }
    protected abstract void Fire();
}
