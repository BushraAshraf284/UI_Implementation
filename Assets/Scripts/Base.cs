using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class User
{
    public int ID;
    public int loggedIn;
    public string name;
    public string email;
    public string phone;
    public string password;
}

[System.Serializable]
public class Root
{
    public List<User> Users;
}
[System.Serializable]
public class PanelSetting
{
    public List<Panels> NestedList;
   // public List<Panels> Panel; // do not do this
   
}
[System.Serializable]
public class Location
{
    public int ID;
    public string latitude;
    public string longitude;
}

[System.Serializable]
public class Locations
{

    public List<Location> Drop;
}

[System.Serializable]
public class Types
{
    public string TypeName;
    public Sprite defaultState;
    public Sprite PressedState;
    public bool HasSubTypes;
    public bool HasColorPallete;
    public Button Color;
    public List<SubType> SubTypes;  
    public Button Icon;
    public List<Prefab> Typesitems;
    //generate on runtime

}
[System.Serializable]
public class SubType
{
    public string SubTypeName;   
    public Button TextButton;   
    public List<Prefab> items;
    
}

[System.Serializable]
public class Prefab
{
    public Image img;
    public Button itemPrefab;
    public bool locked;
    public int cost;
}
[System.Serializable]
public class Tab
{
    public TABSNAME name;
    public List<Types> TypesIcon;
}

public enum TABSNAME:int
{
    Character,
    Dress,
    Items,
    Updates
}
[System.Serializable]
[SerializeField]
public enum Panels
{
    SELECTLOGIN =0,
    LOGIN =1,
    SIGNUP =2,
    MOBILE = 3,
    EMAIL = 4,
    CODE = 5,
    PASSWORD = 6,
    WALLETCONNECT= 7,
    NAME = 8,
    RECOVERY = 9,
    WELCOME =10
   
}

/*Server Side Get & Set Data*/

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

/*[System.Serializable]
public class Iron
{
    public string Name;
    public int Price;
    public int LevelToUnlock;
}

[System.Serializable]
public class Bronze
{
    public string Name;
    public int Price;
    public int LevelToUnlock;
}

[System.Serializable]
public class Silver
{
    public string Name ;
    public int Price ;
    public int LevelToUnlock ;
}

[System.Serializable]
public class Gold
{   
    public string Name ;
    public int Price ;
    public int LevelToUnlock ;
}

*//*[System.Serializable]
public class Item1 {
    public string Name;
    public int Price;
    public int LevelToUnlock;
}*//*

[System.Serializable]
public class Reward
{*//*
    public string name;
    public Item1 item;*//*

    public List<Iron> Iron ;
    public List<Bronze> Bronze ;
    public List<Silver> Silver ;
    public List<Gold> Gold ;
}

[System.Serializable]
public class RewardsInfo
{
    public List<Reward> Rewards ;
}*/

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
[System.Serializable]
public class Item
{
    public string Name ;
    public int Price ;
    public int LevelToUnlock ;
}
[System.Serializable]
public class Reward
{
    public string Type ;
    public List<Item> Items;
}
[System.Serializable]
public class RewardsInfo
{
    public List<Reward> Rewards ;
}






