using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SkillParent : MonoBehaviour
{
    #region Event Control
    protected bool m_toBeDisabled = false;
    protected bool m_toBeDestroyed = false;
    protected bool m_skillExecuting = false;
    public bool skillExecuting { get { return m_skillExecuting; } set { m_skillExecuting = value; } }
    #endregion

    protected WeaponParent m_bundledWeapon;
    protected CharacterMovement m_characterMovement;

    [BoxGroup("SFX")]
    [SerializeField] AudioManager m_audioManager;

    protected virtual void Awake()
    {
        m_characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        //Try to find bundled weapon
        if (transform.parent != null) m_bundledWeapon = transform.parent.GetComponentInChildren<WeaponParent>();

        //Detach Audio Source
        if (m_audioManager != null) m_audioManager.transform.parent = null;
    }

    protected virtual void OnEnable()
    {
        m_toBeDisabled = false;
    }

    protected virtual void Update()
    {
        if (Input.GetButtonDown("Fire") && !m_toBeDisabled)
        {
            if (m_bundledWeapon?.ammoCount > 0 || m_bundledWeapon.isMeleeWeapon) ExecuteSkill();
            else ExecuteSkill();

            m_skillExecuting = true;
        }
    }

    #region Utility Methods
    //Notes:
    //FinishEventLoop() need to be somewhere in the child
    protected virtual void FinishEventLoop()
    {
        m_skillExecuting = false;
        if ((m_bundledWeapon?.ammoCount <= 0 && !m_bundledWeapon.isMeleeWeapon) || m_toBeDestroyed) DestroyEvent();
        if (m_toBeDisabled) DisableEvent();
    }

    protected virtual void DestroyEvent()
    {
        if (m_audioManager != null) m_audioManager.SetToBeDestroyed();
        if (transform.parent != null) Destroy(transform.parent.gameObject);

        GameObject.Find("Simple Inventory")?.GetComponent<SimpleInventory>().ChangeActiveBundle();
        Destroy(gameObject);
    }

    protected virtual void DisableEvent()
    {
        if (transform.parent != null) transform.parent.gameObject.SetActive(false);
        else gameObject.SetActive(false);
    }

    public void SetToBeDisabled(bool _destroy = false)
    {
        m_toBeDisabled = true;
        m_toBeDestroyed = _destroy;
        if (!m_skillExecuting) FinishEventLoop();
    }
    #endregion

    protected abstract void ExecuteSkill();
}
