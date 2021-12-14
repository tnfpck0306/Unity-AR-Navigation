using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class checkName : MonoBehaviour
{
    //Attach this script to a Dropdown GameObject
    [SerializeField] Dropdown m_Dropdown;

    public Canvas lifeStyle_canvas;
    public Canvas restaurant_canvas;
    public Canvas beauty_canvas;
    public Canvas service_canvas;
    public Canvas superMarket_canvas;
    public Canvas entertainment_canvas;
    public Canvas kids_canvas;
    public Canvas clothes_canvas;
    public Canvas others_canvas;
    public Canvas home_canvas;
    public Canvas AR_canvas;
    public Canvas category_Canvas;

    public CanvasGroup lifeStyle;
    public CanvasGroup restaurant;
    public CanvasGroup beauty;
    public CanvasGroup service;
    public CanvasGroup superMarket;
    public CanvasGroup entertainment;
    public CanvasGroup kids;
    public CanvasGroup clothes;
    public CanvasGroup others;
    public CanvasGroup home;
    public CanvasGroup AR;
    public CanvasGroup category;

    public Canvas currCanvas;
    public CanvasGroup currCanvasGroup;

    public Transform transform;
    void Start()
    {
        //Fetch the DropDown component from the GameObject
        m_Dropdown = GetComponent<Dropdown>();
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_Dropdown);
        });

        category.alpha = 0;
        category_Canvas.enabled = false;
        lifeStyle.alpha = 0;
        lifeStyle_canvas.enabled = false;
        restaurant.alpha = 0;
        restaurant_canvas.enabled = false;
        beauty.alpha = 0;
        beauty_canvas.enabled = false;
        service.alpha = 0;
        service_canvas.enabled = false;
        superMarket.alpha = 0;
        superMarket_canvas.enabled = false;
        entertainment.alpha = 0;
        entertainment_canvas.enabled = false;
        kids.alpha = 0;
        kids_canvas.enabled = false;
        clothes.alpha = 0;
        clothes_canvas.enabled = false;
        others.alpha = 0;
        others_canvas.enabled = false;
        home.alpha = 0;
        home_canvas.enabled = false;

        currCanvas = AR_canvas;
        currCanvasGroup = AR;
        AR.alpha = 1;
        AR.interactable = true;
        AR_canvas.enabled = true;
        AR.transform.SetAsLastSibling();
    }

    void Update()
    {
        
    }

    public void DropdownValueChanged(Dropdown change)
    {
        switch (change.value)
        {
            case (0)://라이프스타일
                lifeStyle.alpha = 1;
                lifeStyle.interactable = true;
                lifeStyle_canvas.enabled = true;
                lifeStyle.transform.SetAsLastSibling();
                currCanvasGroup.alpha = 0;
                currCanvasGroup.interactable = false;
                currCanvas.enabled = false;
                currCanvas = lifeStyle_canvas;
                currCanvasGroup = lifeStyle;
                break;
            case (1)://레스토랑 카페
                restaurant.alpha = 1;
                restaurant.interactable = true;
                restaurant_canvas.enabled = true;
                restaurant.transform.SetAsLastSibling();
                currCanvasGroup.alpha = 0;
                currCanvasGroup.interactable = false;
                currCanvas.enabled = false;
                currCanvas = restaurant_canvas;
                currCanvasGroup = restaurant;
                break;
            case (2)://뷰티
                beauty.alpha = 1;
                beauty.interactable = true;
                beauty_canvas.enabled = true;
                beauty.transform.SetAsLastSibling();
                currCanvasGroup.alpha = 0;
                currCanvasGroup.interactable = false;
                currCanvas.enabled = false;
                currCanvas = beauty_canvas;
                currCanvasGroup = beauty;
                break;
            case (3)://서비스
                service.alpha = 1;
                service.interactable = true;
                service_canvas.enabled = true;
                service.transform.SetAsLastSibling();
                currCanvasGroup.alpha = 0;
                currCanvasGroup.interactable = false;
                currCanvas.enabled = false;
                currCanvas = service_canvas;
                currCanvasGroup = service;
                break;
            case (4)://슈퍼마켓
                superMarket.alpha = 1;
                superMarket.interactable = true;
                superMarket_canvas.enabled = true;
                superMarket.transform.SetAsLastSibling();
                currCanvasGroup.alpha = 0;
                currCanvasGroup.interactable = false;
                currCanvas.enabled = false;
                currCanvas = superMarket_canvas;
                currCanvasGroup = superMarket;
                break;
            case (5)://엔터테인먼트
                entertainment.alpha = 1;
                entertainment.interactable = true;
                entertainment_canvas.enabled = true;
                entertainment.transform.SetAsLastSibling();
                currCanvasGroup.alpha = 0;
                currCanvasGroup.interactable = false;
                currCanvas.enabled = false;
                currCanvas = entertainment_canvas;
                currCanvasGroup = entertainment;
                break;
            case (6)://키즈
                kids.alpha = 1;
                kids.interactable = true;
                kids_canvas.enabled = true;
                kids.transform.SetAsLastSibling();
                currCanvasGroup.alpha = 0;
                currCanvasGroup.interactable = false;
                currCanvas.enabled = false;
                currCanvas = kids_canvas;
                currCanvasGroup = kids;
                break;
            case (7)://패션의류
                clothes.alpha = 1;
                clothes.interactable = true;
                clothes_canvas.enabled = true;
                clothes.transform.SetAsLastSibling();
                currCanvasGroup.alpha = 0;
                currCanvasGroup.interactable = false;
                currCanvas.enabled = false;
                currCanvas = clothes_canvas;
                currCanvasGroup = clothes;
                break;
            case (8)://패션잡화
                others.alpha = 1;
                others.interactable = true;
                others_canvas.enabled = true;
                others.transform.SetAsLastSibling();
                currCanvasGroup.alpha = 0;
                currCanvasGroup.interactable = false;
                currCanvas.enabled = false;
                currCanvas = others_canvas;
                currCanvasGroup = others;
                break;
            case (9)://홈퍼니싱
                home.alpha = 1;
                home.interactable = true;
                home_canvas.enabled = true;
                home.transform.SetAsLastSibling();
                currCanvasGroup.alpha = 0;
                currCanvasGroup.interactable = false;
                currCanvas.enabled = false;
                currCanvas = home_canvas;
                currCanvasGroup = home;
                
                break;
            default:
                break;
        }
        Debug.Log("Dropdown Value : " + change.value);
    }

    public void onChangeScene1to2()
    {
        category.alpha = 1;
        category.interactable = true;
        category_Canvas.enabled = true;
        category.transform.SetAsLastSibling();
        lifeStyle.alpha = 1;
        lifeStyle.interactable = true;
        lifeStyle_canvas.enabled = true;
        lifeStyle.transform.SetAsLastSibling();
        currCanvasGroup.alpha = 0;
        currCanvasGroup.interactable = false;
        currCanvas.enabled = false;
        currCanvas = lifeStyle_canvas;
        currCanvasGroup = lifeStyle;
    }

    public void onChangeScene2to1()
    {
       
        AR.alpha = 1;
        AR.interactable = true;
        AR_canvas.enabled = true;
        AR.transform.SetAsLastSibling();
        category.alpha = 0;
        category.interactable = false;
        category_Canvas.enabled = false;
        currCanvasGroup.alpha = 0;
        currCanvasGroup.interactable = false;
        currCanvas.enabled = false;
        currCanvas = AR_canvas;
        currCanvasGroup = AR;
    }


    public void OnClickCategory()
    {
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        string id = clickObj.ToString();
        int len = id.Length;
        len = len - 25;
        id = id.Substring(0, len);
        Debug.Log(id);
    }

}

