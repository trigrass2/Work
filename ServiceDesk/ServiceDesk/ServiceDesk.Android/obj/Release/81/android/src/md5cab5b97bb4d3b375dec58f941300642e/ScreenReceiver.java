package md5cab5b97bb4d3b375dec58f941300642e;


public class ScreenReceiver
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Sfbusyindicator.ScreenReceiver, Syncfusion.SfBusyIndicator.Android", ScreenReceiver.class, __md_methods);
	}


	public ScreenReceiver ()
	{
		super ();
		if (getClass () == ScreenReceiver.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfbusyindicator.ScreenReceiver, Syncfusion.SfBusyIndicator.Android", "", this, new java.lang.Object[] {  });
	}

	public ScreenReceiver (md5cab5b97bb4d3b375dec58f941300642e.SfBusyIndicator p0)
	{
		super ();
		if (getClass () == ScreenReceiver.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfbusyindicator.ScreenReceiver, Syncfusion.SfBusyIndicator.Android", "Com.Syncfusion.Sfbusyindicator.SfBusyIndicator, Syncfusion.SfBusyIndicator.Android", this, new java.lang.Object[] { p0 });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

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
