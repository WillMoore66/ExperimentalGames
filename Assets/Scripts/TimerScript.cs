using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float timer = 0;
    [SerializeField] GameObject timerText;

    private void Update()
    {
        timer += Time.deltaTime;
        timerText.GetComponent<TextMeshProUGUI>().text = timer.ToString("#.00");
    }
}
