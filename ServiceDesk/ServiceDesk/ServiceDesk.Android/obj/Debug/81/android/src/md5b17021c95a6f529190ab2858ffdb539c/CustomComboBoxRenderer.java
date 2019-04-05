package md5b17021c95a6f529190ab2858ffdb539c;


public class CustomComboBoxRenderer
	extends md50bce4418131036bf800de8027e1aebb1.SfComboBoxRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ServiceDesk.Droid.CustomComboBoxRenderer, ServiceDesk.Android", CustomComboBoxRenderer.class, __md_methods);
	}


	public CustomComboBoxRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomComboBoxRenderer.class)
			mono.android.TypeManager.Activate ("ServiceDesk.Droid.CustomComboBoxRenderer, ServiceDesk.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public CustomComboBoxRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomComboBoxRenderer.class)
			mono.android.TypeManager.Activate ("ServiceDesk.Droid.CustomComboBoxRenderer, ServiceDesk.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomComboBoxRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomComboBoxRenderer.class)
			mono.android.TypeManager.Activate ("ServiceDesk.Droid.CustomComboBoxRenderer, ServiceDesk.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
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
