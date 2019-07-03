package md55f3543034dd3f8f0cc98906bc4e15f0e;


public class CustomTransitionsRenderer
	extends md58432a647068b097f9637064b8985a5e0.NavigationPageRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Vertical.Droid.CustomTransitionsRenderer, Vertical.Android", CustomTransitionsRenderer.class, __md_methods);
	}


	public CustomTransitionsRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomTransitionsRenderer.class)
			mono.android.TypeManager.Activate ("Vertical.Droid.CustomTransitionsRenderer, Vertical.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public CustomTransitionsRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomTransitionsRenderer.class)
			mono.android.TypeManager.Activate ("Vertical.Droid.CustomTransitionsRenderer, Vertical.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomTransitionsRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomTransitionsRenderer.class)
			mono.android.TypeManager.Activate ("Vertical.Droid.CustomTransitionsRenderer, Vertical.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
