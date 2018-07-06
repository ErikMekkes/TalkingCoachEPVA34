using NUnit.Framework;

/// <summary>
/// Testsuite with tests of the Pair class.
/// </summary>
public class PairTest {
    
    /// <summary>
    /// Test for the getter of the first field (A).
    /// </summary>
    [Test]
    public void GetterATest() {
        const string firstParam = "test";
        var pair = new Pair<string, int>(firstParam, 1);
        Assert.AreEqual(firstParam, pair.GetA());
    }

    /// <summary>
    /// Test for the getter of the second field (B).
    /// </summary>
    [Test]
    public void GetterBTest() {
        const int secondParam = 1;
        var pair = new Pair<string, int>("", secondParam);
        Assert.AreEqual(secondParam, pair.GetB());
    }
}
