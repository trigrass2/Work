package md5cab5b97bb4d3b375dec58f941300642e;


public class GearBox
	extends android.widget.FrameLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onSizeChanged:(IIII)V:GetOnSizeChanged_IIIIHandler\n" +
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Sfbusyindicator.GearBox, Syncfusion.SfBusyIndicator.Android", GearBox.class, __md_methods);
	}


	public GearBox (android.content.Context p0)
	{
		super (p0);
		if (getClass () == GearBox.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfbusyindicator.GearBox, Syncfusion.SfBusyIndicator.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public GearBox (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == GearBox.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfbusyindicator.GearBox, Syncfusion.SfBusyIndicator.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public GearBox (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == GearBox.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfbusyindicator.GearBox, Syncfusion.SfBusyIndicator.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public GearBox (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == GearBox.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfbusyindicator.GearBox, Syncfusion.SfBusyIndicator.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onSizeChanged (int p0, int p1, int p2, int p3)
	{
		n_onSizeChanged (p0, p1, p2, p3);
	}

	private native void n_onSizeChanged (int p0, int p1, int p2, int p3);

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
