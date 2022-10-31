using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    [SerializeField] protected GameObject card;
    [SerializeField] protected string cardColor;

    private Transform oldCardPos;

    void Start() {
        oldCardPos = card.transform;
    }

    public void setCardValues(string color) {
        setColor(color);
    }

    public void setColor(string color) {
        this.cardColor = color;
    }


    public string getColor() {
        return this.cardColor;
    }



    /***********************************
                CARD UI 
    ***********************************/

    public bool OnMouseDown()
    {
        PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        if (card.transform.parent.gameObject.name.Equals("Player Hand")) {
            if (GameController.currTurn.Equals("Player")) {
                playerController.onCardClick(card);
                return true;
            }
        }
        return false;
    }

    public bool OnMouseOver()
    {
        // Debug.Log($"{this.card.name} Hovering!");
        return true;
    }


    public bool OnMouseEnter()
    {
        if (card.transform.parent.gameObject.name.Equals("Player Hand")) {
            card.transform.position = new Vector3(card.transform.position.x,
                                                            card.transform.position.y + 20f,
                                                            card.transform.position.z);
        }
        return true;
    }

    public bool OnMouseExit()
    {
        if (card.transform.parent.gameObject.name.Equals("Player Hand")) {
            card.transform.position = new Vector3(oldCardPos.position.x, oldCardPos.position.y - 20f, oldCardPos.position.z);
        }
        return true;
    }

}
