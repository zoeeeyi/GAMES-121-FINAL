using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region General Settings
    [Header("General")]
    [SerializeField] bool m_initiatePosToPlayer = false;
    [Range(0, 1)]
    [SerializeField] float m_cameraSizeSmoothTime = 0.5f;
    [SerializeField] float m_cameraSizeMult = 0.5f;
    [SerializeField] float m_cameraMaxSize = 20;
    float m_cameraStartSize;
    float m_cameraTargetSize; //Should be used according to speed
    float m_cameraSizeSmoothV = 0;
    Camera m_camera;
    GameManager m_gameManager;
    #endregion

    #region Player Settings
    [Header("Player")]
    [SerializeField] GameObject m_player;
    PlayerInput m_playerInput;
    Collider2D m_playerCollider;
    Rigidbody2D m_playerRb;
    #endregion

    #region Focus Area Settings
    [Header("Focus area settings")]
    [SerializeField] Vector2 m_focusAreaSize = new Vector2(2, 2);
    [Range(0, 1)]
    [SerializeField] float m_focusPosSmoothTime = 1;
    [SerializeField] bool m_needRecenter = true;
    [Range(0, 1)]
    [SerializeField] float m_focusAreaRecenterTime;
    float m_focusPosSmoothX = 0;
    float m_focusPosSmoothY = 0;
    Vector2 m_focusAreaRecenterSmoothV;
    st_FocusArea m_focusArea;
    #endregion

    #region Look Ahead Setting
    [Header("Look ahead setting")]
    [SerializeField] float m_lookAheadDistant = 0.1f;
    [Range(0, 1)]
    [SerializeField] float m_lookAheadSmoothTime = 0.5f;
    Vector2 m_lookAheadDir;
    Vector2 m_lookAheadTarget;
    Vector2 m_currentLookAhead;
    Vector2 m_lookAheadTargetClearSmoothV;
    float m_lookAheadSmoothX = 0;
    float m_lookAheadSmoothY = 0;
    bool m_lookAheadStopped = true;
    #endregion

    bool m_cameraInitialized = false;

    void Start()
    {
        //Fetch components
        m_camera = GetComponent<Camera>();
        m_playerCollider = m_player.GetComponent<Collider2D>();
        m_playerInput = m_player.GetComponent<PlayerInput>();
        m_playerRb = m_player.GetComponent<Rigidbody2D>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Set focus area
        m_focusArea = new st_FocusArea(m_playerCollider.bounds, m_focusAreaSize, m_initiatePosToPlayer, transform.position);

        //Initialize camera size and position
        if (m_camera.orthographic) m_cameraStartSize = m_camera.orthographicSize;
        else m_cameraStartSize = m_camera.fieldOfView;
        if (m_initiatePosToPlayer) transform.position = m_player.transform.position + Vector3.back * 10;
        m_cameraInitialized = true;
    }

    void LateUpdate()
    {
        if (!m_cameraInitialized) { return; }
        if (m_playerCollider != null) m_focusArea.Update(m_playerCollider.bounds);
        else return;

        #region Look Ahead Setting
        if (m_focusArea.velocity.magnitude != 0)
        {
            m_lookAheadDir.x = Mathf.Sign(m_focusArea.velocity.x);
            m_lookAheadDir.y = Mathf.Sign(m_focusArea.velocity.y);

            bool _shouldWeLookAhead = false;
            if (Mathf.Sign(m_playerInput.GetInput()) == m_lookAheadDir.x && m_playerInput.GetInput() != 0)
            {
                _shouldWeLookAhead = true;
                m_lookAheadStopped = false;
                m_lookAheadTarget.x = m_lookAheadDir.x * m_lookAheadDistant;
            }

            if (!_shouldWeLookAhead)
            {
                if (m_lookAheadStopped == false)
                {
                    m_lookAheadTarget = m_currentLookAhead + (m_lookAheadDir * m_lookAheadDistant - m_currentLookAhead) / 4;
                    m_lookAheadStopped = true;
                }
            }
        }
        #endregion

        #region Calculate New Position
        //Recenter camera
        if (m_playerInput.GetInput() == 0 && m_needRecenter)
        {
            Vector2 _newTargetCenter = m_playerCollider.bounds.center;
            if (m_focusArea.center != _newTargetCenter)
            {
                m_focusArea.center = Vector2.SmoothDamp(m_focusArea.center, _newTargetCenter, ref m_focusAreaRecenterSmoothV, m_focusAreaRecenterTime);
                m_focusArea.Recenter();
            }
            m_lookAheadTarget = Vector2.SmoothDamp(m_lookAheadTarget, Vector2.zero, ref m_lookAheadTargetClearSmoothV, m_focusAreaRecenterTime);
        }

        m_currentLookAhead.x = Mathf.SmoothDamp(m_currentLookAhead.x, m_lookAheadTarget.x, ref m_lookAheadSmoothX, m_lookAheadSmoothTime);
        m_currentLookAhead.y = Mathf.SmoothDamp(m_currentLookAhead.y, m_lookAheadTarget.y, ref m_lookAheadSmoothY, m_lookAheadSmoothTime);

        //calculate new position
        Vector2 _newFocusPosition;
        _newFocusPosition = m_focusArea.center;
        _newFocusPosition.y += m_currentLookAhead.y;

        _newFocusPosition.x = Mathf.SmoothDamp(transform.position.x, _newFocusPosition.x, ref m_focusPosSmoothX, m_focusPosSmoothTime);
        _newFocusPosition.y = Mathf.SmoothDamp(transform.position.y, _newFocusPosition.y, ref m_focusPosSmoothY, m_focusPosSmoothTime);

        //Set camera position
        transform.position = (Vector3)_newFocusPosition + Vector3.back * 10;
        #endregion

        #region Set Camera Size
        //Set camera size
        m_cameraTargetSize = m_cameraSizeMult * Mathf.Abs(m_playerRb.velocity.magnitude);
        m_cameraTargetSize = Mathf.Clamp(m_cameraTargetSize, m_cameraStartSize, m_cameraMaxSize);
        if (m_camera.orthographic) m_camera.orthographicSize = Mathf.SmoothDamp(m_camera.orthographicSize, m_cameraTargetSize, ref m_cameraSizeSmoothV, m_cameraSizeSmoothTime);
        else m_camera.fieldOfView = Mathf.SmoothDamp(m_camera.fieldOfView, m_cameraTargetSize, ref m_cameraSizeSmoothV, m_cameraSizeSmoothTime);
        #endregion
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(m_focusArea.center, m_focusArea.size);
    }

    struct st_FocusArea
    {
        public Vector2 center;
        public Vector2 size;
        public Vector2 velocity;
        Vector2 focusAreaSize;
        float left, right;
        float top, bottom;

        public st_FocusArea(Bounds _targetBounds, Vector2 _size, bool _focusCameraOnPlayer, Vector3 _currentCamPos)
        {
            focusAreaSize = _size;

            if (_focusCameraOnPlayer)
            {
                left = _targetBounds.center.x - focusAreaSize.x;
                right = _targetBounds.center.x + focusAreaSize.x;
                bottom = _targetBounds.center.y - focusAreaSize.y;
                top = _targetBounds.center.y + focusAreaSize.y;
            }
            else
            {
                left = _currentCamPos.x - focusAreaSize.x;
                right = _currentCamPos.x + focusAreaSize.x;
                bottom = _currentCamPos.y - focusAreaSize.y;
                top = _currentCamPos.y + focusAreaSize.y;
            }

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            size = new Vector2(right - left, top - bottom);
        }

        public void Update(Bounds _targetBounds)
        {
            float _shiftX = 0;
            if (_targetBounds.min.x < left)
            {
                _shiftX = _targetBounds.min.x - left;
            }
            else if (_targetBounds.max.x > right)
            {
                _shiftX = _targetBounds.max.x - right;
            }
            left += _shiftX;
            right += _shiftX;

            float _shiftY = 0;
            if (_targetBounds.min.y < bottom)
            {
                _shiftY = _targetBounds.min.y - bottom;
            }
            else if (_targetBounds.max.y > top)
            {
                _shiftY = _targetBounds.max.y - top;
            }
            top += _shiftY;
            bottom += _shiftY;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(_shiftX, _shiftY);
        }

        public void Recenter()
        {
            left = center.x - focusAreaSize.x;
            right = center.x + focusAreaSize.x;
            top = center.y + focusAreaSize.y;
            bottom = center.y - focusAreaSize.y;
        }
    }
}
