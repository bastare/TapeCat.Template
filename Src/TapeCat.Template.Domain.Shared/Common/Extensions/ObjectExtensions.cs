namespace TapeCat.Template.Domain.Shared.Common.Extensions;

using System;

public static class ObjectExtensions
{
	public static TSelf Tap<TSelf> ( this TSelf self , Action<TSelf> tap )
	{
		tap ( self );

		return self;
	}

	public static TReturn Tap<TSelf, TReturn> ( this TSelf self , Func<TSelf , TReturn> tap )
		=> tap ( self );
}