package md5048335796d6a848447231107a6de4b57;


public class DragIndicatorViewRenderer
	extends md51558244f76c53b6aeda52c8a337f2c37.VisualElementRenderer_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onInterceptTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnInterceptTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.ListView.XForms.Android.DragIndicatorViewRenderer, Syncfusion.SfListView.XForms.Android", DragIndicatorViewRenderer.class, __md_methods);
	}


	public DragIndicatorViewRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == DragIndicatorViewRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.ListView.XForms.Android.DragIndicatorViewRenderer, Syncfusion.SfListView.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public DragIndicatorViewRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == DragIndicatorViewRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.ListView.XForms.Android.DragIndicatorViewRenderer, Syncfusion.SfListView.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public DragIndicatorViewRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == DragIndicatorViewRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.ListView.XForms.Android.DragIndicatorViewRenderer, Syncfusion.SfListView.XForms.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
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
