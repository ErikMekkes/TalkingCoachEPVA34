/// <summary>
/// Pair class for handyness.
/// </summary>
/// <typeparam name="TA"></typeparam>
/// <typeparam name="TB"></typeparam>
public class Pair<TA,TB> {

    private readonly TA _a;
    private readonly TB _b;

    /// <summary>
    /// Constructs a pair with types A and B.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public Pair(TA a, TB b) {
        _a = a;
        _b = b;
    }

    /// <summary>
    /// Returns b.
    /// </summary>
    /// <returns>A a</returns>
    public TA GetA() {
        return _a;
    }

    /// <summary>
    /// Returns b.
    /// </summary>
    /// <returns>B b</returns>
    public TB GetB() {
        return _b;
    }

}