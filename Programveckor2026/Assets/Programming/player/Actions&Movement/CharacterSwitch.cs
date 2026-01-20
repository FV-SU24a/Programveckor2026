using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitch : MonoBehaviour
{
    public Animator anim;
    private bool isForm2 = false;

    public void SwitchCharacters()
    {
        isForm2 = !isForm2;

        anim.SetBool("isForm2", isForm2);

        anim.SetTrigger("Switched");

        anim.Rebind();
        anim.Update(0f);

    }


    private void Update()
    {
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            SwitchCharacters();
        }
    }


}




