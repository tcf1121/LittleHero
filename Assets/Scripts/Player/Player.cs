using UnityEngine;

// 플레이어 관리
public class Player : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerStats _playerStats;
    private PlayerUI _playerUI;
    private Rigidbody2D _rigid;
    private Collider2D _collider;
    public PlayerController PlayerController { get { return _playerController; } }
    public PlayerStats PlayerStats { get { return _playerStats; } }
    public PlayerUI PlayerUI { get { return _playerUI; } }
    public Rigidbody2D Rigid { get { return _rigid; } }
    public Collider2D Collider { get { return _collider; } }

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerStats = GetComponent<PlayerStats>();
        _playerUI = GetComponent<PlayerUI>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

}
