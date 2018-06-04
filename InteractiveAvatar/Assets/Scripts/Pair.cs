/// <summary>
/// Pair class for handyness.
/// </summary>
/// <typeparam name="A"></typeparam>
/// <typeparam name="B"></typeparam>
public class Pair<A,B> {

    private A a;
    private B b;

    /// <summary>
    /// Constructs a pair with types A and B.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public Pair(A a, B b) {
        this.a = a;
        this.b = b;
    }

    /// <summary>
    /// Returns b.
    /// </summary>
    /// <returns>A a</returns>
    public A GetA() {
        return a;
    }

    /// <summary>
    /// Returns b.
    /// </summary>
    /// <returns>B b</returns>
    public B GetB() {
        return b;
    }

}