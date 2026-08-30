using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollTileBG : MonoBehaviour
{

    public float speed = 1f;
    public Vector2 direction = new Vector2(1f, 0f);

    private Image img;
    private Vector2 cummulativeSpeed;

    private const float pixelPercent = 1f/84;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        speed = speed / (pixelPercent * 100f); // Speed to our expected pixel size
        cummulativeSpeed = new Vector2(0f, 0f);
        // img.material.mainTextureOffset += new Vector2(/*-pixelPercent*(8/39 + 7/134)*/ 0,0);

    }

    // Update is called once per frame
    void Update()   
    {
        cummulativeSpeed += direction.normalized * Time.deltaTime * speed;
        Vector2 resultingOffset = new Vector2(0f, 0f);
        
        if(cummulativeSpeed.x > pixelPercent)
        {  
            resultingOffset.x = pixelPercent;
            cummulativeSpeed.x = 0;
        }
        
        if(cummulativeSpeed.y > pixelPercent) 
        {
            resultingOffset.y = pixelPercent;
            cummulativeSpeed.y = 0;
        }
        Debug.Log(img.material.mainTextureOffset.x);
        img.material.mainTextureOffset += resultingOffset;
        img.material.mainTextureOffset = new Vector2(
            img.material.mainTextureOffset.x>=1?0:img.material.mainTextureOffset.x,
           img.material.mainTextureOffset.y>=1?0:img.material.mainTextureOffset.y);
    }
}
