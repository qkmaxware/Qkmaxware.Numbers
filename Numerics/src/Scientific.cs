using System;

namespace Qkmaxware.Numbers {
    
/// <summary>
/// Double precision number in scientific notation
/// </summary>
public class Scientific {
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
	}
	
	/// <summary>
	/// Absolute value of this number
	/// </summary>
	/// <returns>absolute value</returns>
	public Scientific Abs() {
		return new Scientific(Math.Abs(this.Significand), this.Exponent);
	}

	/// <summary>
	/// Value represented in normalized scienfific notation
	/// </summary>
	/// <returns>normalized scientific notation version of this value</returns>
	public Scientific Normalized() {
		checked {
			var abs = Math.Abs(this.Significand);
			var sign = Math.Sign(this.Significand);
			
			if (abs >= 1) {
				var powerChange = (int)Math.Floor(Math.Log10((int)abs));
				var next = abs / Math.Pow(10, powerChange);
				
				return new Scientific(sign * next, Exponent + powerChange);
			} else if (abs < 1 && abs > 0) {
				var next = abs;
				var powerChange = 0;
				while((int)next == 0) {
					next *= 10;
					powerChange++;
				}
				return new Scientific(sign * next, Exponent - powerChange);
			} else {
				return new Scientific(this.Significand, this.Exponent);
			}
		}
	}
	
	/// <summary>
	/// Change the exponent while keeping the value the same
	/// </summary>
	/// <param name="x">the new exponent</param>
	/// <returns>same value with the given exponent</returns>
	public Scientific SetExponent(int x) {
		checked {
			if (x == this.Exponent) {
				return this;
			}
				
			var difference = this.Exponent - x;
			return new Scientific(this.Significand * Math.Pow(10, difference), x);
		}
	}

	/// <summary>
	/// Change the exponent by the given amount without changing the value
	/// </summary>
	/// <param name="delta">amount to change</param>
	/// <returns>same value with the exponent shifted by the given amount</returns>
	public Scientific ShiftExponent(int delta) {
		return SetExponent(this.Exponent + delta);
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
		return new Scientific(d, 0).Normalized();
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
			var rl = lhs.SetExponent(commonExponent);
			var rr = rhs.SetExponent(commonExponent);

			return new Scientific(rl.Significand + rr.Significand, commonExponent);
		}
	}
	
	/// <summary>
	/// Subtraction
	/// </summary>
	public static Scientific operator - (Scientific lhs, Scientific rhs) {
		checked {
			var commonExponent = Math.Max(lhs.Exponent, rhs.Exponent);
			var rl = lhs.SetExponent(commonExponent);
			var rr = rhs.SetExponent(commonExponent);
			
			return new Scientific(rl.Significand - rr.Significand, commonExponent);
		}
	}
	
	/// <summary>
	/// Multiplication 
	/// </summary>
	public static Scientific operator * (Scientific lhs, Scientific rhs) {
		checked {
			var commonExponent = Math.Max(lhs.Exponent, rhs.Exponent);
			var rl = lhs.SetExponent(commonExponent);
			var rr = rhs.SetExponent(commonExponent);
			
			return new Scientific(rl.Significand * rr.Significand, commonExponent);
		}
	}
	
	/// <summary>
	/// Division
	/// </summary>
	public static Scientific operator / (Scientific lhs, Scientific rhs) {
		checked {
			var commonExponent = Math.Max(lhs.Exponent, rhs.Exponent);
			var rl = lhs.SetExponent(commonExponent);
			var rr = rhs.SetExponent(commonExponent);
			
			return new Scientific(rl.Significand / rr.Significand, commonExponent);
		}
	}
}

public static class RealExtensions {
	public static Scientific x10(this int value, int power) {
		return new Scientific((double)value, power);
	}
	public static Scientific x10(this double value, int power) {
		return new Scientific(value, power);
	}
}

}