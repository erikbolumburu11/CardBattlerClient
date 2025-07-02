using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject initialMenu;
    [SerializeField] GameObject backButton;

    public GameObject mainMenu;
    public GameObject loginScreen;

    GameObject currentMenu;
    Stack<GameObject> menuHistory;

    public static MenuManager instance;

    void Awake()
    {
        if(instance != null){
            Destroy(gameObject);
            return;
        }
        else {
            instance = this;
        }
    }

    void Start()
    {
        currentMenu = initialMenu;
        menuHistory = new();
        OpenMenu(initialMenu, false);
    }

    public void ClearHistory(){
        menuHistory.Clear();
    }

    public void OpenMenu(GameObject newMenu)
    {
        OpenMenu(newMenu, true);
    }

    public void OpenMenu(GameObject newMenu, bool addToHistory = true)
    {
        if(addToHistory) menuHistory.Push(currentMenu);

        currentMenu.SetActive(false);
        newMenu.SetActive(true);

        currentMenu = newMenu;

        UpdateBackVisibilty();
    }

    public void Back()  
    {
        if(menuHistory.Count <= 1) return;
        OpenMenu(menuHistory.Pop(), false);
    }

    void UpdateBackVisibilty(){
        if(menuHistory.Count == 1) backButton.SetActive(false);
        else backButton.SetActive(true);
    }
}
