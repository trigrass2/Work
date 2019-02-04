package md55905bdc51c98ae8c07a47cb6530c7ba7;


public class GridGroupSummaryCellRenderer
	extends md55905bdc51c98ae8c07a47cb6530c7ba7.GridSummaryCellRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Syncfusion.SfDataGrid.XForms.Droid.GridGroupSummaryCellRenderer, Syncfusion.SfDataGrid.XForms.Android", GridGroupSummaryCellRenderer.class, __md_methods);
	}


	public GridGroupSummaryCellRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == GridGroupSummaryCellRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.GridGroupSummaryCellRenderer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public GridGroupSummaryCellRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == GridGroupSummaryCellRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.GridGroupSummaryCellRenderer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public GridGroupSummaryCellRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == GridGroupSummaryCellRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfDataGrid.XForms.Droid.GridGroupSummaryCellRenderer, Syncfusion.SfDataGrid.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
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
