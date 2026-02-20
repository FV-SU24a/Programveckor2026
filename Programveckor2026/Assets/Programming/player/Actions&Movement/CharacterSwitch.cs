using UnityEngine.InputSystem;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public Animator anim;
    private bool isForm2 = false;

    public void SwitchCharacters()
    {
        isForm2 = !isForm2;

        // Tell the Animator which root to switch to
        anim.SetBool("isForm2", isForm2);

        // Trigger the switch
        anim.SetTrigger("Switched");

    }

    private void Update()
    {
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            SwitchCharacters();
        }
    }
}