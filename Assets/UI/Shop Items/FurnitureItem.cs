using UnityEngine;
using static SaveSystem;

public class FurnitureItem : ShopItem.ItemData
{
    public Transform m_furniturePrefab; //The prefab used to create the furniture when enabled
    [HideInInspector] public Transform m_furnitureObject; //The created furniture in the title screen
    bool m_enabled = false; //Whether the furniture is shown on the title screen
    public bool m_Enabled
    {
        get { return m_enabled; }
        set
        {
            m_enabled = value;

            //Show the furniture object or not
            if (m_furnitureObject != null) m_furnitureObject.gameObject.SetActive(m_enabled);
            
            //Save data on whether the furniture is enabled
            if (m_enabled) m_data.m_enabledFurniture.Add(m_shopItem.m_name);
            else m_data.m_enabledFurniture.Remove(m_shopItem.m_name);
        }
    }

    protected override void Start()
    {
        base.Start();

        //Create furniture object
        m_furnitureObject = Instantiate
        (
            m_furniturePrefab,
            FindObjectOfType<GameManager>().m_titleLevelSegment.transform
        );

        //Set whether the furniture is enabled
        m_furnitureObject.gameObject.SetActive(m_Enabled);
    }

    public override void OnClick()
    {
        m_Enabled = true;
    }

    protected override void Load()
    {
        //Load data on whether the player had enabled this furniture
        if (m_data.m_enabledFurniture.Contains(m_shopItem.name)) m_Enabled = true;
    }
}
