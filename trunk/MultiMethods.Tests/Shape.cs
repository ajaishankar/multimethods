using System;
using System.Reflection;

namespace MultiMethods.Tests
{
	public interface IShape
	{
		string Intersect(IShape other);
	}

	public abstract class Shape : IShape
	{
		private MultiMethod.Func<IShape, string> _intersect;

		public Shape()
		{
			_intersect = Dispatcher.Func<IShape, string>(this.Intersect);
		}

		public string Intersect(IShape other)
		{
			DispatchResult<string> result = _intersect(other); // multi method dispatch

			if (result.Success)
			{
				return result.ReturnValue;
			}
			else if (result.NoMatch)
			{
				return "Shape x Shape";
			}
			else if (result.AmbiguousMatch)
			{
				throw new AmbiguousMatchException();
			}
			else
			{
				throw new Exception("Unknown failure");
			}
		}
	}

	public class Rectangle : Shape
	{
		protected string Intersect(Rectangle other)
		{
			return "Rectangle x Rectangle";
		}

		protected string Intersect(Circle other)
		{
			return "Rectangle x Circle";
		}

		protected virtual string Intersect(Ellipse other)
		{
			return "Rectangle x Ellipse";
		}
	}

	public class RoundedRectangle : Rectangle
	{
		protected new string Intersect(Rectangle other)
		{
			return "RoundedRectange x Rectangle";
		}

		protected override string Intersect(Ellipse other)
		{
			return "RoundedRectange x Ellipse";
		}
	}

	public class Circle : Shape
	{
		int _radius;

		public Circle()
		{
		}

		public Circle(int radius)
		{
			_radius = radius;
		}

		protected string Intersect(Rectangle other)
		{
			return this.ToString() + " x Rectangle";
		}

		protected string Intersect(Circle other)
		{
			return this.ToString() + " x Circle";
		}

		protected new string Intersect(IShape shape)
		{
			return this.ToString() + " x IShape";
		}

		public override string ToString()
		{
			return _radius == 0 ? "Circle" : "Circle(" + _radius + ")";
		}
	}

	public class Morph : Shape
	{
	}

	public class Ellipse : IShape
	{
		public string Intersect(IShape other)
		{
			throw new NotImplementedException();
		}
	}

	public class GenericShape<T> : Shape
	{
		public string Intersect(GenericShape<T> other)
		{
			return string.Format("GenericShape<{0}> x GenericShape<{0}>", typeof(T).Name);
		}
	}
}