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

    private const float pixelPercent = 1f/84f;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        speed = speed / (pixelPercent * 100f); // Speed to our expected pixel size
        cummulativeSpeed = new Vector2(0f, 0f);
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

        img.material.mainTextureOffset += resultingOffset;
    }
}
