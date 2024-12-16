using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private int souls;

    //Las imagenes donde van a estar los trozos de almas
    [SerializeField] private Image[] soulContainer;

    //Los toggles donde van a estar las marcas con los objetos. //TODO: Hacer que no se vea hasta conseguir el objecto y hacer que el inventario se vea mas bonito
    [SerializeField] private Toggle[] habilitiesCheck;

    //Mirar cuantos trozos de almas tienes
    private void CheckSouls()
    {
        souls = GameManager.Instance.GetSouls();

        for (int i = 0; i < soulContainer.Length; i++)
        {
            soulContainer[i].color = Color.black;
        }
        for (int i = 0; i < souls; i++)
        {
            soulContainer[i].color = Color.red;
        }
    }
    //Mirar si tiene las habilidades para mostrarlas en el inventario
    private void CheckHabilities()
    {
        habilitiesCheck[0].isOn = GameManager.Instance.GetHabilities("basic");
        habilitiesCheck[1].isOn = GameManager.Instance.GetHabilities("hook");
        habilitiesCheck[2].isOn = GameManager.Instance.GetHabilities("fireball");
        habilitiesCheck[3].isOn = GameManager.Instance.GetHabilities("doblejump");
        habilitiesCheck[4].isOn = GameManager.Instance.GetHabilities("walljump");
    }
    //Que mire las cosas del inventario cada vez que se active el panel
    private void OnEnable()
    {
        CheckSouls();
        CheckHabilities();
    }
}
