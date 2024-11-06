using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;                                                  //1

public class MainMenuScript : MonoBehaviour
{                                                                                   //2
    [SerializeField] private GameObject ballPrefab;                                 //2.1
    [SerializeField] private float xBound = 1f;                                     //2.2
    [SerializeField] private float zBound = 1f;                                   //2.3
    [SerializeField] private float destroyBorder = -15f;                            //2.4

    private GameObject ball;                                                        //2.5

    void Update()                                                                   //3
    {
        BallSpawner();                                                              //3.1

        BallDestroy(destroyBorder);                                                 //3.2
    }

    void BallSpawner()                                                              //4
    {
        if (ball != null)                                                            //4.1
        {
            return;                                                                 //4.2
        }
        else                                                                        //4.3
        {
            float rXBound = Random.Range(-xBound, xBound);                          //4.4
            float rZBound = Random.Range(-zBound, zBound);                          //4.5

            Vector3 ballPos = new Vector3(rXBound, transform.position.y, rZBound);  //4.6

            ball = Instantiate(ballPrefab, ballPos, Quaternion.identity);           //4.7
            ball.transform.parent = transform;                                      //4.8
        }
    }

    void BallDestroy(float value)                                                   //5
    {
        if (ball.transform.position.y < value)                                      //5.1
        {
            Destroy(ball);                                                          //5.2
        }
    }

    public void AppExit()                                                           //6
    {
#if UNITY_EDITOR                                                            //6.1
        UnityEditor.EditorApplication.ExitPlaymode();                               //6.2
#endif                                                                      //6.3

        Application.Quit();                                                         //6.4

        Debug.Log("Exit");                                                          //6.5
    }

    public void LoadNextScene()                                                     //7
    {
        SceneManager.LoadScene(1);                                                  //7.1
    }
}                                                                                   //8