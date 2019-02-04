package md55905bdc51c98ae8c07a47cb6530c7ba7;


public class GridIndentCellRenderer
	extends md55905bdc51c98ae8c07a47cb6530c7ba7.GridCellBaseRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"n_performClick:()Z:GetPerformClickHandler\n" +
			"n_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.SfDataGrid.XForms.Droid.GridIndentCellRenderer, Syncfusion.SfDataGrid.XForms.Android", GridIndentCellRenderer.class, __md_methods);
	}


	public GridIndentCellRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == GridIndentCellRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.GridIndentCellRenderer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public GridIndentCellRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == GridIndentCellRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.GridIndentCellRenderer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public GridIndentCellRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == GridIndentCellRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.GridIndentCellRenderer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public boolean onTouchEvent (android.view.MotionEvent p0)
	{
		return n_onTouchEvent (p0);
	}

	private native boolean n_onTouchEvent (android.view.MotionEvent p0);


	public boolean performClick ()
	{
		return n_performClick ();
	}

	private native boolean n_performClick ();


	public void onDraw (android.graphics.Canvas p0)
	{
		n_onDraw (p0);
	}

	private native void n_onDraw (android.graphics.Canvas p0);

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
