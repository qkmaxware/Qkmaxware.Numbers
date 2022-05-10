using System;

namespace Qkmaxware.Numbers {

/// <summary>
/// Abstract vector of 3 dimensions
/// </summary>
/// <typeparam name="T">quantity type for each axis</typeparam>
public class Vec3<T> where T:IVectorable<T>  {
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
            var xx = X.ScalarMultiplyBy(X);
            var yy = Y.ScalarMultiplyBy(Y);
            var zz = Z.ScalarMultiplyBy(Z);
            return xx.ScalarAddBy(yy).ScalarAddBy(zz);
        }
    }
    /// <summary>
    /// Length of the vector
    /// </summary>
    public T Length => SqrLength.ScalarSqrt();

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
        var xx = lhs.X.ScalarMultiplyBy(rhs.X);
        var yy = lhs.Y.ScalarMultiplyBy(rhs.Y);
        var zz = lhs.Z.ScalarMultiplyBy(rhs.Z);

        return xx.ScalarAddBy(yy).ScalarAddBy(zz);
    }

    /// <summary>
    /// Cross product of two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>cross product</returns>
    public static Vec3<T> Cross(Vec3<T> lhs, Vec3<T> rhs) {
        var a2b3 = lhs.Y.ScalarMultiplyBy(rhs.Z);
        var a3b2 = lhs.Z.ScalarMultiplyBy(rhs.Y);
        var a1b3 = lhs.X.ScalarMultiplyBy(rhs.Z);
        var a3b1 = lhs.Z.ScalarMultiplyBy(rhs.X);
        var a1b2 = lhs.X.ScalarMultiplyBy(rhs.Y);
        var a2b1 = lhs.Y.ScalarMultiplyBy(rhs.X);

        var i = a2b3.ScalarSubtractBy(a3b2);
        var j = a3b1.ScalarSubtractBy(a1b3);
        var k = a1b2.ScalarSubtractBy(a2b1);

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
            x: rhs.X.ScalarNegation(),
            y: rhs.Y.ScalarNegation(),
            z: rhs.Z.ScalarNegation()
        );
    }

    public static Vec3<T> operator + (Vec3<T> rhs) {
        return rhs;
    }

    public static Vec3<T> operator + (Vec3<T> lhs, Vec3<T> rhs) {
        return new Vec3<T>(
            lhs.X.ScalarAddBy(rhs.X),
            lhs.Y.ScalarAddBy(rhs.Y),
            lhs.Z.ScalarAddBy(rhs.Z)
        );
    } 

    public static Vec3<T> operator - (Vec3<T> lhs, Vec3<T> rhs) {
        return new Vec3<T>(
            lhs.X.ScalarSubtractBy(rhs.X),
            lhs.Y.ScalarSubtractBy(rhs.Y),
            lhs.Z.ScalarSubtractBy(rhs.Z)
        );
    } 

    public static Vec3<T> operator * (T lhs, Vec3<T> rhs) {
        return new Vec3<T>(
            lhs.ScalarMultiplyBy(rhs.X),
            lhs.ScalarMultiplyBy(rhs.Y),
            lhs.ScalarMultiplyBy(rhs.Z)
        );
    }

    public static Vec3<T> operator * (Vec3<T> lhs, T rhs) {
        return new Vec3<T>(
            lhs.X.ScalarMultiplyBy(rhs),
            lhs.Y.ScalarMultiplyBy(rhs),
            lhs.Z.ScalarMultiplyBy(rhs)
        );
    }

    public static Vec3<T> operator / (Vec3<T> lhs, T rhs) {
        return new Vec3<T>(
            lhs.X.ScalarDivideBy(rhs),
            lhs.Y.ScalarDivideBy(rhs),
            lhs.Z.ScalarDivideBy(rhs)
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
    public Vec3<U> Map<U>(Func<T, U> converter) where U:IVectorable<U> {
        return new Vec3<U>(
            converter(this.X),
            converter(this.Y),
            converter(this.Z)
        );
    }

    public static bool operator == (Vec3<T> lhs, Vec3<T> rhs) {
        return lhs.Equals(rhs);
    }

    public static bool operator != (Vec3<T> lhs, Vec3<T> rhs) {
        return !lhs.Equals(rhs);
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(obj, null)) {
            return false;
        }
        
        if (obj is Vec3<T> vec) {
            return this.X.Equals(vec.X) && this.Y.Equals(vec.Y) && this.Z.Equals(vec.Z);
        } else {
            return base.Equals(obj);
        }
    }

    public override int GetHashCode(){
        return (this.X, this.Y, this.Z).GetHashCode();
    }

    public override string ToString() {
        return $"(x:{X},y:{Y},z:{Z})";
    }    
    
}

}