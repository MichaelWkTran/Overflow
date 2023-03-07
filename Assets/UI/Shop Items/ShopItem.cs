using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public string m_name; //The name of the shop item
    public uint m_price; //The price of the shop item
    [SerializeField] bool m_purchased; //Whether the item has been purchased
    public bool m_Purchased
    {
        get { return m_purchased; }
        set { m_priceText.gameObject.SetActive(!(m_purchased = value)); }
    }

    [DisallowMultipleComponent] public abstract class ItemData : MonoBehaviour
    {
        protected ShopItem m_shopItem;
        virtual protected void Start()
        {
            m_shopItem = GetComponent<ShopItem>();
            Load();
        }
        public abstract void OnClick();
        protected abstract void Load();
    }

    ItemData m_itemData; //The type of data from which the shop item stores
    [SerializeField] Button m_itemButton; //The button from which the player uses to purchase the item
    [SerializeField] RectTransform m_purchaseMessage; //The message that appears when the player selects that item to purchase
    [SerializeField] TMPro.TMP_Text m_priceText; //The text that shows the price of the item

    void Start()
    {
        m_itemData = GetComponent<ItemData>();

        //Load shop item data
        Load();

        //Display the price of the shop item
        m_priceText.text = m_price.ToString();
    }

    void Update()
    {
        //Disable the shop item button when the player does not have enough carrots
        m_itemButton.interactable = m_Purchased || m_price <= SaveSystem.m_data.m_carrots;

        //Disable the purchase message when the button is purchased or deselected
        if (m_Purchased || EventSystem.current.currentSelectedGameObject != m_itemButton.gameObject)
            m_purchaseMessage.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        //Checks whether the player first selects the button, if so enable the purchase message
        if (!m_purchaseMessage.gameObject.activeSelf && !m_Purchased)
        {
            m_purchaseMessage.gameObject.SetActive(true);
            return;
        }

        //If the purchase message is open, then purchase the item
        if (m_purchaseMessage.gameObject.activeSelf && !m_Purchased)
        {
            SaveSystem.m_data.m_carrots -= m_price;
            m_Purchased = true;

            //Save Data
            SaveSystem.m_data.AddPurchasedItem(m_name);
        }

        //Use the item
        if (m_Purchased) m_itemData.OnClick();
    }

    void Load()
    {
        //Load the data of whether the player previously purchased this shop item
        m_Purchased = SaveSystem.m_data.m_purchasedItems.Contains(m_name);
    }
}
