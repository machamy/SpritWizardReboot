using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>You must approach through `GoogleSheetManager.SO<GoogleSheetSO>()`</summary>
public class GoogleSheetSO : ScriptableObject
{
	public List<RawCombat> RawCombatList;
	public List<RawRewardChance> RawRewardChanceList;
	public List<RawAddCardWeight> RawAddCardWeightList;
	public List<RawStorePrice> RawStorePriceList;
	public List<RawRuneCard> RawRuneCardList;
	public List<RawSmithedRuneCard> RawSmithedRuneCardList;
	public List<RawMagicCard> RawMagicCardList;
	public List<RawSmithedMagicCard> RawSmithedMagicCardList;
	public List<RawSpecialEffect> RawSpecialEffectList;
	public List<RawSpecialEffectDescribe> RawSpecialEffectDescribeList;
	public List<RawMonster> RawMonsterList;
	public List<RawMonsterPattern> RawMonsterPatternList;
	public List<RawMonsterAction> RawMonsterActionList;
}

[Serializable]
public class RawCombat
{
	/// <summary>대문 체력</summary>
	public int gateHp;
}

[Serializable]
public class RawRewardChance
{
	/// <summary>보상 종류</summary>
	public string rewardType;
	/// <summary>룬 카드추가 등장 확률</summary>
	public int addRuneCardChance;
	/// <summary>마법 카드추가 등장 확률</summary>
	public int addMagicCardChance;
	/// <summary>카드 제거 등장 확률</summary>
	public int destroyCardChance;
	/// <summary>카드 강화 등장 확률</summary>
	public int upgradeCardChance;
	/// <summary>대문 체력 회복 등장 확률</summary>
	public int gateHpRestoreChance;
	/// <summary>대문 체력 회복량 중간값</summary>
	public int gateHpRestoreAmountMiddle;
	/// <summary>대문 체력 회복량 폭</summary>
	public int gateHpRestoreAmountVariation;
	/// <summary>골드 중간값</summary>
	public int goldMiddle;
	/// <summary>골드 폭</summary>
	public int goldVariation;
}

[Serializable]
public class RawAddCardWeight
{
	/// <summary>희귀카드 가중치</summary>
	public int rareCardWeight;
	/// <summary>일반카드 가중치</summary>
	public int commonCardWeight;
}

[Serializable]
public class RawStorePrice
{
	/// <summary>가격 중간값</summary>
	public int priceMiddle;
	/// <summary>가격 폭</summary>
	public int priceRange;
}

[Serializable]
public class RawRuneCard
{
	/// <summary></summary>
	public string id;
	/// <summary>이름</summary>
	public string name;
	/// <summary>설명</summary>
	public string describe;
	/// <summary>비용</summary>
	public int cost;
	/// <summary>곱할거면 MUL</summary>
	public string calcType;
	/// <summary>피해량</summary>
	public int attack;
	/// <summary>투사체</summary>
	public int projectile;
	/// <summary>이동</summary>
	public int movement;
}

[Serializable]
public class RawSmithedRuneCard
{
	/// <summary></summary>
	public string id;
	/// <summary>이름</summary>
	public string name;
	/// <summary>설명</summary>
	public string describe;
	/// <summary>비용</summary>
	public int cost;
	/// <summary>곱할거면 MUL</summary>
	public string calcType;
	/// <summary>피해량</summary>
	public int attack;
	/// <summary>투사체</summary>
	public int projectile;
	/// <summary>이동</summary>
	public int movement;
}

[Serializable]
public class RawMagicCard
{
	/// <summary></summary>
	public string id;
	/// <summary></summary>
	public string name;
	/// <summary></summary>
	public string describe;
	/// <summary></summary>
	public string skillCaster;
	/// <summary></summary>
	public string rarity;
	/// <summary>피해량</summary>
	public int attackDamage;
	/// <summary>공격횟수</summary>
	public int attackCount;
	/// <summary>공격방식</summary>
	public string attackType;
	/// <summary>공격범위(세로)</summary>
	public int attackHeight;
	/// <summary>공격범위(가로)</summary>
	public int attackWidth;
	/// <summary>집중/퍼짐</summary>
	public string attackSpread;
	/// <summary>퍼짐 정도</summary>
	public int spreadRange;
	/// <summary>관통</summary>
	public int pierce;
	/// <summary>이동</summary>
	public int move;
	/// <summary>비용</summary>
	public int cost;
	/// <summary>특수효과</summary>
	public string specialEffectId;
}

[Serializable]
public class RawSmithedMagicCard
{
	/// <summary></summary>
	public string id;
	/// <summary></summary>
	public string name;
	/// <summary></summary>
	public string describe;
	/// <summary></summary>
	public string skillCaster;
	/// <summary></summary>
	public string rarity;
	/// <summary>피해량</summary>
	public int attackDamage;
	/// <summary>공격횟수</summary>
	public int attackCount;
	/// <summary>공격방식</summary>
	public string attackType;
	/// <summary>공격범위(세로)</summary>
	public int attackHeight;
	/// <summary>공격범위(가로)</summary>
	public int attackWidth;
	/// <summary>집중/퍼짐</summary>
	public string attackSpread;
	/// <summary>퍼짐 정도</summary>
	public int spreadRange;
	/// <summary>관통</summary>
	public int pierce;
	/// <summary>이동</summary>
	public int move;
	/// <summary>비용</summary>
	public int cost;
	/// <summary>특수효과</summary>
	public string specialEffectId;
}

[Serializable]
public class RawSpecialEffect
{
	/// <summary>특수효과 아이디</summary>
	public int id;
	/// <summary>효과 이름</summary>
	public string effect;
	/// <summary>값</summary>
	public int value;
}

[Serializable]
public class RawSpecialEffectDescribe
{
	/// <summary>이름</summary>
	public string name;
	/// <summary>설명</summary>
	public string describe;
}

[Serializable]
public class RawMonster
{
	/// <summary>이름</summary>
	public string name;
	/// <summary>id</summary>
	public int id;
	/// <summary>스프라이트</summary>
	public string sprite;
	/// <summary>크기</summary>
	public int size;
	/// <summary>체력 중앙값</summary>
	public int hpMiddle;
	/// <summary>체력 폭</summary>
	public int hpVariation;
	/// <summary>패턴</summary>
	public string pattern;
	/// <summary>최초 패턴</summary>
	public string initPattern;
	/// <summary>페이즈 전환 비율</summary>
	public int phaseSwitchHpRatio;
}

[Serializable]
public class RawMonsterPattern
{
	/// <summary>소속 몬스터</summary>
	public string sosok;
	/// <summary>용도 메모</summary>
	public string dragonSwordMemo;
	/// <summary>id</summary>
	public string id;
	/// <summary>사거리</summary>
	public int range;
	/// <summary>행동 순서</summary>
	public string actionSequence;
	/// <summary>1페/2페</summary>
	public int phase;
	/// <summary>가중치</summary>
	public int weight;
}

[Serializable]
public class RawMonsterAction
{
	/// <summary>소속 몬스터</summary>
	public string sosok;
	/// <summary>용도 메모</summary>
	public string dragonSwordMemo;
	/// <summary>id</summary>
	public string id;
	/// <summary>행동 형태</summary>
	public string action;
	/// <summary></summary>
	public int value;
}

