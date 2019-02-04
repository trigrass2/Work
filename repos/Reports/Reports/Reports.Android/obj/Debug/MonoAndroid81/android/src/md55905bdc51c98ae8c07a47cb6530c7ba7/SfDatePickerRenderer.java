package md55905bdc51c98ae8c07a47cb6530c7ba7;


public class SfDatePickerRenderer
	extends md51558244f76c53b6aeda52c8a337f2c37.DatePickerRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onInterceptTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnInterceptTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.SfDataGrid.XForms.Droid.SfDatePickerRenderer, Syncfusion.SfDataGrid.XForms.Android", SfDatePickerRenderer.class, __md_methods);
	}


	public SfDatePickerRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == SfDatePickerRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.SfDatePickerRenderer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public SfDatePickerRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == SfDatePickerRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.SfDatePickerRenderer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public SfDatePickerRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == SfDatePickerRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.SfDatePickerRenderer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public boolean onInterceptTouchEvent (android.view.MotionEvent p0)
	{
		return n_onInterceptTouchEvent (p0);
	}

	private native boolean n_onInterceptTouchEvent (android.view.MotionEvent p0);

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
