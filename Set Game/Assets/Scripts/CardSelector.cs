using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    private Vector2 PrevScale;
    public CardController Controller;
    
    void OnMouseEnter()
    {
        PrevScale = this.gameObject.transform.localScale;
        this.gameObject.transform.localScale = new Vector2(PrevScale.x + 0.1f, PrevScale.y + 0.1f);

    }

    void OnMouseExit()
    {
        this.gameObject.transform.localScale = PrevScale;
    }

    void OnMouseDown()
    {
        BoardController.Instance.Select(Controller);
    }

}
