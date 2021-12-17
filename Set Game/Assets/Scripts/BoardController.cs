using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardController : MonoBehaviour
{
    public static BoardController Instance;
    public int Rows, Columns;

    public Transform TopLeft, BottomRight;
    private float Width, Height, OffsetX, OffsetY;
    public CardController[] Spaces;

    public List<CardController> Selected;
    public GameObject ParticlesTemplate;

    public bool HasSpace
    {
        get
        {
            foreach (CardController c in this.Spaces)
                if (c == null) return true;
            return false;
        }
    }

    public void Awake()
    {
        Instance = this;
        Init();
    }

    public void Init()
    {
        Spaces = new CardController[Rows * Columns];
        Width = (BottomRight.position.x - TopLeft.position.x) / (Columns - 1);
        Height = (TopLeft.position.y - BottomRight.position.y) / (Rows - 1);
        OffsetX = TopLeft.position.x;
        OffsetY = TopLeft.position.y;
    }

    public void AddCard(CardController toAdd)
    {
        int ix = 0;
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                if (Spaces[ix] == null)
                {
                    Spaces[ix] = toAdd;
                    toAdd.transform.position = new Vector2(OffsetX + (col * Width), OffsetY - (row * Height));
                    return;
                }
                ix++;
            }
        }
        throw new System.Exception("No space!");
    }

    public void RemoveCard(CardController toRemove)
    {
        for (int i = 0; i < Rows * Columns; i++)
        {
            if (Spaces[i] == toRemove)
            {
                Spaces[i] = null;
                return;
            }
        }

        throw new System.Exception("Card not found!");
    }

    public void Select(CardController toSelect)
    {

        if (Selected.Contains(toSelect))
        {
            Selected.Remove(toSelect);
            toSelect.transform.rotation = Quaternion.Euler(0, 0, 0);
            GameInfoController.Instance.HintText.SetText(GetHint());
            return;
        }

        if (Selected.Count < 3)
        {
            toSelect.transform.rotation = Quaternion.Euler(0, 0, -7.5f);
            Selected.Add(toSelect);
        }

        if (Selected.Count == 3)
        {
            CheckSelected();
        }

        GameInfoController.Instance.HintText.SetText(GetHint());
    }

    private void CheckSelected()
    {
        foreach (CardController c in Selected)
        {
            c.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        List<Card> cards = Selected.Select(c => c.model).ToList<Card>();
        if (IsSet(cards))
        {
            SoundController.Instance.Success.Play();
            GameInfoController.Instance.Sets++;
            GameInfoController.Instance.Cards -= 3;
            GameInfoController.Instance.NiceScreen.StartFade();
            foreach (CardController c in Selected)
            {
                this.RemoveCard(c);
                c.CollectCard();
            }
            DeckController.Instance.FillBoard();
        }
        else
        {
            SoundController.Instance.Failure.Play();
            GameInfoController.Instance.ErrorText.SetText(this.GetError(cards));
            GameInfoController.Instance.ErrorScreen.StartFade();
        }

        Selected.Clear();
    }

    public void Discard(int n)
    {
        int count = 0;
        foreach (CardController c in Spaces)
            if (c != null) count++;
        if (GameInfoController.Instance.Cards <= Rows * Columns)
        {
            SoundController.Instance.Failure.Play();
            GameInfoController.Instance.ErrorText.SetText("No Cards Remaining");
            GameInfoController.Instance.ErrorScreen.StartFade();
        }
        else
        {
            GameInfoController.Instance.Discards += n;
            GameInfoController.Instance.Cards -= n;
            while (n > 0)
            {
                int ix = Random.Range(0, Spaces.Length);
                CardController c = Spaces[ix];
                if (c == null) continue;
                this.RemoveCard(c);
                c.CollectCard();
                n--;
            }
            SoundController.Instance.Discard.Play();
            DeckController.Instance.FillBoard();

        }
    }

    public string GetHint()
    {
        List<Card> cards = Selected.Select(c => c.model).ToList();
        if (cards.Count < 2)
        {
            return "Select two cards to receive a hint.";
        }


        int[] colors = cards.Select(c => (int)c.Color).ToArray<int>();
        CardColor color;
        if (AllSame(colors))
        {
            color = (CardColor)colors[0];
        }
        else
        {
            int diff = GetLast(colors);
            color = (CardColor)diff;
        }

        int[] fills = cards.Select(c => (int)c.Fill).ToArray<int>();
        CardFill fill;
        if (AllSame(fills))
        {
            fill = (CardFill)fills[0];
        }
        else
        {
            int diff = GetLast(fills);
            fill = (CardFill)diff;
        }

        int[] shapes = cards.Select(c => (int)c.Shape).ToArray<int>();
        CardShape shape;
        if (AllSame(shapes))
        {
            shape = (CardShape)shapes[0];
        }
        else
        {
            int diff = GetLast(shapes);
            shape = (CardShape)diff;
        }

        int[] values = cards.Select(c => (int)c.Value).ToArray<int>();
        CardValue value;
        if (AllSame(values))
        {
            value = (CardValue)values[0];
        }
        else
        {
            int diff = GetLast(values);
            value = (CardValue)diff;
        }
        string an = fill == CardFill.Empty ? "an" : "a";
        string s = value == CardValue.One ? "" : "s";
        return $"To complete the group you need a card with {value} {fill} {color} {shape}{s}.";
    }

    private int GetLast(int[] cards)
    {
        int sum = cards[0] + cards[1];
        if (sum == 1)
            return 2;
        else if (sum == 2)
            return 1;
        else //(sum == 3)
            return 0;
    }

    public string GetError(List<Card> cards)
    {
        int[] colors = cards.Select(c => (int)c.Color).ToArray<int>();
        if (!AllSameOrDiff(colors))
        {
            return "Colors Are Not A Group";
        }

        int[] fills = cards.Select(c => (int)c.Fill).ToArray<int>();
        if (!AllSameOrDiff(fills))
        {
            return "Shades Are Not A Group";
        }

        int[] values = cards.Select(c => (int)c.Value).ToArray<int>();
        if (!AllSameOrDiff(values))
        {
            return "Values Are Not A Group";
        }

        int[] shapes = cards.Select(c => (int)c.Shape).ToArray<int>();
        if (!AllSameOrDiff(shapes))
        {
            return "Shapes Are Not A Group";
        }

        return "Nice!";
    }

    private bool IsSet(List<Card> cards)
    {
        int[] colors = cards.Select(c => (int)c.Color).ToArray<int>();
        int[] fills = cards.Select(c => (int)c.Fill).ToArray<int>();
        int[] shapes = cards.Select(c => (int)c.Shape).ToArray<int>();
        int[] values = cards.Select(c => (int)c.Value).ToArray<int>();
        return AllSameOrDiff(colors) &&
               AllSameOrDiff(fills) &&
               AllSameOrDiff(shapes) &&
               AllSameOrDiff(values);
    }

    private bool AllSameOrDiff(int[] toCheck)
    {
        return AllSame(toCheck) || AllDiff(toCheck);
    }

    private bool AllSame(int[] toCheck)
    {
        int current = toCheck[0];
        for (int i = 1; i < toCheck.Length; i++)
        {
            if (current != toCheck[i])
            {
                return false;
            }
            current = toCheck[i];
        }
        return true;
    }

    private bool AllDiff(int[] toCheck)
    {
        HashSet<int> seen = new HashSet<int>();
        foreach (int i in toCheck)
        {
            if (seen.Contains(i))
            {
                return false;
            }
            seen.Add(i);
        }
        return true;
    }

}
