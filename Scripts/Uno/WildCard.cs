using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildCard : Card
{
    // Start is called before the first frame update
    [SerializeField] protected string wildType;

    public void setWildType(string wildType) {
        this.wildType = wildType;
    }

    public string getWildType() {
        return this.wildType;
    }
}
