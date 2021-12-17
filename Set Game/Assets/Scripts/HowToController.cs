using System.Collections;
using System.Collections.Generic;
using CaptainCoder.Unity;
using UnityEngine;

public class HowToController : MonoBehaviour
{
    private List<string> Instructions;
    //The deck is made of 81 cards each of which has 4 properties:  Value, Shade, Color, Shape For example:  Two Dark Red Squares One Empty Green Triangle Three Light Blue Circles
    //Find 3 card where each of the 4 properties completely match or completely differ.  For example, if all cards are red, the color is a valid group. If you have a red, green, and blue card, the colors are a valid group. But, if you have 2 red and 1 blue, the colors are not valid. 
    //In short, 3 cards will match if: * All values match or differ * All fills match or differ * All colors match or differ * All shapes match or differ
    //If you get stuck, you can select 2 cards and ask for a hint. This will tell you which card is necessary to complete the group.  If you cannot find any groups with the cards on the board, you can discard 3 cards at random and replace them with new cards.
    //Good luck and have fun!
    public TextGroup TextElement;
    private int CurrentInstruction = 0;

    public void Start()
    {
        
    }

    void OnEnable()
    {
        Instructions = new List<string>();
        Instructions.Add(
@"The deck is made of 81 cards each of which has 4 properties:

Value, Shade, Color, Shape 

For example: 
Two Dark Red Squares 
One Empty Green Triangle 
Three Light Blue Circles");

        Instructions.Add(
@"Find 3 card where each of the 4 properties completely match or completely differ.  
For example, if all cards are red, the color is a valid group. If you have a red, green, and blue card, the colors are a valid group. But, if you have 2 red and 1 blue, the colors are not valid.");

    Instructions.Add(
@"In short, 3 cards will match if: 

* All values match or differ 
* All fills match or differ 
* All colors match or differ 
* All shapes match or differ");

Instructions.Add(
@"If you get stuck, you can select 2 cards and ask for a hint. This will tell you which card is necessary to complete the group.  

If you cannot find any groups with the cards on the board, you can discard 3 cards at random and replace them with new cards.");

Instructions.Add("Good luck and have fun!");
        CurrentInstruction = -1;
        Next();
    }


    public void Next()
    {
        CurrentInstruction = (CurrentInstruction + 1) % Instructions.Count;
        string Instruction = Instructions[CurrentInstruction];
        TextElement.SetText(Instruction);
    }
}
