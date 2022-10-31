using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCard : Card
{
    [SerializeField] protected string actionType;

    public void setActionType(string actionType) {
        this.actionType = actionType;
    }

    public string getActionType() {
        return this.actionType;
    }
}