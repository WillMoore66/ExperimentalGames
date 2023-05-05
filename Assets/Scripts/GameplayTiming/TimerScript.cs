using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    public float timer = 0;
    [SerializeField] GameObject timerText;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "DeclanSceneButActuallyWorks")
        {
            Destroy(gameObject);
        }

        timer += Time.deltaTime;
        timerText.GetComponent<TextMeshProUGUI>().text = timer.ToString("#.00");
    }
}
