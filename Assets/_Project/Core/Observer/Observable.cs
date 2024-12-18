using UnityEngine.Events;

namespace Core.Observer
{
	/// <summary>
	/// Observable provides data from UIController to UIPresentationModel. And vice versa
	/// </summary>
	/// <typeparam name="PropertyType">Can be any value or reference type</typeparam>
	public class Observable<PropertyType> : IResettable
	{
		public readonly UnityEvent<PropertyType> onChanged = new UnityEvent<PropertyType>();
		private readonly PropertyType _defaultValue;
		private PropertyType _value;

		public Observable(PropertyType initialValue)
		{
			_value = initialValue;
			_defaultValue = initialValue;
		}

		public void Set(PropertyType value)
		{
			_value = value;
			onChanged?.Invoke(_value);
		}

		public PropertyType Get()
		{
			return _value;
		}

		public void ResetState()
		{
			Set(_defaultValue);
		}

		public static implicit operator Observable<PropertyType>(PropertyType observable)
		{
			return new Observable<PropertyType>(observable);
		}

		public static explicit operator PropertyType(Observable<PropertyType> observable)
		{
			return observable._value;
		}

		public static implicit operator string(Observable<PropertyType> observable)
		{
			return observable._value.ToString();
		}

		public override string ToString()
		{
			return _value.ToString();
		}

		public bool Equals(Observable<PropertyType> other)
		{
			return other._value.Equals(_value);
		}

		public void ForceChange()
		{
			onChanged?.Invoke(_value);
		}
	}
}