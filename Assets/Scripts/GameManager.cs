using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public HealthComponent hpComponent;
    public ParryComponent parryComponent;


    void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);

        hpComponent = new HealthComponent();
        parryComponent = new ParryComponent();

        
    }

    void Update(){
        if (Input.GetKeyDown("space")) {
            parryComponent.Parry();
        }
    }

    

    public void Win(){

    }

    public void Lose(){

    }
}
