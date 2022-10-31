using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource cardFlipSound;
    
    void Start()
    {
        
    }

    public void playCardFlipSound() {
        cardFlipSound.Play();
    }
}
