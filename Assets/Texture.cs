using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Texture : SingleTone<Texture>
{
    public Texture2D textureImage;
    private Texture2D CopytextureImage;
    public Color FillColor { get; set; } = Color.yellow;
    [SerializeField] private SpriteRenderer background;
    public SpriteRenderer Background { get { return background; } }
    public FloodFilltag[,] resolution { get; set; }


    override public void Awake()
    {
        base.Awake();
        Set();
    }
    private void Set()
    {
        CopytextureImage = Instantiate(textureImage);
        background.sprite = Sprite.Create(CopytextureImage, new Rect(0, 0, CopytextureImage.width, CopytextureImage.height), new Vector2(0.5f, 0.5f));
        resolution = new FloodFilltag[Screen.width, Screen.height];
        ResolutionSet(Screen.width, Screen.height);
    }
    private void ResolutionSet(int maxWidth, int maxHeight)
    {
        int width = 0;
        int height = 0;
        while (height < maxHeight)
        {
            while (width < maxWidth)
            {
                resolution[width, height] = (width.Equals(0) || height.Equals(0)) || (width.Equals(1919) || height.Equals(1079))
                    ? FloodFilltag.Wall
                    : FloodFilltag.None;
                ++width;
            }
            ++height;
            width = 0;
        }
    }

}