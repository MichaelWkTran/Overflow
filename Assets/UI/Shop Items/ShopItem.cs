using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public string m_name; //The name of the shop item
    public uint m_price; //The price of the shop item
    public bool m_purchased; //Whether the item has been purchased

    [DisallowMultipleComponent] public abstract class ItemData : MonoBehaviour { public ShopItem m_shopItem; }
    [SerializeField] ItemData m_itemData; //The type of data from which the shop item stores
    [SerializeField] Button m_itemButton; //The button from which the player uses to purchase the item
    [SerializeField] RectTransform m_purchaseMessage; //The message that appears when the player selects that item to purchase
    [SerializeField] TMPro.TMP_Text m_priceText; //The text that shows the price of the item

    void Start()
    {
        //Print an error when m_itemData has not been set 
        if (m_itemData == null)
        {
            Debug.LogError("Any GameObject using the ShopItem component must have a component inheriting from ItemData");
        }

        //Set 
        m_priceText
    }

    void OnValidate()
    {
        //Set m_shopItem of the item data
        if (m_itemData != null) m_itemData.m_shopItem = this;
    }

    void Update()
    {
        //Disable the purchase message when the button is purchased or deselected
        if (m_purchased || EventSystem.current.currentSelectedGameObject != m_itemButton.gameObject)
            m_purchaseMessage.gameObject.SetActive(false);
    }

    public void OnPressed()
    {
        //Checks whether the player first selects the button, if so enable the purchase message
        if (!m_purchaseMessage.gameObject.activeSelf && !m_purchased)
        {
            m_purchaseMessage.gameObject.SetActive(true);
            return;
        }

        //If the purchase message is open, then purchase the item
        if (m_purchaseMessage.gameObject.activeSelf && !m_purchased) m_purchased = true;

        //Use the item
        //if (m_purchased) switch (m_itemDataType)
        //{
        //    case ItemDataType.Skin: SkinData.m_currentSkin = this; m_skin.SetSkin();  break;
        //    case ItemDataType.Hat: HatData.m_currentHat = m_hat; break;
        //    case ItemDataType.Furniture: m_furniture.m_Enabled = true; break;
        //}
    }
}
