using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public SpriteRenderer[] Shapes;
    public Sprite[] Circles;
    public Sprite[] Squares;
    public Sprite[] Triangles;
    public Color[] Colors;
    public bool isFaceUp;
    public GameObject CardFront;
    public GameObject CardBack;
    public Card model;

    public void Start()
    {
        RenderCard();
    }

    public void RenderCard()
    {

        CardBack.SetActive(!isFaceUp);
        CardFront.SetActive(isFaceUp);

        Sprite[] shapeLookup = model.Shape switch
        {
            CardShape.Circle => Circles,
            CardShape.Square => Squares,
            CardShape.Triangle => Triangles,
            _ => throw new System.Exception($"Unknown shape: {model.Shape}"),
        };

        foreach (SpriteRenderer r in Shapes)
        {
            r.sprite = shapeLookup[(int)model.Fill];
            r.color = this.Colors[(int)model.Color];
            r.gameObject.SetActive(false);
        }

        if (model.Value == CardValue.One)
        {
            Shapes[1].gameObject.SetActive(true);
        }
        else if (model.Value == CardValue.Two)
        {
            Shapes[0].gameObject.SetActive(true);
            Shapes[2].gameObject.SetActive(true);
        }
        else
        {
            Shapes[0].gameObject.SetActive(true);
            Shapes[1].gameObject.SetActive(true);
            Shapes[2].gameObject.SetActive(true);
        }
    }

    public void CollectCard()
    {
        GameObject particles = UnityEngine.Object.Instantiate(BoardController.Instance.ParticlesTemplate);
        Vector3 newPosition = new Vector3(this.transform.position.x, this.transform.position.y, -2);
        particles.transform.position = newPosition;
        UnityEngine.Object.Destroy(this.gameObject);
        particles.SetActive(true);
        
    }


    public void FlipUp()
    {
        this.isFaceUp = true;
        this.RenderCard();
    }

    public void FlipDown()
    {
        this.isFaceUp = false;
        this.RenderCard();
    }
}

[System.Serializable]
public class Card
{
    public CardValue Value;
    public CardColor Color;
    public CardFill Fill;
    public CardShape Shape;

    public Card(CardValue v, CardColor c, CardFill f, CardShape s)
    {
        this.Value = v;
        this.Color = c;
        this.Fill = f;
        this.Shape = s;
    }

    public static List<Card> BuildDeck()
    {
        List<Card> deck = new List<Card>();
        for (int v = 0; v < 3; v++)
        {
            CardValue value = (CardValue)v;
            for (int c = 0; c < 3; c++)
            {
                CardColor color = (CardColor)c;
                for (int f = 0; f < 3; f++)
                {
                    CardFill fill = (CardFill)f;
                    for (int s = 0; s < 3; s++)
                    {
                        CardShape shape = (CardShape)s;
                        Card card = new Card(value, color, fill, shape);
                        deck.Add(card);
                    }
                }
            }
        }
        return deck;
    }
}

public enum CardValue
{
    One,
    Two,
    Three
}

public enum CardColor
{
    Blue,
    Red,
    Green
}

public enum CardShape
{
    Circle,
    Square,
    Triangle
}

public enum CardFill
{
    Dark,
    Empty,
    Light
}