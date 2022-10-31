using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberCard : Card
{
    [SerializeField] protected int number;
    // [SerializeField] protected TMP_Text numberText;
    
    public void setNumber(int number) {
        this.number = number;
        // numberText.text = this.number.ToString();
    }

    public int getNumber() {
        return this.number;
    }
    
}
