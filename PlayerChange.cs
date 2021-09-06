using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This still needs some work.  
public class PlayerChange : MonoBehaviour
{
    public GameObject thirdPerson; // Add your third person character in inspector
    public GameObject firstPerson; // Add your first person character in inspector
                                   // Add your first person character in inspector if have more

    public int playerMode;

    private void Start()
    {
        // Choose what character to start in. One true, can have more than one false
        thirdPerson.SetActive(false);
        firstPerson.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Player"))  // Set "Player" in Edit >> Project Settings >> Input Manager. Increase size by typeing in a bigger number than shown, and add "Player" button to the name of the new button. Will need to add more if's for more than 3 characters
        {

            if (playerMode == 1)
            {
                playerMode = 0;
                thirdPerson.transform.position = firstPerson.transform.position; // makes GameObject appear at correct position
                thirdPerson.transform.rotation = firstPerson.transform.rotation; // makes GameObject appear at correct rotation
            }
            else
            {
                playerMode += 1;
                firstPerson.transform.position = thirdPerson.transform.position; // makes GameObject appear at correct position
                firstPerson.transform.rotation = thirdPerson.transform.rotation; // makes GameObject appear at correct rotation
            }


            StartCoroutine(PlayerMode());
        }
    }

    IEnumerator PlayerMode()
    {
        yield return new WaitForSeconds(.01f); // add more if (playerMode == #) for more modes modes. Only one true
        if (playerMode == 0)
        {
            thirdPerson.SetActive(true);
            firstPerson.SetActive(false);

        }
        if (playerMode == 1)
        {
            firstPerson.SetActive(true);
            thirdPerson.SetActive(false);
        }
    }
}
