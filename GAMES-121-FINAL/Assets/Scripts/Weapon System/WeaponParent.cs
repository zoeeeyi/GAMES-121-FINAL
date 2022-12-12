using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public abstract class WeaponParent : MonoBehaviour
{
    #region Card
    Card m_card;
    public Card card { get { return m_card; } set { m_card = value; } }
    #endregion

    #region Ammo Settings
    [BoxGroup("Weapon Settings")] [SerializeField] bool m_meleeWeapon = false;
    public bool isMeleeWeapon { get { return m_meleeWeapon; }}
    [BoxGroup("Weapon Settings")] [HideIf("m_meleeWeapon", true)] [SerializeField] protected int m_ammoCount;
    public int ammoCount
    {
        get { return m_ammoCount; }
        set { m_ammoCount = value; }
    }
    [BoxGroup("Weapon Settings")] [HideIf("m_meleeWeapon", true)] [SerializeField] protected GameObject m_bulletObject;
    [BoxGroup("Weapon Settings")] [SerializeField] protected Transform m_firePoint;
    #endregion

    #region Mouse Aimming Variables
    private Camera m_mainCam;
    private Vector3 m_mousePos;
    public Vector2 aimDir {get; private set;}
    #endregion

    #region Visual
    [BoxGroup("Visual")]
    [SerializeField] protected SpriteRenderer m_weaponSprite;
    protected Animator m_animator;
    #endregion

    #region SFX
    [BoxGroup("SFX")]
    [SerializeField] protected AudioManager m_audioManager;
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

        //Detach Audio Source
        if (m_audioManager != null) m_audioManager.transform.parent = null;
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
        if (Input.GetButtonDown("Fire") && m_ammoCount > 0)
        {
            m_ammoCount--;
            if (m_card != null) m_card.SetAmmoCount(m_ammoCount);
            Fire();
        }
        #endregion
    }
    protected abstract void Fire();

    private void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return;
        if (m_audioManager != null) m_audioManager.SetToBeDestroyed();
    }
}
