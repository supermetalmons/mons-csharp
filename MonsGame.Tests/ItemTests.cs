// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class ItemTests
{
    [Fact]
    public void MonItem_CreatesMonItemCorrectly()
    {
        var mon = new Mon(Mon.Kind.Demon, Color.White); // Assuming these enums and structs are defined
        var item = Item.MonItem(mon);

        Assert.Equal(ItemType.Mon, item.Type);
        Assert.Equal(mon, item.Mon);
        Assert.Equal(default(Mana), item.Mana);
        Assert.Equal(default(Consumable), item.Consumable);
    }

    [Fact]
    public void ManaItem_CreatesManaItemCorrectly()
    {
        var mana = Mana.Regular(Color.Black);
        var item = Item.ManaItem(mana);

        Assert.Equal(ItemType.Mana, item.Type);
        Assert.Equal(default(Mon), item.Mon);
        Assert.Equal(mana, item.Mana);
        Assert.Equal(default(Consumable), item.Consumable);
    }

    [Fact]
    public void MonProperty_ReturnsMonForMonItem()
    {
        var mon = new Mon(Mon.Kind.Angel, Color.White);
        var item = Item.MonItem(mon);

        Assert.Equal(mon, item.MonProperty);
    }

    // Similar tests for ManaProperty and ConsumableProperty

    [Fact]
    public void Equals_ReturnsTrueForEqualItems()
    {
        var mon = new Mon(Mon.Kind.Demon, Color.Black);
        var item1 = Item.MonItem(mon);
        var item2 = Item.MonItem(mon);

        Assert.True(item1.Equals(item2));
    }

    [Fact]
    public void Equals_ReturnsFalseForDifferentItems()
    {
        var mon = new Mon(Mon.Kind.Demon, Color.Black);
        var mana = Mana.Regular(Color.White);
        var item1 = Item.MonItem(mon);
        var item2 = Item.ManaItem(mana);

        Assert.False(item1.Equals(item2));
    }

    [Fact]
    public void EqualityOperators_WorkCorrectly()
    {
        var mon = new Mon(Mon.Kind.Spirit, Color.White);
        var item1 = Item.MonItem(mon);
        var item2 = Item.MonItem(mon);
        var item3 = Item.ManaItem(Mana.Regular(Color.Black));

        Assert.True(item1 == item2);
        Assert.False(item1 == item3);
        Assert.False(item1 != item2);
        Assert.True(item1 != item3);
    }

    [Fact]
    public void GetHashCode_ReturnsSameValueForEqualObjects()
    {
        var mon = new Mon(Mon.Kind.Demon, Color.White);
        var item1 = Item.MonItem(mon);
        var item2 = Item.MonItem(mon);

        Assert.Equal(item1.GetHashCode(), item2.GetHashCode());
    }
}
