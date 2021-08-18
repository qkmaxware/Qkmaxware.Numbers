namespace Qkmaxware.Numbers {

public static class IntegerExtensions {
    public static bool IsEven(this int x) {
        return x%2 == 0;
    }
    public static bool IsOdd(this int x) {
        return !x.IsEven();
    }
}

}