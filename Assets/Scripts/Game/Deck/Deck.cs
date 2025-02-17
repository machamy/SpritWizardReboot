
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    private List<CardMetaData> _container;
    public List<CardMetaData> Reference => _container;
    [SerializeField] private IntVariableSO deckSize;
    public Deck()
    {
         _container = new List<CardMetaData>();
    }

    public Deck(List<CardMetaData> cardDataList)
    {
         _container = cardDataList;
         
    }

    public void SetCardList(List<CardMetaData> cardDataList)
    {
         _container = cardDataList;
         OnChanged();
    }

    public void AddCard(CardMetaData cardData)
    {
        _container.Add(cardData);
        OnChanged();
    }

    public void RemoveCard(CardMetaData cardData)
    {
        _container.Remove(cardData);
        OnChanged();
    }

    private void OnChanged()
    {
        if(deckSize != null)
            deckSize.Value = _container.Count;
    }

    public List<CardMetaData> GetCopidList(){
      return new List<CardMetaData>(_container);
    }

}
