using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public string m_name;
    public uint m_price;
    public bool m_purchased;

    #region Item Data Structs
    [Serializable] public class SkinData
    {
        public static SkinData m_currentSkin = null;
        public RuntimeAnimatorController m_skin;
        public Sprite m_deathSprite;

        public void SetSkin()
        {
            //Find the player
            Player player = FindObjectOfType<Player>();
            
            //Stop the function if the player does not exist
            if (player == null) return;

            //Set the skin of the player
            player.GetComponent<Animator>().runtimeAnimatorController = m_skin;
            player.deathParticle.textureSheetAnimation.SetSprite(0, m_deathSprite);
        }
    }

    [Serializable] public class HatData
    {
        public static HatData m_currentHat = null;
        public AnimationClip m_hatAnimation;
        public Sprite m_hatSprite;
    }

    [Serializable] public class FurnitureData
    {
        public Transform m_furniturePrefab; //The prefab used to create the furniture when enabled
        Transform m_furnitureObject; //The created object from the prefab, used to destroy it when disabled
        private bool m_enabled = false;
        public bool m_Enabled
        {
            get { return m_enabled; }
            set
            {
                m_enabled = value;
                if (m_enabled)
                {

                }
            }
        }
    }
    #endregion

    public enum ItemDataType { Skin, Hat, Furniture }
    public ItemDataType m_itemDataType;
    [SerializeField] SkinData m_skin;
    [SerializeField] HatData m_hat;
    [SerializeField] FurnitureData m_furniture;

    Button m_itemButton;
    [SerializeField] RectTransform m_purchaseMessage;

    void Start()
    {
        m_itemButton = GetComponent<Button>();
    }

    void Update()
    {
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
        if (m_purchased) switch (m_itemDataType)
        {
            case ItemDataType.Skin: SkinData.m_currentSkin = m_skin; m_skin.SetSkin();  break;
            case ItemDataType.Hat: HatData.m_currentHat = m_hat; break;
            case ItemDataType.Furniture: m_furniture.m_Enabled = true; break;
        }
    }
}
