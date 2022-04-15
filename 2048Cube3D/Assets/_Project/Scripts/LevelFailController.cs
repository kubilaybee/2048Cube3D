using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFailController : MonoBehaviour
{
    public float Timer;

    public float LevelFailStayTime;

    public static LevelFailController Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Timer = 0f;
    }

    private void OnCollisionStay(Collision collision)
    {
        // GAMEOBJECT TYPE IS CUBES
        if (collision.gameObject.GetComponent<Cubes>())
        {
            // THATS CUBE IS NOT MY MAINCUBE AND TIME
            if (!collision.gameObject.GetComponent<Cubes>().IsMainCube && Timer < LevelFailStayTime)
            {
                Timer += Time.deltaTime;
            }
            // GAMEOVER IS FALSE TURN THE TRUE
            if (Timer >= LevelFailStayTime && !GMScript.Instance.IsGameOver)
            {
                GMScript.Instance.IsGameOver = true;
                Debug.Log("GAME OVER");

                // maybe this error
                Timer = 0f;
                StartCoroutine(GMScript.Instance.lostStage()); 
            }
        }
    }
    
}
