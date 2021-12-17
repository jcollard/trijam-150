using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CaptainCoder.Unity;

public class DeckController : MonoBehaviour
{
    public static DeckController Instance;
    public CardController CardTemplate;
    public Transform CardContainer;

    public BoardController Board;

    public Stack<CardController> CardStack;

    public static int MaxZIndex = 0;

    void Start()
    {
        Instance = this;
        Init();
    }

    public void Init()
    {
        UnityEngineUtils.Instance.DestroyChildren(CardContainer);
        this.CardStack = new Stack<CardController>();
        List<Card> Cards = Card.BuildDeck();
        IEnumerable<Card> Shuffled = Cards.OrderBy(x => Random.Range(0,1F));
        int ix = 0;
        foreach(Card c in Shuffled)
        {
            CardController newCard = UnityEngine.Object.Instantiate<CardController>(CardTemplate);
            newCard.transform.parent = CardContainer;
            newCard.transform.localPosition = new Vector3(0.0025f*ix, 0.005f*ix, 0.01f*MaxZIndex--);
            
            newCard.gameObject.name = $"{c.Color} {c.Shape} {c.Fill} {c.Value}";
            newCard.model = c;
            newCard.RenderCard();

            this.CardStack.Push(newCard);
            ix++;
        }
        Board.Init();
        FillBoard();
    }

    public void FillBoard()
    {
        while (Board.HasSpace && CardStack.Count > 0)
        {
            CardController toAdd = CardStack.Pop();
            Board.AddCard(toAdd);
            toAdd.FlipUp();
        }
    }

    
}
