package md593b8d625023f6802361dd1b8a6546be5;


public class FifthActivity
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("App5.FifthActivity, App5, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FifthActivity.class, __md_methods);
	}


	public FifthActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FifthActivity.class)
			mono.android.TypeManager.Activate ("App5.FifthActivity, App5, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
