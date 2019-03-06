package md56214e5d476e2b09e9e709cf3209b995f;


public class FlatEdgeShape
	extends android.graphics.drawable.shapes.Shape
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_draw:(Landroid/graphics/Canvas;Landroid/graphics/Paint;)V:GetDraw_Landroid_graphics_Canvas_Landroid_graphics_Paint_Handler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.XForms.Android.Backdrop.FlatEdgeShape, Syncfusion.SfBackdrop.XForms.Android", FlatEdgeShape.class, __md_methods);
	}


	public FlatEdgeShape ()
	{
		super ();
		if (getClass () == FlatEdgeShape.class)
			mono.android.TypeManager.Activate ("Syncfusion.XForms.Android.Backdrop.FlatEdgeShape, Syncfusion.SfBackdrop.XForms.Android", "", this, new java.lang.Object[] {  });
	}


	public void draw (android.graphics.Canvas p0, android.graphics.Paint p1)
	{
		n_draw (p0, p1);
	}

	private native void n_draw (android.graphics.Canvas p0, android.graphics.Paint p1);

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
