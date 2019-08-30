package md5fde8c6fc4a8c39be3423a3807afda47d;


public class ViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Autocomplete.ViewHolder, Syncfusion.SfAutoComplete.Android", ViewHolder.class, __md_methods);
	}


	public ViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == ViewHolder.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Autocomplete.ViewHolder, Syncfusion.SfAutoComplete.Android", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
