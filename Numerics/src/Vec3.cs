using System;

namespace Qkmaxware.Numbers {

/// <summary>
/// Abstract vector of 3 dimensions
/// </summary>
/// <typeparam name="T">quantity type for each axis</typeparam>
public class Vec3<T> where T:INegatable<T>, ISquareRootable<T>, IAddable<T,T>, ISubtractable<T,T>, IMultiplyable<T,T>, IDividable<T,T>  {
    /// <summary>
    /// X Coordinate
    /// </summary>
    public T X {get; private set;}
    /// <summary>
    /// Y Coordinate
    /// </summary>
    public T Y {get; private set;}
    /// <summary>
    /// Z Coordinate
    /// </summary>
    public T Z {get; private set;}

    /// <summary>
    /// Squared length of the vector
    /// </summary>    
    public T SqrLength {
        get {
            var xx = X.MultiplyBy(X);
            var yy = Y.MultiplyBy(Y);
            var zz = Z.MultiplyBy(Z);
            return xx.Add(yy).Add(zz);
        }
    }
    /// <summary>
    /// Length of the vector
    /// </summary>
    public T Length => SqrLength.Sqrt();

    /// <summary>
    /// Get a vector of unit length in the same direction
    /// </summary>
    public Vec3<T> Normalized {
        get {
            var mag = this.Length;
            return this / mag;
        }
    }

    /// <summary>
    /// Get a vector pointed in the opposite direction
    /// </summary>
    public Vec3<T> Flipped => -this;

    /// <summary>
    /// Create a vector from components
    /// </summary>
    /// <param name="x">x component</param>
    /// <param name="y">y component</param>
    /// <param name="z">z component</param>
    public Vec3(T x, T y, T z) {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    /// <summary>
    /// Create a vector by copying the components of another
    /// </summary>
    /// <param name="other">other vector</param>
    public Vec3(Vec3<T> other) {
        this.X = other.X;
        this.Y = other.Y;
        this.Z = other.Z;
    }

    /// <summary>
    /// Dot product of two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>dot product</returns>
    public static T Dot(Vec3<T> lhs, Vec3<T> rhs) {
        var xx = lhs.X.MultiplyBy(rhs.X);
        var yy = lhs.Y.MultiplyBy(rhs.Y);
        var zz = lhs.Z.MultiplyBy(rhs.Z);

        return xx.Add(yy).Add(zz);
    }

    /// <summary>
    /// Cross product of two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>cross product</returns>
    public static Vec3<T> Cross(Vec3<T> lhs, Vec3<T> rhs) {
        var a2b3 = lhs.Y.MultiplyBy(rhs.Z);
        var a3b2 = lhs.Z.MultiplyBy(rhs.Y);
        var a1b3 = lhs.X.MultiplyBy(rhs.Z);
        var a3b1 = lhs.Z.MultiplyBy(rhs.X);
        var a1b2 = lhs.X.MultiplyBy(rhs.Y);
        var a2b1 = lhs.Y.MultiplyBy(rhs.X);

        var i = a2b3.Subtract(a3b2);
        var j = a3b1.Subtract(a1b3);
        var k = a1b2.Subtract(a2b1);

        return new Vec3<T>(
            i,
            j,
            k
        );
    }
    /// <summary>
    /// Distance between two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>distance</returns>
    public static T Distance (Vec3<T> lhs, Vec3<T> rhs) {
        return (rhs - lhs).Length;
    }
    /// <summary>
    /// Squared distance between two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>squared distance</returns>
    public static T SqrDistance(Vec3<T> lhs, Vec3<T> rhs) {
        return (rhs - lhs).SqrLength;
    }

    public static Vec3<T> operator - (Vec3<T> rhs) {
        return new Vec3<T>(
            x: rhs.X.Negate(),
            y: rhs.Y.Negate(),
            z: rhs.Z.Negate()
        );
    }

    public static Vec3<T> operator + (Vec3<T> rhs) {
        return rhs;
    }

    public static Vec3<T> operator + (Vec3<T> lhs, Vec3<T> rhs) {
        return new Vec3<T>(
            lhs.X.Add(rhs.X),
            lhs.Y.Add(rhs.Y),
            lhs.Z.Add(rhs.Z)
        );
    } 

    public static Vec3<T> operator - (Vec3<T> lhs, Vec3<T> rhs) {
        return new Vec3<T>(
            lhs.X.Subtract(rhs.X),
            lhs.Y.Subtract(rhs.Y),
            lhs.Z.Subtract(rhs.Z)
        );
    } 

    public static Vec3<T> operator * (T lhs, Vec3<T> rhs) {
        return new Vec3<T>(
            lhs.MultiplyBy(rhs.X),
            lhs.MultiplyBy(rhs.Y),
            lhs.MultiplyBy(rhs.Z)
        );
    }

    public static Vec3<T> operator * (Vec3<T> lhs, T rhs) {
        return new Vec3<T>(
            lhs.X.MultiplyBy(rhs),
            lhs.Y.MultiplyBy(rhs),
            lhs.Z.MultiplyBy(rhs)
        );
    }

    public static Vec3<T> operator / (Vec3<T> lhs, T rhs) {
        return new Vec3<T>(
            lhs.X.DivideBy(rhs),
            lhs.Y.DivideBy(rhs),
            lhs.Z.DivideBy(rhs)
        );
    }

    /// <summary>
    /// Deconstruct vector to tuple
    /// </summary>
    /// <param name="x">x value</param>
    /// <param name="y">y value</param>
    /// <param name="z">z value</param>
    public void Deconstruct(out T x, out T y, out T z) {
        x = this.X;
        y = this.Y;
        z = this.Z;
    }

    /// <summary>
    /// Convert vector from one type to another
    /// </summary>
    /// <param name="converter">type conversion function</param>
    /// <typeparam name="U">new type</typeparam>
    /// <returns>new vector of the appropriate type</returns>
    public Vec3<U> Map<U>(Func<T, U> converter) where U:INegatable<U>, ISquareRootable<U>, IAddable<U,U>, ISubtractable<U,U>, IMultiplyable<U,U>, IDividable<U,U> {
        return new Vec3<U>(
            converter(this.X),
            converter(this.Y),
            converter(this.Z)
        );
    }

    public override bool Equals(object obj) {
        if (obj is Vec3<T> vec) {
            return this.X.Equals(vec.X) && this.Y.Equals(vec.Y) && this.Z.Equals(vec.Z);
        } else {
            return base.Equals(obj);
        }
    }

    public override int GetHashCode(){
        return HashCode.Combine(this.X, this.Y, this.Z);
    }

    public override string ToString() {
        return $"(x:{X},y:{Y},z:{Z})";
    }    
    
}

}