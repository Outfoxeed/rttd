namespace RTTD;

public abstract partial class RTTDWindow<T> : RTTDWindowBase
{
	protected T WindowOwner { get; private set; }
	public override void SetOwner(object owner)
	{
		if (owner is not T value)
		{
			Logger.LogError(this, $"Owner for window '{GetName()}' is not of the expected type '{typeof(T).Name}'");
			return;
		}
		
		WindowOwner = value;
	}
}
