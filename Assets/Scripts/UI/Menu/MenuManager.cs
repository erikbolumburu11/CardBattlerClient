using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject initialMenu;
    GameObject currentMenu;
    LinkedList<GameObject> menuHistory;

    void Start()
    {
        initialMenu.SetActive(true);
        
        menuHistory = new();
        menuHistory.AddLast(initialMenu);
    }

    public void OpenMenu(GameObject newMenu)
    {
        menuHistory.AddLast(currentMenu);

        currentMenu.SetActive(false);
        newMenu.SetActive(true);

        currentMenu = newMenu;
    }

    public void Back()  
    {
        menuHistory.RemoveLast();
        GameObject newMenu = menuHistory.Last.Value;

        currentMenu.SetActive(false);
        newMenu.SetActive(true);

        currentMenu = newMenu;
    }
}
