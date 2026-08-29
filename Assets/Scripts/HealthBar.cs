using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        GameManager gm = GameManager.Instance;
        if(gm)
        {
            gm.hpComponent.OnHealthChanged += RefreshHatBar;
            return;
        }
        Debug.Log("No GameManager");
    }

    // Update is called once per frame
    public void RefreshHatBar(int current, bool heal)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject hat = transform.GetChild(i).gameObject;

            if (i<current) { hat.SetActive(true); }
            else { hat.SetActive(false); }
        }
    }
}
