using UnityEngine;
using TMPro;

public class MaxScript : MonoBehaviour
{
    public TextMeshProUGUI text_max;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text_max.SetText("" + PlayerPrefs.GetInt("max"));
    }
}
