using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerStats = GetComponent<PlayerStats>();
        _playerUI = GetComponent<PlayerUI>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
