using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Tabs List")]
    public List<Tab> TabsInfo;
    [Header("Parents of Lists")]
    public GameObject TypesParent;
    public GameObject SubTypesParent;
    public GameObject ColorPalleteBar;
    public GameObject ItemsParent;
    public GameObject ColorPallete;
    public GameObject Tabs;
    
    private Button ActiveButton;
    private Sprite defaultstate;
    private Button ChildActive;
    private GameObject temp,temp2;
    private bool flag, flag2,flag3;
    private float x, y;

    void Start()
    {
        
        
        CreateTypes(0);
        GenerateSubTypes(TabsInfo[0].TypesIcon[0]);
        GenerateItems(TabsInfo[0].TypesIcon[0].SubTypes[0], null);
    }

    

    public void CreateTypes(int t)
    {
        flag2 = false;
        DestroyPreviousObjects(TypesParent.transform);

        foreach (Types types in TabsInfo[t].TypesIcon)
        {
                         
            GameObject Clone = Instantiate(types.Icon.gameObject, TypesParent.transform);
            Clone.GetComponent<Image>().sprite = types.defaultState;
            SpriteState sprite = new SpriteState();
            sprite.selectedSprite = types.PressedState;
            sprite.pressedSprite = types.PressedState;
            sprite.highlightedSprite = types.PressedState;
            Clone.GetComponent<Button>().spriteState = sprite;
            //Clone.GetComponent<Button>().group = TypesParent.GetComponent<ToggleGroup>();
            //Debug.Log("HasColorPallete?"+ types.HasColorPallete);
            Clone.GetComponent<Button>().onClick.AddListener(() => ChangeImage(Clone.GetComponent<Button>(),types));
            if (types.HasSubTypes)
            {
                Clone.GetComponent<Button>().onClick.AddListener(() => GenerateSubTypes(types));
            }
            else
            {
                Clone.GetComponent<Button>().onClick.AddListener(() => GenerateItems(types));
            }
            if (!flag2)
            {
                temp = Clone;
                flag2 = true;
            }
               

            //Debug.Log(TypesParent.transform.GetChild(0).GetComponent<Button>().spriteState);
            //EventSystem.current.SetSelectedGameObject(TypesParent.transform.GetChild(0).gameObject);
        }

        Debug.Log("Going in Invoke");
        if(temp)
        temp.GetComponent<Button>().onClick.Invoke();
        //TypesParent.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
        //TypesParent.transform.GetChild(0).GetComponent<Button>().Select();
        

    }

    public void GenerateSubTypes(Types t)
    {
        flag3 = false;
        Debug.Log("Type Name: "+t.TypeName);
        Debug.Log("After Invoke");

        ColorPallete.transform.parent.gameObject.SetActive(false);
        ItemsParent.transform.parent.gameObject.SetActive(true);
        SetSize(26.00435f, 0.00022f);
        SubTypesParent.transform.parent.gameObject.SetActive(true);
        if (t.HasColorPallete)
        {
            SetSize(26.00435f, 95.82499f);
            ColorPalleteBar.SetActive(true);           
            GenerateColors(t.Color.gameObject);
        }
        else
        {
            //SetSize(0, 0);
            ColorPalleteBar.SetActive(false);
        }

        DestroyPreviousObjects(SubTypesParent.transform);       
        foreach (var s in t.SubTypes)
        {
            //SetSize
            GameObject Clone = Instantiate(s.TextButton.gameObject, SubTypesParent.transform);
            Clone.GetComponent<Button>().onClick.AddListener(() => GenerateItems(s,Clone.GetComponent<Button>()));
            Clone.GetComponent<TMP_Text>().SetText(s.SubTypeName);
            if (!flag3)
            {
                temp2 = Clone;
                flag3 = true;
            }
        }
       

        //SubTypesParent.transform.GetChild(0).GetComponent<Button>().Select();
        //Invoke("SubTypesParent.transform.GetChild(0).GetComponent<Button>().onClick", 5f);
        temp2.GetComponent<Button>().onClick.Invoke();
        Debug.Log("Invoking On Click");

    }

    public void GenerateItems(SubType s,Button b)
    {
        Debug.Log("Inside Function");
        ItemsParent.GetComponent<AutoExpandGridLayoutGroup>().cellSize = new Vector2(106f, 165f);
        ItemsParent.GetComponent<AutoExpandGridLayoutGroup>().spacing = new Vector2(35f, 0);
        ItemsParent.GetComponent<AutoExpandGridLayoutGroup>().padding.left = 65;
        if (b != null)
        {
            if (ChildActive != null)
                ChildActive.GetComponent<TMP_Text>().color = new Color32(178, 178, 178, 255);
            b.gameObject.GetComponent<TMP_Text>().color = new Color32(166, 0, 218, 255);
            ChildActive = b;
        }

        DestroyPreviousObjects(ItemsParent.transform);
        foreach (Prefab pf in s.items)
        {
            GameObject Clone = Instantiate(pf.itemPrefab.gameObject, ItemsParent.transform);
            Clone.GetComponent<ItemScript>().locked.SetActive(pf.locked);
            Clone.GetComponent<ItemScript>().cost.text = pf.cost.ToString();
            Clone.GetComponent<ItemScript>().img = pf.img;
        }
    }

    public void GenerateItems(Types t)
    {
        Debug.Log("TypeName" + t.TypeName);
      
        if (t.TypeName == "Body")
        {
            Debug.Log("Should work");
           
            ItemsParent.GetComponent<AutoExpandGridLayoutGroup>().cellSize = new Vector2(119f, 290f);
            ItemsParent.GetComponent<AutoExpandGridLayoutGroup>().spacing = new Vector2(35, 35);
            ItemsParent.GetComponent<AutoExpandGridLayoutGroup>().padding.left = 35;
        }
        else
        {
            ItemsParent.GetComponent<AutoExpandGridLayoutGroup>().cellSize = new Vector2(106f, 165f);
            ItemsParent.GetComponent<AutoExpandGridLayoutGroup>().spacing = new Vector2(35f, 0);
            ItemsParent.GetComponent<AutoExpandGridLayoutGroup>().padding.left = 65;
        }        
    
        SubTypesParent.transform.parent.gameObject.SetActive(false);
        SetSize(-27.84572f, -0.0022f);
        ColorPalleteBar.SetActive(false);
        DestroyPreviousObjects(SubTypesParent.transform);
        DestroyPreviousObjects(ItemsParent.transform);
        foreach (Prefab pf in t.Typesitems)
        {
            //SetSize
            GameObject Clone = Instantiate(pf.itemPrefab.gameObject, ItemsParent.transform);
            Clone.GetComponent<ItemScript>().locked.SetActive(pf.locked);
            Clone.GetComponent<ItemScript>().cost.text = pf.cost.ToString();
            Clone.GetComponent<ItemScript>().img = pf.img; 
        }
    }

    public void DestroyPreviousObjects(Transform Parent)
    {
        foreach (Transform child in Parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ChangeImage(Button but, Types t)
    {
        if (but.image.sprite == t.defaultState)
        {
            if(ActiveButton!=null)
                ActiveButton.image.sprite = defaultstate;
            but.image.sprite = t.PressedState;
            defaultstate = t.defaultState;
            ActiveButton = but;
        }            
    }
    public void GenerateColors(GameObject Color)
    {
        GameObject Clone = Instantiate(Color, ColorPallete.transform);
    }

    public void ToggleColor()
    {
        if (flag)
        {
            ItemsParent.transform.parent.gameObject.SetActive(false);
            ColorPallete.transform.parent.gameObject.SetActive(true);
            flag = false;
        }
        else
        {
            ItemsParent.transform.parent.gameObject.SetActive(true);
            ColorPallete.transform.parent.gameObject.SetActive(false);
            flag = true;
        }
    }

    

    void SetSize(float top, float bottom)
    {
        RectTransform Panel = ItemsParent.transform.parent.GetComponent<RectTransform>();
        RectTransformExtensions.SetTop(Panel, top);
        RectTransformExtensions.SetBottom(Panel, bottom);

    }
}


