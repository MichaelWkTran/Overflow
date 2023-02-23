using UnityEngine;

public class FurnitureItem : ShopItem.ItemData
{
    public Transform m_furniturePrefab; //The prefab used to create the furniture when enabled
    [HideInInspector] public Transform m_furnitureObject; //The created furniture in the title screen

    void Start()
    {
        //Create furniture object
        m_furnitureObject = Instantiate
        (
            m_furniturePrefab,
            FindObjectOfType<GameManager>().m_titleLevelSegment.transform
        );
    }

    bool m_enabled = false;
    public bool m_Enabled
    {
        get { return m_enabled; }
        set
        {
            m_enabled = value;
            if (m_furnitureObject != null) m_furnitureObject.gameObject.SetActive(m_enabled);
        }
    }
}
