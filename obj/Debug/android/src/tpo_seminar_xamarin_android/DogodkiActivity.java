package tpo_seminar_xamarin_android;


public class DogodkiActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("TPO_Seminar_Xamarin_Android.DogodkiActivity, TPO_Seminar_Xamarin_Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DogodkiActivity.class, __md_methods);
	}


	public DogodkiActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DogodkiActivity.class)
			mono.android.TypeManager.Activate ("TPO_Seminar_Xamarin_Android.DogodkiActivity, TPO_Seminar_Xamarin_Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
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