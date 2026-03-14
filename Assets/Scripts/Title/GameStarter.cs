using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private AudioClip _titleBGM;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(_titleBGM);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(1);
        }
    }
}
