using System;
using System.Numerics;

namespace Qkmaxware.Numbers {
    
/// <summary>
/// Double precision number in scientific notation
/// </summary>
public class Scientific : INumeric<Scientific>, IScalable<Scientific,Scientific> {
	public static readonly Scientific Zero = new Scientific(0, 0);
	public static readonly Scientific One = new Scientific(1, 0);

	/// <summary>
	/// Significant digits
	/// </summary>
	/// <value>significant digits</value>
	public double Significand {get; private set;}
	/// <summary>
	/// Exponent
	/// </summary>
	/// <value>power of 10</value>
	public int Exponent {get; private set;}
	/// <summary>
	/// Sign of the value
	/// </summary>
	/// <returns>sign</returns>
	public int Sign => Math.Sign(this.Significand);

	/// <summary>
	/// Create a new scientific number
	/// </summary>
	/// <param name="value">significant digits</param>
	/// <param name="power">power of 10</param>	
	public Scientific (double value, int power = 0) {
		this.Significand = value;
		this.Exponent = power;

		normalize();
	}
	
	/// <summary>
	/// Absolute value of this number
	/// </summary>
	/// <returns>absolute value</returns>
	public Scientific Abs() {
		return new Scientific(Math.Abs(this.Significand), this.Exponent);
	}

	/// <summary>
	/// Floor of this value
	/// </summary>
	/// <returns>floored value</returns>
	public Scientific Floor() {
		checked {
			return new Scientific(Math.Floor((double)this), 0);
		}
	}

	/// <summary>
	/// Ceiling of this value
	/// </summary>
	/// <returns>ceiled value</returns>
	public Scientific Ceil() {
		checked {
			return new Scientific(Math.Ceiling((double)this), 0);
		}
	}

	/// <summary>
	/// Value represented in normalized scienfific notation
	/// </summary>
	/// <returns>normalized scientific notation version of this value</returns>
	private void normalize() {
		checked {
			// Guards
			if (double.IsInfinity(this.Significand)) {
				this.Exponent = 0;
				return;
			} else if (double.IsNaN(this.Significand)) {
				return;
			} else if (this.Significand == 0) {
				this.Exponent = 0;
				return;
			}

			// Normalize
			var abs = Math.Abs(this.Significand);
			var sign = Math.Sign(this.Significand);
			
			if (abs >= 1) {
				var powerChange = (int)Math.Floor(Math.Log10((int)abs));
				var next = abs / Math.Pow(10, powerChange);
				
				this.Significand = sign * next;
				this.Exponent = Exponent + powerChange;
			} else if (abs < 1 && abs > 0) {
				var next = abs;
				var powerChange = 0;
				while((int)next == 0) {
					next *= 10;
					powerChange++;
				}
				this.Significand = sign * next;
				this.Exponent = Exponent - powerChange;
			}
		}
	}
	
	/// <summary>
	/// Change the exponent while keeping the value the same
	/// </summary>
	/// <param name="x">the new exponent</param>
	/// <returns>same value with the given exponent</returns>
	private double setExponent(int x) {
		checked {
			if (x == this.Exponent) {
				return this.Significand;
			}
				
			var difference = this.Exponent - x;
			return this.Significand * Math.Pow(10, difference);
		}
	}
	
	/// <summary>
	/// String representation of this value in scienfitic notifiation
	/// </summary>
	/// <returns>string</returns>
	public override string ToString() {
		return this.Significand.ToString() + "E" + this.Exponent.ToString();
	}
	
	/// <summary>
	/// Convert from a double to scientific notation
	/// </summary>
	/// <param name="d">value</param>
	public static implicit operator Scientific(double d) {
		return new Scientific(d, 0);
	}

	/// <summary>
	/// Convert from scientific notation back into a double
	/// </summary>
	/// <param name="value">value in scientific notation</param>	
	public static explicit operator double(Scientific value) {
		checked {
			return value.Significand * Math.Pow(10, value.Exponent);
		}
	}

	/// <summary>
	/// Implicly upcast to an arbitrary
	/// </summary>
	/// <param name="value">value in scientific notation</param>
	public static implicit operator Arbitrary(Scientific value) {
		// Convert significand * (1*10^exponent)
		return ((Arbitrary)(value.Significand)) * (new Arbitrary(1, value.Exponent));
	}
	
	/// <summary>
	/// Unitary addition
	/// </summary>
	public static Scientific operator + (Scientific lhs) {
		checked {
			return new Scientific(lhs.Significand, lhs.Exponent);
		}
	}
	
	/// <summary>
	/// Unitary subtraction
	/// </summary>
	public static Scientific operator - (Scientific lhs) {
		checked {
			return new Scientific(-lhs.Significand, lhs.Exponent);
		}
	}
	
