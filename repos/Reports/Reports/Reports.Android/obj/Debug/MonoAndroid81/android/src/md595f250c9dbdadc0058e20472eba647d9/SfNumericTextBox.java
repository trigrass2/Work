package md595f250c9dbdadc0058e20472eba647d9;


public class SfNumericTextBox
	extends android.widget.EditText
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_setRawInputType:(I)V:GetSetRawInputType_IHandler\n" +
			"n_onEditorAction:(I)V:GetOnEditorAction_IHandler\n" +
			"n_onTextContextMenuItem:(I)Z:GetOnTextContextMenuItem_IHandler\n" +
			"n_onTextChanged:(Ljava/lang/CharSequence;III)V:GetOnTextChanged_Ljava_lang_CharSequence_IIIHandler\n" +
			"n_onFocusChanged:(ZILandroid/graphics/Rect;)V:GetOnFocusChanged_ZILandroid_graphics_Rect_Handler\n" +
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Numerictextbox.SfNumericTextBox, Syncfusion.SfNumericTextBox.Android", SfNumericTextBox.class, __md_methods);
	}


	public SfNumericTextBox (android.content.Context p0)
	{
		super (p0);
		if (getClass () == SfNumericTextBox.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Numerictextbox.SfNumericTextBox, Syncfusion.SfNumericTextBox.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public SfNumericTextBox (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == SfNumericTextBox.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Numerictextbox.SfNumericTextBox, Syncfusion.SfNumericTextBox.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public SfNumericTextBox (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == SfNumericTextBox.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Numerictextbox.SfNumericTextBox, Syncfusion.SfNumericTextBox.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public SfNumericTextBox (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == SfNumericTextBox.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Numerictextbox.SfNumericTextBox, Syncfusion.SfNumericTextBox.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void setRawInputType (int p0)
	{
		n_setRawInputType (p0);
	}

	private native void n_setRawInputType (int p0);


	public void onEditorAction (int p0)
	{
		n_onEditorAction (p0);
	}

	private native void n_onEditorAction (int p0);


	public boolean onTextContextMenuItem (int p0)
	{
		return n_onTextContextMenuItem (p0);
	}

	private native boolean n_onTextContextMenuItem (int p0);


	public void onTextChanged (java.lang.CharSequence p0, int p1, int p2, int p3)
	{
		n_onTextChanged (p0, p1, p2, p3);
	}

	private native void n_onTextChanged (java.lang.CharSequence p0, int p1, int p2, int p3);


	public void onFocusChanged (boolean p0, int p1, android.graphics.Rect p2)
	{
		n_onFocusChanged (p0, p1, p2);
	}

	private native void n_onFocusChanged (boolean p0, int p1, android.graphics.Rect p2);

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
