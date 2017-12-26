namespace Duality.Editor.Extensibility.DataConversion
{
	public interface IObjectRefHolder
	{
		IContentRef ResourceReference { get; }
		GameObject GameObjectReference { get;  }
		Component ComponentReference { get; }
	}
}