	/// <summary>
	/// Addition
	/// </summary>
	public static Scientific operator + (Scientific lhs, Scientific rhs) {
		checked {
			var commonExponent = Math.Max(lhs.Exponent, rhs.Exponent);
			var rl = lhs.setExponent(commonExponent);
			var rr = rhs.setExponent(commonExponent);

			return new Scientific(rl + rr, commonExponent);
		}
	}
	
	/// <summary>
	/// Subtraction
	/// </summary>
	public static Scientific operator - (Scientific lhs, Scientific rhs) {
		checked {
			var commonExponent = Math.Max(lhs.Exponent, rhs.Exponent);
			var rl = lhs.setExponent(commonExponent);
			var rr = rhs.setExponent(commonExponent);
			
			return new Scientific(rl - rr, commonExponent);
		}
	}
	
	/// <summary>
	/// Multiplication 
	/// </summary>
	public static Scientific operator * (Scientific lhs, Scientific rhs) {
		checked {
			return new Scientific(lhs.Significand * rhs.Significand, lhs.Exponent + rhs.Exponent);
		}
	}
	
	/// <summary>
	/// Division
	/// </summary>
	public static Scientific operator / (Scientific lhs, Scientific rhs) {
		checked {
			return new Scientific(lhs.Significand / rhs.Significand, lhs.Exponent - rhs.Exponent);
		}
	}

	/// <summary>
	/// Greater than
	/// </summary>
	/// <returns>true if the first value is greater than the second</returns>
	public static bool operator > (Scientific lhs, Scientific rhs) {
		// Can do this because we are normalized
		return lhs.Exponent > rhs.Exponent || (lhs.Exponent == rhs.Exponent && lhs.Significand > rhs.Significand);
	}

	/// <summary>
	/// Less than
	/// </summary>
	/// <returns>true if the first value is less than the second</returns>
	public static bool operator < (Scientific lhs, Scientific rhs) {
		// Can do this because we are normalized
		return lhs.Exponent < rhs.Exponent || (lhs.Exponent == rhs.Exponent && lhs.Significand < rhs.Significand);
	}

	public Scientific ScaleBy(Scientific value) => MultiplyBy(value);

	public Scientific Negate() {
        return -this;
    }

    public Scientific Add(Scientific rhs) {
        return this + rhs;
    }

    public Scientific Subtract(Scientific rhs) {
        return this - rhs;
    }

    public Scientific MultiplyBy(Scientific rhs) {
        return this * rhs;
    }

    public Scientific DivideBy(Scientific rhs) {
        return this / rhs;
    }

	public Scientific Sqrt() {
		if (this.Exponent.IsEven()) {
			// if n is even just take the square root of x and 10^n and multiply them
			return new Scientific(Math.Sqrt(this.Significand), this.Exponent / 2);
		} else {
			// if n is odd, mutiply x by 10 and reduce n by 1 to make it even and then take square root of each and multiply them
			return new Scientific(Math.Sqrt(this.Significand * 10), (this.Exponent - 1)/2);
		}
		
	}

	/// <summary>
	/// Equality comparison of two scientific numbers
	/// </summary>
	/// <param name="lhs">first number</param>
	/// <param name="rhs">second number</param>
	/// <returns>true if the two numbers are the same</returns>
	public static bool operator == (Scientific lhs, Scientific rhs) {
		// Since values are normalized then we can do this
		return lhs.Exponent == rhs.Exponent && lhs.Significand == rhs.Significand;
	}
	/// <summary>
	/// Inequality comparison of two scientific numbers
	/// </summary>
	/// <param name="lhs">first number</param>
	/// <param name="rhs">second number</param>
	/// <returns>true if the two numbers are not the same</returns>
	public static bool operator != (Scientific lhs, Scientific rhs) {
		return !(lhs == rhs);
	}

	// override object.Equals
	public override bool Equals(object obj) {
		if (obj == null)
			return false;
		if (obj is Scientific sci) {
			return this == sci; // call the defined == operator
		} else {
			return false;
		}
	}
	
	// override object.GetHashCode
	public override int GetHashCode() {
		return HashCode.Combine(this.Significand, this.Exponent);
	}
}

public static class RealExtensions {
	public static Scientific x10(this int value, int power) {
		return new Scientific((double)value, power);
	}
	public static Scientific x10(this double value, int power) {
		return new Scientific(value, power);
	}

	public static Arbitrary x10(this long value, int power) {
		return new Arbitrary(value, power);
	}

	public static Arbitrary x10(this BigInteger value, int power) {
		return new Arbitrary(value, power);
	}
}

}