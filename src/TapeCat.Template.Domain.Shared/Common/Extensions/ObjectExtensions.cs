namespace TapeCat.Template.Domain.Shared.Common.Extensions;

public static class ObjectExtensions
{
	public delegate void TapActionRef<TSelf> ( ref TSelf self )
		where TSelf : struct;

	public delegate ref TReturn TapFuncRef<TSelf, TReturn> ( ref TSelf self )
		where TSelf : struct;

	public static TSelf Tap<TSelf> ( this TSelf self , Action<TSelf> tap )
		where TSelf : notnull
	{
		tap ( self );

		return self;
	}

	public static ref TSelf Tap<TSelf> ( this ref TSelf self , TapActionRef<TSelf> tap )
		where TSelf : struct
	{
		tap ( ref self );

		return ref self;
	}

	public static async Task<TSelf> TapAsync<TSelf> ( this TSelf self , Func<TSelf , Task> tapAsync )
		where TSelf : notnull
	{
		await tapAsync ( self );

		return self;
	}

	public static TReturn? Tap<TSelf, TReturn> ( this TSelf self , Func<TSelf , TReturn> tap )
		where TSelf : notnull
			=> tap ( self );

	public static ref TReturn Tap<TSelf, TReturn> ( this ref TSelf self , TapFuncRef<TSelf , TReturn> tap )
		where TSelf : struct
			=> ref tap ( ref self );

	public static async Task<TReturn?> TapAsync<TSelf, TReturn> ( this TSelf self , Func<TSelf , Task<TReturn>> tapAsync )
		where TSelf : notnull
			=> await tapAsync ( self );
}