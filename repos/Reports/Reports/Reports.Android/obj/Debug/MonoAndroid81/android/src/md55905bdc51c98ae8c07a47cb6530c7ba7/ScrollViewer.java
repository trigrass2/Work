package md55905bdc51c98ae8c07a47cb6530c7ba7;


public class ScrollViewer
	extends android.widget.FrameLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_measureChild:(Landroid/view/View;II)V:GetMeasureChild_Landroid_view_View_IIHandler\n" +
			"n_measureChildWithMargins:(Landroid/view/View;IIII)V:GetMeasureChildWithMargins_Landroid_view_View_IIIIHandler\n" +
			"n_addView:(Landroid/view/View;)V:GetAddView_Landroid_view_View_Handler\n" +
			"n_computeHorizontalScrollOffset:()I:GetComputeHorizontalScrollOffsetHandler\n" +
			"n_computeHorizontalScrollRange:()I:GetComputeHorizontalScrollRangeHandler\n" +
			"n_computeHorizontalScrollExtent:()I:GetComputeHorizontalScrollExtentHandler\n" +
			"n_computeVerticalScrollExtent:()I:GetComputeVerticalScrollExtentHandler\n" +
			"n_computeVerticalScrollOffset:()I:GetComputeVerticalScrollOffsetHandler\n" +
			"n_computeVerticalScrollRange:()I:GetComputeVerticalScrollRangeHandler\n" +
			"n_computeScroll:()V:GetComputeScrollHandler\n" +
			"n_onAttachedToWindow:()V:GetOnAttachedToWindowHandler\n" +
			"n_onDetachedFromWindow:()V:GetOnDetachedFromWindowHandler\n" +
			"n_onInterceptTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnInterceptTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"n_onOverScrolled:(IIZZ)V:GetOnOverScrolled_IIZZHandler\n" +
			"n_draw:(Landroid/graphics/Canvas;)V:GetDraw_Landroid_graphics_Canvas_Handler\n" +
			"n_onScrollChanged:(IIII)V:GetOnScrollChanged_IIIIHandler\n" +
			"n_scrollTo:(II)V:GetScrollTo_IIHandler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.SfDataGrid.XForms.Droid.ScrollViewer, Syncfusion.SfDataGrid.XForms.Android", ScrollViewer.class, __md_methods);
	}


	public ScrollViewer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ScrollViewer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.ScrollViewer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public ScrollViewer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ScrollViewer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.ScrollViewer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public ScrollViewer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ScrollViewer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.ScrollViewer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public ScrollViewer (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == ScrollViewer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.ScrollViewer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void measureChild (android.view.View p0, int p1, int p2)
	{
		n_measureChild (p0, p1, p2);
	}

	private native void n_measureChild (android.view.View p0, int p1, int p2);


	public void measureChildWithMargins (android.view.View p0, int p1, int p2, int p3, int p4)
	{
		n_measureChildWithMargins (p0, p1, p2, p3, p4);
	}

	private native void n_measureChildWithMargins (android.view.View p0, int p1, int p2, int p3, int p4);


	public void addView (android.view.View p0)
	{
		n_addView (p0);
	}

	private native void n_addView (android.view.View p0);


	public int computeHorizontalScrollOffset ()
	{
		return n_computeHorizontalScrollOffset ();
	}

	private native int n_computeHorizontalScrollOffset ();


	public int computeHorizontalScrollRange ()
	{
		return n_computeHorizontalScrollRange ();
	}

	private native int n_computeHorizontalScrollRange ();


	public int computeHorizontalScrollExtent ()
	{
		return n_computeHorizontalScrollExtent ();
	}

	private native int n_computeHorizontalScrollExtent ();


	public int computeVerticalScrollExtent ()
	{
		return n_computeVerticalScrollExtent ();
	}

	private native int n_computeVerticalScrollExtent ();


	public int computeVerticalScrollOffset ()
	{
		return n_computeVerticalScrollOffset ();
	}

	private native int n_computeVerticalScrollOffset ();


	public int computeVerticalScrollRange ()
	{
		return n_computeVerticalScrollRange ();
	}

	private native int n_computeVerticalScrollRange ();


	public void computeScroll ()
	{
		n_computeScroll ();
	}

	private native void n_computeScroll ();


	public void onAttachedToWindow ()
	{
		n_onAttachedToWindow ();
	}

	private native void n_onAttachedToWindow ();


	public void onDetachedFromWindow ()
	{
		n_onDetachedFromWindow ();
	}

	private native void n_onDetachedFromWindow ();


	public boolean onInterceptTouchEvent (android.view.MotionEvent p0)
	{
		return n_onInterceptTouchEvent (p0);
	}

	private native boolean n_onInterceptTouchEvent (android.view.MotionEvent p0);


	public boolean onTouchEvent (android.view.MotionEvent p0)
	{
		return n_onTouchEvent (p0);
	}

	private native boolean n_onTouchEvent (android.view.MotionEvent p0);


	public void onOverScrolled (int p0, int p1, boolean p2, boolean p3)
	{
		n_onOverScrolled (p0, p1, p2, p3);
	}

	private native void n_onOverScrolled (int p0, int p1, boolean p2, boolean p3);


	public void draw (android.graphics.Canvas p0)
	{
		n_draw (p0);
	}

	private native void n_draw (android.graphics.Canvas p0);


	public void onScrollChanged (int p0, int p1, int p2, int p3)
	{
		n_onScrollChanged (p0, p1, p2, p3);
	}

	private native void n_onScrollChanged (int p0, int p1, int p2, int p3);


	public void scrollTo (int p0, int p1)
	{
		n_scrollTo (p0, p1);
	}

	private native void n_scrollTo (int p0, int p1);

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
