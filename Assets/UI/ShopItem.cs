using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] static public List<ShopItem> m_shopItems;
    public int m_id = -1;

    #region Item Data Structs
    [Serializable] public class ItemData
    {
        public string m_name;
        public uint m_price;
        public bool m_purchased;
    }

    [Serializable] public class SkinData : ItemData
    {
        public static SkinData m_currentSkin = null;
        public RuntimeAnimatorController m_skin;
        public Sprite m_deathSprite;

        public void SetSkin()
        {
            Player player = FindObjectOfType<Player>();
            if (player == null) return;

            player.GetComponent<Animator>().runtimeAnimatorController = m_skin;
            player.deathParticle.textureSheetAnimation.SetSprite(0, m_deathSprite);
        }
    }

    [Serializable] public class HatData : ItemData
    {
        public static HatData m_currentHat = null;
        public AnimationClip m_hatAnimation;
        public Sprite m_hatSprite;
    }

    [Serializable] public class FurnitureData : ItemData
    {
        public Transform m_furniturePrefab;

        public bool m_enabled;
    }
    #endregion

    ItemData m_itemData;
    enum ItemDataType { Skin, Hat, Furniture }
    [SerializeField] ItemDataType m_itemDataType;
    [SerializeField] SkinData m_skin;
    [SerializeField] HatData m_hat;
    [SerializeField] FurnitureData m_furniture;

    Button m_itemButton;
    [SerializeField] RectTransform m_purchaseMessage;

    void Start()
    {
        m_itemButton = GetComponent<Button>();
        switch(m_itemDataType)
        {
            case ItemDataType.Skin: m_itemData = m_skin; break;
            case ItemDataType.Hat: m_itemData = m_hat; break;
            case ItemDataType.Furniture: m_itemData = m_furniture; break;
        }
    }

    void Update()
    {
        if (m_itemData.m_purchased || EventSystem.current.currentSelectedGameObject != m_itemButton.gameObject)
            m_purchaseMessage.gameObject.SetActive(false);
    }

    public void OnPressed()
    {
        //Checks whether the player first selects the button, if so enable the purchase message
        if (!m_purchaseMessage.gameObject.activeSelf && !m_itemData.m_purchased)
        {
            m_purchaseMessage.gameObject.SetActive(true);
            return;
        }

        //If the purchase message is open, then purchase the item
        if (m_purchaseMessage.gameObject.activeSelf && !m_itemData.m_purchased)
            m_itemData.m_purchased = true;

        //Use the item
        if (m_itemData.m_purchased) switch (m_itemDataType)
        {
            case ItemDataType.Skin: SkinData.m_currentSkin = m_skin; m_skin.SetSkin();  break;
            case ItemDataType.Hat: HatData.m_currentHat = m_hat; break;
            case ItemDataType.Furniture: m_furniture.m_enabled = true; break;
        }
    }
}
