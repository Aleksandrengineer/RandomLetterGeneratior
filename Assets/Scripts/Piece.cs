using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    public Sprite[] spriteArray;
    private SpriteRenderer _displayedImage;

    //Image image;
    // Start is called before the first frame update
    void Start()
    {
        //image = GetComponent<Image>();
        _displayedImage = GetComponent<SpriteRenderer>();
        ChangeSprite();
    }

    public void ChangeSprite()
    {
        _displayedImage.sprite = spriteArray[Random.Range(0, spriteArray.Length)];
    }
}
